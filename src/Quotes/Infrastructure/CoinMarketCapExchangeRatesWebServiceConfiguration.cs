using System;

namespace CryptoExchangeRates.Quotes.Infrastructure
{
    public sealed class CoinMarketCapExchangeRatesWebServiceConfiguration
    {
        public Uri BaseUrl { get; set; }
        public string ApiKey { get; set; }

        public void Validate()
        {
            if (BaseUrl is null)
                throw new InvalidOperationException(
                    $"{nameof(CoinMarketCapExchangeRatesWebServiceConfiguration)} {nameof(BaseUrl)} is required");
            if (string.IsNullOrWhiteSpace(ApiKey))
                throw new InvalidOperationException(
                    $"{nameof(CoinMarketCapExchangeRatesWebServiceConfiguration)} {nameof(ApiKey)} can't be blank");
        }
    }
}