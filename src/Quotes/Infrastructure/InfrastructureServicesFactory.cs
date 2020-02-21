using CryptoExchangeRates.Quotes.Infrastructure.ExchangeRatesService;

namespace CryptoExchangeRates.Quotes.Infrastructure
{
    public sealed class InfrastructureServicesFactory
    {
        internal InfrastructureServicesFactory() { }

        public ExchangeRatesServiceFactory ExchangeRatesService => new ExchangeRatesServiceFactory();
    }
}