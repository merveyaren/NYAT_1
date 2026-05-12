using System;

namespace NYAT_1.CargoServices
{
    // Yurtiçi Kargo'nun API'si (Metot isimleri ve parametreleri Aras'tan tamamen farklı)
    public class YurticiApi
    {
        public string CreateYurticiTracking()
        {
            return "YRT-" + DateTime.Now.Ticks.ToString();
        }

        public decimal GetPrice(double weight)
        {
            // Yurtiçi fiyatlamada sadece ağırlığı baz alıyor diyelim
            return (decimal)(weight * 2.5 + 20);
        }
    }
}