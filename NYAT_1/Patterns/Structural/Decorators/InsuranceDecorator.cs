using NYAT_1.Core.Interfaces;

namespace NYAT_1.Patterns.Structural.Decorators
{
    // Concrete Decorator (Somut Sarmalayıcı): Siparişin kargo sürecine "Sigortalı Taşıma" yeteneği kazandırır.
    public class InsuranceDecorator : CargoDecorator
    {
        public InsuranceDecorator(ICargoService cargoService) : base(cargoService)
        {
        }

        public override decimal CalculatePrice(double weight, double distance)
        {
            // Delegation ile içteki (sarmalanan) bileşenin fiyatını hesaplatıyoruz.
            decimal basePrice = base.CalculatePrice(weight, distance);

            // Hesaplanan mevcut fiyata 50 TL sabit sigorta işlem bedelini ekliyoruz.
            return basePrice + 50m;
        }

        public override string GenerateTrackingNumber()
        {
            // Decorator sadece matematiksel hesaplama yapmaz; aynı zamanda string metodolojilerini de 
            // ezip (override), mevcut davranışın yapısını (örneğin kargo takip kodunu) modifiye edebilir.
            return base.GenerateTrackingNumber() + "-INS"; // INS: Insurance (Sigorta) takısı
        }
    }
}