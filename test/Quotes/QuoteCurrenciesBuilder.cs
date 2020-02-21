using System.Collections.Generic;
using System.Linq;
using CryptoExchangeRates.Quotes.Models;

namespace CryptoExchangeRates.Quotes
{
    internal static class QuoteCurrenciesBuilder
    {
        public static List<QuoteCurrency> QuoteCurrenciesOf(
            params (string code, decimal exchangeRate)[] quoteCurrencyCodeAndExchangeRatePairs) =>
                quoteCurrencyCodeAndExchangeRatePairs.Select(pair =>
                    QuoteCurrency.Of(
                        CurrencyCode.Of(pair.code),
                        CurrencyExchangeRate.Of(pair.exchangeRate)))
                .ToList();
    }
}