namespace NYAT_1.Core.Interfaces
{
    public interface ICargoService
    {
        string GenerateTrackingNumber();
        decimal CalculatePrice(double weight, double distance);
    }
}