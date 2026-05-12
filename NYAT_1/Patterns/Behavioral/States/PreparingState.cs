using NYAT_1.Core.Interfaces;
using NYAT_1.Models;
using System;

namespace NYAT_1.Patterns.Behavioral.States
{
    public class PreparingState : IOrderState
    {
        public void Ship(OrderContext context)
        {
            Console.WriteLine("Sipariş kargoya verildi.");
            context.SetState(new ShippedState());
        }

        public void Cancel(OrderContext context) => throw new InvalidOperationException("Hazırlanan sipariş iptal edilemez, kargoyu bekleyip iade edin.");
        public void Approve(OrderContext context) => throw new InvalidOperationException("Sipariş zaten onaylandı ve hazırlanıyor!");
        public void Prepare(OrderContext context) => throw new InvalidOperationException("Zaten hazırlanıyor!");
        public void ReturnProduct(OrderContext context) => throw new InvalidOperationException("Ürün kargolanmadan iade edilemez!");
    }
}