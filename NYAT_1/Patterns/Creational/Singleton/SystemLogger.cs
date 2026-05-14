using Microsoft.Extensions.DependencyInjection;
using NYAT_1.Data;
using NYAT_1.Models;
using System;

namespace NYAT_1.Patterns.Creational.Singleton
{
    // Creational Pattern: Uygulama yaşam döngüsü boyunca bellekte yalnızca tek bir örneğin (Instance) bulunmasını garanti eder.
    // sealed anahtar kelimesi ile sınıftan miras alınması engellenerek Singleton yapısı güvenceye alınmıştır.
    public sealed class SystemLogger
    {
        private static SystemLogger _instance;
        private static readonly object _lock = new object(); // Asenkron işlemlerde kilit mekanizması için kullanılır.

        // Constructor private yapılarak dışarıdan "new SystemLogger()" ile nesne üretilmesi engellenmiştir.
        private SystemLogger() { }

        // Sisteme global erişim noktası (Global Access Point).
        public static SystemLogger GetInstance()
        {
            // Çoklu iş parçacığı (Multi-threading) ortamlarında çakışmayı ve birden fazla nesne üretimini önlemek için 
            // literatürdeki "Double-Checked Locking" (Çift Kontrollü Kilitleme) algoritması kullanılmıştır.
            if (_instance == null)
            {
                lock (_lock)
                {
                    if (_instance == null)
                        _instance = new SystemLogger();
                }
            }
            return _instance;
        }

        // Singleton nesnesi üzerinden veritabanına log yazma işlemi.
        public void LogToDb(IServiceProvider serviceProvider, string message, string userEmail = "System")
        {
            // Anti-Pattern tuzağından kaçış: Singleton (uygulama ömürlü) bir sınıfın içine, Scoped (istek ömürlü) 
            // olan DbContext doğrudan enjekte edilemez. Bu yüzden IServiceProvider ile anlık bir "Scope" oluşturularak 
            // bellek sızıntısı (Memory Leak) olmadan güvenli bir veritabanı erişimi sağlanmıştır.
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