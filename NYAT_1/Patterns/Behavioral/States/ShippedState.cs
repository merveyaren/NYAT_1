using System;
using NYAT_1.Core.Interfaces;
using NYAT_1.Models;

namespace NYAT_1.Patterns.Behavioral.States
{
    public class ShippedState : IOrderState
    {
        public void Cancel(OrderContext context)
        {
            // ŞARTNAME KURALI UYGULANIYOR: if-else kullanmadan polimorfik olarak çözdük.
            throw new InvalidOperationException("HATA: Kargoya verilmiş bir sipariş iptal edilemez! Lütfen ürün size ulaşınca iade talebi oluşturun.");
        }

        public void ReturnProduct(OrderContext context)
        {
            Console.WriteLine("İade süreci başlatıldı. Durum 'İade Edildi' olarak güncelleniyor.");
            context.SetState(new ReturnedState()); // Durum geçişi
        }

        public void Approve(OrderContext context) => throw new InvalidOperationException("Ürün zaten kargoda!");
        public void Prepare(OrderContext context) => throw new InvalidOperationException("Ürün çoktan hazırlandı ve kargoda!");
        public void Ship(OrderContext context) => throw new InvalidOperationException("Ürün zaten kargoya verilmiş durumda!");
    }
}