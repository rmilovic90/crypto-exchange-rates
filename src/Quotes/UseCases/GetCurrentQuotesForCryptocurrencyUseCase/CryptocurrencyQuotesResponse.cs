using System;
using System.Collections.Generic;
using System.Linq;
using CryptoExchangeRates.Quotes.Models;

namespace CryptoExchangeRates.Quotes.UseCases.GetCurrentQuotesForCryptocurrencyUseCase
{
    public sealed class CryptocurrencyQuotesResponse
    {
        public static CryptocurrencyQuotesResponse From(
            CurrencyCode baseCurrencyCode, IEnumerable<QuoteCurrency> quotes)
        {
            if (baseCurrencyCode is null)
                throw new ArgumentNullException(
                    nameof(baseCurrencyCode),
                    $"{nameof(CryptocurrencyQuotesResponse)} {nameof(baseCurrencyCode)} is required");
            if (quotes is null)
                throw new ArgumentNullException(
                    nameof(quotes),
                    $"{nameof(CryptocurrencyQuotesResponse)} {nameof(quotes)} is required");

            return new CryptocurrencyQuotesResponse
            {
                BaseCurrencyCode = baseCurrencyCode,
                Quotes = quotes.Select(quote =>
                    new QuoteCurrencyDetails
                    {
                        Code = quote.Code,
                        Rate = quote.Rate
                    }).ToList()
            };
        }

        public string BaseCurrencyCode { get; set; }
        public List<QuoteCurrencyDetails> Quotes { get; set; }
    }

    public sealed class QuoteCurrencyDetails
    {
        public string Code { get; set; }
        public decimal Rate { get; set; }
    }
}