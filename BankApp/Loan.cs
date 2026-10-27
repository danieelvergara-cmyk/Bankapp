using System;
using System.Collections.Generic;
using System.Text;

namespace BankApp
{
    public class Loan
    {
        public int LoanId { get; set; } // Unique identifier for the loan
        public decimal Amount { get; set; } // How much the user is borrowing
        public decimal InterestRate { get; set; } // Interest rate for the loan
        public int TermMonths { get; set; } // Duration of the loan in months
        public decimal TotalInterest { get; set; } // Total interest cost for the loan
        public decimal RemainingBalance { get; set; } // Remaining balance to be paid on the loan
        public string? Status { get; set; } // Status of the loan (e.g., Active, Paid, Denied)
        public DateTime StartDate { get; set; } // Date when the loan was created


        // Calculates the total interest cost for the loan based on the amount, interest rate, and term in months
        public decimal CalculateTotalInterest()
        {
            TotalInterest = Amount * (InterestRate / 100) * (TermMonths / 12m);
            return TotalInterest; 
        }

        // Calculates the monthly payment for the loan based on the amount, total interest, and term in months
        public decimal CalculateMonthlyPayment()
        {
            if (TermMonths <= 0)
            {
                return 0; // Return 0 if term is 0 or negative to avoid division by zero
            }
            
            CalculateTotalInterest(); // Ensure TotalInterest is allways calculated before calculating monthly payment
            decimal monthlyPayment = (Amount + TotalInterest) / TermMonths;
            return monthlyPayment;
        }
    }
}
