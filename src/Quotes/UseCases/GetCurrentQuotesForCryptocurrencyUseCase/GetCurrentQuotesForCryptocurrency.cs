using System;
using CryptoExchangeRates.Quotes.Gateways;
using CryptoExchangeRates.Quotes.Models;

namespace CryptoExchangeRates.Quotes.UseCases.GetCurrentQuotesForCryptocurrencyUseCase
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

            var baseCryptocurrencyCode = CurrencyCode.Of(request.CryptocurrencyCode);
            var quotes = _exchangeRatesService.GetQuotesFor(baseCryptocurrencyCode);

            return CryptocurrencyQuotesResponse.From(baseCryptocurrencyCode, quotes);
        }
    }
}