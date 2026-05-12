using NYAT_1.Core.Interfaces;
using NYAT_1.Models;
using System;
using System.Collections.Generic;

namespace NYAT_1.Patterns.Creational.Factory
{
    public class ProductFactory
    {
        // Şartnamedeki "if-else kullanmayın" kuralını aşmak için
        // Tipi (string) alıp, geriye IProduct üreten bir metot (Func) döndüren Sözlük kullanıyoruz.
        private readonly Dictionary<string, Func<IProduct>> _productCreators;

        public ProductFactory()
        {
            _productCreators = new Dictionary<string, Func<IProduct>>(StringComparer.OrdinalIgnoreCase)
            {
                { "Basit", () => new BasitUrun() },
                { "Karmasik", () => new KarmasikUrun() }
            };
        }

        public IProduct CreateProduct(string productType)
        {
            // Eğer sözlükte bu tip varsa (örn: "Basit"), if-else yapmadan direkt üret!
            if (_productCreators.TryGetValue(productType, out var creator))
            {
                return creator();
            }

            throw new ArgumentException("Hata: Geçersiz ürün tipi istendi!");
        }
    }
}