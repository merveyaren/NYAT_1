namespace NYAT_1.Patterns.Behavioral.Observer
{
    // Dinleyici (Abone) arayüzümüz
    public interface IStockObserver
    {
        void Update(string productName, int currentStock);
    }
}