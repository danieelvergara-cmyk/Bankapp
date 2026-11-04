using System;
using System.Collections.Generic;
using System.Text;

namespace BankApp 
{
    // To convert between currencies, use Convert(amount, from, to).
    // Example: Convert(100, Currency.EUR, Currency.SEK) converts 100 EUR to SEK.
    public class ExchangeRate
    {
        public decimal EurToSek { get; private set; } = 10.98m; // Default value for EUR to SEK exchange rate
        public decimal UsdToSek { get; private set; } = 9.74m; // Default value for USD to SEK exchange rate
        public DateOnly UpdatedAt { get; private set; } = DateOnly.FromDateTime(DateTime.Now); // The date when the exchange rates were last updated (default value is the current date)

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
            // Returns the exchange rate for the 'from' currency and multiplies it by the amount to get the value in SEK
            decimal inSek = amount * GetRate(from); 

            // Returns the exchange rate for the 'to' currency and divides the value in SEK by it to get the value in the target currency.
            // Also rounds the result to 2 decimal places for better readability and usability in financial contexts.
            return Math.Round(inSek / GetRate(to), 2); 
        }

        // Updates the exchange rate for a specific currency (EUR or USD) and sets the UpdatedAt property to the current date.
        public bool UpdateExchangeRate(Currency currency, decimal newRate)
        {
            if (newRate <= 0)
            {
                Console.WriteLine("Kan inte vara 0 eller negativt nummer!");
                return false; // Not updating if newRate is 0 or below
            }

            switch (currency)
            {
                case Currency.SEK:
                    Console.WriteLine("Kan inte ändra SEK"); // SEK is the base currency and cannot be changed
                    return false;

                case Currency.EUR:
                    EurToSek = newRate;
                    UpdatedAt = DateOnly.FromDateTime(DateTime.Now);
                    return true;


                case Currency.USD:
                    UsdToSek = newRate;
                    UpdatedAt = DateOnly.FromDateTime(DateTime.Now);
                    return true;

                default:
                    return false; // This never happens, just to avoid compilation errors
            }


        }

    }
}
