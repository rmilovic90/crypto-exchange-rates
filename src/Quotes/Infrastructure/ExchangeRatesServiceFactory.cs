using CryptoExchangeRates.Quotes.Gateways;

namespace CryptoExchangeRates.Quotes.Infrastructure
{
    public sealed class ExchangeRatesServiceFactory
    {
        internal ExchangeRatesServiceFactory() { }

        public IExchangeRatesService CreateCoinMarketCapExchangeRatesWebService() =>
            new CoinMarketCapExchangeRatesWebService();
    }
}