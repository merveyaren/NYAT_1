using NYAT_1.Core.Interfaces;
using NYAT_1.Models;
using System;
using System.Collections.Generic;

namespace NYAT_1.Patterns.Creational.Factory
{
    // Creational Pattern (Yaratımsal Desen): Nesne üretim mantığını istemci (Client) kodundan soyutlar.
    public class ProductFactory
    {
        // OCP (Open/Closed Principle) ilkesine uymak ve "if-else / switch-case" karmaşasını önlemek için 
        // Dictionary (Sözlük) tabanlı dinamik bir Factory haritası (Registry) kullanılmıştır.
        private readonly Dictionary<string, Func<IProduct>> _productCreators;

        public ProductFactory()
        {
            // Yeni bir ürün tipi eklendiğinde mevcut metotları değiştirmek yerine sadece buraya kayıt atılır.
            _productCreators = new Dictionary<string, Func<IProduct>>(StringComparer.OrdinalIgnoreCase)
            {
                { "Basit", () => new BasitUrun() },
                { "Karmasik", () => new KarmasikUrun() }
            };
        }

        // İstemcinin (örneğin Controller'ın) nesnenin nasıl üretildiğini bilmeden sadece arayüz (IProduct) talep ettiği metot.
        public IProduct CreateProduct(string productType)
        {
            // Eğer talep edilen tip sözlükte kayıtlıysa, nesne doğrudan bellekten (delegate aracılığıyla) anında üretilir.
            if (_productCreators.TryGetValue(productType, out var creator))
            {
                return creator();
            }

            // Desteklenmeyen bir tip istenirse sistemin çökmesini engellemek için kontrollü hata fırlatılır.
            throw new ArgumentException($"Hata: '{productType}' geçersiz bir ürün tipidir!");
        }
    }
}