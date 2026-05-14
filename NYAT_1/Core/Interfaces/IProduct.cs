namespace NYAT_1.Core.Interfaces
{
    // Product Interface, Factory Method deseni tarafından üretilen tüm nesnelerin uyması gereken ortak yapıdır.
    // İstemci (Client) kodların, somut ürün sınıflarına (BasitUrun, KarmasikUrun) sıkı sıkıya bağlanmasını engeller
    public interface IProduct
    {
        string Name { get; set; }
        decimal GetPrice();
        int GetStock();
    }
}