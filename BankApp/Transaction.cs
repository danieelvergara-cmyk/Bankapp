using System;
using System.Collections.Generic;
using System.Text;
using System.Transactions;

namespace BankApp
{
    internal class Transaction
    {
        public int TransactionId { get; set; }
        public int FromAccountId { get; set; }
        public int ToAccountId { get; set; }
        public decimal Amount { get; set; }
        public string TransactionStatus { get; set; }
        private decimal LargeTransactionLimit { get; set; }

         public string TransactionType { get; set; } 
         public string Direction { get; set; }


    }
}


