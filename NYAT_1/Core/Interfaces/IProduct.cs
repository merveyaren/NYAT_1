namespace NYAT_1.Core.Interfaces
{
    public interface IProduct
    {
        string Name { get; set; }
        decimal GetPrice();
        int GetStock();
    }
}