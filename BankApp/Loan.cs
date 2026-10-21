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

        public void CalculateTotalIntrest()
        {
        }

        public void CalculateMonthlyPayment()
        {
        }

    }
}
