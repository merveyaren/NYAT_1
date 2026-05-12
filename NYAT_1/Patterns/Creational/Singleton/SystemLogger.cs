using Microsoft.Extensions.DependencyInjection;
using NYAT_1.Data;
using NYAT_1.Models;
using System;

namespace NYAT_1.Patterns.Creational.Singleton
{
    public sealed class SystemLogger
    {
        private static SystemLogger _instance;
        private static readonly object _lock = new object();

        private SystemLogger() { }

        public static SystemLogger GetInstance()
        {
            if (_instance == null)
            {
                lock (_lock)
                {
                    if (_instance == null) _instance = new SystemLogger();
                }
            }
            return _instance;
        }

        // Veritabanına log yazma metodu
        public void LogToDb(IServiceProvider serviceProvider, string message, string userEmail = "System")
        {
            // Singleton içinden Scoped olan DbContext'e güvenli erişim için scope oluşturuyoruz
            using (var scope = serviceProvider.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

                var log = new Loglar
                {
                    Message = message,
                    UserEmail = userEmail,
                    Timestamp = DateTime.Now
                };

                context.Logs.Add(log);
                context.SaveChanges();
            }
        }
    }
}