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

        // Checks if the user can borrow the requested amount based on the LoanRules and if the termMonths is greater than 0
        // If the user can borrow, creates a new Loan object with the requested amount and termMonths and returns it; otherwise, returns null
        public Loan? CreateLoan(string user, decimal amount, int termMonths, decimal totalBalance)
        {
           if (rules.CanBorrow(amount, totalBalance) && termMonths > 0)
            {
                Loan newLoan = new Loan();
                newLoan.Amount = amount;
                newLoan.TermMonths = termMonths;
                newLoan.StartDate = DateTime.Now;
                newLoan.Status = "Active";

                return newLoan; 

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
