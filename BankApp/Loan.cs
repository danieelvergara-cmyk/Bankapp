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

        // Calculates the monthly payment for the loan based on the amount, total interest, and term in months
        // Note: The calculation uses TotalInterest so it should be called after CalculateTotalInterest() to ensure TotalInterest is up to date
        public decimal CalculateMonthlyPayment()
        {
            if (TermMonths > 0)
            {
                decimal monthlyPayment = (Amount + TotalInterest) / TermMonths;
                return monthlyPayment;
            }

            else
            {
                return 0; // Return 0 if term is 0 or negative to avoid division by zero
            }

        }
    }
}
