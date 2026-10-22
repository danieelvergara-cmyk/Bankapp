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

        // Checks if user can borrow a certain amount based on their total balance and the maximum loan multiplier
        // if the requested amount exceeds the maximum loan amount or is less than or equal to zero, return false; otherwise, return true
        public bool CanBorrow(decimal amount, decimal totalBalance)
        {
            if (amount > CalculateMaxLoan(totalBalance) || amount <= 0)
            {
                return false;
            }
          
            else
            {
                return true;
            }
        }

        public void UpdateInterestRules(LoanRules rules)
        {

        }

    }
}
