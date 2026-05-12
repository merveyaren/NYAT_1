namespace NYAT_1.Core.Interfaces
{
    public interface IPaymentStrategy
    {
        // Tüm stratejiler bu metodu kendilerine göre doldurmak zorundadır
        bool Pay(decimal amount);
    }
}