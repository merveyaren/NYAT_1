using System;

namespace NYAT_1.Patterns.Behavioral.Observer
{
    // ConcreteObserver (Somut Gözlemci): Stok azaldığında Depo sorumlusuna sistem içi alert (bildirim) gönderir.
    public class DepoNotifier : IStockObserver
    {
        public void Update(string productName, int currentStock)
        {
            // Sistem bildirimi simülasyonu (Konsolda uyarı niteliği taşıması için Sarı renk kullanıldı).
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"🔔 [SİSTEM BİLDİRİMİ] Depo Sorumlusuna: {productName} ürünü bitmek üzere (Kalan: {currentStock}). Lütfen ön rafları kontrol edin.");
            Console.ResetColor();
        }
    }
}