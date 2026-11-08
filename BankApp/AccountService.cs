using System;

namespace BankApp;


public class AccountService
{
    private int _nextAccountId = 1;
    private const decimal annualInterestRate = 3m; // Default interest rate for savings accounts
    public Account CreateAccount(User user, string name, Currency currency)
    {
        Account account = new Account(_nextAccountId++, user.UserId, name, 0m, currency);
        account.AddToUser(user); 
        return account;
    } 

    public SavingsAccount CreateSavingsAccount(User user, string name, Currency currency)
    {
        SavingsAccount savingsAccount = new SavingsAccount(_nextAccountId++, user.UserId, name, 0m, currency, annualInterestRate);
        savingsAccount.AddToUser(user); 
        return savingsAccount;
    }

    public void PrintSavingsInterestEstimate(SavingsAccount savingsAccount,decimal depositAmount)
    {
        decimal interest = savingsAccount.CalculateYearlyInterest(depositAmount);

        Console.WriteLine($"Uppskattad ränta efter ett år: {interest:F2} {savingsAccount.Currency}");
    }

    public void DepositToSavingsAccount(SavingsAccount savingsAccount, decimal amount)
    {
        // Calculate the interest for the deposit amount
        decimal interest =
            savingsAccount.CalculateYearlyInterest(amount);

        // Show the information to the user before making the deposit
        Console.WriteLine($"Insättning: {amount:F2} {savingsAccount.Currency}");
        Console.WriteLine($"Årsränta: {savingsAccount.AnnualInterestRate}%");
        Console.WriteLine($"Beräknad ränta efter ett år: {interest:F2} {savingsAccount.Currency}");
        Console.WriteLine($"Totalt efter ett år: {amount + interest:F2} {savingsAccount.Currency}");

        // Puts the deposit amount into the savings account
        savingsAccount.Deposit(amount);
    }
}