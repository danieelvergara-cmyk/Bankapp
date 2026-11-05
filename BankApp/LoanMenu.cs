using System;
using System.Collections.Generic;
using System.Text;

namespace BankApp
{
    internal class LoanMenu
    {
        // Private fields to hold the LoanService and LoanRules instances.
        // They are readonly because they should not be modified after initialization.3
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

            Console.Clear();

            while (isRunning)
            {

                Console.WriteLine("\n--- Lån ---");
                Console.WriteLine("Välj ett alternativ:");
                Console.WriteLine("1. Skapa ett nytt lån");
                Console.WriteLine("2. Visa mina lån");
                Console.WriteLine("0. Tillbaka till Huvudmeny");

                string userInput = Console.ReadLine();

                switch (userInput)
                {
                    case "1":
                        ShowTakeLoanMenu(user);
                        break;
                    case "2":
                        break;
                    case "0":
                        isRunning = false;
                        break;
                    default:
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
                Console.WriteLine("Ogiltigt belopp, skriv ett heltal:");
            }

            Console.Clear();

            bool isConfirming = true;

            while (isConfirming)
            {
                Console.WriteLine($"Du kommer låna {amount} SEK och betala av det i {termMonths} månader.");
                Console.WriteLine($"Du kommer har {_loanRules.CalculateInterestRate(user.CreditScore)}% ränta.");
                Console.WriteLine("Goodkän lånet?");
                Console.WriteLine("1. Ja\n0. Nej");

                string userInput = Console.ReadLine();

                switch (userInput)
                {
                    case "1":
                        _loanService.CreateLoan(user, amount, termMonths);
                        isConfirming = false;
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

    }
}

