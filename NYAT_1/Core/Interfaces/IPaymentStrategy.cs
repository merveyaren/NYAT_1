using System;

namespace NYAT_1.Core.Interfaces
{
    // Strategy Interface (Strateji Arayüzü): Farklı ödeme algoritmalarının (Kredi Kartı, Havale, Kripto vb.) 
    // ortak bir imzaya (metot yapısına) sahip olmasını sağlar
    // istediği ödeme algoritmasını seçip polimorfik olarak çalıştırabilir.
    public interface IPaymentStrategy
    {
        // Ödeme işlemini gerçekleştiren ve sonucunu (başarılı/başarısız) dönen metot.
        bool Pay(decimal amount);
    }
}