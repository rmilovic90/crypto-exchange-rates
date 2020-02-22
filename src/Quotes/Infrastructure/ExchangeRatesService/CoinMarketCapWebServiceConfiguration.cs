using System;

namespace CryptoExchangeRates.Quotes.Infrastructure.ExchangeRatesService
{
    public sealed class CoinMarketCapWebServiceConfiguration
    {
        public Uri BaseUrl { get; set; }
        public string ApiKey { get; set; }

        public void Validate()
        {
            if (BaseUrl is null)
                throw new InvalidOperationException(
                    $"{nameof(CoinMarketCapWebServiceConfiguration)} {nameof(BaseUrl)} is required");
            if (string.IsNullOrWhiteSpace(ApiKey))
                throw new InvalidOperationException(
                    $"{nameof(CoinMarketCapWebServiceConfiguration)} {nameof(ApiKey)} can't be blank");
        }
    }
}