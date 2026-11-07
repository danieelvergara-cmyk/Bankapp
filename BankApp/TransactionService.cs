using System;
using System.Collections.Generic;





namespace BankApp
{
    public class TransactionService

    {


        // Creates an in-memory list  of all transfers: pending, completed, or failed.
        private readonly List<Transaction> transactions = new List<Transaction>();

       
        private readonly ExchangeRate exchangeRate;

        private readonly UserService userService;

        public TransactionService(UserService userService, ExchangeRate exchangeRate ) // constractor

        {
            this.userService = userService;
            this.exchangeRate = exchangeRate;


        }



        // Lock used to protect accounts and the transaction list from concurrent changes
        private readonly object _bankLock = new object();  // "Lock" prevents data corruption and calculation errors 

        // Reference to the accounts managed by the bank.
        //private readonly List<Account> accounts; //Stores a reference to the account list supplied to the constructor.

        // Constructor receives the account list when the service is created
        //public TransactionService(List<Account> accounts)
        //{ 
        //    this.accounts = accounts;
        //}

        
        // Creates a pending transfer that can be processed later.
        public void AddPendingTransfer(
                  int fromAccountId,
                  int toAccountId,
                  decimal amount,
                  bool sameUser,
                  Currency curency)
        {
            if (amount <= 0)
            {
                throw new Exception("Beloppet måste vara större än 0.");
            }

            // Decide whether this is a transfer between the user's own accounts or to another user.
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
                Currency = curency,
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
            foreach (User user in userService.Users)

            {     foreach (Account account in user.Accounts)
                {

                    if (account.AccountId == accountId)
                    {
                        return account;
                    }
                }
                  
            }

            return null;



        }

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


        public decimal ConvertToSek(decimal amount, Currency currency)
        {
            return exchangeRate.Convert(amount, currency, Currency.SEK);
        }

        // Performs the actual transfer.
        private decimal Transfer(

            User user,
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
                throw new ArgumentException("Beloppet måste vara större än 0.");
            }




                // Find both accounts in the account service.
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

            // Checks that the sender account belongs to the logged-in user.
            
            if (fromAccount.OwnerId != user.UserId)
                {
                    throw new InvalidOperationException(
                        "Överföringen nekades eftersom kontot inte tillhör den inloggade användaren.");
                }

            // A user cannot transfer money to the same account.
            if (fromAccount.AccountId == toAccount.AccountId)
                {
                    throw new InvalidOperationException($"kan inte övervföra till samma konto");
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
                    throw new InvalidOperationException("Använd TransferToOwnAccount för egna konton.");
                }

            decimal amountToWithdraw = exchangeRate.Convert(amount, currency, fromAccount.Currency);
            decimal amountToDeposit = exchangeRate.Convert(amount, currency, toAccount.Currency);

            if (amountToWithdraw <= 0 || amountToDeposit <= 0)
            {
                throw new InvalidOperationException(" Beloppet är för litet efter valutaväxling.");
            }

            if ( fromAccount.Balance < amountToWithdraw)

               {   
                    throw new InvalidOperationException(
                        "Betalningen misslyckades. Det finns inte tillräckligt med pengar på kontot.");
                }

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





        // Processes all pending transfers and marks them completed or failed.
        public void ProcessPendingTransactions()
        {
            lock (_bankLock)
            {
                foreach (Transaction transaction in transactions)
                {
                    // Skip transactions that have already been processed.
                    if (transaction.Status != TransactionStatus.Pending)
                        continue;

                    

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

                            Console.WriteLine(
                              $"Transaktion{transaction.TransactionId} misslyckades:  {error.Message}");
                       
                    }
                }
            }

        }



        // Transfers money between two accounts owned by the same user
        public decimal TransferToOwnAccount(
            User user,
            int fromAccountId,
            int toAccountId, 
            decimal amount,
            Currency currency)
        {
            /* Lock so two transfers cannot change the same balances at the same time were it happend*/
            lock (_bankLock) 
            {
                
                return Transfer(
                    user,
                  fromAccountId,
                  toAccountId,
                  amount,
                  currency,
                  TransactionType.OwnTransfer,
                  sameUser: true);

                
            }
            
        }



        // Transfers money to another user's account
        public decimal TransfertoOtherAccount(
            User user,
            int fromAccountId,
            int toAccountId,
            decimal amount,
            Currency currency)
        {
            /* Lock so two transfers cannot change the same balances at the same time were it happend*/
            lock ( _bankLock) 
            {
                return Transfer(
                 user,
                 fromAccountId,
                 toAccountId,
                 amount,
                 currency,
                 TransactionType.TransferToOther,
                 sameUser: false);

               
            }

        }


    }
}
