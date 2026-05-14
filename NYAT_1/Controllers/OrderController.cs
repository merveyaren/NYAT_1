using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NYAT_1.Core.Interfaces;
using NYAT_1.Data;
using NYAT_1.Models;
using NYAT_1.Patterns.Behavioral.Observer;
using NYAT_1.Patterns.Behavioral.Strategies;
using NYAT_1.Patterns.Creational.Factory;
using NYAT_1.Patterns.Creational.Singleton;
using NYAT_1.Patterns.Structural.Adapters;
using NYAT_1.Patterns.Structural.Decorators;

namespace NYAT_1.Controllers
{
    // MVC Mimarisi: Controller (Denetleyici) Katmanı. 
    // Kullanıcı isteklerini karşılar, Modelleri işler ve ilgili View'a (Görünüme) aktarır.
    public class OrderController : Controller
    {
        private readonly ApplicationDbContext _context;

        // Dependency Injection (Bağımlılıkların Enjeksiyonu): 
        // Veritabanı context'i dışarıdan enjekte edilerek (IoC Container üzerinden) sınıflar arası gevşek bağlılık (Loose Coupling) sağlanmıştır.
        public OrderController(ApplicationDbContext context)
        {
            _context = context;
        }

        // ========================================================
        // 1. MÜŞTERİ İŞLEMLERİ
        // ========================================================

        [Authorize(Roles = "Musteri, Admin")]
        public IActionResult Index()
        {
            ViewBag.Products = _context.Products.ToList();
            return View();
        }

        [Authorize(Roles = "Musteri, Admin")]
        public IActionResult MyOrders()
        {
            var userEmail = User.Identity.Name;
            var myOrders = _context.Orders.Where(o => o.CustomerEmail == userEmail).OrderByDescending(o => o.OrderDate).ToList();
            return View(myOrders);
        }

        [Authorize(Roles = "Musteri, Admin")]
        [HttpPost]
        public IActionResult ProcessOrder(string productName, int quantity, string paymentMethod, string cargoCompany, bool useInsurance, double distance)
        {
            var dbProduct = _context.Products.FirstOrDefault(p => p.Name == productName);

            // Guard Clause (Erken Çıkış) Prensibi 1: Geçersiz ürün kontrolü
            if (dbProduct == null)
            {
                TempData["ErrorMessage"] = "Lütfen geçerli bir ürün seçin.";
                return RedirectToAction("Index");
            }

            // Guard Clause (Erken Çıkış) Prensibi 2: Stok yetersizliği kontrolü. 
            // Sistemin tutarsız duruma (boş sayfa veya eksi stok) düşmesi engellenir.
            if (dbProduct.Stock < quantity)
            {
                TempData["ErrorMessage"] = $"Maalesef stokta yeterli ürün yok! {dbProduct.Name} için mevcut stok: {dbProduct.Stock} adet.";
                return RedirectToAction("Index");
            }

            // --- İş Kuralları (Business Logic) Başlangıcı ---
            decimal finalPrice = 0;

            // 1. BEHAVIORAL PATTERN: OBSERVER (Gözlemci)
            // Stok yöneticisi (Subject) oluşturulur ve ilgili departmanlar (Concrete Observers) sisteme abone edilir.
            ProductStockManager stockManager = new ProductStockManager(dbProduct.Name, dbProduct.Stock, 5);
            stockManager.Attach(new SatinAlmaEmailNotifier());
            stockManager.Attach(new DepoNotifier());

            // Stok düşüldüğünde eşik değer aşılırsa abonelere otomatik "Update" tetiklenir.
            stockManager.DecreaseStock(quantity);
            dbProduct.Stock -= quantity;

            // 2. STRUCTURAL PATTERN: COMPOSITE ALTYAPISI (Parça-Bütün İlişkisi)
            // Karmaşık ürünlerin içindeki alt bileşenlerin (Leaf) ağırlıkları iteratif olarak ana ürüne (Composite) eklenir.
            double totalWeightPerUnit = dbProduct.Weight;

            if (dbProduct.ProductType == "Karmasik" && !string.IsNullOrEmpty(dbProduct.Components))
            {
                var componentNames = dbProduct.Components.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Select(c => c.Trim()).ToList();
                foreach (var cName in componentNames)
                {
                    var compProduct = _context.Products.FirstOrDefault(p => p.Name.ToLower() == cName.ToLower());
                    if (compProduct != null && compProduct.Stock >= quantity)
                    {
                        compProduct.Stock -= quantity; // Alt bileşenin de stoğu düşülür
                        totalWeightPerUnit += compProduct.Weight;
                    }
                }
            }

            double totalOrderWeight = totalWeightPerUnit * quantity;
            _context.SaveChanges();

            // 3. STRUCTURAL PATTERNS: ADAPTER (Uyumlaştırıcı) & DECORATOR (Sarmalayıcı)
            ICargoService cargo;

            // Adapter: Farklı kargo firmalarının (Adaptee) uyumsuz metotlarını ortak bir arayüzde (ICargoService - Target) standartlaştırır.
            if (cargoCompany == "Aras") cargo = new ArasCargoAdapter();
            else cargo = new YurticiCargoAdapter();

            // Decorator: Kalıtım (Inheritance) kullanmadan, çalışma zamanında (Runtime) nesneye "Sigorta" özelliği sarmalanarak eklenir.
            if (useInsurance) cargo = new InsuranceDecorator(cargo);

            // Fiyat hesaplaması dinamik sarmalayıcı zincirinden geçerek sonuçlanır.
            decimal cargoCost = cargo.CalculatePrice(totalOrderWeight, distance);
            finalPrice = (dbProduct.Price * quantity) + cargoCost;

            // 4. BEHAVIORAL PATTERN: STRATEGY (Strateji)
            // Bağlam (Context) nesnesi oluşturulur. İstemcinin seçimine göre ödeme algoritması çalışma zamanında değiştirilir.
            OrderContext orderContext = new OrderContext { OrderId = new Random().Next(1000, 9999), TotalAmount = finalPrice };

            if (paymentMethod == "CreditCard") orderContext.SetPaymentStrategy(new CreditCardPayment("4321-xxxx", "123"));
            else orderContext.SetPaymentStrategy(new BankTransferPayment("TR00..."));

            orderContext.ProcessPayment(); // Seçilen strateji (algoritma) polimorfik olarak çalıştırılır.

            // --- Veritabanı Kayıt İşlemleri (Persistence) ---
            var newOrder = new OrderRecord
            {
                CustomerEmail = User.Identity.Name,
                ProductName = productName,
                Quantity = quantity,
                TotalPrice = finalPrice,
                Status = "Beklemede",
                OrderDate = DateTime.Now
            };
            _context.Orders.Add(newOrder);
            _context.SaveChanges();

            // View (Arayüz) katmanına veri taşıma işlemi
            ViewBag.FinalPrice = finalPrice;
            ViewBag.Order = orderContext;
            ViewBag.ProductName = productName;
            ViewBag.Quantity = quantity;
            ViewBag.CargoInfo = $"Firma: {cargoCompany} | Takip: {cargo.GenerateTrackingNumber()} | Kargo Ücreti: {cargoCost:F2} TL (Ağırlık: {totalOrderWeight} KG, Mesafe: {distance} KM)";

            // 5. CREATIONAL PATTERN: SINGLETON
            // Thread-safe Singleton üzerinden bellek sızıntısı yaratmadan güvenli loglama işlemi yapılır.
            SystemLogger.GetInstance().LogToDb(HttpContext.RequestServices, $"SİPARİŞ ONAYLANDI: {productName} x {quantity}. Toplam Tutar: {finalPrice} TL. Kargo: {cargoCompany}", User.Identity.Name ?? "System");

            return View("OrderResult");
        }

        [Authorize(Roles = "Musteri, Admin")]
        [HttpPost]
        public IActionResult CancelMyOrder(int orderId)
        {
            var dbOrder = _context.Orders.FirstOrDefault(o => o.Id == orderId);
            if (dbOrder != null)
            {
                try
                {
                    // State tasarımı öncesi basit durum kontrolü. (Gerçek state entegrasyonu Kurye paneli ve detaylarında çalışır).
                    if (dbOrder.Status == "Kargoda" || dbOrder.Status == "Teslim Edildi")
                    {
                        throw new InvalidOperationException("Kargodaki ürün iptal edilemez, sadece iade sürecine geçebilirsiniz!");
                    }

                    dbOrder.Status = "İptal Edildi";
                    _context.SaveChanges();
                    TempData["SuccessMessage"] = "Siparişiniz başarıyla iptal edildi.";

                    SystemLogger.GetInstance().LogToDb(HttpContext.RequestServices, $"SİPARİŞ İPTAL EDİLDİ: #{dbOrder.Id}", User.Identity.Name ?? "System");
                }
                catch (Exception ex)
                {
                    TempData["ErrorMessage"] = ex.Message;
                }
            }
            return RedirectToAction("MyOrders");
        }

        // ========================================================
        // 2. DEPO İŞLEMLERİ
        // ========================================================

        [Authorize(Roles = "DepoGorevlisi, Admin")]
        public IActionResult StokYonetimi()
        {
            ViewBag.Products = _context.Products.ToList();
            ViewBag.SimpleProducts = _context.Products.Where(p => p.ProductType == "Basit").ToList();
            return View();
        }

        [Authorize(Roles = "DepoGorevlisi, Admin")]
        [HttpPost]
        public IActionResult AddNewProduct(string productType, string productName, int stock, decimal price, double weight, List<string> selectedComponents)
        {
            // 6. CREATIONAL PATTERN: FACTORY METHOD
            // İstemci (Controller), ürün nesnesinin nasıl üretileceğini bilmez. Sorumluluk Factory sınıfına devredilir (SoC).
            ProductFactory factory = new ProductFactory();
            IProduct logicalProduct = factory.CreateProduct(productType);

            string componentsString = (selectedComponents != null && selectedComponents.Any())
                                      ? string.Join(",", selectedComponents)
                                      : "";

            var newEntity = new ProductEntity
            {
                Name = productName,
                ProductType = productType,
                Stock = stock,
                Price = price,
                Weight = weight,
                Components = componentsString ?? ""
            };

            _context.Products.Add(newEntity);
            _context.SaveChanges();

            SystemLogger.GetInstance().LogToDb(HttpContext.RequestServices, $"YENİ ÜRÜN EKLENDİ: {productName} (Tip: {productType}, Stok: {stock}, Fiyat: {price} TL)", User.Identity.Name ?? "System");

            return RedirectToAction("StokYonetimi");
        }

        [Authorize(Roles = "DepoGorevlisi, Admin")]
        [HttpPost]
        public IActionResult IncreaseStock(string productName, int amount)
        {
            var dbProduct = _context.Products.FirstOrDefault(p => p.Name == productName);
            if (dbProduct != null)
            {
                dbProduct.Stock += amount;
                _context.SaveChanges();
            }

            SystemLogger.GetInstance().LogToDb(HttpContext.RequestServices, $"STOK ARTTIRILDI: {productName} (+{amount} adet)", User.Identity.Name ?? "System");

            return RedirectToAction("StokYonetimi");
        }

        // ========================================================
        // 3. KURYE İŞLEMLERİ
        // ========================================================

        [Authorize(Roles = "Kurye, Admin")]
        public IActionResult KargoDurumu()
        {
            var allOrders = _context.Orders.OrderByDescending(o => o.OrderDate).ToList();
            return View(allOrders);
        }

        [Authorize(Roles = "Kurye, Admin")]
        [HttpPost]
        public IActionResult UpdateOrderStatus(int orderId, string newStatus)
        {
            var dbOrder = _context.Orders.FirstOrDefault(o => o.Id == orderId);

            if (dbOrder != null)
            {
                string oldStatus = dbOrder.Status;
                dbOrder.Status = newStatus;

                _context.SaveChanges();

                // Sistem genelindeki tek log nesnesini çağır ve veritabanına yaz.
                SystemLogger.GetInstance().LogToDb(HttpContext.RequestServices, $"Sipariş #{dbOrder.Id} durumu güncellendi: '{oldStatus}' -> '{newStatus}'", User.Identity.Name ?? "System");
            }

            return RedirectToAction("KargoDurumu");
        }
    }
}