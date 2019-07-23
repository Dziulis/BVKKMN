using System;

namespace PalukanuSkaiciuokle
{
    internal class PaymentProcessor
    {
        private decimal _totalCredit;
        private decimal _monthlyPayment;
        private decimal _leftAmount;
        private DateTime _paymentDate;
        private Payment _payment;
        private int _amountOfPayments;
        private int _payDay;
        private decimal _interest;
        private double _rates;
        public decimal PayedTotal;

        public PaymentProcessor(ArgumentParser arguments)
        {
            _totalCredit = arguments.Sum;
            _payDay = arguments.DayOfPayment;
            _rates = arguments.Interest;
            _amountOfPayments = arguments.Period;
            calculateMonthlyPayment(arguments);
            _leftAmount = arguments.Sum;
            _paymentDate = DateTime.Today;
            _payment = new Payment { MonthlyPayment = 0, PaymentDate = DateTime.Today, PaymentLeft = 0, Number = 0, Interest = 0, CreditCovered = _leftAmount, AmountLeftAfterPayment = _leftAmount };
        }

        public void PrintPayments()
        {
            Console.WriteLine(_payment.ToString());
            _payment.MonthlyPayment = _monthlyPayment;
            PayedTotal = 0;
            for (int i = 1; i <= _amountOfPayments; i++)
            {
                _payment.Number = i;
                _paymentDate = _paymentDate.Month != 12
                    ? new DateTime(_paymentDate.Year, _paymentDate.AddMonths(1).Month, _payDay)
                    : new DateTime(_paymentDate.AddYears(1).Year, _paymentDate.AddMonths(1).Month, _payDay);
                _payment.PaymentDate = _paymentDate;
                _payment.PaymentLeft = _leftAmount;
                _payment.CreditCovered = _monthlyPayment - calculateInterestCovered();
                _payment.Interest = calculateInterestCovered();
                _payment.AmountLeftAfterPayment = _leftAmount - _payment.CreditCovered;
                Console.WriteLine(_payment.ToString());
                _leftAmount = _payment.AmountLeftAfterPayment;
                PayedTotal += _monthlyPayment;
            }

            Console.WriteLine();
            Console.WriteLine("BVKKMN - " + Math.Round(Effect(Rate(_amountOfPayments, Convert.ToDouble(_monthlyPayment), Convert.ToDouble(_totalCredit * -1)) * 12, 12) * 100, 2));
        }

        private void calculateMonthlyPayment(ArgumentParser arguments)
        {
            _monthlyPayment = (arguments.Sum * Convert.ToDecimal(arguments.Interest / 12)) / (1 - Convert.ToDecimal(Math.Pow(1 + (arguments.Interest / 12), arguments.Period * -1)));
        }

        private decimal calculateInterestCovered()
        {
            return _leftAmount * Convert.ToDecimal(_rates / 12);
        }

        private double Rate(double NPer, double Pmt, double PV, double FV = 0, DueDate Due = DueDate.EndOfPeriod, double Guess = 0.1)
        {
            double dTemp;
            double dRate0;
            double dRate1;
            double dY0;
            double dY1;
            int I;

            // Check for error condition
            if (NPer <= 0.0)
                throw new ArgumentException("NPer must by greater than zero");

            dRate0 = Guess;
            dY0 = LEvalRate(dRate0, NPer, Pmt, PV, FV, Due);
            if (dY0 > 0)
                dRate1 = (dRate0 / 2);
            else
                dRate1 = (dRate0 * 2);

            dY1 = LEvalRate(dRate1, NPer, Pmt, PV, FV, Due);

            for (I = 0; I <= 39; I++)
            {
                if (dY1 == dY0)
                {
                    if (dRate1 > dRate0)
                        dRate0 = dRate0 - cnL_IT_STEP;
                    else
                        dRate0 = dRate0 - cnL_IT_STEP * (-1);
                    dY0 = LEvalRate(dRate0, NPer, Pmt, PV, FV, Due);
                    if (dY1 == dY0)
                        throw new ArgumentException("Divide by zero");
                }

                dRate0 = dRate1 - (dRate1 - dRate0) * dY1 / (dY1 - dY0);

                // Secant method of generating next approximation
                dY0 = LEvalRate(dRate0, NPer, Pmt, PV, FV, Due);
                if (Math.Abs(dY0) < cnL_IT_EPSILON)
                    return dRate0;

                dTemp = dY0;
                dY0 = dY1;
                dY1 = dTemp;
                dTemp = dRate0;
                dRate0 = dRate1;
                dRate1 = dTemp;
            }

            throw new ArgumentException("Can not calculate rate");
        }

        private double LEvalRate(double Rate, double NPer, double Pmt, double PV, double dFv, DueDate Due)
        {
            double dTemp1;
            double dTemp2;
            double dTemp3;

            if (Rate == 0.0)
                return (PV + Pmt * NPer + dFv);
            else
            {
                dTemp3 = Rate + 1.0;
                // WARSI Using the exponent operator for pow(..) in C code of LEvalRate. Still got
                // to make sure that they (pow and ^) are same for all conditions
                dTemp1 = Math.Pow(dTemp3, NPer);

                if (Due != 0)
                    dTemp2 = 1 + Rate;
                else
                    dTemp2 = 1.0;
                return (PV * dTemp1 + Pmt * dTemp2 * (dTemp1 - 1) / Rate + dFv);
            }
        }

        private double Effect(double nr, double npery)
        {
            return Math.Pow(1 + (nr / npery), npery) - 1;
        }

        private const double cnL_IT_STEP = 0.00001;
        private const double cnL_IT_EPSILON = 0.0000001;

        private enum DueDate
        {
            EndOfPeriod = 0,
            BegOfPeriod = 1
        }
    }
}