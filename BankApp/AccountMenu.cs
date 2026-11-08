using System;
using System.Linq;
using System.Text.Json.Serialization;

namespace BankApp
{
    public class AccountMenu
    {
        private readonly AccountService _accountService;

        string errorMessage = "";

        public AccountMenu(AccountService accountService)
        {
            _accountService = accountService;
        }

        public void ShowAccountMenu(User user)
        {
            bool isRunning = true;

            while (isRunning)
            {
                Console.Clear();

                Console.WriteLine($"\n==== Konto-meny ({user.Username}) ====");
                Console.WriteLine("1. Visa mina konton och saldon");
                Console.WriteLine("2. Öppna vanligt konto");
                Console.WriteLine("3. Öppna sparkonto");
                Console.WriteLine("4. Sätt in pengar på vanligt konto");
                Console.WriteLine("5. Sätt in pengar på sparkonto och visa ränta");
                Console.WriteLine("0. Tillbaka");
                Console.Write("\nVälj ett alternativ:");

                errorMessage = UserInterface.ShowErrorMessage(errorMessage);

                string? choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        ShowAccounts(user);
                        break;
                    case "2":
                        OpenRegularAccount(user);
                        break;
                    case "3":
                        OpenSavingsAccount(user);
                        break;
                    case "4":
                        DepositIntoSavingsAccount(user);
                        break;
                    case "0":
                        isRunning = false;
                        break;
                    default:
                        errorMessage = "Ogiltig input. Skriv in ett tal mellan 0-5.";
                        break;
                }
            }
        }

        private static void ShowAccounts(User user)
        {
            Console.Clear();
            Console.WriteLine($"==== {user.Username}s Konton ====");
            Account.ShowAccounts(user);
            Console.WriteLine("Tryck på valfri knapp för att fortsätta.");
            Console.ReadKey();
        }

        private void OpenRegularAccount(User user)
        {
            Console.Clear();
            Console.WriteLine("=== Öppna nytt konto ===");
            string accountName = ReadRequiredText("Namn på kontot: ");
            Currency currency = ReadCurrency();

            Console.Clear();
            Account account = _accountService.CreateAccount(user, accountName, currency);
            Console.WriteLine("Kontot öppnades:");
            account.PrintAccount();
            Console.WriteLine("Tryck på valfri knapp för att fortsätta.");
            Console.ReadKey();
        }



        private Account? ChooseRegularAccount(User user)
        {
            var accounts = user.Accounts.Where(a => !(a is SavingsAccount)).ToList();

            if (accounts.Count == 0)
            {
                errorMessage = "Du har inga vanliga konton. Öppna ett konto först.";
                return null;
            }

            Console.Clear();
            Console.WriteLine("Välj konto:");
            for (int i = 0; i < accounts.Count; i++)
            {
                Console.Write($"{i + 1}. ");
                accounts[i].PrintAccount();
            }

            int choice = ReadNumberInRange("Välj nummer: ", 1, accounts.Count);
            return accounts[choice - 1];
        }

        private void OpenSavingsAccount(User user)
        {
            Console.Clear();
            Console.WriteLine("=== Öppna sparkonto ===");
            string accountName = ReadRequiredText("Namn på sparkontot: ");
            Currency currency = ReadCurrency();

            SavingsAccount savingsAccount = _accountService.CreateSavingsAccount(user, accountName, currency);

            Console.Clear();
            Console.WriteLine("Sparkontot öppnades:");
            savingsAccount.PrintAccount();
            Console.WriteLine($"Årsränta: {savingsAccount.AnnualInterestRate}%");
            Console.WriteLine("Tryck på valfri knapp för att fortsätta.");
            Console.ReadKey();
        }

        private void DepositIntoSavingsAccount(User user)
        {
            SavingsAccount? savingsAccount = ChooseSavingsAccount(user);
            if (savingsAccount == null) return;

            Console.Clear();
            savingsAccount.PrintAccount();
            decimal amount = ReadPositiveAmount("Hur mycket vill du sätta in? ");
            _accountService.DepositToSavingsAccount(savingsAccount, amount);
            Console.WriteLine("Tryck på valfri knapp för att fortsätta.");
            Console.ReadKey();
        }

        private SavingsAccount? ChooseSavingsAccount(User user)
        {
            var savingsAccounts = user.Accounts.OfType<SavingsAccount>().ToList();

            if (savingsAccounts.Count == 0)
            {
                errorMessage = "Du har inga sparkonton. Öppna ett sparkonto först.";
                return null;
            }

            Console.Clear();
            Console.WriteLine("Välj sparkonto:");
            for (int i = 0; i < savingsAccounts.Count; i++)
            {
                Console.Write($"{i + 1}. ");
                savingsAccounts[i].PrintAccount();
            }

            int choice = ReadNumberInRange("Välj nummer: ", 1, savingsAccounts.Count);
            return savingsAccounts[choice - 1];
        }

        private static Currency ReadCurrency()
        {
            Console.Clear();
            while (true)
            {
                Console.WriteLine("Välj valuta: 1. SEK  2. EUR  3. USD");
                Console.Write("Val: ");

                switch (Console.ReadLine())
                {
                    case "1": return Currency.SEK;
                    case "2": return Currency.EUR;
                    case "3": return Currency.USD;
                    default:
                        Console.Clear();
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Välj 1, 2 eller 3.");
                        Console.ResetColor();
                        break;
                }
            }
        }

        private static string ReadRequiredText(string prompt)
        {
            while (true)
            {
                Console.Write(prompt);
                string? value = Console.ReadLine();

                if (!string.IsNullOrWhiteSpace(value))
                {
                    return value.Trim();
                }
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Fältet får inte vara tomt.");
                Console.ResetColor();

            }
        }

        private static decimal ReadPositiveAmount(string prompt)
        {
            while (true)
            {
                Console.Write(prompt);

                if (decimal.TryParse(Console.ReadLine(), out decimal amount) && amount > 0)
                {
                    return amount;
                }
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Ange ett belopp som är större än noll.");
                Console.ResetColor();
            }
        }
        // Reads a number within a specified range. And if you put number outside the range it will ask you to put a number within the range.
        private static int ReadNumberInRange(string prompt, int minimum, int maximum)
        {
            while (true)
            {
                Console.Write(prompt);

                if (int.TryParse(Console.ReadLine(), out int number) &&
                    number >= minimum && number <= maximum)
                {
                    return number;
                }

                Console.WriteLine($"Välj ett nummer mellan {minimum} och {maximum}.");
            }
        }
    }
}