using System;

namespace PalukanuSkaiciuokle
{
    internal class Payment
    {
        public decimal MonthlyPayment { get; set; }
        public int Number { get; set; }
        public decimal Interest { get; set; }
        public decimal CreditCovered { get; set; }
        public DateTime PaymentDate { get; set; }
        public decimal PaymentLeft { get; set; }
        public decimal AmountLeftAfterPayment { get; set; }

        public override string ToString()
        {
            return $"{Number};{PaymentDate.ToString("yyyy-MM-dd")};{decimal.Round(PaymentLeft, 2)};{decimal.Round(CreditCovered, 2)};{decimal.Round(Interest, 2)};{decimal.Round(MonthlyPayment)};{decimal.Round(AmountLeftAfterPayment, 2)}";
        }
    }
}