using NYAT_1.Core.Interfaces;
using System;
using System.IO;

namespace NYAT_1.Patterns.Creational
{
    // Sınıfı 'sealed' yapıyoruz ki başka sınıflar bundan miras alıp kuralı bozamasın.
    public sealed class SystemLogger : ILoggerService
    {
        // 1. Kendi instance'ını tutacak statik ve gizli bir değişken
        private static SystemLogger? _instance;

        // 2. Çoklu işlemlerde (Thread-Safe) çakışmayı önlemek için kilit nesnesi
        private static readonly object _lock = new object();

        private readonly string _logFilePath;

        // 3. Constructor (Yapıcı Metot) PRIVATE olmalı! (Dışarıdan new SystemLogger() denemez)
        private SystemLogger()
        {
            // Logların kaydedileceği dosyayı belirliyoruz (Projenin ana dizininde system_logs.txt)
            _logFilePath = Path.Combine(Directory.GetCurrentDirectory(), "system_logs.txt");
        }

        // 4. Dış dünyanın bu nesneye ulaşabileceği TEK global nokta (Factory metodu gibi davranır)
        public static SystemLogger GetInstance()
        {
            // Eğer nesne daha önce üretilmediyse üret, üretildiyse var olanı ver.
            if (_instance == null)
            {
                lock (_lock) // Aynı anda iki sipariş gelirse, biri beklesin diye kilitliyoruz.
                {
                    if (_instance == null)
                    {
                        _instance = new SystemLogger();
                    }
                }
            }
            return _instance;
        }

        // Interface'den gelen metotların gövdeleri
        public void LogInfo(string message)
        {
            string logText = $"[BİLGİ] {DateTime.Now:dd/MM/yyyy HH:mm:ss} : {message}\n";
            File.AppendAllText(_logFilePath, logText);
        }

        public void LogError(string message)
        {
            string logText = $"[HATA] {DateTime.Now:dd/MM/yyyy HH:mm:ss} : {message}\n";
            File.AppendAllText(_logFilePath, logText);
        }
    }
}