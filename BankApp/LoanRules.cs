using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace BankApp
{
    internal class LoanRules
    {
        public decimal BaseIntrestRate { get; set; }
        public decimal MaxLoanMultiplier { get; set; }
        public decimal CreditScoreFactor { get; set; }

        public decimal CalculateIntrestRate(int creditScore)
        {
            return BaseIntrestRate; // Placeholder implementation
        }

        public decimal CalculateMaxLoan(decimal totalBalance)
        {
            return BaseIntrestRate; // Placeholder implementation
        }
        public bool CanBorrow(decimal amount, decimal totalBalance)
        {
            return true; // Placeholder implementation
        }

        public void UpdeateIntrestRules(LoanRules rules)
        {

        }

    }
}
