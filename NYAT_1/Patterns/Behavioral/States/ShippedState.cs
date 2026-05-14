using System;
using NYAT_1.Core.Interfaces;
using NYAT_1.Models;

namespace NYAT_1.Patterns.Behavioral.States
{
    // Concrete State (Somut Durum): Siparişin müşteriye doğru yola çıktığı durum.
    public class ShippedState : IOrderState
    {
        public void Cancel(OrderContext context)
        {
            // KURALI: "Kargoya verilmiş sipariş iptal edilemez." 
            // Bu kritik iş kuralı, karmaşık if-else blokları yerine doğrudan sınıf davranışı olarak uygulanmıştır.
            throw new InvalidOperationException("HATA: Kargoya verilmiş bir sipariş iptal edilemez! Lütfen ürün size ulaşınca iade talebi oluşturun.");
        }

        public void ReturnProduct(OrderContext context)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("🔄 İade süreci başlatıldı. Durum 'İade Edildi' olarak güncelleniyor.");
            Console.ResetColor();

            // Durum Geçişi: Kargolandı -> İade Edildi
            context.SetState(new ReturnedState());
        }

        // Geriye dönük veya geçersiz işlemler engellenir.
        public void Approve(OrderContext context) => throw new InvalidOperationException("Ürün zaten kargoda!");
        public void Prepare(OrderContext context) => throw new InvalidOperationException("Ürün çoktan hazırlandı ve kargoda!");
        public void Ship(OrderContext context) => throw new InvalidOperationException("Ürün zaten kargoya verilmiş durumda!");
    }
}