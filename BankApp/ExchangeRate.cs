using System;
using System.Collections.Generic;
using System.Text;

namespace BankApp
{
    public class ExchangeRate
    {
        public decimal EurToSek { get; private set; } = 10.98m; // Default value for EUR to SEK exchange rate
        public decimal UsdToSek { get; private set; } = 9.74m; // Default value for USD to SEK exchange rate
        public DateOnly UpdatedAt { get; private set; }

        public decimal GetRate(Currency currency)
        {
            switch (currency)
            {
                case Currency.SEK:
                    return 1m; // The Base Currency value in SEK

                case Currency.EUR:
                    return EurToSek;

                case Currency.USD:
                    return UsdToSek;

                default:
                    return 1m; // This never happens, just to avoid compilation errors

            }
        }

        // Converts an amount from one currency to another using the exchange rates defined in the ExchangeRate properties.
        // The conversion is done by first converting the amount to SEK and then converting it to the target currency.
        public decimal Convert(decimal amount, Currency from, Currency to)
        {
            decimal inSek = amount * GetRate(from); // Returns the exchange rate for the 'from' currency and multiplies it by the amount to get the value in SEK
            return inSek / GetRate(to); // Returns the exchange rate for the 'to' currency and divides the value in SEK by it to get the value in the target currency
        }

        public bool UpdateExchangeRate(Currency currency, decimal newRate)
        {
            if (newRate <= 0)
            {
                return false; // Not updating if newRate is 0 or bellow
            }

            switch (currency)
            {
                case Currency.SEK:
                    Console.WriteLine("Kan inte ändra SEK");
                    return false;

                case Currency.EUR:
                    EurToSek = newRate;
                    UpdatedAt = new DateOnly();
                    return true;


                case Currency.USD:
                    UsdToSek = newRate;
                    UpdatedAt = new DateOnly();
                    return true;

                default:
                    return false; // This never happens, just to avoid compilation errors
            }


        }

    }
}
