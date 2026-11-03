using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace BankApp
{
    internal class TransactionService
    {
      
        public StatisticsService GetStatisticsService () 
        {
            return new StatisticsService(transactions); }


        // Stores every transfer made in the bank
        private List<Transaction> transactions = new List<Transaction>();

        //private List<Account> accounts = new List<Account>();


        // Lock object: makes sure only one thread can change balances at a time
        private readonly object _bankLock = new object();

        private Account GetAccount(int fromAccountId)
        {

           
            
                return accounts.Find(a => a.AccountId == fromAccountId);

            
        }
        public List<Transaction> TransactionHistory(int accountId)
        {
            List<Transaction> history = new List<Transaction>();

            foreach (Transaction transaction in transactions)
            {
                // Add the transaction if this account sent or received money
                if (transaction.FromAccountId == accountId ||
                    transaction.ToAccountId == accountId)
                {
                    history.Add(transaction);
                }
            }

            return history;
        }
  
        private decimal Transfer(int fromAccountId, int toAccountId, decimal amount, TransactionType transactionType, bool SameUser)
        {
            // Find both accounts
            Account fromAccount = GetAccount(fromAccountId);
            Account toAccount = GetAccount(toAccountId);

            //decimal balance = fromAccount; // connect to the users bak



            // Check that the receiving account exists
            if (fromAccount == null)
            {
                throw new Exception($" Kontot med Id {fromAccountId} finns inte");
            }

            if (toAccount == null)
            {
                throw new Exception($" Kontot med Id {toAccount} finns inte");

            }

            /*
             * If this is a transfer between the same user's accounts,
             * both accounts must belong to the same user
             */
            if (SameUser && fromAccount.UserId != toAccount.UserId)
            {
                throw new Exception($" kontona tillhör inte samma användare.");
            }

            /*  
                If this is a transfer to another user,
                the accounts must not belong to the same user
             */
            if (!SameUser && fromAccount.UserId == toAccount.UserId)
            {
                throw new Exception("Använd TransferToOwnAccount för egna konton.");
            }

            if (amount <= 0)
            {
                throw new Exception("Beloppet måste vara större än 0.");
            }

            if (fromAccount.Balance < amount)
            {
                throw new Exception("Betalningen misslyckades. Det finns inte tillräckligt med pengar på kontot.");
            }


            // Lock so two transfers cannot change the same balances at the same time
            lock (_bankLock)
            {

                // Remove money from sender
                fromAccount.WithDraw(amount);

                // Add money to receiver
                toAccount.Deposit(amount);


                // Save the completed transfer
                Transaction transaction = new Transaction
                {
                    
                    FromAccountId = fromAccountId,
                    ToAccountId = toAccountId,
                    Amount = amount,
                    Status = TransactionStatus.Completed,
                    Type = TransactionType.OwnTransfer,
                    Timestamp = DateTime.UtcNow
                    
                };

                transactions.Add(transaction);

            }

            // Return the sender's new balance
            return fromAccount.Balance;

        }


        // Transfers money between two accounts owned by the same user
        public decimal TransferToOwnAccount(int fromAccountId, int toAccountId, decimal amount)
        {
          var result = Transfer(fromAccountId, toAccountId, amount, TransactionType.OwnTransfer, SameUser: true);

            return result;
        }

        // Transfers money to another user's account
        public decimal TransfertoOtherAccount(int fromAccountId, int toAccountId, decimal amount)
        {
    
           var result = Transfer( fromAccountId, toAccountId, amount, TransactionType.TransferToOther, SameUser: false);

           return result;
        }


    }
}
