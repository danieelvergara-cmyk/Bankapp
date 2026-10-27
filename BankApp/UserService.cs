using System;
using System.Collections.Generic;
using System.Security.Principal;
using System.Text;

namespace BankApp
{
    public class UserService
    {
        // Lista över alla användare
        public List<User> Users { get; set; } = new();

        // Skapar en ny användare
        //public User CreateUser(
        //    string username,
        //    string password,
        //    string email,
        //    string phone)
        //{
        //}

        // Uppdaterar användarens kontaktuppgifter
        public void UpdateUserInfo(
            User user,
            string email,
            string phone)
        {
        }

        // Beräknar kreditvärdighet
        //public int CalculateCreditScore(User user)
        //{
        //}

        // Lägger till konto på användaren
        public void AddAccountToUser(
            User user,
            Account account)
        {
        }

        // Lägger till lån på användaren
        public void AddLoanToUser(
            User user,
            Loan loan)
        {
        }

        // Hämtar användare via ID
        //public User GetUserById(int id)
        //{
        //}
    }
}
