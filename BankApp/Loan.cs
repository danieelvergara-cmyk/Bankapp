using System;
using System.Collections.Generic;
using System.Text;

namespace BankApp
{
    internal class Loan
    {
        int LoanID { get; set; }
        decimal Amount { get; set; }
        decimal IntrestRate { get; set; }
        int TermMonths { get; set; }
        decimal TotalIntrest { get; set; }
        decimal RemaningBalance { get; set; }
        string? Status { get; set; }
        DateTime StartDate { get; set; }

        // Might change the method to be public if we want to access it from outside the class
        static void CalculateTotalIntrest()
        {
        }

        static void CalculateMonthlyPayment()
        {
        }

    }
}
