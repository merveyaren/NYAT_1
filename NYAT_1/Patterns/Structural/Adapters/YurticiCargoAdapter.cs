using NYAT_1.Core.Interfaces;
using NYAT_1.CargoServices; // YurticiApi'nin bulunduğu yer

namespace NYAT_1.Patterns.Structural.Adapters
{
    public class YurticiCargoAdapter : ICargoService
    {
        private readonly YurticiApi _yurticiApi;

        public YurticiCargoAdapter()
        {
            _yurticiApi = new YurticiApi();
        }

        public string GenerateTrackingNumber()
        {
            return _yurticiApi.CreateYurticiTracking(); // Yurtiçi'nin metodunu çağırıyor
        }

        public decimal CalculatePrice(double weight, double distance)
        {
            // Yurtiçi API'si mesafe (distance) parametresi almıyor, bu yüzden sadece ağırlık yolluyoruz!
            // İşte Adaptör deseninin gücü: Eksik parametreyi tolere ettik.
            return _yurticiApi.GetPrice(weight);
        }
    }
}