using System;
using System.Collections.Generic; // Gives access to List<T>



namespace BankApp
{
    
    public class TransactionMenu
    {
        /*Dependency injection through a constructor.It lets Transaction_menu use a TransactionService object.
           Instead of creating its own service, the menu asks for one from the outside.


           1.  A field that stores a TransactionService object. 
               It stores a reference to the service the menu will use (like a private toolbox).*/
        private readonly TransactionService transactionService;

        string errorMessage = "";

        /*2.  A field that stores a TransactionService object. 
              The menu tells the program: "I cannot start without this service..*/
        public TransactionMenu(
            TransactionService transactionService)
            //NotificationService notificationService)
        {
            /* 3. We take the service that was just delivered to us (right side)
                 and lock it into our private toolbox (left side) so the rest of the menu can use it.*/
            this.transactionService = transactionService;
            

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
                    Console.WriteLine("0. Tillbaka");
                    Console.WriteLine(" ----------------------- ");
                    Console.WriteLine("              ");
                    Console.Write("\nVälj ett alternativ:");

                    errorMessage = UserInterface.ShowErrorMessage(errorMessage);

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
                            errorMessage = "Ogiltig input. Skriv in ett tal mellan 0-2.";
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
                    Console.WriteLine("0. Tillbaka");
                    Console.WriteLine(" ");
                    Console.Write("\nVälj ett alternativ:");

                    errorMessage = UserInterface.ShowErrorMessage(errorMessage);

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
                            errorMessage = "Ogiltig input. Skriv in ett tal mellan 0-2.";
                            break;
                    }
                } 
            }
        }

        private Currency ChooseCurrency()
        {
            Console.WriteLine(" Välj valuta för beloppet:");
            Console.WriteLine("1. SEK ");
            Console.WriteLine("2. EUR");
            Console.WriteLine("3. USD ");
            Console.WriteLine("  ");
            Console.Write("\nVälj ett alternativ:");

            while (true)
            {

                errorMessage = UserInterface.ShowErrorMessage(errorMessage);


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
                        errorMessage = "Ogiltig input. Skriv in ett tal mellan 1-3.";
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
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Ogiltigt kontonummer. Ange bara siffror.");
                Console.ResetColor();
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
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(" Fel konto");
                Console.ResetColor();
                Console.ReadKey();
                return;
            }
                
                // Get all transactions where this account sent or received money
            List<Transaction> history = transactionService.GetTransactionHistory(accountId);


            if (history.Count == 0)
            {
                Console.WriteLine("Inga transaktioner hittades.");
                Console.ReadKey();
                return;

            }

            Console.WriteLine($" Transaktioner för konto {accountId}: ");
            Console.WriteLine("---------------------------------------------------------------------");
            Console.WriteLine("    ");

                // Print every transaction
            foreach (Transaction transaction in history)
            {

                string directionDetails = "";
                if (transaction.FromAccountId == accountId)
                {
                    directionDetails = $"skickat till konto {transaction.ToAccountId}";
                }
                else if (transaction.ToAccountId == accountId) 
                {
                    directionDetails = $"Mottaget från konto {transaction.FromAccountId}"; 
                }

                Console.WriteLine(
                    $"{transaction.Timestamp} | " +
                    $" {transaction.Type} | " +
                    $" {transaction.Amount :F2} {transaction.Currency}| " +
                    $" {directionDetails} "+

                    $"Status: {transaction.Status}");

                if (transaction.Status == TransactionStatus.Failed) // If the transaction failed, show the reason
                {
                    Console.WriteLine($"Orsak till misslyckad transaktion: ");
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine(transaction.FailureReason);
                    Console.ResetColor();
                }


            }
            Console.WriteLine("            ");
            Console.WriteLine("------------------------------------------------------------------------");
            Console.WriteLine("Tryck på valfri tangent för att gå tillbaka.");
            Console.ReadKey();
        }

        private void HandleOwnAccountTransfer(User user)
        {
            try
            {
                //Console.Clear();

                Console.Write("Från kontonummer: ");
                string fromaccountIdInput = Console.ReadLine() ?? string.Empty;

                if (!int.TryParse(fromaccountIdInput, out int fromAccountId))
                    
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Ogiltigt kontonummer");
                    Console.ResetColor();
                    Console.ReadKey();
                    return;
                }

               
                Console.Write("Till kontonummer: ");
                string toaccountInput = Console.ReadLine() ?? string.Empty;

                if (!int.TryParse(toaccountInput, out int toAccountId))

                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Ogiltigt kontonummer");
                    Console.ResetColor();
                    Console.ReadKey();
                    return;
                }
              

                Console.Write("Belopp: ");
                string amountInput = Console.ReadLine() ?? string.Empty;

                if (!decimal.TryParse(amountInput, out decimal amount))
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Ogiltigt belopp.");
                    Console.ResetColor();
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

            }
            catch (Exception error)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Överföringen misslyckades: {error.Message}");
                Console.ResetColor();
            }
            Console.ReadKey();
        }

        private void HandleOtherAccountTransfer(User user)
        {
            try
            {
                Console.Clear();

                Console.Write("Från kontonummer: ");
                string fromaccountIdInput = Console.ReadLine() ?? string.Empty;

                if (!int.TryParse(fromaccountIdInput, out int fromAccountId))

                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Ogiltigt kontonummer");
                    Console.ResetColor();
                    Console.ReadKey();
                    return;
                }

               
                Console.Write("Till kontonummer: ");
                string toaccountInput = Console.ReadLine() ?? string.Empty;

                if (!int.TryParse(toaccountInput, out int toAccountId))

                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Ogiltigt kontonummer");
                    Console.ResetColor();
                    Console.ReadKey();
                    return;
                }
               
                Console.Write("Belopp: ");
                string amountInput = Console.ReadLine() ?? string.Empty;

                if (!decimal.TryParse(amountInput, out decimal amount))
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Ogiltigt belopp.");
                    Console.ResetColor();
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

               
            }
            catch (Exception error)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Överföringen misslyckades: {error.Message}");
                Console.ResetColor();

            }
            Console.ReadKey();
        }

    
    }
}
