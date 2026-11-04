
using System;
using System.Linq;

namespace BankApp
{
    internal class AccountMenu
    {
        private static int _nextUserId = 1;

        static void Main(string[] args)
        {
            // Dessa objekt håller informationen medan programmet körs.
            // I en riktig bank skulle informationen sparas i en databas.
            UserService userService = new UserService();
            AccountService accountService = new AccountService();
            TransactionService transactionService = new TransactionService(accountService);
            Scheduler scheduler = new Scheduler(transactionService);

            // Scheduler behandlar väntande överföringar var 15:e minut.
            _ = scheduler.RunAsync();

            bool programIsRunning = true;

            while (programIsRunning)
            {
                Console.WriteLine("\n=== Välkommen till banken ===");
                Console.WriteLine("1. Logga in");
                Console.WriteLine("2. Registrera ny användare");
                Console.WriteLine("0. Avsluta");
                Console.Write("Välj: ");                                                                                                    
                string? choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        User? user = LoginUser(userService);
                        if (user != null)
                        {
                            ShowUserMenu(user, userService, accountService, transactionService);
                        }               
                        break;
                    case "2":
                        Console.WriteLine("\n=== Registrera ny användare ===")
;                        string username = ReadRequiredText("Användarnamn: ");
                        string password = ReadRequiredText("Lösenord: ");
                        Console.WriteLine($"Användare {username} registrerad.");        
                        break;
                    case "0":
                        programIsRunning = false;   
                        break;
                    default:
                        Console.WriteLine("Välj ett av alternativen i menyn.");
                        break;          
                }
            }
        }

        private static User? LoginUser(UserService userService)
        {
            Console.WriteLine("\n=== Logga in ===");
            Console.Write("Användarnamn: ");
            string? username = Console.ReadLine();
            Console.Write("Lösenord: ");
            string? password = Console.ReadLine();

            User? user = userService.AuthenticateUser(username, password);

            if (user == null)
            {
                Console.WriteLine("Felaktigt användarnamn eller lösenord.");
            }

            return user;
        }                                                                                                               
        private static void ShowUserMenu(
            User user,
            UserService userService,
            AccountService accountService,
            TransactionService transactionService)
        {
            bool userIsLoggedIn = true;

            while (userIsLoggedIn)
            {
                Console.WriteLine($"\n=== Meny för {user.Username} ===");
                Console.WriteLine("1. Visa mina konton och saldon");
                Console.WriteLine("2. Öppna vanligt konto");
                Console.WriteLine("3. Öppna sparkonto");
                Console.WriteLine("4. Sätt in pengar på sparkonto och visa ränta");
                Console.WriteLine("5. Överför pengar till ett annat konto");
                Console.WriteLine("0. Logga ut");
                Console.Write("Välj: ");

                string? choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        // BACKLOGG: "Användare ska kunna se en lista med alla sina bankkonton och saldon."
                        // `user` är användaren som just loggade in. Därför visas bara user.Accounts.
                        // Sparkonton syns också eftersom SavingsAccount ärver från Account.
                        Account.ShowAccounts(user);
                        break;
                    case "2":
                        // BACKLOGG: "Användare ska kunna öppna upp nya konton i banken."
                        // Öppnar ett vanligt konto som kopplas till den inloggade användaren.
                        OpenRegularAccount(user, accountService);
                        break;
                    case "3":
                        // BACKLOGG: användaren ska kunna öppna ett sparkonto.
                        OpenSavingsAccount(user, accountService);
                        break;
                    case "4":
                        // BACKLOGG: visa uppskattad ränta på beloppet användaren tänker sätta in.
                        // Här väljer användaren ett sparkonto och skriver in insättningsbeloppet.
                        DepositIntoSavingsAccount(user, accountService);
                        break;
                    case "5":
                        MakeTransfer(user, userService, transactionService);
                        break;
                    case "0":
                        userIsLoggedIn = false;
                        break;
                    default:
                        Console.WriteLine("Välj ett av alternativen i menyn.");
                        break;
                }
            }
        }

        private static void OpenRegularAccount(User user, AccountService accountService)
        {
            string accountName = ReadRequiredText("Namn på kontot: ");

            // BACKLOGG: "Användare ska kunna ha ett visst konto i en annan valuta."
            // Användaren väljer valutan när kontot öppnas; valet skickas vidare till AccountService.
            Currency currency = ReadCurrency();

            // AccountService skapar kontot, ger det ett ID och kopplar det till user.
            Account account = accountService.CreateAccount(user, accountName, currency);
            Console.WriteLine("Kontot öppnades:");
            account.PrintAccount();
        }


        private static void OpenSavingsAccount(User user, AccountService accountService)
        {
            string accountName = ReadRequiredText("Namn på sparkontot: ");

            // Sparkontot får också en valuta när det skapas.
            Currency currency = ReadCurrency();

            // AccountService skapar sparkontot och lägger det i samma kontolista som vanliga konton.
            SavingsAccount savingsAccount =
                accountService.CreateSavingsAccount(user, accountName, currency);

            Console.WriteLine("Sparkontot öppnades:");
            savingsAccount.PrintAccount();
            Console.WriteLine($"Årsränta: {savingsAccount.AnnualInterestRate}%");
        }

// denna metod används för att sätta in pengar på sparkontot och se räntan. Den är kopplad till alternativ 4 i
        private static void DepositIntoSavingsAccount(User user, AccountService accountService)
        {
            SavingsAccount? savingsAccount = ChooseSavingsAccount(user);

            if (savingsAccount == null)
            {
                return;
            }

            decimal amount = ReadPositiveAmount("Hur mycket vill du sätta in? ");

            // BACKLOGG: "...se hur mycket ränta de kommer kunna få på pengarna de sätter in."
            // AccountService räknar och visar räntan för amount. Därefter gör den insättningen.
            accountService.DepositToSavingsAccount(savingsAccount, amount);
        }
// denna metod används för att välja ett sparkonto från användarens konton. och i menyn är det kopplat till alternativ 4, där användaren kan sätta in pengar på sparkontot och se räntan.
        private static SavingsAccount? ChooseSavingsAccount(User user)
        {
            var savingsAccounts = user.Accounts.OfType<SavingsAccount>().ToList();

            if (savingsAccounts.Count == 0)
            {
                Console.WriteLine("Du har inga sparkonton. Öppna ett sparkonto först.");
                return null;
            }

            Console.WriteLine("Välj sparkonto:");

            for (int i = 0; i < savingsAccounts.Count; i++)
            {
                Console.Write($"{i + 1}. ");
                savingsAccounts[i].PrintAccount();
            }

            int choice = ReadNumberInRange("Välj nummer: ", 1, savingsAccounts.Count);
            return savingsAccounts[choice - 1];
        }

        private static void MakeTransfer(
            User user,
            UserService userService,
            TransactionService transactionService)
        {
            Console.WriteLine("Välj kontot du vill överföra från:");
            Account? fromAccount = ChooseAccount(user);

            if (fromAccount == null)
            {
                return;
            }

            int destinationId = ReadPositiveInteger("Mottagarens kontonummer: ");

            Account? toAccount = userService.Users
                .SelectMany(registeredUser => registeredUser.Accounts)
                .FirstOrDefault(account => account.AccountId == destinationId);

            if (toAccount == null)
            {
                Console.WriteLine("Kontot hittades inte.");
                return;
            }

            if (fromAccount.AccountId == toAccount.AccountId)
            {
                Console.WriteLine("Du kan inte överföra till samma konto.");
                return;
            }

            if (fromAccount.Currency != toAccount.Currency)
            {
                Console.WriteLine("Överföring mellan olika valutor är inte tillgänglig ännu.");
                return;
            }

            decimal amount = ReadPositiveAmount("Belopp att överföra: ");

            if (amount > fromAccount.Balance)
            {
                Console.WriteLine("Kontot har inte tillräckligt saldo.");
                return;
            }

            if (fromAccount.OwnerId == toAccount.OwnerId)
            {
                transactionService.TransferToOwnAccount(
                    fromAccount.AccountId,
                    toAccount.AccountId,
                    amount);
            }
            else
            {
                transactionService.TransferToOtherAccount(
                    fromAccount.AccountId,
                    toAccount.AccountId,
                    amount);
            }

            Console.WriteLine("Överföringen är registrerad och väntar på behandling.");
            Console.WriteLine("Scheduler behandlar väntande överföringar var 15:e minut.");
        }

        private static Account? ChooseAccount(User user)
        {
            if (user.Accounts.Count == 0)
            {
                Console.WriteLine("Du har inga konton att överföra från.");
                return null;
            }

            for (int i = 0; i < user.Accounts.Count; i++)
            {
                Console.Write($"{i + 1}. ");
                user.Accounts[i].PrintAccount();
            }

            int choice = ReadNumberInRange("Välj nummer: ", 1, user.Accounts.Count);
            return user.Accounts[choice - 1];
        }

        private static Currency ReadCurrency()
        {
            // Valet blir kontots Currency, till exempel SEK, EUR eller USD.
            // Själva växlingen mellan valutor vid en överföring är en separat funktion.
            while (true)
            {
                Console.WriteLine("Välj valuta: 1. SEK  2. EUR  3. USD");
                Console.Write("Val: ");

                switch (Console.ReadLine())
                {
                    case "1": return Currency.SEK;
                    case "2": return Currency.EUR;
                    case "3": return Currency.USD;
                    default: Console.WriteLine("Välj 1, 2 eller 3."); break;
                }
            }
        }
   // denna undermetod används för att läsa in text som inte får vara tom, t.ex. användarnamn, lösenord och kontonamn.
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

                Console.WriteLine("Fältet får inte vara tomt.");
            }
        }
// denna undermetod används för att läsa in belopp som måste vara större än noll, t.ex. insättningar och överföringar.
        private static decimal ReadPositiveAmount(string prompt)
        {
            while (true)
            {
                Console.Write(prompt);

                if (decimal.TryParse(Console.ReadLine(), out decimal amount) && amount > 0)
                {
                    return amount;
                }

                Console.WriteLine("Ange ett belopp som är större än noll.");
            }
        }
// denna undermetod används för att läsa in heltal som måste vara större än noll, t.ex. kontonummer.
        private static int ReadPositiveInteger(string prompt)
        {
            while (true)
            {
                Console.Write(prompt);

                if (int.TryParse(Console.ReadLine(), out int number) && number > 0)
                {
                    return number;
                }

                Console.WriteLine("Ange ett heltal som är större än noll.");
            }
        }

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
