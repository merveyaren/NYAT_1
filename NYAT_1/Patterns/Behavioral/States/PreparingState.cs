using System;
using NYAT_1.Core.Interfaces;
using NYAT_1.Models;

namespace NYAT_1.Patterns.Behavioral.States
{
    // Concrete State (Somut Durum): Siparişin onaylandığı ve depoda "Hazırlanıyor" olduğu durum.
    // State deseni sayesinde, "hazırlık aşamasındaki sipariş iptal edilemez" kuralı burada kapsüllenmiştir (Encapsulation).
    public class PreparingState : IOrderState
    {
        public void Ship(OrderContext context)
        {
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine("📦 Sipariş başarıyla kargoya verildi.");
            Console.ResetColor();

            // Durum Geçişi: Hazırlanıyor -> Kargolandı
            context.SetState(new ShippedState());
        }

        // Şartname Kuralı: İptal engellemesi polimorfik olarak çözülmüştür.
        public void Cancel(OrderContext context) => throw new InvalidOperationException("Hazırlanan sipariş iptal edilemez, kargoyu bekleyip iade edin.");
        public void Approve(OrderContext context) => throw new InvalidOperationException("Sipariş zaten onaylandı ve hazırlanıyor!");
        public void Prepare(OrderContext context) => throw new InvalidOperationException("Zaten hazırlanıyor!");
        public void ReturnProduct(OrderContext context) => throw new InvalidOperationException("Ürün kargolanmadan iade edilemez!");
    }
}