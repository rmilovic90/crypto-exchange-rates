using System;
using System.Collections.Generic;
using CryptoExchangeRates.Quotes.Gateways;
using CryptoExchangeRates.Quotes.Models;

namespace CryptoExchangeRates.Quotes.Infrastructure
{
    internal sealed class CoinMarketCapExchangeRatesWebService : IExchangeRatesService
    {
        public IReadOnlyList<QuoteCurrency> GetQuotesFor(CurrencyCode baseCurrencyCode)
        {
            if (baseCurrencyCode is null)
                throw new ArgumentNullException(nameof(baseCurrencyCode));

            throw new NotImplementedException();
        }
    }
}