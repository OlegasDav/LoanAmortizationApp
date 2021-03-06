using Contracts.Models;
using System;

namespace LoanAmortizationApp
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("--------------------------------");
            Console.WriteLine("|KREDITAS|");
            Console.WriteLine("--------------------------------");

            while (true)
            {
                Console.Write("\nPasirinkite veiksma: \n" +
                    "1 - Paskolos grafiko skaiciuokle; \n" +
                    "0 - Uzdaryti programa. \n\n");

                Console.Write("Jusu pasirinkimas: ");
                var veiksmas = Convert.ToInt32(Console.ReadLine());
                Console.WriteLine();

                switch (veiksmas)
                {
                    case 1:
                        Console.Write("Iveskite paskolos suma: ");
                        var loanAmount = Convert.ToDecimal(Console.ReadLine());

                        Console.Write("Iveskite paskolos termina (men.): ");
                        var loanTerm = Convert.ToInt32(Console.ReadLine());

                        Console.Write("Iveskite metine palukanu norma: ");
                        var interestRate = Convert.ToDouble(Console.ReadLine());

                        Console.Write("Nurodykite men. mokejimo diena (1-31): ");
                        var dayPayment = Convert.ToInt32(Console.ReadLine());

                        Console.WriteLine();
                        Console.WriteLine($"Date || MonthlyPayment || Interest || Principal || Balance");

                        var loan = new Loan()
                        {
                            LoanAmount = loanAmount,
                            InterestRate = interestRate,
                            LoanTerm = loanTerm,
                            DayPayment = dayPayment
                        };

                        PrintLoanAmortizationSchedule(loan);

                        var totalPayment = CalculateTotalPayment(loan);
                        var totalInterest = totalPayment - loan.LoanAmount;

                        Console.WriteLine($"Is viso || {Math.Round(totalPayment, 2)} || {Math.Round(totalInterest, 2)} || {loan.LoanAmount} || -----");
                        Console.WriteLine();

                        var effectiveRate = CalculateEffectiveRate(loan.InterestRate);

                        Console.WriteLine($"BVKKMN {Math.Round(effectiveRate, 2)} proc.");

                        break;

                    case 0:
                        return;

                    default:
                        Console.WriteLine("Negalima atlikti tokio veiksmo. Bandykite dar karta! \n");
                        break;
                }
            }
        }

        public static void PrintLoanAmortizationSchedule(Loan loan)
        {
            var balance = loan.LoanAmount;
            var monthlyPayment = CalculateMonthlyPayment(loan);
            var date = DateTime.Now;

            for (var i = 0; i < loan.LoanTerm; i++)
            {
                date = DayOfMonthFromDateTime(date, loan.DayPayment);

                var interest = (decimal)((loan.InterestRate / 100) / 12) * balance;
                var principal = monthlyPayment - interest;

                balance -= principal;

                Console.WriteLine($"{date:d} || {Math.Round(monthlyPayment, 2)} || {Math.Round(interest, 2)} || {Math.Round(principal, 2)} || {Math.Round(balance, 2)}");

                date = date.AddMonths(1);
            }
        }

        public static DateTime DayOfMonthFromDateTime(DateTime dateTime, int day)
        {
            var lastDayOfMonth = DateTime.DaysInMonth(dateTime.Year, dateTime.Month);

            if (day > lastDayOfMonth)
            {
                day = lastDayOfMonth;
            }

            return new(dateTime.Year, dateTime.Month, day);
        }

        public static decimal CalculateTotalPayment(Loan loan)
        {
            decimal totalPayment;
            decimal monthlyPayment = CalculateMonthlyPayment(loan);

            totalPayment = loan.LoanTerm * monthlyPayment;

            return totalPayment;
        }

        public static decimal CalculateMonthlyPayment(Loan loan)
        {
            decimal monthlyPayment;
            double intRate = (loan.InterestRate / 100) / 12;

            monthlyPayment = (loan.LoanAmount * (decimal)intRate) / (decimal)(1 - (Math.Pow((1 + intRate), -loan.LoanTerm)));

            return monthlyPayment;
        }

        public static double CalculateEffectiveRate(double intRate)
        {
            var effectiveRate = (Math.Pow((1 + (intRate / 100) / 12), 12) - 1) * 100;

            return effectiveRate;
        }
    }
}
