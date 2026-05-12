using System;

namespace NYAT_1.Patterns.Behavioral.Observer
{
    // Depo sorumlusuna sistem içi bildirim atacak sınıf
    public class DepoNotifier : IStockObserver
    {
        public void Update(string productName, int currentStock)
        {
            Console.WriteLine($"[SİSTEM BİLDİRİMİ] Depo Sorumlusuna: {productName} ürünü azaldı (Kalan: {currentStock}). Lütfen rafları düzenleyin.");
        }
    }
}