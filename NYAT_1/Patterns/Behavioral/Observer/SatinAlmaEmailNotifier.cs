using System;

namespace NYAT_1.Patterns.Behavioral.Observer
{
    // ConcreteObserver (Somut Gözlemci): Stok azaldığında Satın Alma birimi için e-posta sürecini yönetir.
    public class SatinAlmaEmailNotifier : IStockObserver
    {
        public void Update(string productName, int currentStock)
        {
            // E-posta gönderim simülasyonu (Konsolda dikkat çekmesi için Mavi renk kullanıldı).
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine($"📧 [E-POSTA] Satın Alma Birimine: DİKKAT! {productName} stoğu kritik seviyede! Kalan: {currentStock}. Lütfen acil tedarik sürecini başlatın.");
            Console.ResetColor();
        }
    }
}