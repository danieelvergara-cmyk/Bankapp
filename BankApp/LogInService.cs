using System;
using System.Collections.Generic;
using System.Text;

namespace BankApp
{
    public class LoginService
    {
        // Används för att komma åt samma användarlista Som UserService använder
        private UserService userService;

        // Tar emot UserService när LoginService skapas
        public LoginService(UserService userService)
        {
            this.userService = userService;
        }

        // Autentiserar användaren
        public bool Authenticate(string username, string password)
        {
            User? foundUser = null;

            // Söker efter användaren via användarnamnet
            foreach (User user in userService.Users)
            {
                if (user.Username == username)
                {
                    foundUser = user;
                    break;
                }
            }

            // Avslutar om användaren inte hittades
            if (foundUser == null)
            {
                Console.WriteLine("Invalid username or password.");
                return false;
            }

            // Kontrollerar om kontot redan är låst
            if (CheckLockStatus(foundUser))
            {
                Console.WriteLine("Account is locked.");
                return false;
            }

            // Kontrollerar lösenordet
            if (foundUser.Password == password)
            {
                foundUser.ResetFailedLogin();

                Console.WriteLine("Login successful.");

                return true;
            }

            // Ökar antalet misslyckade inloggningar
            foundUser.IncrementFailedLogin();

            Console.WriteLine("Invalid username or password.");

            // Låser kontot efter tre misslyckade försök
            if (foundUser.FailedLogins >= 3)
            {
                foundUser.LockAccount();

                Console.WriteLine(
                    "Account has been locked after three failed login attempts.");
            }

            return false;
        }

        // Kontrollerar om användaren är låst
        public bool CheckLockStatus(User user)
        {
            return user.IsLocked;
        }
    }
}