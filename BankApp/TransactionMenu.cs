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



        /*2.  A field that stores a TransactionService object. 
              The menu tells the program: "I cannot start without this service..*/
        public TransactionMenu(TransactionService transactionService) 
        {
            /* 3. We take the service that was just delivered to us (right side)
                 and lock it into our private toolbox (left side) so the rest of the menu can use it.*/
            this.transactionService = transactionService; 

        }

        public void RunTransferMenu()

        {

            Console.WriteLine("1. Se en logg på alla överföringar");
            Console.WriteLine("2. Överför pengar");
            Console.Write("Välj 1 eller 2:");


            string choice1 = Console.ReadLine();


            if (choice1 == "1")
            {
                    ShowTransactionHistory();
            } 
            else if (choice1 == "2")
            {
                

                Console.WriteLine("Vill du överföra pengar till:");
                Console.WriteLine("1. Eget konto");
                Console.WriteLine("2. Annan person");
                Console.Write("Välj 1 eller 2: ");

                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        HandleOwnAccountTransfer();
                        break;

                    case "2":
                        HandleOtherAccountTransfer();
                        break;

                    default:
                        Console.WriteLine("Ogiltigt val. Vänligen välj 1 eller 2.");
                        break;
                }

            } 
            else
            {
                Console.WriteLine("Ogiltigt val.");
            }

        }

        //Shows all transactions for one account
        private void ShowTransactionHistory()

        {
                Console.Write("Ange ditt kontonummer: ");  // i need try catch
                int accountId = int.Parse(Console.ReadLine());

                // Get all transactions where this account sent or received money
                List<Transaction> history = transactionService.TransactionHistory(accountId);


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
                int fromAccountId = int.Parse(Console.ReadLine());

                Console.Write("Till kontonummer: ");
                int toAccountId = int.Parse(Console.ReadLine());

                Console.Write("Belopp: ");
                decimal amount = decimal.Parse(Console.ReadLine());


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
                int fromAccountId = int.Parse(Console.ReadLine());

                Console.Write("Till kontonummer: ");
                int toAccountId = int.Parse(Console.ReadLine());

                Console.Write("Belopp: ");
                decimal amount = decimal.Parse(Console.ReadLine());

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
