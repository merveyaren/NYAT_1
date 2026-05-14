using System;

namespace NYAT_1.Core.Interfaces
{   //adapter ve decorator ın çalışmasını sağlar
    // Aras veya Yurtiçi kargo sınıflarına (Düşük seviye modül) doğrudan bağlanmaz, sadece bu arayüzü tanır.
    // Bu sayede Adapter ve Decorator desenleri bu arayüz üzerinden sistemi esnekçe genişletebilir.
    public interface ICargoService
    {
        // Kargo firmasına özgü takip numarası üretir.
        string GenerateTrackingNumber();
        // Kargonun ağırlık ve mesafeye göre fiyatını hesaplar.
        decimal CalculatePrice(double weight, double distance);

        
    }
}