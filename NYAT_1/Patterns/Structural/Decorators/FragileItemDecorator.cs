using NYAT_1.Core.Interfaces;

namespace NYAT_1.Patterns.Structural.Decorators
{
    public class FragileItemDecorator : CargoDecorator
    {
        public FragileItemDecorator(ICargoService cargoService) : base(cargoService)
        {
        }

        public override decimal CalculatePrice(double weight, double distance)
        {
            decimal basePrice = base.CalculatePrice(weight, distance);

            // Kırılacak eşya için %10 ekstra ambalaj ücreti ekleyelim
            return basePrice + (basePrice * 0.10m);
        }
    }
}