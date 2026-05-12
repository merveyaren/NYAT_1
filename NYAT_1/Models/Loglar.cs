using System;
using System.ComponentModel.DataAnnotations;

namespace NYAT_1.Models
{
    public class Loglar
    {
        [Key]
        public int Id { get; set; }
        public string Message { get; set; }
        public string UserEmail { get; set; }
        public DateTime Timestamp { get; set; }
    }
}