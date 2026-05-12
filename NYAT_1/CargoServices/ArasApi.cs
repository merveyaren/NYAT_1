using System;

namespace NYAT_1.CargoServices
{
    // Aras Kargo'nun kendi sistemindeki API'si (Bunu değiştiremeyiz, adamların kendi kodu)
    public class ArasApi
    {
        public string GetArasBarcode()
        {
            return "ARS-" + Guid.NewGuid().ToString().Substring(0, 8).ToUpper();
        }

        public decimal ComputeCost(double kg, double km)
        {
            return (decimal)((kg * 1.5) + (km * 0.5));
        }
    }
}