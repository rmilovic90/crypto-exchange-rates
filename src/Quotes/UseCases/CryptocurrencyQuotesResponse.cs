using System.Collections.Generic;

namespace CryptoExchangeRates.Quotes.UseCases
{
    public sealed class CryptocurrencyQuotesResponse
    {
        public string BaseCryptocurrencyCode { get; set; }
        public List<CurrencyQuotation> Quotes { get; set; }
    }

    public sealed class CurrencyQuotation
    {
        public string QuoteCurrencyCode { get; set; }
        public decimal Rate { get; set; }
    }
}