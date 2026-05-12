using System.ComponentModel.DataAnnotations;

namespace NYAT_1.Models
{
    public class ProductEntity
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string ProductType { get; set; } // "Basit" veya "Karmasik"
        public int Stock { get; set; }
        public decimal Price { get; set; }
        public string Components { get; set; }
        public double Weight { get; set; }
    }
}