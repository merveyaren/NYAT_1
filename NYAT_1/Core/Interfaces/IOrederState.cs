using NYAT_1.Models;

namespace NYAT_1.Core.Interfaces
{
    // State Interface (Durum Arayüzü): Siparişin yaşam döngüsündeki her bir adımın (Beklemede, Kargolandı vb.) 
    // cevap verebileceği durum geçiş olayları

    // nesnenin bulunduğu anki duruma göre farklı davranışlar sergilemesini (Polimorfizm) sağlar.
    public interface IOrderState
    {
        void Approve(OrderContext context);         // Siparişi Onayla
        void Prepare(OrderContext context);         // Depoda Hazırla
        void Ship(OrderContext context);            // Kargoya Ver
        void Cancel(OrderContext context);          // İptal Et
        void ReturnProduct(OrderContext context);   // İade Et
    }
}