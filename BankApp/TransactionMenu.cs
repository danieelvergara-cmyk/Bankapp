using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates; // Gives access to List<T>


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



        /*2.  A field that stores a TransactionService object. 
              The menu tells the program: "I cannot start without this service..*/
        public TransactionMenu(TransactionService transactionService)
        {
            /* 3. We take the service that was just delivered to us (right side)
                 and lock it into our private toolbox (left side) so the rest of the menu can use it.*/
            this.transactionService = transactionService;

        }

        public void ShowTransactionMenu(User user)
        {
            bool isValidChoice = false;

            do
            {

                Console.WriteLine("1. Se en logg på alla överföringar");
                Console.WriteLine("2. Överför pengar");
                Console.Write("Välj 1, 2 eller 3:");


                string choice = Console.ReadLine() ?? string.Empty; // // pssible null value


                switch (choice)
                {
                    case "1":
                        ShowTransactionHistory();
                        break;

                    case "2":
                        TransfarTo();
                        break;

                    case "3":
                        isValidChoice = true;

                        continue; 

                    default:
                        Console.WriteLine("Ogiltigt val. Vänligen välj 1 eller 2.");
                        break;
                }

            } while (!isValidChoice);
        }

            public void TransfarTo()

            {
                bool isValidChoice2 = false;

                do
                {


                    Console.WriteLine("Vill du överföra pengar till:");
                    Console.WriteLine("1. Eget konto");
                    Console.WriteLine("2. Annan person");
                    Console.Write("3.Exit ");
                    Console.Write("Välj 1, 2 eller 3:");
                   

                    string choice2 = Console.ReadLine() ?? string.Empty;

                    switch (choice2)
                    {
                        case "1":
                            HandleOwnAccountTransfer();
                            break;

                        case "2":
                            HandleOtherAccountTransfer();
                            break;

                        case "3":
                            isValidChoice2 = true;
                            break;

                    default:
                            Console.WriteLine("Ogiltigt val. Vänligen välj 1 eller 2.");
                            break;
                    }
                } while (!isValidChoice2);
        }

        //Shows all transactions for one account
        private void ShowTransactionHistory()

        {
              Console.Write("Ange ditt kontonummer: "); 
            
              string accountIdInput = Console.ReadLine() ?? string.Empty;

            if (!int.TryParse(accountIdInput, out int accountId))
            {
                Console.WriteLine("Ogiltigt kontonummer. Ange bara siffror.");
            }

                // Get all transactions where this account sent or received money
                List<Transaction> history = transactionService.GetTransactionHistory(accountId);


            if (history.Count == 0)
            {

                Console.WriteLine("Inga Transaktioner hittades. ");
                    return;

            }

            Console.WriteLine(" Dina transaktioner är: ");


                // Print every transaction
                foreach (Transaction transaction in history)
                {
                    Console.WriteLine(
                        $"{transaction.Timestamp} " +
                        $": {transaction.Type}) " +
                        $" {transaction.Amount} kr - " +
                        $"{transaction.Status}");
                
                }
        }

        private void HandleOwnAccountTransfer()
        {
            try
            {
                Console.Write("Från kontonummer: ");
                string fromaccountIdInput = Console.ReadLine() ?? string.Empty;

                if (!int.TryParse(fromaccountIdInput, out int fromAccountId))
                    
                {
                    Console.WriteLine("Ogiltigt kontonummer");
                    return;
                }


                Console.Write("Till kontonummer: ");
                string toaccountInput = Console.ReadLine() ?? string.Empty;

                if (!int.TryParse(toaccountInput, out int toAccountId))

                {
                    Console.WriteLine("Ogiltigt kontonummer");
                    return;
                }
               

                string amountInput = Console.ReadLine() ?? string.Empty;

                if (!int.TryParse(amountInput, out int amount))
                {
                    Console.Write("Belopp: ");
                }


            // Make the transfer
                decimal newBalance = transactionService.TransferToOwnAccount(
                        fromAccountId,
                        toAccountId,
                        amount);

                Console.WriteLine($"Överföringen lyckades. Nytt saldo: {newBalance}");
            }
            catch (Exception error)
            {
                Console.WriteLine($"Överföringen misslyckades: {error.Message}");
            }
        }

        private void HandleOtherAccountTransfer()
        {
            try
            {
                Console.Write("Från kontonummer: ");
                string fromaccountIdInput = Console.ReadLine() ?? string.Empty;

                if (!int.TryParse(fromaccountIdInput, out int fromAccountId))

                {
                    Console.WriteLine("Ogiltigt kontonummer");
                    return;
                }


                Console.Write("Till kontonummer: ");
                string toaccountInput = Console.ReadLine() ?? string.Empty;

                if (!int.TryParse(toaccountInput, out int toAccountId))

                {
                    Console.WriteLine("Ogiltigt kontonummer");
                    return;
                }


                string amountInput = Console.ReadLine() ?? string.Empty;

                if (!int.TryParse(amountInput, out int amount))
                {
                    Console.Write("Belopp: ");
                }

                // Make the transfer
                decimal newBalance = transactionService.TransfertoOtherAccount(
                        
                    fromAccountId,
                    toAccountId,
                    amount);
     
                Console.WriteLine($"Överföringen lyckades. Nytt saldo: {newBalance}");
            }
            catch (Exception error)
            {
                Console.WriteLine($"Överföringen misslyckades: {error.Message}");
            }
        }

    
    }
}
