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
                Console.WriteLine("0. Avsluta programmet");
                Console.Write("\nVälj ett alternativ:");

                errorMessage = ShowErrorMessage(errorMessage);

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


        // Collects the login credentials
        // and sends them to LoginService.
        public void ShowLoginMenu(
            LoginService loginService,
            AccountMenu accountMenu,
            TransactionMenu transactionMenu,
            LoanMenu loanMenu,
            AdminMenu adminMenu)
        {
            Console.Clear();
            Console.WriteLine();
            Console.WriteLine("LOGGA IN");

            Console.Write("Skriv användarnamn: ");
            string username =
                Console.ReadLine() ?? string.Empty;

            Console.Write("Skriv lösenord: ");
            string password =
                Console.ReadLine() ?? string.Empty;

            User? loggedInUser =
                loginService.Authenticate(
                    username,
                    password);

            if (loggedInUser == null)
            {
                Console.WriteLine("Tryck på valfri knapp för att fortsätta.");
                Console.ReadKey();
                return;
            }

            // Opens the correct menu based on the user role.
            if (loggedInUser is Admin)
            {
                adminMenu.ShowAdminMenu(loggedInUser);
            }
            else
            {
                ShowLogo();

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
                Console.Clear();
                Console.WriteLine();
                Console.WriteLine($"Välkommen {user.Username}!");

                Console.WriteLine("\nAnvändarmeny");
                Console.WriteLine("\n1. Konton");
                Console.WriteLine("2. Transaktioner");
                Console.WriteLine("3. Lån");
                Console.WriteLine("0. Logga ut");
                Console.Write("\nVälj ett alternativ:");

                errorMessage = ShowErrorMessage(errorMessage);

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

                    case "0":
                        Console.WriteLine("Loggar ut...");
                        isLoggedIn = false;
                        break;

                    default:
                        errorMessage = "Ogiltig input. Skriv in ett tal mellan 0-3.";
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
            Console.WriteLine("     BankApp");
            Console.ResetColor();
            Console.WriteLine("==================");
            Console.WriteLine("\nTryck på valfri knapp för att fortsätta");
            Console.ReadKey();
        }

        public static string ShowErrorMessage(string errorMessage)
        {

            // If there is a wrong input, show the error message at the bottom of the menu and clear it after showing it once.
            if (errorMessage != "")
            {
                Console.WriteLine();
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(errorMessage);
                Console.ResetColor();
            }

            // Always return an empty string so the caller can clear its errorMessage.
            return "";
        }
}
}
