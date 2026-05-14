using System;
using NYAT_1.Core.Interfaces;
using NYAT_1.Models;

namespace NYAT_1.Patterns.Behavioral.States
{
    // Concrete State (Somut Durum): Siparişin "İade Edildi" olduğu son durum (Terminal State).
    // Bu aşamadan sonra sipariş yaşam döngüsünü tamamlar ve üzerinde hiçbir state değişikliği yapılamaz.
    public class ReturnedState : IOrderState
    {
        public void Approve(OrderContext context) => throw new InvalidOperationException("İade edilen sipariş onaylanamaz.");
        public void Prepare(OrderContext context) => throw new InvalidOperationException("İade edilen sipariş hazırlanamaz.");
        public void Ship(OrderContext context) => throw new InvalidOperationException("İade edilen sipariş kargolanamaz.");
        public void Cancel(OrderContext context) => throw new InvalidOperationException("İade edilen sipariş iptal edilemez.");
        public void ReturnProduct(OrderContext context) => throw new InvalidOperationException("Ürün zaten iade edildi.");
    }
}