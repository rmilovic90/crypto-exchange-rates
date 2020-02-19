namespace CryptoExchangeRates.Quotes.UseCases
{
    public sealed class UseCasesFactory
    {
        internal UseCasesFactory() { }

        public GetCurrentQuotesForCryptocurrencyFactory GetCurrentQuotesForCryptocurrency =>
            new GetCurrentQuotesForCryptocurrencyFactory();
    }
}