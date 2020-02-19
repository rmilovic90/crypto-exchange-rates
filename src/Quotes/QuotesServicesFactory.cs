using CryptoExchangeRates.Quotes.UseCases;

namespace CryptoExchangeRates.Quotes
{
    public static class QuotesServicesFactory
    {
        public static UseCasesFactory UseCases => new UseCasesFactory();
    }
}