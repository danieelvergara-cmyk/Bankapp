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
        // Nästa unika användar-ID
        private int nextUserId = 1;



        // Skapar en ny användare
        public User? CreateUser(
            string username,
            string password,
            string email,
            string phone)
        {
            // Kontrollerar om användarnamnet redan finns
            foreach (User user in Users)
            {
                if (user.Username == username)
                {
                    Console.WriteLine("Användarnamnet är redan taget.");
                    return null;
                }
            }

            // Skapar användaren
            User newUser = new User();

            newUser.UserId = nextUserId;
            newUser.Username = username;
            newUser.Password = password;
            newUser.Email = email;
            newUser.Phone = phone;
            newUser.CreditScore = 500;

            nextUserId++;

            // Sparar användaren i listan
            Users.Add(newUser);

            return newUser;
        }


        // Uppdaterar användarens kontaktuppgifter
        public void UpdateUserInfo(
            User user,
            string email,
            string phone)
        {
            if (user == null)
            {
                Console.WriteLine("User not found.");
                return;
            }
            if (string.IsNullOrWhiteSpace(email) || !email.Contains("@"))
            {
                Console.WriteLine("Invalid email address.");
                return;
            }

            if (string.IsNullOrWhiteSpace(phone) || phone.Length < 8)
            {
                Console.WriteLine("Invalid phone number.");
                return;
            }

            user.UpdateInfo(email, phone);
            Console.WriteLine("User information updated successfully.");
        }
        // Beräknar användarens kreditvärdighet
        public int CalculateCreditScore(User user)
        {
            int creditScore = 500;

            if (user.Accounts.Count > 0)
            {
                creditScore += 50;
            }

            if (user.Loans.Count == 0)
            {
                creditScore += 50;
            }

            if (creditScore > 1000)
            {
                creditScore = 1000;
            }

            user.CreditScore = creditScore;

            return creditScore;
        }

        // Hämtar en användare via ID
        public User? GetUserById(int id)
        {
            foreach (User user in Users)
            {
                if (user.UserId == id)
                {
                    return user;
                }
            }

            Console.WriteLine("User not found.");

            return null;
        }
    }
}