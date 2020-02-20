using CryptoExchangeRates.Quotes.Gateways;

namespace CryptoExchangeRates.Quotes.UseCases.GetCurrentQuotesForCryptocurrencyUseCase
{
    public sealed class GetCurrentQuotesForCryptocurrencyFactory
    {
        internal GetCurrentQuotesForCryptocurrencyFactory() { }

        public IGetCurrentQuotesForCryptocurrency Create(IExchangeRatesService exchangeRatesService) =>
            new GetCurrentQuotesForCryptocurrency(exchangeRatesService);
    }
}