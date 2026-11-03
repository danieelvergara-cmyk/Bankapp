using System;
using System.Collections.Generic;
using System.Text;
using static BankApp.StatisticsService;

namespace BankApp
{
    
    internal class Transaction_menu
    {

  
        private readonly TransactionService transactionService;
        private readonly StatisticsService statistics;
        private readonly User currentUser;


        public Transaction_menu(TransactionService transactionService)
            {
                this.transactionService = transactionService;
                this.statistics = transactionService.GetStatisticsService();
                //this.currentUser = currentUser;
        }

        public void RunTransferMenuAsync()


        {

          
            Console.WriteLine("1. se en logg på alla överföringar");
            Console.WriteLine("2. Se statistik");
            Console.WriteLine("3. överför pengar");
            Console.Write(" välj 1, 2 eller 3:  ");

            string choice1 = Console.ReadLine();
            if (choice1 == "1")
            {
                 ShowTransactionHistory();
            }
            else if (choice1 == "2")
            {
                // Show statistics, but only for an admin
                ShowStatistics(currentUser);
            }


            else if (choice1 == "3")
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
                Console.WriteLine("Ogiltigt val i huvudmenyn.");
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
        private void ShowStatistics(User currentUser)
             {

            // Stop if the user is not an admin
                       if (!currentUser.IsAdmin) //True if this user is the bank owner / admin public bool IsAdmin { get; set; }
                       {
                              Console.WriteLine("Endast bankägaren kan se statistik.");
                              return;
                       }
                 // Get the five accounts that receive the most money
                   List<StatisticsService.TopAccount> topAccounts = statistics.TopRecipientAccounts(5);

                         Console.WriteLine($"Totalt överfört: {statistics.TotalTransferred()} kr");
                         Console.WriteLine($"Genomsnittligt belopp: {statistics.AverageTransactionAmount()} kr");

                         Console.WriteLine("Mest populära konton:");


                 

                         foreach (StatisticsService.TopAccount topAccount in topAccounts)
                         {
                              Console.WriteLine( $"Konto {topAccount.AccountId}: " + $"Skickat {topAccount.TotalSent} kr, " + $"Mottaget {topAccount.TotalReceived} kr");
                         }

             }     
    }
}
