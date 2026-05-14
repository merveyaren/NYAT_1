using System;
using NYAT_1.Core.Interfaces;
using NYAT_1.Models;

namespace NYAT_1.Patterns.Behavioral.States
{
    // Concrete State (Somut Durum): Siparişin ilk oluşturulduğu, henüz onaylanmadığı başlangıç durumu.
    // İş kuralları gereği sadece onaylanabilir veya iptal edilebilir. Diğer işlemler Exception fırlatır.
    public class PendingState : IOrderState
    {
        public void Approve(OrderContext context)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("✅ Sipariş onaylandı. Durum 'Onaylandı' olarak güncelleniyor.");
            Console.ResetColor();

            // State Geçişi (State Transition): Context'in mevcut durumu ApprovedState olarak değiştirilir.
            context.SetState(new ApprovedState());
        }

        public void Cancel(OrderContext context)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("❌ Sipariş henüz onaylanmamıştı, başarıyla iptal edildi.");
            Console.ResetColor();
            // Burada durum CancelledState yapılarak geçiş sağlanabilir.
        }

        // Aşağıdaki işlemler bu state için geçersizdir. if-else bloklarına gerek kalmadan direkt engellenir.
        public void Prepare(OrderContext context) => throw new InvalidOperationException("Sipariş onaylanmadan hazırlanamaz!");
        public void Ship(OrderContext context) => throw new InvalidOperationException("Sipariş onaylanmadan kargolanamaz!");
        public void ReturnProduct(OrderContext context) => throw new InvalidOperationException("Müşteride olmayan ürün iade edilemez!");
    }
}