using System;
using System.ComponentModel.DataAnnotations;

namespace NYAT_1.Models
{
    public class OrderRecord
    {
        [Key]
        public int Id { get; set; }
        public string CustomerEmail { get; set; } // Siparişi kimin verdiğini tutacağız
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public decimal TotalPrice { get; set; }
        public string Status { get; set; } // Beklemede, Hazırlanıyor, Kargoda, Teslim Edildi...
        public DateTime OrderDate { get; set; }
    }
}