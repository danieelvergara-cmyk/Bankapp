using System;
using System.Collections.Generic;
using System.Security.Principal;
using System.Text;

namespace BankApp
{
    internal class StatisticsService
    {

        public class TopAccount
        {
            public int AccountId { get; set; }
            public decimal TotalSent { get; set; }
            public decimal TotalReceived { get; set; }
        }
        private List<Transaction> transactions;

        public StatisticsService(List<Transaction> transactions)
        {
            this.transactions = transactions;
        }

        public decimal TotalTransferred()
        {
            decimal total = 0;

            foreach (Transaction transaction in transactions)
            {
                if (transaction.Type == TransactionType.OwnTransfer ||
                    transaction.Type == TransactionType.TransferToOther)
                {
                    total += transaction.Amount;
                }
            }

            return total;
        }

        public decimal AverageTransactionAmount()
        {
            if (transactions.Count == 0)
            {
                return 0;
            }

            decimal total = 0;

            foreach (Transaction transaction in transactions)
            {
                total += transaction.Amount;
            }

            return total / transactions.Count;
        }

        public List<TopAccount> TopRecipientAccounts(int count)
        {
            Dictionary<int, TopAccount> accountStatistics =
                new Dictionary<int, TopAccount>();

            foreach (Transaction transaction in transactions)
            {
                // Pengar skickade
                if (accountStatistics.ContainsKey(transaction.FromAccountId))
                {
                    accountStatistics[transaction.FromAccountId].TotalSent +=
                        transaction.Amount;
                }
                else
                {
                    accountStatistics[transaction.FromAccountId] = new TopAccount
                    {
                        AccountId = transaction.FromAccountId,
                        TotalSent = transaction.Amount,
                        TotalReceived = 0
                    };
                }

                // Pengar mottagna
                if (accountStatistics.ContainsKey(transaction.ToAccountId))
                {
                    accountStatistics[transaction.ToAccountId].TotalReceived +=
                        transaction.Amount;
                }
                else
                {
                    accountStatistics[transaction.ToAccountId] = new TopAccount
                    {
                        AccountId = transaction.ToAccountId,
                        TotalSent = 0,
                        TotalReceived = transaction.Amount
                    };
                }
            }

            List<TopAccount> topAccounts = new List<TopAccount>();

            foreach (var item in accountStatistics.Values)
            {
                topAccounts.Add(item);
            }

            // Sortera efter mest mottagna pengar
            topAccounts.Sort((a, b) => b.TotalReceived.CompareTo(a.TotalReceived));

            if (topAccounts.Count > count)
            {
                topAccounts = topAccounts.GetRange(0, count);
            }

            return topAccounts;
        }




    }
}