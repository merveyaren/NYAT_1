using NYAT_1.Core.Interfaces;

namespace NYAT_1.Patterns.Structural.Decorators
{
    public abstract class CargoDecorator : ICargoService
    {
        // Sarmaladığımız (içine aldığımız) asıl kargo nesnesi
        protected ICargoService _cargoService;

        public CargoDecorator(ICargoService cargoService)
        {
            _cargoService = cargoService;
        }

        // Varsayılan olarak, gelen isteği içindeki nesneye iletiyor
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