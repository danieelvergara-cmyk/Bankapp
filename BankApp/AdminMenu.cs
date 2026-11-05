using System;
using System.Collections.Generic;
using System.Text;

namespace BankApp
{
    public class AdminMenu
    {
        private readonly UserService _userService;
        private readonly LoanRules _loanRules;
        private readonly ExchangeRate _exchangeRate;
        private readonly StatisticsService _statisticsService;

        // This constructor initializes the AdminMenu with the provided UserService, LoanRules, and ExchangeRate instances.
        // The StatisticsService is commented out for now, but can be added later.
        public AdminMenu(UserService userService, LoanRules loanRules, ExchangeRate exchangeRate) //, StatisticsService statisticsService) add later
        {
            _userService = userService;
            _loanRules = loanRules;
            _exchangeRate = exchangeRate;
            //_statisticsService = statisticsService;
        }

        public void ShowAdminMenu(User admin)
        {
            Console.Clear();

            bool isRunning = true;
            string errorMessage = "";

            while (isRunning)
            {
                Console.Clear();

                Console.WriteLine("--- Admin ---");
                Console.WriteLine("1. Skapa ny användare");
                Console.WriteLine("2. Ändra låneregler");
                Console.WriteLine("3. Ändra växelkurser");
                Console.WriteLine("4. Visa statistik");
                Console.WriteLine("0. Logga ut");

                // If there is a wrong input, show the error message at the bottom of the menu and clear it after showing it once.
                if (errorMessage != "")
                {
                    Console.WriteLine();
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine(errorMessage);
                    Console.ResetColor();
                    errorMessage = "";
                }


                string userInput = Console.ReadLine();

                switch (userInput)
                {
                    case "1":
                        ShowCreateUserMenu();
                        break;
                    case "2":
                        ShowLoanRulesMenu();
                        break;
                    case "3":
                        ShowExchangeRateMenu();
                        break;
                    case "4":
                        ShowStatisticsMenu();
                        break;
                    case "0":
                        isRunning = false;
                        break;
                    default:
                        errorMessage = "Ogiltig input, skriv ett tall mellan 0-4."; // Set the error message to be shown in the next iteration of the loop.
                        break;
                }
            }
        }

        public void ShowCreateUserMenu()
        {
            string username;
            string password;
            string email;
            string phone;

            Console.WriteLine();
            Console.WriteLine("---- Skapa en ny användare ----");

            do
            {
                Console.Write("Skriv Användarnamn: ");

                username = Console.ReadLine() ?? string.Empty;

                if (string.IsNullOrWhiteSpace(username))
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Användarnamn kan inte vara tomt.");
                    Console.ResetColor();
                }
            } while (string.IsNullOrWhiteSpace(username));

            do
            {
                Console.Write("Skriv Lösenord: ");

                password = Console.ReadLine() ?? string.Empty;

                if (string.IsNullOrWhiteSpace(password))
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Lösenordet kan inte vara tomt.");
                    Console.ResetColor();
                }
            } while (string.IsNullOrWhiteSpace(password));


            do
            {
                Console.Write("Skriv Email: ");

                email = Console.ReadLine() ?? string.Empty;

                if (string.IsNullOrWhiteSpace(email) || !email.Contains("@"))
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Ogiltig email, den måste innehålla @.");
                    Console.ResetColor();
                }
            } while (string.IsNullOrWhiteSpace(email) || !email.Contains("@"));

            do
            {
                Console.Write("Skriv mobilnummer: ");

                phone = Console.ReadLine() ?? string.Empty;

                if (string.IsNullOrWhiteSpace(phone) || phone.Length < 8)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Ogiltigt mobilnummer, det måste vara minst 8 tecken.");
                    Console.ResetColor();
                }
            } while (string.IsNullOrWhiteSpace(phone) || phone.Length < 8);


            User? newUser = _userService.CreateUser(
                username,
                password,
                email,
                phone);

            if (newUser != null)
            {
                Console.WriteLine($"Användaren {newUser.Username} har skapats med ID nummer: {newUser.UserId}.");
            }

            Console.WriteLine("\nTryck på valfri knapp för att gå tillbaka till menyn.");
            Console.ReadKey();
        }
            
        public void ShowLoanRulesMenu()
        {
            Console.Clear();
            Console.WriteLine("---- Ändra låneregler ----");

            Console.WriteLine();
            Console.WriteLine("Vilken regel ");
            Console.WriteLine();

            Console.WriteLine("---- Nuvarande låneregler ----");
            Console.WriteLine();
            Console.WriteLine($"Basränta:             {_loanRules.BaseInterestRate}%");
            Console.WriteLine("  Räntan för en kund med kreditvärdighet 500.");
            Console.WriteLine();
            Console.WriteLine($"Maxlån:               {_loanRules.MaxLoanMultiplier} gånger saldot");
            Console.WriteLine("  Hur mycket en kund får låna jämfört med sitt saldo.");
            Console.WriteLine();
            Console.WriteLine($"Kreditvärdighetsfaktor: {_loanRules.CreditScoreFactor}% per 100 poäng");
            Console.WriteLine("  Hur mycket räntan ändras per 100 poäng från 500.");
            Console.WriteLine("------------------------------");



        }

        public void ShowExchangeRateMenu()
        {

        }

        // Will be implemented later when the StatisticsService is implemented.
        public void ShowStatisticsMenu()
        {

        }
    }
}
