using System;
using System.Collections.Generic;
using System.Text;

namespace BankApp
{
    internal class LoanService
    {
        LoanRules rules = new LoanRules();


        // String user is a placeholder until the User class is accessible
        // decimal totalBalance is a placeholder until the User class is accessible
        public Loan? CreateLoan(string user, decimal amount, int termMonths, decimal totalBalance)
        {
           if (rules.CanBorrow(amount, totalBalance))
            {
                Loan newLoan = new Loan();
                newLoan.Amount = amount;
                newLoan.TermMonths = termMonths;

                return newLoan; // Placeholder implementation

            }

            else 
            {
                return null; // Placeholder implementation
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
