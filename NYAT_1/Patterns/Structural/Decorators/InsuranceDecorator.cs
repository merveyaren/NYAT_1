using NYAT_1.Core.Interfaces;

namespace NYAT_1.Patterns.Structural.Decorators
{
    public class InsuranceDecorator : CargoDecorator
    {
        public InsuranceDecorator(ICargoService cargoService) : base(cargoService)
        {
        }

        public override decimal CalculatePrice(double weight, double distance)
        {
            // Önce sarmaladığı kargonun (örneğin Aras) kendi fiyatını hesaplatıyoruz
            decimal basePrice = base.CalculatePrice(weight, distance);

            // Sonra üzerine 50 TL sabit sigorta bedeli ekliyoruz
            return basePrice + 50m;
        }

        public override string GenerateTrackingNumber()
        {
            // Takip numarasının sonuna Sigorta (INS) etiketi ekleyebiliriz
            return base.GenerateTrackingNumber() + "-INS";
        }
    }
}