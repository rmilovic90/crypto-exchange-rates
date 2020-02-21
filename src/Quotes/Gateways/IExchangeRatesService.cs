using System.Collections.Generic;
using System.Threading.Tasks;
using CryptoExchangeRates.Quotes.Models;

namespace CryptoExchangeRates.Quotes.Gateways
{
    public interface IExchangeRatesService
    {
        Task<IReadOnlyList<QuoteCurrency>> GetQuotesFor(CurrencyCode baseCryptocurrencyCode);
    }
}