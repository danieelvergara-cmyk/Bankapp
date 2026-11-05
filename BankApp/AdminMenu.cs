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

        string errorMessage = "";

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
            bool isRunning = true;

            while (isRunning)
            {
                Console.Clear();
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

                Console.WriteLine();
                Console.WriteLine("1. Ändra värden");
                Console.WriteLine("0. Tillbaka");

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
                        ChangeLoanRules();
                        Console.WriteLine("--------------- Nya regler ---------------");
                        Console.WriteLine($"Basränta:             {_loanRules.BaseInterestRate}%");
                        Console.WriteLine();
                        Console.WriteLine($"Maxlån:               {_loanRules.MaxLoanMultiplier} gånger saldot");
                        Console.WriteLine();
                        Console.WriteLine($"Kreditvärdighetsfaktor: {_loanRules.CreditScoreFactor}% per 100 poäng");
                        Console.WriteLine("------------------------------------------");
                        Console.WriteLine("\nTryck på valfri knapp för att gå tillbaka till menyn.");

                        isRunning = false;
                        Console.ReadKey();
                        break;
                    case "0":
                        isRunning = false;
                        break;
                    default:
                        errorMessage = "Ogiltig input, skriv 1 eller 0."; // Set the error message to be shown in the next iteration of the loop.
                        break;
                }
            }
        }

        private bool ChangeLoanRules()
        {
            decimal newBaseInterestRate;
            decimal newMaxLoanMultiplier;
            decimal newCreditScoreFactor;

            Console.Clear();

            Console.WriteLine("Skriv in ett nytt värde för basräntan (i procent):");
            while (!decimal.TryParse(Console.ReadLine(), out newBaseInterestRate))
            {
                    Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Ogiltigt, skriv ett tal:");
                    Console.ResetColor();
            }
            Console.Clear();

            Console.WriteLine("Skriv in ett nytt värde för maxlån (i multipel av saldot):");
            while (!decimal.TryParse(Console.ReadLine(), out newMaxLoanMultiplier))
            {
                    Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Ogiltigt, skriv ett tal:");
                    Console.ResetColor();
            }
            Console.Clear();

            Console.WriteLine("Skriv in ett nytt värde för kreditvärdighetsfaktorn (i procent per 100 poäng):");
            while (!decimal.TryParse(Console.ReadLine(), out newCreditScoreFactor))
            {
                    Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Ogiltigt, skriv ett tal:");
                    Console.ResetColor();
            }
            Console.Clear();

            LoanRules newRules = new LoanRules
            {
                BaseInterestRate = newBaseInterestRate,
                MaxLoanMultiplier = newMaxLoanMultiplier,
                CreditScoreFactor = newCreditScoreFactor
            };

            return _loanRules.UpdateInterestRules(newRules);
        }

        public void ShowExchangeRateMenu()
        {
            bool isRunning = true;

            while (isRunning)
            {
                Console.Clear();
                Console.WriteLine("---- Nuvarande växelkurser ----");
                Console.WriteLine();
                Console.WriteLine($"1 EUR = {_exchangeRate.EurToSek} SEK - Senast uppdaterad: {_exchangeRate.EurUpdatedAt}");
                Console.WriteLine($"1 USD = {_exchangeRate.UsdToSek} SEK - Senast uppdaterad: {_exchangeRate.UsdUpdatedAt}");
                Console.WriteLine();
                Console.WriteLine("-------------------------------");

                Console.WriteLine();
                Console.WriteLine("1. Ändra EUR");
                Console.WriteLine("2. Ändra USD");
                Console.WriteLine("0. Tillbaka");

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
                        ChangeExchangeRate(Currency.EUR);
                        break;
                    case "2":
                        ChangeExchangeRate(Currency.USD);
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

        // Asks for a new rate for the chosen currency and updates it.
        private void ChangeExchangeRate(Currency currency)
        {
            decimal newRate;

            Console.Clear();
            Console.WriteLine($"Skriv in en ny kurs för {currency}:");

            while (!decimal.TryParse(Console.ReadLine(), out newRate))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Ogiltigt, skriv ett tal:");
                Console.ResetColor();
            }
            Console.Clear();

            bool rateChanged = _exchangeRate.UpdateExchangeRate(currency, newRate);

            if (rateChanged)
            {
                Console.WriteLine($"Växelkursen för {currency} har uppdaterats till {newRate} SEK.");
            }
            else
            {
                Console.WriteLine($"Växelkursen för {currency} kunde inte uppdateras.");
            }

            Console.WriteLine("\nTryck på valfri knapp för att gå tillbaka till menyn.");
            Console.ReadKey();

        }

        // Will be implemented later when the StatisticsService is implemented.
        public void ShowStatisticsMenu()
        {

        }
    }
}
