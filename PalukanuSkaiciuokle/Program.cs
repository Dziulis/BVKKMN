using System;

namespace PalukanuSkaiciuokle
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            try
            {
                var arguments = new ArgumentParser(args);
                var processor = new PaymentProcessor(arguments);
                processor.PrintPayments();
                Console.WriteLine();
                Console.WriteLine(processor.PayedTotal);
            }
            catch (ArgumentOutOfRangeException)
            {
                Console.WriteLine("The payment date entered is invalid and must be between 1 to 31");
            }
            catch (Exception)
            {
                Console.WriteLine("The entered program parameters are invalid");
            }
            Console.ReadKey();
        }
    }
}