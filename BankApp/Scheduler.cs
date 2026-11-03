using System;
using System.Collections.Generic;
using System.Text;
using System.Threading; // This is for the scheduler to run every 15 minutes
using System.Threading.Tasks; // This is for the scheduler to run every 15 minutes

namespace BankApp
{
    internal class Scheduler
    {
    
        private readonly TimeSpan _processingInterval = TimeSpan.FromMinutes(15);

        private readonly TransactionService _transactionService;

        public Scheduler(TransactionService transactionService)
        {
            _transactionService = transactionService;
        }


        public void RunOnce()
        {
            _transactionService.ProcessPendingTransactions();
        }

    public async Task RunAsync()
        {
            while (true)
            {
                await Task.Delay(_processingInterval);
                RunOnce();
            }
        }
    }
}
