using System;
using System.Collections.Generic;
using System.Text;

namespace BankApp
{
    internal class TransactionService
    {

        // denna gör att processa alla pending transactions, dvs de som är på hold. så att de kan gå från pending till själva transaktionen, så att lånet kan gå igenom.
        public void ProcessPendingTransactions()
        {
            foreach (Transaction transaction in _transactions)
            {
                if (transaction.TransactionStatus != "Pending")
                {
                    continue;
                }

                Account? fromAccount = FindAccountById(transaction.FromAccountId);
                Account? toAccount = FindAccountById(transaction.ToAccountId);

                if (fromAccount == null || toAccount == null)
                {
                    transaction.TransactionStatus = "Failed";
                    continue;
                }

                if (transaction.Amount <= 0 || transaction.Amount > fromAccount.Balance)
                {
                    transaction.TransactionStatus = "Failed";
                    continue;
                }

                fromAccount.WithDraw(transaction.Amount);
                toAccount.Deposit(transaction.Amount);
                transaction.TransactionStatus = "Completed";
            }
        }

        private Account? FindAccountById(int accountId)
        {
            foreach (User user in _userService.Users)
            {
                foreach (Account account in user.Accounts)
                {
                    if (account.AccountId == accountId)
                    {
                        return account;
                    }
                }
            }

            return null;
        }


        public void AddPendingTransaction(Transaction transaction)
        {
            transaction.TransactionStatus = "Pending";
            _transactions.Add(transaction);
        }

        private readonly UserService _userService;

        public TransactionService(UserService userService)
        {
            _userService = userService;
        }

        private readonly List<Transaction> _transactions = new();

        public void TransferToOwnAccount(int fromAccountId, int toAccountId, decimal amount)
        {
            Transaction transaction = new Transaction
            {
                FromAccountId = fromAccountId,
                ToAccountId = toAccountId,
                Amount = amount
            };

            AddPendingTransaction(transaction);
        }

        public void TransferToOtherAccount(int fromAccountId,int toAccountId, decimal amount)
        {
            Transaction transaction = new Transaction
            {
                FromAccountId = fromAccountId,
                ToAccountId = toAccountId,
                Amount = amount
            };

            AddPendingTransaction(transaction);
        }

        public List<Transaction> TransactionHistory(int accountId)
        {
            return new List<Transaction>();
        }



    }
}
