namespace CryptoExchangeRates.Quotes.UseCases
{
    public interface IGetCurrentQuotesForCryptocurrency
    {
        CryptocurrencyQuotesResponse Execute(CryptocurrencyQuotesRequest request);
    }
}