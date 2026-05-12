using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using NYAT_1.Models; // Bunu eklemeyi unutma

namespace NYAT_1.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // VERİTABANINA EKLENECEK YENİ TABLOMUZ
        public DbSet<ProductEntity> Products { get; set; }
        // Products tablosunun hemen altına bunu ekle:
        public DbSet<OrderRecord> Orders { get; set; }
        public DbSet<Loglar> Logs { get; set; }
    }
}