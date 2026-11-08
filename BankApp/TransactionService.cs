using System;
using System.Collections.Generic;






namespace BankApp
{
    public class TransactionService

    {    //=============================================================================//
         //    Fields and Dependencies                                                                         //
         //============================================================================//

        // Stores all transfers: pending, completed, and failed.
        private readonly List<Transaction> transactions = new List<Transaction>();

        // Protects accounts and transactions from concurrent changes.
        private readonly object _bankLock = new object();  
        private readonly NotificationService notificationService;
        private readonly ExchangeRate exchangeRate;
        private readonly UserService userService;

        //==============================================================================//
        //   constructor                                                                           //
        //==============================================================================//
        public TransactionService(
            UserService userService,
            ExchangeRate exchangeRate,
            NotificationService notificationService) // constructor

        {
            this.userService = userService; 
            this.notificationService = notificationService;
            this.exchangeRate = exchangeRate;

        }
        //=============================================================================//                                                                              
        //                                                                                 
        //=============================================================================//


                                      // Creates a transfer that will be processed later.//
        public void AddPendingTransfer(
            User user,
            int fromAccountId,
            int toAccountId,
            decimal amount,
            bool sameUser,
            Currency currency)
        {

            // Find both accounts in the account service.
            Account? fromAccount = GetAccount(fromAccountId);
            Account? toAccount = GetAccount(toAccountId);

            if (fromAccount == null)
            {
                throw new InvalidOperationException($" Kontot med Id {fromAccountId} finns inte");
            }

            // The logged-in user must own the account the money is sent from.
            if (fromAccount.OwnerId != user.UserId)
            {
                throw new InvalidOperationException(
                    "Överföringen nekades eftersom kontot inte tillhör den inloggade användaren.");
            }

            if (toAccount == null)
            {
                throw new InvalidOperationException($" Kontot med Id {toAccountId} finns inte");

            }
            // A user cannot transfer money to the same account.
            if (fromAccount.AccountId == toAccount.AccountId)  
            {
                throw new InvalidOperationException($"Kan inte överföra till samma konto");
            }


            /*  For own transfers,
             *  both accounts must belong to the same user */
            if (sameUser && fromAccount.OwnerId != toAccount.OwnerId)
            {
                throw new InvalidOperationException($" kontona tillhör inte samma användare.");
            }

            /*  For transfers to another user, 
             *  the accounts must not belong to the same user. */
            if (!sameUser && fromAccount.OwnerId == toAccount.OwnerId)
            {
                throw new InvalidOperationException("Båda kontona tillhör dig." +
                    " Välj alternativet 'Eget konto' i menyn för att göra denna överföring.");
            }

            if (amount <= 0)
            {
                throw new ArgumentException("Beloppet måste vara större än 0.");
            }
            decimal amountToWithdraw = exchangeRate.Convert(amount, currency, fromAccount.Currency);
            decimal amountToDeposit = exchangeRate.Convert(amount, currency, toAccount.Currency);

            if (amountToWithdraw <= 0 || amountToDeposit <= 0)
            {
                throw new InvalidOperationException(" Beloppet är för litet efter valutaväxling.");
            }

            if (fromAccount.Balance < amountToWithdraw)

            {
                throw new InvalidOperationException(
                    "Betalningen misslyckades. Det finns inte tillräckligt med pengar på kontot.");
            }

            // Decide whether this is a transfer between
            // the user's own accounts or to another user.
            TransactionType type;

            if (sameUser)
            {
                type = TransactionType.OwnTransfer;
            }
            else
            {
                type = TransactionType.TransferToOther;
            }


            // Create the pending transaction.
            Transaction transaction = new Transaction
            {
                FromAccountId = fromAccountId,
                ToAccountId = toAccountId,
                Amount = amount,
                Currency = currency,
                Status = TransactionStatus.Pending,
                Type = type
            };

            lock (_bankLock)    // Add the transaction safely if other threads use the service.
            {
                transactions.Add(transaction);

            }
        }


        /* Finds an account by its ID, or returns null if it does not exist.
         * Because null is possible, ? is used */
        private Account? GetAccount(int accountId)   
        {
            lock (_bankLock)
            {
                foreach (User user in userService.Users)

                {     foreach (Account account in user.Accounts)
                    {

                        if (account.AccountId == accountId)
                        {
                            return account;
                        }
                    }
                }
            }

            return null;



        }
        //======================================================


        // Returns all transactions where the account sent or received money.
        public List<Transaction> GetTransactionHistory(int accountId)
        {
            List<Transaction> history = new List<Transaction>();
            lock (_bankLock)
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

        //================================================

        public decimal ConvertToSek(decimal amount, Currency currency)
        {
            return exchangeRate.Convert(amount, currency, Currency.SEK);
        }

        //===============================

        // Performs the actual transfer.
        private decimal Transfer(

            int fromAccountId,
            int toAccountId,
            decimal amount,
            Currency currency,
            TransactionType transactionType,
            bool sameUser,
            Transaction? existingTransaction = null)
        {
            if (amount <= 0)
            {
                throw new Exception(
                    "Beloppet måste vara större än 0.");
            }
           
            Account? fromAccount = GetAccount(fromAccountId);
            Account? toAccount = GetAccount(toAccountId);


            // Check that the receiving account exists
            if (fromAccount == null)
            {
                throw new InvalidOperationException($" Kontot med Id {fromAccountId} finns inte");
            }

            if (toAccount == null)
            {
                throw new InvalidOperationException($" Kontot med Id {toAccountId} finns inte");
            }

             decimal amountToWithdraw = exchangeRate.Convert(amount, currency, fromAccount.Currency);
             decimal amountToDeposit = exchangeRate.Convert(amount, currency, toAccount.Currency);


            /* Remove money from the sender
             * and add it to the receiver.*/
            fromAccount.WithDraw(amountToWithdraw);
            toAccount.Deposit(amountToDeposit);


            /* Reuse the pending transaction if one was supplied;
              otherwise create a new one.*/
            Transaction transaction = existingTransaction ?? new Transaction
                {
                    FromAccountId = fromAccountId,
                    ToAccountId = toAccountId,
                    Amount = amount,
                    Currency = currency,
                    Type = transactionType

                };

                 // Mark the transfer as completed.
                transaction.Status = TransactionStatus.Completed;
                transaction.Timestamp = DateTime.UtcNow;

                // Save new completed transfers in the history.
                if (existingTransaction == null)
                {
                    transactions.Add(transaction);
                }
                decimal newBalance = fromAccount.Balance;
                return newBalance; // Return the sender's new balance
        }  
        
        //==========================================


        // Processes all pending transfers and marks them completed or failed.
        public void ProcessPendingTransactions()
        {
            lock (_bankLock)
            {
                foreach (Transaction transaction in transactions)
                {
                    // Skip transactions that have already been processed.
                    if (transaction.Status != TransactionStatus.Pending)
                    { 
                        continue;
                    }
                    

                    try
                    {  // Own transfers are between accounts with the same owner.
                        bool sameUser = transaction.Type == TransactionType.OwnTransfer;

                        // Try to complete the pending transfer.
                        Transfer(
                           transaction.FromAccountId,
                           transaction.ToAccountId,
                           transaction.Amount,
                           transaction.Currency,
                           transaction.Type,
                           sameUser,
                           transaction);

                    }

                    catch (Exception error) 
                    {
                        // Mark the transaction as failed if the transfer cannot be completed.
                            transaction.Status = TransactionStatus.Failed;
                            transaction.FailureReason = error.Message;
                    }



                    // Notifiera endast efter en genomförd överföring.
                    if (transaction.Status == TransactionStatus.Completed)
                    {
                        try
                        {
                            Notifysender(transaction);
                        }
                        catch (Exception error)
                        {
                            Console.WriteLine($"Överföringen är genomförd, men notifikationen misslyckades: {error.Message}");
                        }
                    }
                }
            }

        }
        //===========================

        private void Notifysender(Transaction transaction)
        {
            Account? fromAccount = GetAccount(transaction.FromAccountId);
            if (fromAccount == null)
            {
                return;
            }

            // Find the specific user who owns this account
            foreach (User user in userService.Users)

            {
                if (user.UserId == fromAccount.OwnerId)
                {

                    notificationService.TransactionNotification(
                        user,
                        transaction.Amount,
                        transaction.Currency);

                    return;
                }

            }

        }



    }
}
