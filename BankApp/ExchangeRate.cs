using System;
using System.Collections.Generic;
using System.Text;

namespace BankApp
{
    internal class ExchangeRate
    {
        public decimal EurToSek { get; set; } = 10.98m; // Default value for EUR to SEK exchange rate
        public decimal UsdToSek { get; set; } = 9.74m; // Default value for USD to SEK exchange rate

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


    }
}
