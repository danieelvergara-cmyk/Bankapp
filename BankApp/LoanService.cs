using System;
using System.Collections.Generic;
using System.Text;

namespace BankApp
{
    public class LoanService
    {
        private LoanRules rules; // Field to hold the loan rules for the service
        private int nextLoanId = 1000; // Starting loan ID for new loans

        // Constructor to initialize the LoanService with the provided LoanRules
        public LoanService(LoanRules rules)
        {
            this.rules = rules;
        }


        // Checks if the user can borrow the requested amount based on the LoanRules and if the termMonths is greater than 0
        // If the user can borrow, a new Loan object is created with the specified amount, termMonths, and other relevant details.
        // The new Loan object is returned. If the user cannot borrow, null is returned.
        public Loan? CreateLoan(User user, decimal amount, int termMonths)
        {
            
            decimal totalBalance = 0; 
            decimal existingDebt = 0; // Used to stop users from borrowing more than they are allowed. 

            // Gather the total balance of all the user's accounts to check against the borrowing rules
            foreach (Account account in user.Accounts)
            {
                // Only SEK accounts count until ExchangeRate can convert other currencies. 
                if (account.Currency == Currency.SEK)
                {
                    totalBalance += account.Balance; // When ExchangeRate is implemented, this will be changed to convert all currencies to SEK before adding them up.
                }
            }

            // Gather the total existing debt from all the user's loans to check against the borrowing rules
            foreach (Loan loan in user.Loans)
            {
                existingDebt += loan.RemainingBalance;
            }
            decimal canBorrowAmount = rules.CalculateMaxLoan(totalBalance) - existingDebt; // Calculate the maximum amount the user can borrow based on their total balance and existing debt


            if (rules.CanBorrow(amount + existingDebt, totalBalance) && termMonths > 0)
            {
                Loan newLoan = new Loan();
                newLoan.LoanId = nextLoanId++; // Assign a unique loan ID and increment for the next loan
                newLoan.Amount = amount;
                newLoan.TermMonths = termMonths;
                newLoan.StartDate = DateTime.Now;
                newLoan.Status = "Active";
                newLoan.InterestRate = rules.CalculateInterestRate(user.CreditScore); // Calculate interest rate based on the user's credit score
                newLoan.CalculateTotalInterest(); // Calculate total interest based on the above amount, interest rate and termMonths
                newLoan.RemainingBalance = newLoan.Amount + newLoan.TotalInterest; // Set the remaining balance to the total amount owed 
                newLoan.MonthlyPayment = newLoan.CalculateMonthlyPayment(); // Calculate the monthly payment based on the total amount owed and termMonths

                user.Loans.Add(newLoan); // Add the new loan to the user's list of loans

                Console.WriteLine($"You have borrowed {amount:F2} SEK for {termMonths} months with an interest rate of {newLoan.InterestRate}%");
                Console.WriteLine($"Your monthly payment will be {newLoan.MonthlyPayment:F2} SEK");
                Console.WriteLine($"The interest you need to pay is {newLoan.TotalInterest:F2} SEK for a total of {newLoan.RemainingBalance:F2} SEK");

                return newLoan;

            }

            // Loan denied due to insufficient balance, invalid term, invalid amount or existing debt.
            else
            {
                if (amount <= 0)
                {
                    Console.WriteLine("Loan denied: Amount must be greater than 0.");
                    return null;
                }

                if (termMonths <= 0)
                {
                    Console.WriteLine("Loan denied: Term must be greater than 0.");
                    return null;
                }

                if (canBorrowAmount <= 0)
                {
                    Console.WriteLine("Loan denied: You have reached your borrowing limit.");
                    return null;
                }

                // If the users allready has a loan and is trying to borrow more than they are allowed, this will catch it and deny the loan.
                if (!rules.CanBorrow(amount + existingDebt, totalBalance)) 
                {
                    Console.WriteLine("Loan denied: Insufficient balance to borrow the requested amount.");
                    Console.WriteLine($"You can borrow at most {canBorrowAmount:F2} SEK.");
                    return null;
                }
                return null;
            }
        }
    }
}
