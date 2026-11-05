using System;
using System.Collections.Generic;
using System.Text;

namespace BankApp
{
    public class UserInterface
    {
        // Displays the main menu.
        public void ShowMainMenu(
            UserService userService,
            LoginService loginService,
            AccountMenu accountMenu,
            TransactionMenu transactionMenu,
            LoanMenu loanMenu,
            AdminMenu adminMenu)
        {
            bool isRunning = true;

            while (isRunning)
            {
                Console.WriteLine();
                Console.WriteLine("MAIN MENU");
                Console.WriteLine("1. Register new user");
                Console.WriteLine("2. Log in");
                Console.WriteLine("3. Exit");
                Console.Write("Choose an option: ");

                string choice =
                    Console.ReadLine() ?? string.Empty;

                switch (choice)
                {
                    case "1":
                        ShowRegisterUserMenu(userService);
                        break;

                    case "2":
                        ShowLoginMenu(
                            loginService,
                            accountMenu,
                            transactionMenu,
                            loanMenu,
                            adminMenu);
                        break;

                    case "3":
                        Console.WriteLine("Closing BankApp.");
                        isRunning = false;
                        break;

                    default:
                        Console.WriteLine("Invalid option.");
                        break;
                }
            }
        }


        // Collects the information required
        // to create a new user.
        public void ShowRegisterUserMenu(
            UserService userService)
        {
            Console.WriteLine();
            Console.WriteLine("CREATE NEW USER");

            Console.Write("Enter username: ");
            string username =
                Console.ReadLine() ?? string.Empty;

            Console.Write("Enter password: ");
            string password =
                Console.ReadLine() ?? string.Empty;

            Console.Write("Enter email: ");
            string email =
                Console.ReadLine() ?? string.Empty;

            Console.Write("Enter phone number: ");
            string phone =
                Console.ReadLine() ?? string.Empty;

            User? newUser = userService.CreateUser(
                username,
                password,
                email,
                phone);

            if (newUser != null)
            {
                Console.WriteLine(
                    $"User {newUser.Username} was created " +
                    $"with ID {newUser.UserId}.");
            }
        }


        // Collects the login credentials
        // and sends them to LoginService.
        public void ShowLoginMenu(
            LoginService loginService,
            AccountMenu accountMenu,
            TransactionMenu transactionMenu,
            LoanMenu loanMenu,
            AdminMenu adminMenu)
        {
            Console.WriteLine();
            Console.WriteLine("LOG IN");

            Console.Write("Enter username: ");
            string username =
                Console.ReadLine() ?? string.Empty;

            Console.Write("Enter password: ");
            string password =
                Console.ReadLine() ?? string.Empty;

            User? loggedInUser =
                loginService.Authenticate(
                    username,
                    password);

            if (loggedInUser == null)
            {
                return;
            }

            // Opens the correct menu based on the user role.
            if (loggedInUser is Admin)
            {
                adminMenu.ShowAdminMenu(loggedInUser);
            }
            else
            {
                ShowUserMenu(
                    loggedInUser,
                    accountMenu,
                    transactionMenu,
                    loanMenu);
            }
        }


        // Displays the navigation menu
        // for a regular authenticated user.
        public void ShowUserMenu(
            User user,
            AccountMenu accountMenu,
            TransactionMenu transactionMenu,
            LoanMenu loanMenu)
        {
            bool isLoggedIn = true;

            while (isLoggedIn)
            {
                Console.WriteLine();
                Console.WriteLine($"Welcome {user.Username}");

                Console.WriteLine("USER MENU");
                Console.WriteLine("1. Accounts");
                Console.WriteLine("2. Transactions");
                Console.WriteLine("3. Loans");
                Console.WriteLine("4. Log out");
                Console.Write("Choose an option: ");

                string choice =
                    Console.ReadLine() ?? string.Empty;

                switch (choice)
                {
                    case "1":
                        accountMenu.ShowAccountMenu(user);
                        break;

                    case "2":
                        transactionMenu.ShowTransactionMenu(user);
                        break;

                    case "3":
                        loanMenu.ShowLoanMenu(user);
                        break;

                    case "4":
                        Console.WriteLine("You have logged out.");
                        isLoggedIn = false;
                        break;

                    default:
                        Console.WriteLine("Invalid option.");
                        break;
                }
            }
        }


        // Displays the application logo.
        public void ShowLogo()
        {
            Console.WriteLine("============================");
            Console.WriteLine("          BANKAPP");
            Console.WriteLine("============================");
            Console.WriteLine("Welcome to BankApp");
        }


        // Changes the console text color.
        public void SetColors(ConsoleColor color)
        {
            Console.ForegroundColor = color;
        }
    }
}