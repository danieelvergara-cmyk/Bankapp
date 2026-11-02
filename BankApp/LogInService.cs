using System;
using System.Collections.Generic;
using System.Text;

namespace BankApp
{
    public class LoginService
    {
        // Lista över registrerade användare
        public List<User> Users { get; set; } = new();

        // Autentiserar användaren
        public bool Authenticate(string username, string password)
        {
            return false;
        }

        // Registrerar en ny användare
        public void RegisterUser(User user)
        {
        }

        // Kontrollerar om användaren är låst
        public bool CheckLockStatus(User user)
        {
            return false;
        }
    }
}
