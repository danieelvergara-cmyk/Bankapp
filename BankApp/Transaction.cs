using System;
using System.Collections.Generic;
using System.Text;
using System.Transactions;

namespace BankApp
{
    public enum TransactionType
    {
        OwnTransfer,
        TransferToOther,
        Deposit,
        Withdrawal
    }

    public enum TransactionStatus
    {
        Pending,
        Completed,
        Failed
    }


    internal class Transaction
    {
        // The first ID track and generate unique auto-incrementing identifiers 
        private static int _nextId = 1;
        public int TransactionId { get; } = Interlocked.Increment(ref _nextId);

        public int FromAccountId { get; set; }
        public int ToAccountId { get; set; }
        public decimal Amount { get; set; }

        public TransactionStatus Status { get; set; }
        public TransactionType Type { get; set; }

        public DateTime Timestamp { get; set; } = DateTime.UtcNow;


    }
}