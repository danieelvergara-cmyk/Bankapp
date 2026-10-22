using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace BankApp
{
    internal class LoanRules
    {
        public decimal BaseInterestRate { get; set; }
        public decimal MaxLoanMultiplier { get; set; } = 5m; // Default value for max loan multiplier
        public decimal CreditScoreFactor { get; set; }

        public decimal CalculateInterestRate(int creditScore)
        {
            return BaseInterestRate; // Placeholder implementation
        }

        public decimal CalculateMaxLoan(decimal totalBalance)
        {
            return totalBalance * MaxLoanMultiplier;
        }
        public bool CanBorrow(decimal amount, decimal totalBalance)
        {
            if (amount > CalculateMaxLoan(totalBalance))
            {
                return false;
            }
            else return true;
        }

        public void UpdateInterestRules(LoanRules rules)
        {

        }

    }
}
