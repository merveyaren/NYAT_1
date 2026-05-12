using NYAT_1.Core.Interfaces;
using NYAT_1.CargoServices; // ArasApi'nin bulunduğu yer

namespace NYAT_1.Patterns.Structural.Adapters
{
    public class ArasCargoAdapter : ICargoService
    {
        // 1. Adapte edeceğimiz dış sistemi içeri alıyoruz
        private readonly ArasApi _arasApi;

        public ArasCargoAdapter()
        {
            _arasApi = new ArasApi();
        }

        // 2. Kendi arayüzümüzü (ICargoService), dış sistemin metotlarına bağlıyoruz
        public string GenerateTrackingNumber()
        {
            return _arasApi.GetArasBarcode(); // Aras'ın metodunu çağırıyor
        }

        public decimal CalculatePrice(double weight, double distance)
        {
            return _arasApi.ComputeCost(weight, distance); // Aras'ın metodunu çağırıyor
        }
    }
}