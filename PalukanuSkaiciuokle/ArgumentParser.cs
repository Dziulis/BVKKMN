using System;

namespace PalukanuSkaiciuokle
{
    public class ArgumentParser
    {
        public decimal Sum { get; }
        public int Period { get; }
        public double Interest { get; }
        public byte DayOfPayment { get; }

        public ArgumentParser(string[] args)
        {
            if (args.Length != 4)
            {
                throw new ArgumentException();
            }

            Sum = decimal.Parse(args[0]);
            Period = int.Parse(args[1]);
            Interest = double.Parse(args[2]);
            byte tempResult;
            DayOfPayment = byte.TryParse(args[3], out tempResult)
                ? (tempResult <= 31 && tempResult > 0
                    ? tempResult
                    : throw new ArgumentOutOfRangeException())
                : throw new ArgumentException();
        }
    }
}