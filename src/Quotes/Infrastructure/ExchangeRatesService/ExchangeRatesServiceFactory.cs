using System.Net.Http;
using CryptoExchangeRates.Quotes.Gateways;

namespace CryptoExchangeRates.Quotes.Infrastructure.ExchangeRatesService
{
    public sealed class ExchangeRatesServiceFactory
    {
        internal ExchangeRatesServiceFactory() { }

        public IExchangeRatesService CreateCoinMarketCapExchangeRatesWebService(
            IHttpClientFactory httpClientFactory,
            CoinMarketCapExchangeRatesWebServiceConfiguration configuration) =>
                new CoinMarketCapExchangeRatesWebService(httpClientFactory, configuration);
    }
}