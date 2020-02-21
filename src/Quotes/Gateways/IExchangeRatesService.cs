using System.Collections.Generic;
using CryptoExchangeRates.Quotes.Models;

namespace CryptoExchangeRates.Quotes.Gateways
{
    public interface IExchangeRatesService
    {
        IReadOnlyList<QuoteCurrency> GetQuotesFor(CurrencyCode baseCryptocurrencyCode);
    }
}