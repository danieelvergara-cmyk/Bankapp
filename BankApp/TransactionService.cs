using System;
using System.Collections.Generic;
using System.Text;




namespace BankApp
{
    internal class TransactionService

    {

        
        
        private readonly List<Transaction> transactions = new List<Transaction>();  // Creat a memory list and stores every complited transfer.

        private readonly object _bankLock = new object();  // "Lock" prevents data corruption and calculation errors 
        private readonly List<Account> accounts;

       
        public TransactionService(List<Account> accounts)  // constractor that takes the account list from Account class
        {
            this.accounts = accounts;
        }

      

        private Account GetAccount(int accountId)
        {
            foreach (Account account in accounts)
            {
                if (account.AccountId == accountId)
                {
                    return account;
                }
            }

            return null;



        }
        public List<Transaction> TransactionHistory(int accountId)
        {
            List<Transaction> history = new List<Transaction>();
            lock(_bankLock)  
            {


            
                   foreach (Transaction transaction in transactions)
                   {
                    // Add the transaction if this account sent or received money
                      if (transaction.FromAccountId == accountId ||
                        transaction.ToAccountId == accountId) 
                      {
                          history.Add(transaction);
                      }
                   }
            }
                  return history;
        }
  
        private decimal Transfer(int fromAccountId, int toAccountId, decimal amount, TransactionType transactionType, bool SameUser)
        {
     
           

            if (amount <= 0)
            {
                throw new Exception("Beloppet måste vara större än 0.");
            }
           
            // Find both accounts in accountservis
            Account fromAccount = GetAccount(fromAccountId);
            Account toAccount = GetAccount(toAccountId);

            lock (_bankLock)   // Lock so two transfers cannot change the same balances at the same time
            {

               

                // Check that the receiving account exists
                if (fromAccount == null)
                {
                     throw new Exception($" Kontot med Id {fromAccount} finns inte");
                }

                if (toAccount == null)
                {
                     throw new Exception($" Kontot med Id {toAccount} finns inte");

                }

              /*
             * If this is a transfer between the same user's accounts,
             * both accounts must belong to the same user
               */ 
                if (SameUser && fromAccount.OwnerId != toAccount.OwnerId)   
                {
                     throw new Exception($" kontona tillhör inte samma användare.");
                }

               /*  
                If this is a transfer to another user,
                the accounts must not belong to the same user
               */
                if (!SameUser && fromAccount.OwnerId == toAccount.OwnerId)
                {
                    throw new Exception("Använd TransferToOwnAccount för egna konton.");
                }

          

                if (fromAccount.Balance < amount)
                {
                     throw new Exception("Betalningen misslyckades. Det finns inte tillräckligt med pengar på kontot.");
                }


               
                fromAccount.WithDraw(amount); // Remove money from sender
                toAccount.Deposit(amount); // Add money to receiver


                // Save the completed transfer

                Transaction transaction = new Transaction
                {
                    FromAccountId = fromAccountId,
                    ToAccountId = toAccountId,
                    Amount = amount,
                    Status = TransactionStatus.Completed,
                    Type = transactionType,
                    Timestamp = DateTime.UtcNow
                };
          
                 transactions.Add(transaction); 

            }
            decimal newBalance = fromAccount.Balance;
            return newBalance;  // Return the sender's new balance

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
