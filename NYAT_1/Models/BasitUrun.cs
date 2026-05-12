using NYAT_1.Core.Interfaces;

namespace NYAT_1.Models
{
    public class BasitUrun : IProduct
    {
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int Stock { get; set; }

        public decimal GetPrice() => Price;
        public int GetStock() => Stock;
    }
}