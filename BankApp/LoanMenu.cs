using System;
using System.Collections.Generic;
using System.Text;

namespace BankApp
{
    public class LoanMenu
    {
        // Private fields to hold the LoanService and LoanRules instances.
        // They are readonly because they should not be modified after initialization.
        private readonly LoanService _loanService;
        private readonly LoanRules _loanRules;

        // This constructor initializes the LoanMenu with the provided LoanService and LoanRules instances.
        public LoanMenu(LoanService loanService, LoanRules loanRules)
        {
            _loanRules = loanRules;
            _loanService = loanService;
        }

        public void ShowLoanMenu(User user)
        {
            bool isRunning = true;
            string errorMessage = "";

            Console.Clear();

            while (isRunning)
            {
                Console.Clear();

                Console.WriteLine("==== Lån ====");
                Console.WriteLine("1. Skapa ett nytt lån");
                Console.WriteLine("2. Visa mina lån");
                Console.WriteLine("0. Tillbaka till Huvudmeny");
                Console.Write("\nVälj ett alternativ:");

                // If there is a wrong input, show the error message at the bottom of the menu and clear it after showing it once.
                errorMessage = UserInterface.ShowErrorMessage(errorMessage);


                string userInput = Console.ReadLine();

                switch (userInput)
                {
                    case "1":
                        ShowTakeLoanMenu(user);
                        break;
                    case "2":
                        ShowMyLoansMenu(user);
                        break;
                    case "0":
                        isRunning = false;
                        break;
                    default:
                        errorMessage = "Ogiltig input, skriv 1, 2 eller 0."; // Set the error message to be shown in the next iteration of the loop.
                        break;
                }
               
            }
        }

        public void ShowTakeLoanMenu(User user)
        {
            decimal amount;
            int termMonths;

            Console.Clear();

            Console.WriteLine("Hur mycket vill du låna?");
            while (!decimal.TryParse(Console.ReadLine(), out amount))
            {
                Console.WriteLine("Ogiltigt belopp, skriv ett tal:");
            }

            Console.Clear();

            Console.WriteLine($"Hur många månader vill du betala av ditt lån?");
            while (!int.TryParse(Console.ReadLine(), out termMonths))
            {
                Console.WriteLine("Ogiltig input, skriv ett heltal:");
            }

            Console.Clear();

            bool isConfirming = true;

            while (isConfirming)
            {
                Console.WriteLine($"Du kommer låna {amount:F2} SEK och betala av det i {termMonths} månader.");
                Console.WriteLine($"Du kommer att ha {_loanRules.CalculateInterestRate(user.CreditScore)}% ränta.");
                Console.WriteLine("Godkänn lånet?");
                Console.WriteLine("1. Ja\n0. Nej");

                string userInput = Console.ReadLine();

                switch (userInput)
                {
                    case "1":
                        isConfirming = false;
                        Console.Clear();
                        _loanService.CreateLoan(user, amount, termMonths);
                        Console.WriteLine("\nTryck på valfri knapp för att gå tillbaka till menyn.");
                        Console.ReadKey();
                        break;
                    case "0":
                        isConfirming = false;
                        Console.WriteLine("Avbryter lånet, tryck på valfri knapp för att fortsätta.");
                        Console.ReadKey();
                        Console.Clear();
                        break;
                    default:
                        Console.WriteLine("Ogiltig input, skriv 1 eller 0.");
                        break;

                }
            }


        }

        public void ShowMyLoansMenu(User user) 
        {
            Console.Clear();

            if (user.Loans.Count == 0)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("================");
                Console.WriteLine("Du har inga lån");
                Console.WriteLine("================");
                Console.ResetColor();
            }
            else
            {
                Console.WriteLine($"\n---{user.Username}'s lån---");

                foreach (Loan loan in user.Loans)
                {
                    Console.WriteLine("================================");
                    Console.WriteLine($"Lån Nr: {loan.LoanId}");
                    Console.WriteLine($"Datum: {loan.StartDate:yyyy-MM-dd}");
                    Console.WriteLine($"Belopp: {loan.Amount:F2} SEK, Löptid: {loan.TermMonths} månader");
                    Console.WriteLine($"Månadsbelopp: {loan.MonthlyPayment:F2} SEK");
                    Console.WriteLine($"Kvar att betala: {loan.RemainingBalance:F2} SEK");
                    Console.WriteLine($"Ränta: {loan.InterestRate}%");
                    Console.WriteLine("================================");
                }

            }
            Console.WriteLine("Tryck på valfri knapp för att gå tillbaka till menyn.");
            Console.ReadKey();
            Console.Clear();
        }

    }
}

