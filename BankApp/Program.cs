using System;
using System.Linq;

namespace BankApp
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // Services, created once and shared
            UserService userService = new UserService();

            LoginService loginService = new LoginService(userService);

            AccountService accountService = new AccountService();

            LoanRules loanRules = new LoanRules();

            LoanService loanService = new LoanService(loanRules);

            //TransactionService transactionService = new TransactionService(userService);
            UserInterface userInterface = new UserInterface();
            ExchangeRate exchangeRate = new ExchangeRate(); 


            //Scheduler scheduler = new Scheduler(TransactionService);
            //_ = scheduler.RunAsync();

            // A Admin user pre-created 
            Admin admin = new Admin { UserId = 0, Username = "admin", Password = "admin" };
            userService.Users.Add(admin);

            userInterface.ShowLogo();
            userInterface.ShowMainMenu();
        }
            
    }
}
