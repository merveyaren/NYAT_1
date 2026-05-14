using System;
using NYAT_1.Core.Interfaces;
using NYAT_1.Models;

namespace NYAT_1.Patterns.Behavioral.States
{
    // Concrete State (Somut Durum): Siparişin onaylandığı ve depo ekipleri tarafından hazırlanmayı beklediği durum.
    public class ApprovedState : IOrderState
    {
        public void Prepare(OrderContext context)
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("Sipariş depo birimine iletildi. Durum 'Hazırlanıyor' olarak güncelleniyor.");
            Console.ResetColor();

            // State Geçişi (State Transition): Onaylandı -> Hazırlanıyor
            context.SetState(new PreparingState());
        }

        public void Cancel(OrderContext context)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Sipariş hazırlık aşamasına geçmeden önce müşteri tarafından başarıyla iptal edildi.");
            Console.ResetColor();
            // Burada durum CancelledState yapılarak iptal süreci tamamlanabilir.
        }

        // Geçersiz İşlemler: Nesnenin bulunduğu state'e uygun olmayan davranışlar if-else kontrolü 
        // gerektirmeden (polimorfik olarak) kendi içinde engellenir.
        public void Approve(OrderContext context) => throw new InvalidOperationException("Sipariş zaten onaylanmış durumda!");
        public void Ship(OrderContext context) => throw new InvalidOperationException("Sipariş hazırlanmadan doğrudan kargolanamaz!");
        public void ReturnProduct(OrderContext context) => throw new InvalidOperationException("Müşteriye ulaşmayan ürün iade edilemez!");
    }
}