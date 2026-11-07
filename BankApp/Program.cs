using System;
using System.Linq;

namespace BankApp
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // Services, created once and shared

            ExchangeRate exchangeRate = new ExchangeRate();

            UserService userService = new UserService();

            LoginService loginService = new LoginService(userService);

            AccountService accountService = new AccountService();

            LoanRules loanRules = new LoanRules();

            LoanService loanService = new LoanService(loanRules, exchangeRate);

            //TransactionService transactionService = new TransactionService(userService);

            UserInterface userInterface = new UserInterface();


            LoanMenu loanMenu = new LoanMenu(loanService, loanRules);

            AdminMenu adminMenu = new AdminMenu(userService, loanRules, exchangeRate);


            //Scheduler scheduler = new Scheduler(TransactionService);
            //_ = scheduler.RunAsync();

            // A Admin user pre-created and a test user for demonstration purposes.
            Admin admin = new Admin { UserId = 0, Username = "admin", Password = "admin" };
            User testUser = new User { UserId = 256, Username = "user", Password = "user" };
            userService.Users.Add(admin);
            userService.Users.Add(testUser);

            userInterface.ShowLogo();
            userInterface.ShowMainMenu(user,accountMenu,transactionMenu,loanMenu);
        }
            
    }
}
