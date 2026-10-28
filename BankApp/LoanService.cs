using System;
using System.Collections.Generic;
using System.Text;

namespace BankApp
{
    public class LoanService
    {
        private List<Loan> loans = new List<Loan>(); // A private list to keep track of all loans and used to find loans for repayment. (Does not conflict with the user's list of loans)
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
            
            decimal totalBalance = 0; // Balance will be added up without currency conversion until the exchange rate feature is implemented.
            decimal existingDebt = 0; // Used to stop users from borrowing more than they are allowed. 

            // Gather the total balance of all the user's accounts to check against the borrowing rules
            foreach (Account account in user.Accounts)
            {
                totalBalance += account.Balance;
            }

            // Gather the total existing debt from all the user's loans to check against the borrowing rules
            foreach (Loan loan in user.Loans)
            {
                existingDebt += loan.RemainingBalance;
            }


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

                user.Loans.Add(newLoan); // Add the new loan to the user's list of loans
                loans.Add(newLoan); // Add the new loan to a private list of loans for use in the RepayLoan method

                Console.WriteLine($"You have borrowed {amount:F2} for {termMonths} months with an interest rate of {newLoan.InterestRate}%");
                Console.WriteLine($"Total interest to be paid: {newLoan.TotalInterest:F2}");
                Console.WriteLine($"Total to pay back: {newLoan.RemainingBalance:F2}");

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

                if (!rules.CanBorrow(amount + existingDebt, totalBalance))
                {
                    Console.WriteLine("Loan denied: Insufficient balance to borrow the requested amount.");
                    return null;
                }

                return null;
            }
        }


        public List<Loan> GetUserLoans(int userId) 
        {
            return new List<Loan>(); // Placeholder implementation
        }

        public void RepayLoan(int loanId, decimal amount)
        {
            // Looks for the user's loan id and if found, initializes the foundLoan variable with the loan object.
            // If not found, it will print "Loan not found." and return from the method.
            Loan? foundLoan = null;

            foreach (Loan loan in loans)
            {
                if (loan.LoanId == loanId)
                {
                    foundLoan = loan;
                    break;
                }
            }

            // Error handling for the repayment process. If the loan is not found, if the loan is already paid off or if the repayment amount is zero or negative,
            // it will print an appropriate message and return from the method.
            if (foundLoan == null)
            {
                Console.WriteLine("Loan not found.");
                return;
            }

            if (foundLoan.Status == "Paid")
            {
                Console.WriteLine("Loan is already paid off.");
                return;
            }

            if (amount <= 0)
            {
                Console.WriteLine("Repayment amount cannot be zero or negative.");
                return;
            }

            // If the repayment amount is greater than or equal to the remaining balance of the loan,
            // it will adjust the repayment amount to the remaining balance if necessary.
            if (amount >= foundLoan.RemainingBalance)
            {
                if (amount > foundLoan.RemainingBalance)
                {
                    amount = foundLoan.RemainingBalance; // Takes only what is left on the loan
                    Console.WriteLine("Payment amount exceeds remaining balance. Adjusting payment to remaining balance.");
                }
                // -----------------------------------//
                // Placeholder:                       //
                //                                    //
                // Withdraw the repayment amount      //
                // from the user's account balance.   //
                //                                    //
                //------------------------------------//
                foundLoan.RemainingBalance = 0;
                foundLoan.Status = "Paid";
                Console.WriteLine($"{amount} has been repaid and the loan is now paid off."); // Remember to add currency formatting later
                return;
            }

            // Partial payment 
            // -----------------------------------//
            // Placeholder:                       //
            //                                    //
            // Withdraw the repayment amount      //
            // from the user's account balance.   //
            //                                    //
            //------------------------------------//
            foundLoan.RemainingBalance -= amount;
            Console.WriteLine($"{amount} has been repaid. Remaining balance is now {foundLoan.RemainingBalance}."); // Remember to add currency formatting later

        }

    }
}
