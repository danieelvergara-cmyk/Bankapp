using System;
using System.Collections.Generic;
using System.Text;

namespace BankApp
{
    internal class Loan
    {
        int LoanId { get; set; }
        decimal Amount { get; set; }
        decimal InterestRate { get; set; }
        int TermMonths { get; set; }
        decimal TotalInterest { get; set; }
        decimal RemainingBalance { get; set; }
        string? Status { get; set; }
        DateTime StartDate { get; set; }

        public decimal CalculateTotalIntrest()
        {
            return TotalInterest; // Placeholder implementation
        }

        public decimal CalculateMonthlyPayment()
        {
            return 0; // Placeholder implementation
        }
    }
}
