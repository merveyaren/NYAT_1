using System;

namespace NYAT_1.Patterns.Behavioral.Observer
{
    // Satın Alma birimine E-Posta atacak sınıf
    public class SatinAlmaEmailNotifier : IStockObserver
    {
        public void Update(string productName, int currentStock)
        {
            Console.WriteLine($"[E-POSTA] Satın Alma Birimine: DİKKAT! {productName} stoğu kritik seviyede! Kalan: {currentStock}. Lütfen yeni sipariş oluşturun.");
        }
    }
}