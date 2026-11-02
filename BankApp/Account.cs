using System;
using System.ComponentModel.DataAnnotations;

namespace BankApp;


public class Account
{
    public Account(int accountid, int userid, string name, decimal balance, Currency currency)
    {
        AccountId = accountid;
        UserId = userid;
        Name = name;
        Balance = balance;
        Currency = currency;
    }

    public int AccountId{get; set;}
    public int UserId{get; set;}
    public string Name{get; set;}
    public decimal Balance{get; private set;} 
    public Currency Currency{get; set;}


    public decimal Deposit(decimal amount)
    {
        if(amount > 0)
        {
            Balance += amount;
        }
        else
        {
            Console.WriteLine($"Du måste ange tal större än noll!");
        }
        return Balance;
    }
    public decimal WithDraw(decimal amount)
    {
        if(amount > 0 && amount <= Balance)
        {
            Balance -= amount;
        }
        else
        {
            Console.WriteLine($"Du måste ange tal större än noll!");
        }
        return Balance;
    }

    public static void PrintAccount(Account account) // tip i learned hovra over method to see how it works
    {
        Console.WriteLine($"Saldo: {account.Balance},{account.Currency}");
    }
   

}