using NYAT_1.Core.Interfaces;
using NYAT_1.Models;
using System;

namespace NYAT_1.Patterns.Behavioral.States
{
    public class ApprovedState : IOrderState
    {
        public void Prepare(OrderContext context)
        {
            Console.WriteLine("Sipariş hazırlanıyor...");
            context.SetState(new PreparingState());
        }

        public void Cancel(OrderContext context)
        {
            Console.WriteLine("Onaylanan sipariş iptal edildi.");
            // İptal durumuna geçilebilir
        }

        public void Approve(OrderContext context) => throw new InvalidOperationException("Sipariş zaten onaylı!");
        public void Ship(OrderContext context) => throw new InvalidOperationException("Önce hazırlanması gerekiyor!");
        public void ReturnProduct(OrderContext context) => throw new InvalidOperationException("Müşteriye gitmeyen ürün iade edilemez!");
    }
}