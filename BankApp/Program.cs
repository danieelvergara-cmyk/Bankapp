using System;
using System.Linq;

namespace BankApp
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // Rules and exchange rate, created once and shared

            ExchangeRate exchangeRate = new ExchangeRate();

            LoanRules loanRules = new LoanRules();


            // Services, created once and shared

            UserService userService = new UserService();

            LoginService loginService = new LoginService(userService);

            AccountService accountService = new AccountService();

            LoanService loanService = new LoanService(loanRules, exchangeRate, userService);

            NotificationService notificationService = new NotificationService();

            TransactionService transactionService = new TransactionService(userService,exchangeRate);

            // Menus, created once and shared

            UserInterface userInterface = new UserInterface();

            AccountMenu accountMenu = new AccountMenu(accountService);

            TransactionMenu transactionMenu = new TransactionMenu(transactionService, notificationService);

            LoanMenu loanMenu = new LoanMenu(loanService, loanRules);

            AdminMenu adminMenu = new AdminMenu(userService, loanRules, exchangeRate);

            // Start the scheduler to run in the background

            CancellationToken cancellationToken = new CancellationToken();


            Scheduler scheduler = new Scheduler(transactionService);
            _ = scheduler.RunAsync(cancellationToken);

            // A Admin user pre-created and a test user for demonstration purposes.
            Admin admin = new Admin { UserId = 0, Username = "admin", Password = "admin" };
            User testUser = new User { UserId = 256, Username = "user", Password = "user" };
            userService.Users.Add(admin);
            userService.Users.Add(testUser);

            //adminMenu.ShowAdminMenu(admin); for quick testing of the admin menu without login.
            //userInterface.ShowUserMenu(testUser, accountMenu, transactionMenu, loanMenu); for quick testing of the user menu without login.

            userInterface.ShowMainMenu(userService, loginService,accountMenu,transactionMenu,loanMenu, adminMenu);
        }
            
    }
}
