using System;
using System.Collections.Generic;
using System.Text;

namespace BankApp
{
    public class UserInterface
    {

        string errorMessage = "";

        // Displays the main menu.
        public void ShowMainMenu(
            LoginService loginService,
            AccountMenu accountMenu,
            TransactionMenu transactionMenu,
            LoanMenu loanMenu,
            AdminMenu adminMenu)
        {



            bool isRunning = true;

            while (isRunning)
            {
                Console.Clear();
                Console.WriteLine("\n================");
                Console.WriteLine("    BankApp");
                Console.WriteLine("================");
                Console.WriteLine("1. Logga in");
                Console.WriteLine("2. Avsluta Programet");
                Console.WriteLine("\nVälj ett alternativ:");

                ShowErrorMessage(errorMessage);

                string choice =
                    Console.ReadLine() ?? string.Empty;

                switch (choice)
                {
                    case "1":
                        ShowLoginMenu(
                           loginService,
                           accountMenu,
                           transactionMenu,
                           loanMenu,
                           adminMenu);
                        break;

                    case "0":
                        Console.WriteLine("Avslutar BankApp.");
                        isRunning = false;
                        break;

                    default:
                        errorMessage = "Ogiltig input. Skriv in ett tal mellan 0-1.";
                        break;

                }
            }
        }



        // ShowRegisterUserMenu has been moved to AdminMenu.cs,
        // as it requires admin privileges to create new users.
        public void ShowRegisterUserMenu(
            UserService userService)
        {
           
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
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("\r\n               _.----------.               \r\n           ,-''    ,ooo.    `--.           \r\n        ,-'       _¥$$$£..      `-.        \r\n      ,'      ,_o$$$''$$$$o_       `.      \r\n    ,'      ,o$$$$,o$$$$o.88o.       `.    \r\n   /       ,$$$,d$$$$¥¥$$$£888£.       \\   \r\n  /        d$£d$$$¥'    ¥$£d888'        \\  \r\n ;         ¥$$$$$.      '\"'88£'          : \r\n ;          ¥$'¥$$o_     '¥£'            : \r\n;            '¥8¥$$$£.                    :\r\n|              `\"'¥$$$$o.                 |\r\n:                  '`¥$$$$o.              ;\r\n :                    '`¥$$$.            ; \r\n :           _.          '$$£.           ; \r\n  \\        _.88.        ,$$$££          /  \r\n   \\      ,88._8£oooooo$$$£'8£         /   \r\n    `.    '¥888.$$$$$$$$£'88£        ,'    \r\n      `.    `\"'8'',o.o.'8£\"'       ,'      \r\n        `-.     ''Y888£         ,-'        \r\n           `--.    ¥££'     _.-'           \r\n               `----------''               \r\n");
            Console.ResetColor();
            Console.WriteLine("\n==================");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Welcome to BankApp");
            Console.ResetColor();
            Console.WriteLine("==================");

        }

        public void ShowErrorMessage(string errorMessage)
        {

            // If there is a wrong input, show the error message at the bottom of the menu and clear it after showing it once.
            if (errorMessage != "")
            {
                Console.WriteLine();
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(errorMessage);
                Console.ResetColor();
                errorMessage = "";
            }
        }
}
}
