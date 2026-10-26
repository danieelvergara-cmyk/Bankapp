using System;
using System.Collections.Generic;
using System.Text;

namespace BankApp
{
    internal class LoanService
    {
        private List<Loan> loans = new List<Loan>(); // List to store all loans (placeholder until the User class is accessible)
        private LoanRules rules; // Field to hold the loan rules for the service
        private int nextLoanId = 1000; // Starting loan ID for new loans


        // Constructor to initialize the LoanService with the provided LoanRules
        public LoanService(LoanRules rules)
        {
            this.rules = rules;
        }

        // String user is a placeholder until the User class is accessible
        // decimal totalBalance is a placeholder until the User class is accessible

        // Checks if the user can borrow the requested amount based on the LoanRules and if the termMonths is greater than 0
        // If the user can borrow, a new Loan object is created with the specified amount, termMonths, and other relevant details.
        // The new Loan object is returned. If the user cannot borrow, null is returned.

        public Loan? CreateLoan(string user, decimal amount, int termMonths, decimal totalBalance)
        {
           if (rules.CanBorrow(amount, totalBalance) && termMonths > 0)
            {
                Loan newLoan = new Loan();
                newLoan.LoanId = nextLoanId++; // Assign a unique loan ID and increment for the next loan
                newLoan.Amount = amount;
                newLoan.TermMonths = termMonths;
                newLoan.StartDate = DateTime.Now;
                newLoan.Status = "Active";
                newLoan.InterestRate = rules.BaseInterestRate; 
                newLoan.CalculateTotalInterest(); // Calculate total interest based on the above amount, interest rate and termMonths
                newLoan.RemainingBalance = newLoan.Amount + newLoan.TotalInterest; // Set the remaining balance to the total amount owed 
                
                loans.Add(newLoan); // Add the new loan to the list of loans
                return newLoan;

            }

            // Loan denied due to insufficient balance, invalid term or invalid amount
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

                if (!rules.CanBorrow(amount, totalBalance))
                {
                    Console.WriteLine("Loan denied: Insufficient balance to borrow the requested amount.");
                    return null;
                }

                return null;
            }
        }


        public Loan CalculateLoanOffer(string user, decimal amount, int termMonths)
        {

            return new Loan(); // Placeholder implementation
        }

        public List<Loan> GetUserLoans(int userId) 
        {
            return new List<Loan>(); // Placeholder implementation
        }

        public void RepayLoan(int loanId, decimal amount)
        {

        }

    }
}
