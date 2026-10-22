using System;
using System.Collections.Generic;
using System.Text;

namespace BankApp
{
    public class UserInterface
    {

        public void ShowMainMenu()
        {
            Console.WriteLine("Imagine a menu here: \n1.\n2.\n3.");
        }

        // This method is used to show the login menu and get the username and password from the user. 
        // It will then return the username and password to the caller.
        // The username and password are currently just placeholders and will be replaced -
        // with actual logic to get the username and password from the user.
        public void ShowLoginMenu()
        {
            Console.WriteLine("Enter username:");
            string username = Console.ReadLine();

            Console.WriteLine("Enter password:");
            string userPassword = Console.ReadLine();

        }

        public void ShowLogo()
        {
            Console.WriteLine("\r\n               _.----------.               \r\n           ,-''    ,ooo.    `--.           \r\n        ,-'       _¥$$$£..      `-.        \r\n      ,'      ,_o$$$''$$$$o_       `.      \r\n    ,'      ,o$$$$,o$$$$o.88o.       `.    \r\n   /       ,$$$,d$$$$¥¥$$$£888£.       \\   \r\n  /        d$£d$$$¥'    ¥$£d888'        \\  \r\n ;         ¥$$$$$.      '\"'88£'          : \r\n ;          ¥$'¥$$o_     '¥£'            : \r\n;            '¥8¥$$$£.                    :\r\n|              `\"'¥$$$$o.                 |\r\n:                  '`¥$$$$o.              ;\r\n :                    '`¥$$$.            ; \r\n :           _.          '$$£.           ; \r\n  \\        _.88.        ,$$$££          /  \r\n   \\      ,88._8£oooooo$$$£'8£         /   \r\n    `.    '¥888.$$$$$$$$£'88£        ,'    \r\n      `.    `\"'8'',o.o.'8£\"'       ,'      \r\n        `-.     ''Y888£         ,-'        \r\n           `--.    ¥££'     _.-'           \r\n               `----------''               \r\n");
            Console.WriteLine("Welcome to BankApp");
        }

        public void SetColors(ConsoleColor color) 
        {
            Console.ForegroundColor = color;
        }

        public void ShowError(string message) 
        {

        }

    }
}
