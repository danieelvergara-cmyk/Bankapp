using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace BankApp
{
    internal class TransactionService
    {

        /#/
        public decimal TransferToOwnAccount(int fromAccountId, int toAccountId, decimal amount)
        {

          // try and catch   

            int fromAccount= GetAccount(fromAccountId);
            int toAccount = GetAccount(toAccountId);

            if (fromAccount == null)
            {
                throw new Exception($" Kontot {fromAccount} finns inte");
            }

            if (toAccountId == null)
            {
                throw new Exception($" Kontot {toAccount} finns inte");

            }

            if(fromAccount.UserId  != toAccount.UserId) 
            {
                throw new Exception($" kontona tillhör inte samma användare.");
            }

            if (amount <= 0)
            {
                throw new Exception("Beloppet måste vara större än 0."); 
            }

            if  (fromAccount.Balance < amount)
            {
                throw new Exception("Betalningen misslyckades. Det finns inte tillräckligt med pengar på kontot."
            }

            fromAccount.Balance -= amount;
            toAccount.Balance += amount;



        }

        public decimal TransferToOtherAccount(int fromAccountId,int toAccountId, decimal amount)
        {
            decimal placeholder2 = 22;
            return placeholder2;

        }

        public List<Transaction> TransactionHistory(int accountId)
        {
            return new List<Transaction>();
        }



    }
}
