using NYAT_1.Core.Interfaces;

namespace NYAT_1.Patterns.Structural.Decorators
{
    // Concrete Decorator (Somut Sarmalayıcı): Temel kargo işlevselliğini bozmadan,
    // "Kırılacak Eşya" kuralını sisteme dinamik olarak sarmalayarak (wrap) ekler.
    // OCP (Open/Closed Principle) ilkesine sadıktır; yeni bir özellik eklerken mevcut kargo sınıflarına dokunulmaz.
    public class FragileItemDecorator : CargoDecorator
    {
        public FragileItemDecorator(ICargoService cargoService) : base(cargoService)
        {
        }

        public override decimal CalculatePrice(double weight, double distance)
        {
            // 1. Adım: Sarmalanan nesnenin (asıl kargo firmasının veya bir önceki decorator'ın) taban fiyatını al.
            decimal basePrice = base.CalculatePrice(weight, distance);

            // 2. Adım: Kendi ekstra iş mantığını (Kırılacak eşya için %10 ambalaj ücreti) taban fiyata ekle.
            return basePrice + (basePrice * 0.10m);
        }
    }
}