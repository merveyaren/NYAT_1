using NYAT_1.Core.Interfaces;
using System;

namespace NYAT_1.Patterns.Behavioral.Strategies
{
    public class CreditCardPayment : IPaymentStrategy
    {
        private string _cardNumber;
        private string _cvv;

        public CreditCardPayment(string cardNumber, string cvv)
        {
            _cardNumber = cardNumber;
            _cvv = cvv;
        }

        public bool Pay(decimal amount)
        {
            Console.WriteLine($"Kredi Kartı ({_cardNumber.Substring(0, 4)} ****) ile {amount} TL ödeme alındı.");
            // Gerçek projelerde burada banka API'sine gidilir
            return true;
        }
    }
}