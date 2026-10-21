using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace BankApp
{
    internal class LoanRules
    {
        public decimal BaseInterestRate { get; set; }
        public decimal MaxLoanMultiplier { get; set; }
        public decimal CreditScoreFactor { get; set; }

        public decimal CalculateInterestRate(int creditScore)
        {
            return BaseInterestRate; // Placeholder implementation
        }

        public decimal CalculateMaxLoan(decimal totalBalance)
        {
            return BaseInterestRate; // Placeholder implementation
        }
        public bool CanBorrow(decimal amount, decimal totalBalance)
        {
            return true; // Placeholder implementation
        }

        public void UpdateInterestRules(LoanRules rules)
        {

        }

    }
}
