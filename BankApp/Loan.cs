using System;
using System.Collections.Generic;
using System.Text;

namespace BankApp
{
    internal class Loan
    {
        public int LoanId { get; set; }
        public decimal Amount { get; set; }
        public decimal InterestRate { get; set; }
        public int TermMonths { get; set; }
        public decimal TotalInterest { get; set; }
        public decimal RemainingBalance { get; set; }
        public string? Status { get; set; }
        public DateTime StartDate { get; set; }

        // Calculates the total interest cost for the loan based on the amount, interest rate, and term in months
        public decimal CalculateTotalInterest()
        {
            TotalInterest = Amount * (InterestRate / 100) * (TermMonths / 12m);
            return TotalInterest; 
        }

        public decimal CalculateMonthlyPayment()
        {
            return 0; // Placeholder implementation
        }
    }
}
