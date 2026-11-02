using System;
using System.Collections.Generic;
using System.Text;

namespace BankApp
{
    
    internal class Transaction_menu
    {


            private readonly TransactionService transactionService;

            public Transaction_menu(TransactionService transactionService)
            {
                this.transactionService = transactionService;
            }

        public void RunTransferMenuAsync()


        {

            Console.WriteLine("1. överför pengar");
            Console.WriteLine("2. se en logg på alla överföringar");
            Console.Write(" välj 1 eller 2:  ");

            string choice1 = Console.ReadLine();
            if (choice1 == "2")
            {
                 ShowTransactionHistory();
            }

            else if (choice1 == "1")
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


            private void ShowTransactionHistory()

            {
                  Console.Write("Ange ditt kkontonummer: ");
                  int accountId = int.Parse(Console.ReadLine());

                  List<Transaction> history = transactionService.TransactionHistory(accountId);


                if (history.Count == 0)
                {

                    Console.WriteLine("Inga Transaktioner hittades. ");
                     return;

                }

                Console.WriteLine(" Dina transaktioner: ");

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

                    decimal newBalance = transactionService.TransfertoOtherAccount( fromAccountId,  toAccountId, amount);

                    Console.WriteLine($"Överföringen lyckades. Nytt saldo: {newBalance}");
                }
                catch (Exception error)
                {
                    Console.WriteLine($"Överföringen misslyckades: {error.Message}");
                }
            }
        }
    }
