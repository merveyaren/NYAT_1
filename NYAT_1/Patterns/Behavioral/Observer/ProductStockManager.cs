using System;
using System.Collections.Generic;

namespace NYAT_1.Patterns.Behavioral.Observer
{
    // Subject (Yayıncı/Konu) sınıfı: Stok durumunu takip eder ve değişiklikleri abonelere bildirir.
    public class ProductStockManager
    {
        public string ProductName { get; private set; }
        private int _stock;
        private readonly int _threshold; // Otomatik aksiyonu tetikleyecek sınır (Eşik) değer.

        // Polimorfizm kullanılarak abonelerin tutulduğu liste. (Sınıflara değil, arayüze bağımlıyız).
        private List<IStockObserver> _observers = new List<IStockObserver>();

        public ProductStockManager(string productName, int initialStock, int threshold)
        {
            ProductName = productName;
            _stock = initialStock;
            _threshold = threshold;
        }

        // Sisteme yeni bir gözlemci (bildirim kanalı) eklemek için kullanılır (Attach/Subscribe).
        public void Attach(IStockObserver observer)
        {
            _observers.Add(observer);
        }

        // Sipariş geçildikçe stok miktarını düşüren iş mantığı.
        public void DecreaseStock(int amount)
        {
            _stock -= amount;
            Console.WriteLine($"{ProductName} stoğundan {amount} adet düşüldü. Mevcut Stok: {_stock}");

            // Stok kritik eşiğin altına indiyse, insan müdahalesi olmadan sistemi uyar.
            if (_stock < _threshold)
            {
                NotifyAllObservers();
            }
        }

        // Kapsülleme (Encapsulation) gereği dışarıdan çağrılamayan, sadece sınıf içi bildirim metodu.
        private void NotifyAllObservers()
        {
            foreach (var observer in _observers)
            {
                // Ana sistem, kimin e-posta kimin bildirim attığını bilmez. Sadece "Update" komutu gönderir.
                observer.Update(ProductName, _stock);
            }
        }
    }
}