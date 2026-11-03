using System;
using System.Collections.Generic;
using System.Text;

namespace BankApp
{
    public class UserInterface
    {
        // This method is used to show the main menu of the application.
        // The main menu is currently just a placeholder and will be replaced later
        // The logic will be in another class later, but for now, it will just show a menu with 3 options.
        public void ShowMainMenu()
        {
            Console.WriteLine("Imagine a menu here: \n1.\n2.\n3.");
        }

        // This method is used to show the login menu and get the username and password from the user. 
        // It will then return the username and password to the caller.
        // The username and password are currently just placeholders and will be replaced -
        // with actual logic to get the username and password in a LoginService class later.
        public void ShowLoginMenu()
        {
            Console.WriteLine("Enter username:");
            string username = Console.ReadLine();

            Console.WriteLine("Enter password:");
            string userPassword = Console.ReadLine();

        }
        //Added Menu method to register a new user. Used do and while loop for correct input.
        public void ShowRegisterUserMenu(UserService userService)
        {
            Console.WriteLine("Create new user");

            string username;

            do
            {
                Console.Write("Enter username: ");
                username = Console.ReadLine() ?? string.Empty;

                if (string.IsNullOrWhiteSpace(username))
                {
                    Console.WriteLine("Username cannot be empty.");
                }

            } while (string.IsNullOrWhiteSpace(username));


            string password;

            do
            {
                Console.Write("Enter password: ");
                password = Console.ReadLine() ?? string.Empty;

                if (string.IsNullOrWhiteSpace(password))
                {
                    Console.WriteLine("Password cannot be empty.");
                }

            } while (string.IsNullOrWhiteSpace(password));


            string email;

            do
            {
                Console.Write("Enter email: ");
                email = Console.ReadLine() ?? string.Empty;

                if (string.IsNullOrWhiteSpace(email) || !email.Contains("@"))
                {
                    Console.WriteLine("Invalid email address.");
                }

            } while (string.IsNullOrWhiteSpace(email) || !email.Contains("@"));


            string phone;

            do
            {
                Console.Write("Enter phone number: ");
                phone = Console.ReadLine() ?? string.Empty;

                if (string.IsNullOrWhiteSpace(phone) || phone.Length < 8)
                {
                    Console.WriteLine("Invalid phone number.");
                }

            } while (string.IsNullOrWhiteSpace(phone) || phone.Length < 8);


            User newUser = userService.CreateUser(
                username,
                password,
                email,
                phone);

            Console.WriteLine(
                $"User {newUser.Username} was created with ID {newUser.UserId}.");
        }

        // This method is used to show the logo of the application.
        // it will be called after the user has logged in successfully and will be displayed on the main menu.
        public void ShowLogo()
        {
            Console.WriteLine("\r\n               _.----------.               \r\n           ,-''    ,ooo.    `--.           \r\n        ,-'       _¥$$$£..      `-.        \r\n      ,'      ,_o$$$''$$$$o_       `.      \r\n    ,'      ,o$$$$,o$$$$o.88o.       `.    \r\n   /       ,$$$,d$$$$¥¥$$$£888£.       \\   \r\n  /        d$£d$$$¥'    ¥$£d888'        \\  \r\n ;         ¥$$$$$.      '\"'88£'          : \r\n ;          ¥$'¥$$o_     '¥£'            : \r\n;            '¥8¥$$$£.                    :\r\n|              `\"'¥$$$$o.                 |\r\n:                  '`¥$$$$o.              ;\r\n :                    '`¥$$$.            ; \r\n :           _.          '$$£.           ; \r\n  \\        _.88.        ,$$$££          /  \r\n   \\      ,88._8£oooooo$$$£'8£         /   \r\n    `.    '¥888.$$$$$$$$£'88£        ,'    \r\n      `.    `\"'8'',o.o.'8£\"'       ,'      \r\n        `-.     ''Y888£         ,-'        \r\n           `--.    ¥££'     _.-'           \r\n               `----------''               \r\n");
            Console.WriteLine("Welcome to BankApp");
        }

        // This method is used to set the color of the console text.
        // It takes a ConsoleColor as a parameter and sets the ForegroundColor of the console to that color.
        public void SetColors(ConsoleColor color) 
        {
            Console.ForegroundColor = color;
        }

        public void ShowError(string message) 
        {

        }

    }
}
