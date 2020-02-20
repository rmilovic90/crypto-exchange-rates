using CryptoExchangeRates.Quotes.Infrastructure;
using CryptoExchangeRates.Quotes.UseCases;

namespace CryptoExchangeRates.Quotes
{
    public static class QuotesServicesFactory
    {
        public static UseCasesFactory UseCases => new UseCasesFactory();

        public static InfrastructureServicesFactory Infrastructure => new InfrastructureServicesFactory();
    }
}