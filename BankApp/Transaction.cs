using System;
using System.Collections.Generic;
using System.Text;
using System.Threading; //Needed for Interlocked


namespace BankApp
{
    public enum TransactionType // Defines a fixed list of possible TransactionType
    {
        OwnTransfer,
        TransferToOther,
        Deposit,
        Withdrawal
    }

    public enum TransactionStatus //Defines the possible states of a transaction
    {
        Pending,
        Completed,
        Failed
    }




    public class Transaction
    {
        // The first ID track and generate unique auto-incrementing identifiers 
        private static int _nextId = 0;

        public Transaction()

        {    /*Interlocked.Increment is used atomicly  instead of _nextId++,
              * if two threads create transactions at once, they cannot receive the same ID */

            TransactionId = Interlocked.Increment(ref _nextId);   
            Timestamp = DateTime.UtcNow;
        }

        public Transaction(
            
            int transactionId, 
            int fromAccountId, 
            int toAccountId,
            decimal amount,
            TransactionStatus status, 
            TransactionType type,
            DateTime timestamp)
        

        {
            TransactionId = Interlocked.Increment(ref _nextId);
            FromAccountId = fromAccountId;
            ToAccountId = toAccountId;
            Amount = amount;
            Status = status;
            Type = type;
            Timestamp = DateTime.UtcNow;
        }

        public int TransactionId { get; set; } 
        public int FromAccountId { get; set; }
        public int ToAccountId { get; set; }
        public decimal Amount { get; set; }
        public TransactionStatus Status { get; set; } 
        public TransactionType Type { get; set; }
        public DateTime Timestamp { get; set; } 

    }
}