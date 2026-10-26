using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace BankApp
{
    internal class LoanRules
    {
        public decimal BaseInterestRate { get; set; } = 5m; // The base interest rate for loans (default value is 5%)
        public decimal MaxLoanMultiplier { get; set; } = 5m; // Maximum multiplier for the loan amount based on the user's total balance (default value is 5 times the total balance)
        public decimal CreditScoreFactor { get; set; } // Factor that adjusts the interest rate based on the user's credit score

        public decimal CalculateInterestRate(int creditScore)
        {
            return BaseInterestRate; // Placeholder implementation
        }

        // Calculates the maximum loan amount a user can borrow based on their total balance and the maximum loan multiplier
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

        // Let's Admin update the interest rules, but only if the new values are greater than zero.
        // If the new values are less than or equal to zero, we will not update the properties.
        public void UpdateInterestRules(LoanRules newRules)
        {

            if (newRules == null)
            {
                return; // Not updating if newRules is null
            }
            
            if (newRules.BaseInterestRate > 0)
            {
                BaseInterestRate = newRules.BaseInterestRate;
            }

            if (newRules.MaxLoanMultiplier > 0)
            {
                MaxLoanMultiplier = newRules.MaxLoanMultiplier;
            }

            if (newRules.CreditScoreFactor > 0)
            {
                CreditScoreFactor = newRules.CreditScoreFactor;
            }

        }

    }
}
