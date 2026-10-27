using System;
using System.Collections.Generic;
using System.Security.Principal;
using System.Text;

namespace BankApp
{
    public class User
    {
        // Unikt ID för användaren
        public int UserId { get; set; }

        // Användarnamn för inloggning
        public string Username { get; set; } = string.Empty;

        // Lösenord
        public string Password { get; set; } = string.Empty;

        // E-postadress
        public string Email { get; set; } = string.Empty;

        // Telefonnummer
        public string Phone { get; set; } = string.Empty;

        // Antal misslyckade inloggningsförsök
        public int FailedLogins { get; set; }

        // Om kontot är låst eller inte
        public bool IsLocked { get; set; }

        // Lista med användarens konton
        public List<Account> Accounts { get; set; } = new();

        // Lista med användarens lån
        public List<Loan> Loans { get; set; } = new();

        // Kreditvärdighet
        public int CreditScore { get; set; }

        // Uppdaterar kontaktuppgifter
        public void UpdateInfo(string email, string phone)
        {
            // Logik läggs till senare
        }

        // Ökar antalet misslyckade inloggningar
        public void IncrementFailedLogin()
        {
            // Logik läggs till senare
        }

        // Nollställer antalet misslyckade inloggningar
        public void ResetFailedLogin()
        {
            // Logik läggs till senare
        }

        // Låser användarkontot
        public void LockAccount()
        {
            // Logik läggs till senare
        }
    }
}
