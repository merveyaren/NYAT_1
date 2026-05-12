using System;
using System.Collections.Generic;

namespace NYAT_1.Patterns.Behavioral.Observer
{
    public class ProductStockManager
    {
        public string ProductName { get; private set; }
        private int _stock;
        private readonly int _threshold; // Uyarı sınırımız (Eşik değer)

        // Abone listemiz (Polimorfik bir liste, arayüz tipinde tutuyoruz)
        private List<IStockObserver> _observers = new List<IStockObserver>();

        public ProductStockManager(string productName, int initialStock, int threshold)
        {
            ProductName = productName;
            _stock = initialStock;
            _threshold = threshold;
        }

        // Sisteme yeni bir bildirimci eklemek için (Subscribe)
        public void Attach(IStockObserver observer)
        {
            _observers.Add(observer);
        }

        // Stok düşürme işlemi
        public void DecreaseStock(int amount)
        {
            _stock -= amount;
            Console.WriteLine($"{ProductName} stoğundan {amount} adet düşüldü. Mevcut Stok: {_stock}");

            // Eğer stok kritik seviyenin altına düştüyse haber sal
            if (_stock < _threshold)
            {
                NotifyAllObservers();
            }
        }

        // Sadece kendi içinden çağrılan bildirim metodu
        private void NotifyAllObservers()
        {
            foreach (var observer in _observers)
            {
                // Kimin nasıl haber verdiğini bilmiyoruz, sadece Update diyoruz
                observer.Update(ProductName, _stock);
            }
        }
    }
}