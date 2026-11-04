using System;
using System.Collections.Generic;
using System.Text;

namespace BankApp
{
    internal class ExchangeRate
    {
        public decimal EurToSek { get; set; } = 10.98m; // Default value for EUR to SEK exchange rate
        public decimal UsdToSek { get; set; } = 9.74m; // Default value for USD to SEK exchange rate

        ExchangeRate exchangeRate = new ExchangeRate(); // Will move to main later
        public decimal GetRate(Currency currency)
        {
            switch (currency)
            {
                case Currency.SEK:
                    return 1m; // The Base Currency calue in SEK

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

    }
}
