using System;
using System.Collections.Generic;
using System.Linq;
using CryptoExchangeRates.Quotes.Models;

namespace CryptoExchangeRates.Quotes.UseCases.GetCurrentQuotesForCryptocurrencyUseCase
{
    public sealed class CryptocurrencyQuotesResponse
    {
        public static CryptocurrencyQuotesResponse From(
            CurrencyCode baseCryptocurrencyCode, IEnumerable<QuoteCurrency> quotes)
        {
            if (baseCryptocurrencyCode is null)
                throw new ArgumentNullException(
                    nameof(baseCryptocurrencyCode),
                    $"{nameof(CryptocurrencyQuotesResponse)} {nameof(baseCryptocurrencyCode)} is required");
            if (quotes is null)
                throw new ArgumentNullException(
                    nameof(quotes),
                    $"{nameof(CryptocurrencyQuotesResponse)} {nameof(quotes)} is required");

            return new CryptocurrencyQuotesResponse
            {
                BaseCryptocurrencyCode = baseCryptocurrencyCode,
                Quotes = quotes.Select(quote =>
                    new QuoteCurrencyDetails
                    {
                        Code = quote.Code,
                        ExchangeRate = quote.ExchangeRate
                    }).ToList()
            };
        }

        public string BaseCryptocurrencyCode { get; set; }
        public List<QuoteCurrencyDetails> Quotes { get; set; }
    }

    public sealed class QuoteCurrencyDetails
    {
        public string Code { get; set; }
        public decimal ExchangeRate { get; set; }
    }
}