using System;

namespace BankApp;

public class SavingsAccount : Account
{
    public SavingsAccount(int accountid, int ownerid, string name, decimal balance, Currency currency, decimal annualInterestRate) : base(accountid, ownerid, name, balance, currency)
    {
        AnnualInterestRate = annualInterestRate;
    }

    // Årlig ränta i procent. Till exempel betyder 5m en ränta på 5 % per år.
    public decimal AnnualInterestRate { get; private set; }

    // Räknar ut den uppskattade räntan på ett insatt belopp under ett år.
    // Exempel: 1 000 kr med 5 % ränta ger 1 000 * (5 / 100) = 50 kr.
    // Metoden räknar bara ut en uppskattning; den ändrar inte kontots saldo.
    public decimal CalculateYearlyInterest(decimal amount) 
    {
        return amount * (AnnualInterestRate / 100m);
    }
    
    
}
