using System;
using System.Collections.Generic;
using System.Linq;
using CryptoExchangeRates.Quotes.Models;
using FluentAssertions;
using Xunit;

using static CryptoExchangeRates.Quotes.CurrencyCodes;
using static CryptoExchangeRates.Quotes.QuoteCurrenciesBuilder;
using static CryptoExchangeRates.Quotes.SampleCryptocurrencyExchangeRates;

namespace CryptoExchangeRates.Quotes.UseCases.GetCurrentQuotesForCryptocurrencyUseCase
{
    public sealed class CryptocurrencyQuotesResponseTests
    {
        [Fact]
        public void Requires_base_cryptocurrency_code_when_created_from_base_cryptocurrency_code_and_quotes()
        {
            Action createCryptocurrencyQuotesResponseFromBaseCryptocurrencyCodeAndQuotes = () =>
                CryptocurrencyQuotesResponse.From(null, Enumerable.Empty<QuoteCurrency>());

            createCryptocurrencyQuotesResponseFromBaseCryptocurrencyCodeAndQuotes.Should()
                .ThrowExactly<ArgumentNullException>()
                    .Which.ParamName.Should().Be("baseCryptocurrencyCode");
        }

        [Fact]
        public void Requires_quotes_when_created_from_base_cryptocurrency_code_and_quotes()
        {
            Action createCryptocurrencyQuotesResponseFromBaseCryptocurrencyCodeAndQuotes = () =>
                CryptocurrencyQuotesResponse.From(CurrencyCode.Of(BTC), null);

            createCryptocurrencyQuotesResponseFromBaseCryptocurrencyCodeAndQuotes.Should()
                .ThrowExactly<ArgumentNullException>()
                    .Which.ParamName.Should().Be("quotes");
        }

        [Fact]
        public void Is_successfully_created_from_base_cryptocurrency_code_and_quotes_when_both_values_are_present()
        {
            var expectedCryptocurrencyQuotesResponse = new CryptocurrencyQuotesResponse
            {
                BaseCryptocurrencyCode = BTC,
                Quotes = new List<QuoteCurrencyDetails>
                {
                    new QuoteCurrencyDetails { Code = USD, ExchangeRate = BTC_to_USD }
                }
            };

            var cryptocurrencyQuotesResponse = CryptocurrencyQuotesResponse.From(
                CurrencyCode.Of(BTC),
                QuoteCurrenciesOf((USD, BTC_to_USD)));

            cryptocurrencyQuotesResponse.Should().BeEquivalentTo(expectedCryptocurrencyQuotesResponse);
        }
    }
}