using System;
using System.Collections.Generic;
using System.Transactions;


namespace BankApp
{
    
    internal class Transaction_menu
    {

  
        private readonly TransactionService transactionService;
        private readonly StatisticsService statistics;

        public Transaction_menu(TransactionService transactionService) ///11
        {
            this.transactionService = transactionService;
            this.statistics = transactionService.GetStatisticsService();
        }

        public void RunTransferMenu()


        {

          
            Console.WriteLine("\n1. se en logg på alla överföringar\n2. överför pengar ");  
            Console.Write(" välj 1 eller 2:  ");

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
                  Console.Write("Ange ditt kkontonummer: ");
                  int accountId = int.Parse(Console.ReadLine());

                 // Get all transactions where this account sent or received money
                 List<Transaction> history = transactionService.TransactionHistory(accountId);


                if (history.Count == 0)
                {

                    Console.WriteLine("Inga Transaktioner hittades. ");
                     return;

                }

                Console.WriteLine(" Dina transaktioner: ");


                  // Print every transaction
                  foreach (Transaction transaction in history)
                  {
                        Console.WriteLine($"{transaction.Timestamp} : {transaction.Type}) " + $" {transaction.Amount} kr - {transaction.Status}");
                
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
                    decimal newBalance = transactionService.TransferToOwnAccount(fromAccountId, toAccountId, amount);

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
 
                    decimal newBalance = transactionService.TransfertoOtherAccount(fromAccountId, toAccountId, amount);
     
                    Console.WriteLine($"Överföringen lyckades. Nytt saldo: {newBalance}");
            }
            catch (Exception error)
            {
                Console.WriteLine($"Överföringen misslyckades: {error.Message}");
            }
        }



        // Shows statistics, but only if the current user is an admin
          private void ShowStatistics()
               {

         
                 // Get the five accounts that receive the most money
                     List<StatisticsService.TopAccount> topAccounts = statistics.TopRecipientAccounts(7);

                     Console.WriteLine($"Totalt överfört: {statistics.TotalTransferred()} kr");
                     Console.WriteLine($"Genomsnittligt belopp per transaktion: {statistics.AverageTransactionAmount()} kr");
                     Console.WriteLine("Mest populära konton:");


                     foreach (StatisticsService.TopAccount topAccount in topAccounts)
                     {
                          Console.WriteLine($" {topAccount.AccountId}: " + $"Skickat {topAccount.TotalSent} kr, " + $"Mottaget {topAccount.TotalReceived} kr");
               
                          List<Transaction> accountTransactions = transactionService.TransactionHistory(topAccount.AccountId);
                    
                
                          if (accountTransactions.Count == 0)
                          {
                               Console.WriteLine("Inga transaktioner loggade");
                               continue;
                          }
                          foreach (Transaction transaction in accountTransactions)
                          {
                               Console.WriteLine($"   {transaction.Timestamp} | {transaction.Type} | {transaction.Amount} kr | Status: {transaction.Status}");
                          }
                        


                    

            
                     }

          }     
    }
}
