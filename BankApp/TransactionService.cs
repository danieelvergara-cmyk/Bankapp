using System;
using System.Collections.Generic;
using System.Text;

namespace BankApp
{
    internal class TransactionService
    {
        public decimal TransferToOwnAccount(int fromAccountId, int toAccountId, decimal amont)
        {
            decimal placeholder = 22; // to change later
            return placeholder;
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
