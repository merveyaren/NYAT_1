using NYAT_1.Core.Interfaces;
using NYAT_1.Patterns.Behavioral.States; // Durum sınıflarının namespace'i
using System;

namespace NYAT_1.Models
{
    public class OrderContext
    {
        public int OrderId { get; set; }
        public decimal TotalAmount { get; set; } // Ödenecek Toplam Tutar

        // Siparişin o anki durumu bu değişkende tutulur
        private IOrderState _currentState;

        private IPaymentStrategy _paymentStrategy;
        public OrderContext()
        {
            // Bir sipariş ilk oluştuğunda varsayılan durum 'Beklemede'dir
            _currentState = new PendingState();
        }

        // Durumu dışarıdan veya durum sınıfları içinden değiştirmek için metot
        public void SetState(IOrderState state)
        {
            _currentState = state;
            // Burada istersen Singleton Logger'ı çağırıp durum değişimini loglayabilirsin.
        }

        // Kullanıcı veya sistem bu metotları çağırdığında, 
        // Order sınıfı if-else sormaz, işi direkt o anki state sınıfına paslar.
        public void Approve() => _currentState.Approve(this);
        public void Prepare() => _currentState.Prepare(this);
        public void Ship() => _currentState.Ship(this);
        public void Cancel() => _currentState.Cancel(this);
        public void ReturnProduct() => _currentState.ReturnProduct(this);
        // Ödeme yöntemini dinamik olarak ayarlıyoruz
        public void SetPaymentStrategy(IPaymentStrategy strategy)
        {
            _paymentStrategy = strategy;
        }

        // Ödemeyi gerçekleştiriyoruz
        public void ProcessPayment()
        {
            if (_paymentStrategy == null)
            {
                throw new InvalidOperationException("Ödeme yöntemi seçilmedi!");
            }

            // HANGİ YÖNTEM SEÇİLDİYSE ONUN 'Pay' METODU ÇALIŞIR (Polimorfizm)
            bool isSuccess = _paymentStrategy.Pay(TotalAmount);

            if (isSuccess)
            {
                Console.WriteLine("Ödeme başarılı. Sipariş onaylanabilir.");
                // İstersen burada otomatik olarak Approve() çağırıp durumu değiştirebilirsin
            }
        }

    }
}