using Microsoft.AspNetCore.Mvc;
using NYAT_1.Models;
using NYAT_1.Data;
using NYAT_1.Core.Interfaces;
using NYAT_1.Patterns.Creational.Factory;
using NYAT_1.Patterns.Behavioral.Strategies;
using NYAT_1.Patterns.Structural.Adapters;
using NYAT_1.Patterns.Structural.Decorators;
using NYAT_1.Patterns.Behavioral.Observer;
using Microsoft.AspNetCore.Authorization;
using System.Collections.Generic;
using System.Linq;
using System;

namespace NYAT_1.Controllers
{
    public class OrderController : Controller
    {
        private readonly ApplicationDbContext _context;

        public OrderController(ApplicationDbContext context)
        {
            _context = context;
        }

        // --------------------------------------------------------
        // 1. MÜŞTERİ İŞLEMLERİ
        // --------------------------------------------------------

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
        public IActionResult ProcessOrder(string productName, int quantity, string paymentMethod, string cargoCompany, bool useInsurance)
        {
            var dbProduct = _context.Products.FirstOrDefault(p => p.Name == productName);

            if (dbProduct != null && dbProduct.Stock >= quantity)
            {
                // DESEN 1: OBSERVER - Stok takibi (Eşik: 5)
                ProductStockManager stockManager = new ProductStockManager(dbProduct.Name, dbProduct.Stock, 5);
                stockManager.Attach(new SatinAlmaEmailNotifier());
                stockManager.Attach(new DepoNotifier());

                stockManager.DecreaseStock(quantity);
                dbProduct.Stock -= quantity;

                // DESEN 2: COMPOSITE - Karmaşık Ürün Alt Parça Düşümü (Dinamik)
                if (dbProduct.ProductType == "Karmasik" && !string.IsNullOrEmpty(dbProduct.Components))
                {
                    var componentNames = dbProduct.Components
                        .Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                        .Select(c => c.Trim())
                        .ToList();

                    foreach (var cName in componentNames)
                    {
                        var compProduct = _context.Products.FirstOrDefault(p => p.Name.ToLower() == cName.ToLower());
                        if (compProduct != null && compProduct.Stock >= quantity)
                        {
                            compProduct.Stock -= quantity;
                        }
                    }
                }
                _context.SaveChanges();
            }

            // DESEN 3: STRATEGY - Ödeme Yöntemi
            OrderContext orderContext = new OrderContext { OrderId = new Random().Next(1000, 9999), TotalAmount = 500 };
            if (paymentMethod == "CreditCard")
                orderContext.SetPaymentStrategy(new CreditCardPayment("4321-xxxx", "123"));
            else
                orderContext.SetPaymentStrategy(new BankTransferPayment("TR00..."));

            orderContext.ProcessPayment();

            // DESEN 4: ADAPTER - Kargo Firması Seçimi (Senin API sınıflarınla çalışan kısım)
            ICargoService cargo;
            if (cargoCompany == "Aras")
                cargo = new ArasCargoAdapter();
            else
                cargo = new YurticiCargoAdapter();

            // DESEN 5: DECORATOR - Kargo Sigortası
            if (useInsurance)
                cargo = new InsuranceDecorator(cargo);

            // SİPARİŞİ VERİTABANINA KAYDETME (State başlangıcı: Beklemede)
            var newOrder = new OrderRecord
            {
                CustomerEmail = User.Identity.Name,
                ProductName = productName,
                Quantity = quantity,
                TotalPrice = 500,
                Status = "Beklemede",
                OrderDate = DateTime.Now
            };
            _context.Orders.Add(newOrder);
            _context.SaveChanges();

            // Sonuç ekranı için veriler
            ViewBag.Order = orderContext;
            ViewBag.ProductName = productName;
            ViewBag.Quantity = quantity;
            ViewBag.CargoInfo = $"Firma: {cargoCompany} | Takip: {cargo.GenerateTrackingNumber()} | Ücret: {cargo.CalculatePrice(2.0, 100)} TL";

            return View("OrderResult");
        }

        [Authorize(Roles = "Musteri, Admin")]
        [HttpPost]
        public IActionResult CancelMyOrder(int orderId)
        {
            var dbOrder = _context.Orders.FirstOrDefault(o => o.Id == orderId);
            if (dbOrder != null)
            {
                // DESEN: STATE (Durum) Simülasyonu
                try
                {
                    if (dbOrder.Status == "Kargoda" || dbOrder.Status == "Teslim Edildi")
                    {
                        throw new InvalidOperationException("Kargodaki ürün iptal edilemez, sadece iade sürecine geçebilirsiniz!");
                    }

                    dbOrder.Status = "İptal Edildi";
                    _context.SaveChanges();
                    TempData["SuccessMessage"] = "Siparişiniz başarıyla iptal edildi.";
                }
                catch (Exception ex)
                {
                    TempData["ErrorMessage"] = ex.Message;
                }
            }
            return RedirectToAction("MyOrders");
        }

        // --------------------------------------------------------
        // 2. DEPO İŞLEMLERİ
        // --------------------------------------------------------

        [Authorize(Roles = "DepoGorevlisi, Admin")]
        public IActionResult StokYonetimi()
        {
            ViewBag.Products = _context.Products.ToList();
            ViewBag.SimpleProducts = _context.Products.Where(p => p.ProductType == "Basit").ToList();
            return View();
        }

        [Authorize(Roles = "DepoGorevlisi, Admin")]
        [HttpPost]
        public IActionResult AddNewProduct(string productType, string productName, int stock, decimal price, List<string> selectedComponents)
        {
            // DESEN 6: FACTORY METHOD
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
                Components = componentsString
            };

            _context.Products.Add(newEntity);
            _context.SaveChanges();

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
            return RedirectToAction("StokYonetimi");
        }

        // --------------------------------------------------------
        // 3. KURYE İŞLEMLERİ
        // --------------------------------------------------------

        [Authorize(Roles = "Kurye, Admin")]
        public IActionResult KargoDurumu()
        {
            return View();
        }
    }
}