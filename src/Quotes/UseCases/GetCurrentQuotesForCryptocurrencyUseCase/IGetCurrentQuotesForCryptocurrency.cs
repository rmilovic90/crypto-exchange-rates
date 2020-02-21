using System.Threading.Tasks;

namespace CryptoExchangeRates.Quotes.UseCases.GetCurrentQuotesForCryptocurrencyUseCase
{
    public interface IGetCurrentQuotesForCryptocurrency
    {
        Task<CryptocurrencyQuotesResponse> Execute(CryptocurrencyQuotesRequest request);
    }
}