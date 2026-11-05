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
                        break;
                }
            }
        }

        public void ShowCreateUserMenu()
        {

        }

        public void ShowLoanRulesMenu()
        {

        }

        public void ShowExchangeRateMenu()
        {

        }

        public void ShowStatisticsMenu()
        {

        }
    }
}
