using System;

namespace CryptoExchangeRates.Quotes.UseCases
{
    internal sealed class GetCurrentQuotesForCryptocurrency : IGetCurrentQuotesForCryptocurrency
    {
        public CryptocurrencyQuotesResponse Execute(CryptocurrencyQuotesRequest request)
        {
            if (request is null)
                throw new ArgumentNullException(nameof(request));

            throw new NotImplementedException();
        }
    }

    public sealed class GetCurrentQuotesForCryptocurrencyFactory
    {
        internal GetCurrentQuotesForCryptocurrencyFactory() { }

        public IGetCurrentQuotesForCryptocurrency Create() => new GetCurrentQuotesForCryptocurrency();
    }
}