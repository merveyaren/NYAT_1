using System;
using NYAT_1.Core.Interfaces;
using NYAT_1.Models;

namespace NYAT_1.Patterns.Behavioral.States
{
    public class PendingState : IOrderState
    {
        public void Approve(OrderContext context)
        {
            Console.WriteLine("Sipariş onaylandı. Durum 'Onaylandı' olarak güncelleniyor.");
            context.SetState(new ApprovedState()); // Durum geçişi (Beklemede -> Onaylandı)
        }

        public void Cancel(OrderContext context)
        {
            Console.WriteLine("Sipariş henüz onaylanmamıştı, başarıyla iptal edildi.");
            // Burada durum CancelledState yapılabilir
        }

        public void Prepare(OrderContext context) => throw new InvalidOperationException("Sipariş onaylanmadan hazırlanamaz!");
        public void Ship(OrderContext context) => throw new InvalidOperationException("Sipariş onaylanmadan kargolanamaz!");
        public void ReturnProduct(OrderContext context) => throw new InvalidOperationException("Müşteride olmayan ürün iade edilemez!");
    }
}