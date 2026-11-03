using System;


namespace BankApp;


public class Account
{
    public Account(int accountid, int ownerid, string name, decimal balance, Currency currency)
    {
        if(accountid <= 0)
        {
            throw new ArgumentException("Account ID must be greater than zero.", nameof(accountid));
        }
        if(ownerid <= 0)
        {
            throw new ArgumentException("Owner ID must be greater than zero.", nameof(ownerid));
        }
        if (balance < 0)
        {
            throw new ArgumentException("Balance cannot be negative.", nameof(balance));
        }
        AccountId = accountid;
        OwnerId = ownerid;
        Name = name;
        Balance = balance;
        Currency = currency;
    }

    public int AccountId{get; set;}
    public int OwnerId{get; set;}
    public string Name{get; set;}
    public decimal Balance{get; private set;} 
    public Currency Currency{get; set;}




    public decimal Deposit(decimal amount) // lägger till pengar på kontot, 
    {
        if(amount <= 0)
        {
            throw new ArgumentException("Amount must be greater than zero.", nameof(amount));
        }
        else
        {
            Balance += amount;
        }
        return Balance;
    }
    public decimal WithDraw(decimal amount) // tar ut pengar från kontot, kollar
    {
        if(amount > 0 && amount <= Balance)
        {
            Balance -= amount;
        }
        else
        {
            throw new ArgumentException("Amount must be greater than zero and less than or equal to the balance.", nameof(amount));
        }
        return Balance;
    }

    public void PrintAccount() // tip i learned hovra over method to see how it works
    {
        Console.WriteLine($"{Name} - AccountId: {AccountId} Balance: {Balance}{Currency}");
    }
    // Metod för att visa alla konton som tillhör en användare användaren som skickas
    public static void ShowAccounts(User user)
    {
        if (user.Accounts.Count == 0)
        {
            Console.WriteLine("Du har inga konton ännu.");
            return;
        }

        foreach (Account account in user.Accounts)
        {
            account.PrintAccount();
        }
    }

    
    public void AddToUser(User user)
    {
        if (user.UserId != OwnerId)
        {
            throw new ArgumentException("Kontot tillhör inte den här användaren.");
        }

        user.Accounts.Add(this);
    }
   

}