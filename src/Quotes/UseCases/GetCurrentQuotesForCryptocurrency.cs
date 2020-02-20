using System;
using CryptoExchangeRates.Quotes.Gateways;
using CryptoExchangeRates.Quotes.Models;

namespace CryptoExchangeRates.Quotes.UseCases
{
    internal sealed class GetCurrentQuotesForCryptocurrency : IGetCurrentQuotesForCryptocurrency
    {
        private readonly IExchangeRatesService _exchangeRatesService;

        public GetCurrentQuotesForCryptocurrency(IExchangeRatesService exchangeRatesService)
        {
            _exchangeRatesService = exchangeRatesService ?? throw new ArgumentNullException(nameof(exchangeRatesService));
        }

        public CryptocurrencyQuotesResponse Execute(CryptocurrencyQuotesRequest request)
        {
            if (request is null)
                throw new ArgumentNullException(nameof(request));

            var baseCurrencyCode = CurrencyCode.Of(request.CryptocurrencyCode);
            var quotes = _exchangeRatesService.GetQuotesFor(baseCurrencyCode);

            return CryptocurrencyQuotesResponse.From(baseCurrencyCode, quotes);
        }
    }

    public sealed class GetCurrentQuotesForCryptocurrencyFactory
    {
        internal GetCurrentQuotesForCryptocurrencyFactory() { }

        public IGetCurrentQuotesForCryptocurrency Create(IExchangeRatesService exchangeRatesService) =>
            new GetCurrentQuotesForCryptocurrency(exchangeRatesService);
    }
}