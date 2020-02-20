namespace CryptoExchangeRates.Quotes.UseCases.GetCurrentQuotesForCryptocurrencyUseCase
{
    public interface IGetCurrentQuotesForCryptocurrency
    {
        CryptocurrencyQuotesResponse Execute(CryptocurrencyQuotesRequest request);
    }
}