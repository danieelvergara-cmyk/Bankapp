using System;
using System.Collections.Generic;
using System.Text;
using System.Transactions;

namespace BankApp
{
    internal class Transaction
    {
        // The first ID track and generate unique auto-incrementing identifiers 
        private static int _nextId = 1; 
        public int TransactionId { get; } = Interlocked.Increment(ref _nextId);

        public int FromAccountId { get; set; }
        public int ToAccountId { get; set; }
        public decimal Amount { get; set; }
        public string TransactionStatus { get; set; }
       

         public string TransactionType { get; set; } 
         public string Direction { get; set; }


    }
}


