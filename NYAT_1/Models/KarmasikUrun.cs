using NYAT_1.Core.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace NYAT_1.Models
{
    public class KarmasikUrun : IProduct
    {
        public string Name { get; set; }

        // İçindeki alt parçaları tutan liste
        private List<IProduct> _subProducts = new List<IProduct>();

        public void AddPart(IProduct product)
        {
            _subProducts.Add(product);
        }

        public decimal GetPrice()
        {
            // İçindeki tüm parçaların fiyatını toplayarak kendi fiyatını bulur
            return _subProducts.Sum(p => p.GetPrice());
        }

        public int GetStock()
        {
            // Montajlı bir ürünün stoğu, içindeki en az stoğa sahip parçaya bağlıdır
            return _subProducts.Any() ? _subProducts.Min(p => p.GetStock()) : 0;
        }
    }
}