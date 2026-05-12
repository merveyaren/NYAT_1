using NYAT_1.Core.Interfaces;
using System;

namespace NYAT_1.Patterns.Behavioral.Strategies
{
    public class BankTransferPayment : IPaymentStrategy
    {
        private string _iban;

        public BankTransferPayment(string iban)
        {
            _iban = iban;
        }

        public bool Pay(decimal amount)
        {
            Console.WriteLine($"Havale işlemi bekleniyor. Lütfen {amount} TL tutarını şu IBAN'a gönderin: {_iban}");
            return true;
        }
    }
}