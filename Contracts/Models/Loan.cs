namespace Contracts.Models
{
    public class Loan
    {
        public decimal LoanAmount { get; set; }

        public int LoanTerm { get; set; }

        public double InterestRate { get; set; }

        public int DayPayment { get; set; }
    }
}
