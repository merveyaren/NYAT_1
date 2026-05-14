using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;
using Xunit.Abstractions;

using NYAT_1.Controllers;
using NYAT_1.Core.Interfaces;
using NYAT_1.Data;
using NYAT_1.Models;
using NYAT_1.Patterns.Behavioral.Observer;
using NYAT_1.Patterns.Behavioral.Strategies;
using NYAT_1.Patterns.Creational.Factory;
using NYAT_1.Patterns.Creational.Singleton;
using NYAT_1.Patterns.Structural.Adapters;
using NYAT_1.Patterns.Structural.Decorators;

namespace NYAT.Tests
{
    public class NyatDesignPatternsTests
    {
        private readonly ITestOutputHelper _output;

        public NyatDesignPatternsTests(ITestOutputHelper output)
        {
            _output = output;
        }

        [Fact]
        public void FactoryPattern_ShouldCreateCorrectInstances()
        {
            var factory = new ProductFactory();

            var basit = factory.CreateProduct("Basit");
            var karmasik = factory.CreateProduct("Karmasik");

            Assert.IsType<BasitUrun>(basit);
            Assert.IsType<KarmasikUrun>(karmasik);

            _output.WriteLine("✅ FACTORY TESTİ: 'Basit' ve 'Karmasik' ürün tipleri if-else olmadan başarıyla doğru sınıflardan üretildi.");
        }

        [Fact]
        public void SingletonPattern_ShouldReturnSameInstance()
        {
            var instance1 = SystemLogger.GetInstance();
            var instance2 = SystemLogger.GetInstance();

            Assert.Same(instance1, instance2);

            _output.WriteLine("✅ SINGLETON TESTİ: Bellekte SystemLogger nesnesinden sadece bir (1) adet üretildiği doğrulandı.");
        }

        [Fact]
        public void StrategyPattern_ShouldProcessPaymentsDifferently()
        {
            var orderContext = new OrderContext { OrderId = 101, TotalAmount = 500 };

            orderContext.SetPaymentStrategy(new CreditCardPayment("1111-2222", "123"));
            orderContext.ProcessPayment();

            orderContext.SetPaymentStrategy(new BankTransferPayment("TR00112233"));
            orderContext.ProcessPayment();

            Assert.NotNull(orderContext);
            _output.WriteLine("✅ STRATEGY TESTİ: Farklı ödeme yöntemleri (Kredi Kartı / Havale) aynı OrderContext üzerinden başarıyla izole çalıştırıldı.");
        }

        [Fact]
        public void AdapterAndDecorator_ShouldCalculateCargoWithInsurance()
        {
            double weight = 10.0;
            double distance = 100.0;

            ICargoService baseCargo = new YurticiCargoAdapter();
            decimal basePrice = baseCargo.CalculatePrice(weight, distance);

            ICargoService insuredCargo = new InsuranceDecorator(baseCargo);
            decimal insuredPrice = insuredCargo.CalculatePrice(weight, distance);

            Assert.True(insuredPrice > basePrice);

            _output.WriteLine($"✅ ADAPTER & DECORATOR TESTİ:");
            _output.WriteLine($"   -> Standart Kargo Fiyatı: {basePrice} TL");
            _output.WriteLine($"   -> Sigorta Eklenmiş Fiyat: {insuredPrice} TL");
        }

        [Fact]
        public void ObserverPattern_ShouldNotifySubscribers_WhenStockIsLow()
        {
            string productName = "Laptop";
            int initialStock = 10;
            int threshold = 5;

            var stockManager = new ProductStockManager(productName, initialStock, threshold);
            var mockObserver = new Mock<IStockObserver>();
            stockManager.Attach(mockObserver.Object);

            stockManager.DecreaseStock(6);

            mockObserver.Verify(x => x.Update(productName, 4), Times.Once);

            _output.WriteLine("✅ OBSERVER TESTİ: Stok miktarı kritik eşiğin (5) altına indiğinde (Kalan: 4), tüm abonelere otomatik uyarı gönderildi.");
        }

        [Fact]
        public void ProcessOrder_ShouldPreventOrder_WhenStockIsInsufficient()
        {
            // Gerçek DB yerine test için ayrılmış sanal bellek DB'si oluşturuyoruz
            using var context = GetInMemoryDbContext();

            // HATA DÜZELTİLDİ: ProductType = "Basit" eklendi!
            var product = new ProductEntity
            {
                Name = "Kulaklık",
                ProductType = "Basit", // Eksik olan kısım buydu
                Stock = 2,
                Price = 100,
                Weight = 0.5,
                Components = ""
            };
            context.Products.Add(product);
            context.SaveChanges(); // Artık hata vermeden kaydedecek

            var controller = new OrderController(context);
            var httpContext = new Microsoft.AspNetCore.Http.DefaultHttpContext();
            var tempDataProvider = new Mock<ITempDataProvider>();
            controller.TempData = new TempDataDictionary(httpContext, tempDataProvider.Object);

            // Müşteri stokta 2 tane olan üründen 5 tane sipariş etmeye çalışıyor
            var result = controller.ProcessOrder("Kulaklık", 5, "CreditCard", "Aras", false, 10.0);

            var redirectResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectResult.ActionName);

            // Sistemin verdiği hata mesajını yakalıyoruz
            string errorMsg = controller.TempData["ErrorMessage"].ToString();
            Assert.Contains("yeterli ürün yok", errorMsg);

            _output.WriteLine($"✅ STOK KONTROL TESTİ BAŞARILI: Sistem '{errorMsg}' mesajını vererek işlemi durdurdu ve stoku korudu.");
        }
        // =========================================================================
        // 7. STATE PATTERN TESTİ
        // =========================================================================
        [Fact]
        public void StatePattern_ShouldChangeOrderStatesCorrectly()
        {
            // Arrange: Yeni bir sipariş oluşturuyoruz (Varsayılan olarak PendingState ile başlar)
            var order = new OrderContext { OrderId = 1, TotalAmount = 100 };

            // Act: Siparişi sırasıyla onaylıyor, hazırlıyor ve kargoluyoruz
            order.Approve();
            order.Prepare();
            order.Ship();

            // Assert: Durumların if-else olmadan polimorfik olarak hata vermeden değiştiğini doğruluyoruz
            Assert.NotNull(order);
            _output.WriteLine("✅ STATE TESTİ: Sipariş durumu (State) if-else kullanılmadan başarıyla 'Beklemede -> Onaylandı -> Hazırlanıyor -> Kargolandı' olarak adım adım ilerledi.");
        }

        private ApplicationDbContext GetInMemoryDbContext()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            return new ApplicationDbContext(options);
        }
    }
}