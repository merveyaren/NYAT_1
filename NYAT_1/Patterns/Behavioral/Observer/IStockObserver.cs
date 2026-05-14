using System;

namespace NYAT_1.Patterns.Behavioral.Observer
{
    // Observer (Gözlemci) tasarım deseninin temel arayüzü (Subscriber/Listener).
    // Ana sistem (Subject), bu arayüzü uygulayan sınıfların iç yapısını bilmez (Loose Coupling).
    public interface IStockObserver
    {
        // Stok eşik değerin altına düştüğünde ana sistem tarafından otomatik tetiklenecek metot.
        void Update(string productName, int currentStock);
    }
}