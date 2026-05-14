using NYAT_1.Core.Interfaces;

namespace NYAT_1.Patterns.Structural.Decorators
{
    // Base Decorator (Temel Sarmalayıcı): Decorator deseninin merkez sınıfı.
    // İş mantığını statik alt sınıflar (Kalıtım/Inheritance) ile şişirmek yerine, nesnelere çalışma zamanında 
    // (runtime) dinamik özellikler eklemek (Composition/Birleştirme) için kullanılır.
    public abstract class CargoDecorator : ICargoService
    {
        // Kapsüllenmiş (Wrapped) asıl bileşen. 
        // ICargoService arayüzünü uygulayan herhangi bir kargo sınıfını (Aras, Yurtici vb.) tutabilir.
        protected ICargoService _cargoService;

        public CargoDecorator(ICargoService cargoService)
        {
            _cargoService = cargoService;
        }

        // Delegation (Temsil): Metot çağrıları varsayılan olarak sarmalanan asıl nesneye iletilir.
        public virtual decimal CalculatePrice(double weight, double distance)
        {
            return _cargoService.CalculatePrice(weight, distance);
        }

        public virtual string GenerateTrackingNumber()
        {
            return _cargoService.GenerateTrackingNumber();
        }
    }
}