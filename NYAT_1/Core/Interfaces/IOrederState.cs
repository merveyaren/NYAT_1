using NYAT_1.Models;

namespace NYAT_1.Core.Interfaces
{
    public interface IOrderState
    {
        // Sipariş üzerinde yapılabilecek tüm aksiyonlar
        void Approve(OrderContext context);
        void Prepare(OrderContext context);
        void Ship(OrderContext context);
        void Cancel(OrderContext context);
        void ReturnProduct(OrderContext context);
    }
}