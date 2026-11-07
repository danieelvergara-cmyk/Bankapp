using System;
using System.Collections.Generic; // Gives access to List<T>


/* to call in man Transaction_menu menu = new Transaction_menu(transactionService);
        menu.RunTransferMenu();*/

namespace BankApp
{
    
    public class TransactionMenu
    {
        /*Dependency injection through a constructor.It lets Transaction_menu use a TransactionService object.
           Instead of creating its own service, the menu asks for one from the outside.


           1.  A field that stores a TransactionService object. 
               It stores a reference to the service the menu will use (like a private toolbox).*/
        private readonly TransactionService transactionService;



        private readonly NotificationService notificationService;




        /*2.  A field that stores a TransactionService object. 
              The menu tells the program: "I cannot start without this service..*/
        public TransactionMenu(
            TransactionService transactionService,
            NotificationService notificationService)
        {
            /* 3. We take the service that was just delivered to us (right side)
                 and lock it into our private toolbox (left side) so the rest of the menu can use it.*/
            this.transactionService = transactionService;
            this.notificationService = notificationService;

        }

        public void ShowTransactionMenu(User user)
        {
            bool isValidChoice = true;

            while (isValidChoice)
            {
                
                    Console.Clear();
                    Console.WriteLine("              ");
                    Console.WriteLine("=====================");
                    Console.WriteLine("              ");
                    Console.WriteLine("1. Se en logg på alla överföringar");
                    Console.WriteLine("2. Överför pengar");
                    Console.WriteLine("0. Exit ");
                    Console.WriteLine(" ----------------------- ");
                    Console.WriteLine("              ");
                    Console.Write("Välj 1, 2 eller 0:");


                    string choice = Console.ReadLine() ?? string.Empty; // // possible null value


                    switch (choice)
                    {
                        case "1":
                            ShowTransactionHistory(user);
                            break;

                        case "2":
                            TransferTo(user);
                            break;

                        case "0":
                            isValidChoice = false;
                            return;
                            

                        default:
                            Console.WriteLine("Ogiltigt val. Vänligen välj 1, 2 eller 0.");
                            Console.ReadKey();
                            break;
        
                    }
                
        }    }

        public void TransferTo(User user)

        {
            bool isValidChoice2 = true;

            while (isValidChoice2)
            {
                {

                    Console.Clear();
                    Console.WriteLine("              ");
                    Console.WriteLine("======= Överföring  =====");
                    Console.WriteLine("              ");
                    Console.WriteLine("Vill du överföra pengar till:");
                    Console.WriteLine("1. Eget konto");
                    Console.WriteLine("2. Annan person");
                    Console.WriteLine("0.Exit ");
                    Console.WriteLine(" ");
                    Console.Write("Välj 1, 2 eller 0:");


                    string choice2 = Console.ReadLine() ?? string.Empty;

                    switch (choice2)
                    {
                        case "1":
                            HandleOwnAccountTransfer(user);
                            break;

                        case "2":
                            HandleOtherAccountTransfer(user);
                            break;

                        case "0":
                            isValidChoice2 = false;
                            break;

                        default:
                            Console.WriteLine("Ogiltigt val. Vänligen välj 1, 2 eller 0.");
                            Console.ReadKey();
                            break;
                    }
                } 
            }
        }

        private Currency ChooseCurrency()
        {
            while (true)
           
    {
                Console.WriteLine(" Välj valuta för beloppet:");
                Console.WriteLine( "1. SEK ");
                Console.WriteLine("2. EUR");
                Console.WriteLine("3. USD ");
                Console.WriteLine("  ");
                Console.WriteLine("Ditt val: ");

                string choice3 = Console.ReadLine() ?? string.Empty;
                        
                switch (choice3)
                {

                    case "1":
                        return Currency.SEK;
                        

                    case "2":
                        return Currency.EUR;

                    case "3":
                        return Currency.USD;

                    default:
                        Console.WriteLine("Ogiltigt val. Försök igen.");
                        break;
                }

            }

        }







        //Shows all transactions for one account
        private void ShowTransactionHistory(User user)

        {
            Console.Clear();
            Console.Write("Ange ditt kontonummer: "); 

           string accountIdInput = Console.ReadLine() ?? string.Empty;

            if (!int.TryParse(accountIdInput, out int accountId))
            {
                Console.WriteLine("Ogiltigt kontonummer. Ange bara siffror.");
                Console.ReadKey();
                return;
            }

            bool ownsAccount = false;
            
            foreach (Account account in user.Accounts)
            {
                if (account.AccountId == accountId)
                {
                    ownsAccount = true;
                    break;
                }
            }

            if (!ownsAccount)
            {
                Console.WriteLine(" Fel konto");
                Console.ReadKey();
                return;
            }
                
                // Get all transactions where this account sent or received money
            List<Transaction> history = transactionService.GetTransactionHistory(accountId);


            if (history.Count == 0)
            {

                Console.WriteLine("Inga Transaktioner hittades. ");
                Console.ReadKey();
                return;

            }

            Console.WriteLine($" Transaktioner för konto {accountId}: ");
            Console.WriteLine("-----------------------------------------");
                // Print every transaction
            foreach (Transaction transaction in history)
            {
                Console.WriteLine(
                    $"{transaction.Timestamp} | " +
                    $" {transaction.Type} | " +
                    $" {transaction.Amount :F2} | " +
                    $"Status: {transaction.Status}");
                
            }
            Console.WriteLine("Tryck på valfri tangent för att gå tillbaka.");
            Console.ReadKey();
        }


          




        private void HandleOwnAccountTransfer(User user)
        {
            try
            {
                Console.Clear();

                Console.Write("Från kontonummer: ");
                string fromaccountIdInput = Console.ReadLine() ?? string.Empty;

                if (!int.TryParse(fromaccountIdInput, out int fromAccountId))
                    
                {
                    Console.WriteLine("Ogiltigt kontonummer");
                    Console.ReadKey();
                    return;
                }

               
                Console.Write("Till kontonummer: ");
                string toaccountInput = Console.ReadLine() ?? string.Empty;

                if (!int.TryParse(toaccountInput, out int toAccountId))

                {
                    Console.WriteLine("Ogiltigt kontonummer");
                    Console.ReadKey();
                    return;
                }

                
                Console.Write("Belopp: ");
                string amountInput = Console.ReadLine() ?? string.Empty;

                if (!decimal.TryParse(amountInput, out decimal amount))
                {
                    Console.WriteLine("Ogiltigt belopp.");
                    Console.ReadKey();
                    return;
                }


                Currency currency = ChooseCurrency();

            // Make the transfer
              transactionService.AddPendingTransfer(
                  user,
                        fromAccountId,
                        toAccountId,
                        amount,
                        true,
                        currency);

                Console.WriteLine("Överföringen har lagts i kö. Status: Pending.");

                decimal amountInSek = transactionService.ConvertToSek(amount,currency);
                notificationService.TransactionNotification(user, amountInSek);
            }
            catch (Exception error)
            {
                Console.WriteLine($"Överföringen misslyckades: {error.Message}");
            }
            Console.ReadKey();
        }







        private void HandleOtherAccountTransfer(User user)
        {
            try
            {
               
                Console.Write("Från kontonummer: ");
                string fromaccountIdInput = Console.ReadLine() ?? string.Empty;

                if (!int.TryParse(fromaccountIdInput, out int fromAccountId))

                {
                    Console.WriteLine("Ogiltigt kontonummer");
                    Console.ReadKey();
                    return;
                }

               
                Console.Write("Till kontonummer: ");
                string toaccountInput = Console.ReadLine() ?? string.Empty;

                if (!int.TryParse(toaccountInput, out int toAccountId))

                {
                    Console.WriteLine("Ogiltigt kontonummer");
                    Console.ReadKey();
                    return;
                }
               
                Console.Write("Belopp: ");
                string amountInput = Console.ReadLine() ?? string.Empty;

                if (!decimal.TryParse(amountInput, out decimal amount))
                {
                    Console.WriteLine("Ogiltigt belopp.");
                    Console.ReadKey();
                    return;
                  
                }
                Currency currency = ChooseCurrency();

                // Make the transfer
                transactionService.AddPendingTransfer(
                    user,
                    fromAccountId,
                    toAccountId,
                    amount,
                    false,
                    currency);
     
                Console.WriteLine("Överföringen har lagts i kö. Status: Pending.");

                decimal amountInSek = transactionService.ConvertToSek(amount,currency);

                notificationService.TransactionNotification(user, amountInSek);// notification call if the amount is above the limit
            }
            catch (Exception error)
            {
                Console.WriteLine($"Överföringen misslyckades: {error.Message}");
              
            }
            Console.ReadKey();
        }

    
    }
}
