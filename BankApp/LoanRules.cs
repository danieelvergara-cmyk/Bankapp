using System;
using System.Collections.Generic;
using System.Text;

namespace BankApp
{
    public class LoanRules
    {
        public decimal BaseInterestRate { get; set; } = 5m; // The base interest rate for loans (default value is 5%)
        public decimal MaxLoanMultiplier { get; set; } = 5m; // Maximum multiplier for the loan amount based on the user's total balance (default value is 5 times the total balance)
        public decimal CreditScoreFactor { get; set; } = 1m; // Factor that adjusts the interest rate based on the user's credit score (1% for every 100 points away from 500) (Default value is 1%)



        // Calculates the interest rate based on the credit score (1-1000, with 500 as the default). A higher credit score gives a lower interest rate.
        public decimal CalculateInterestRate(int creditScore)
        {
            if (creditScore < 1)
            {
                creditScore = 1; // Ensure credit score is at least 1 to avoid excessively high interest rates
            }
            
            else if (creditScore > 1000)
            {
                creditScore = 1000; // Cap credit score at 1000 to avoid excessively low interest rates
            }

            // A score of 500 is neutral and gives the BaseInterestRate. Every 100 points below 500 adds CreditScoreFactor percent,
            // and every 100 points above 500 removes CreditScoreFactor percent.
            decimal extraRate = (500 - creditScore) / 100m * CreditScoreFactor; // Calculate the extra interest rate based on how far the credit score is from 500
            decimal interestRate = BaseInterestRate + extraRate; // Calculate the final interest rate by adding the base rate and the extra rate based on credit score
            
            if (interestRate < 1m)
            {
                interestRate = 1m; // Ensure the interest rate does not go below 1%
            }

            return interestRate;
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


        // Let's Admin update the interest rules, invalid values will not be accepted and a message will be printed to the console.
        // BaseInterestRate and MaxLoanMultiplier must be greater than 0, CreditScoreFactor must be greater than or equal to 0.
        public bool UpdateInterestRules(LoanRules newRules)
        {
            bool allValid = true; // Flag to track if all values are valid

            if (newRules == null)
            {
                return false; // Not updating if newRules is null
            }
            
            if (newRules.BaseInterestRate > 0)
            {
                BaseInterestRate = newRules.BaseInterestRate;
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("\nBasräntan måste vara större än 0.");
                Console.ResetColor();
                allValid = false;
            }

            if (newRules.MaxLoanMultiplier > 0)
            {
                MaxLoanMultiplier = newRules.MaxLoanMultiplier;
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("\nMaxlånefaktorn måste vara större än 0.");
                Console.ResetColor();
                allValid = false;
            }

            if (newRules.CreditScoreFactor >= 0)
            {
                CreditScoreFactor = newRules.CreditScoreFactor;
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("\nKreditvärdighetsfaktor måste vara större än eller lika med 0.");
                Console.ResetColor();
                allValid = false;
            }

            if (allValid)
            {
                Console.WriteLine("Räntereglerna har uppdaterats.");
            }
            else
            {
                Console.WriteLine("\nVissa värden var ogiltiga och uppdaterades inte.");
            }

            return allValid; // True if all values were saved, false if at least one was rejected (valid ones are still saved)

        }

    }
}
