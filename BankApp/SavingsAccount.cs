using System;

namespace BankApp;

public class SavingsAccount : Account
{
    public SavingsAccount(int accountid, int userid, string name, decimal balance, Currency currency, decimal interestrate) : base(accountid, userid, name, balance, currency)
    {
        InterestRate = interestrate;
    }

     public decimal InterestRate{get;set;}


     public decimal CalculateInterest(decimal amount)
    {
        return amount * InterestRate;
    }

}
