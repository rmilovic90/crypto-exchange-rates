using System;
using System.Collections.Generic;
using System.Linq;
using CryptoExchangeRates.Quotes.Models;
using FluentAssertions;
using Xunit;

namespace CryptoExchangeRates.Quotes.UseCases.GetCurrentQuotesForCryptocurrencyUseCase
{
    public sealed class CryptocurrencyQuotesResponseTests
    {
        [Fact]
        public void Requires_base_currency_code_when_created_from_base_currency_code_and_quotes()
        {
            Action createCryptocurrencyQuotesResponseFromBaseCurrencyCodeAndQuotes = () =>
                CryptocurrencyQuotesResponse.From(null, Enumerable.Empty<QuoteCurrency>());

            createCryptocurrencyQuotesResponseFromBaseCurrencyCodeAndQuotes.Should()
                .ThrowExactly<ArgumentNullException>()
                    .Which.ParamName.Should().Be("baseCurrencyCode");
        }

        [Fact]
        public void Requires_quotes_when_created_from_base_currency_code_and_quotes()
        {
            Action createCryptocurrencyQuotesResponseFromBaseCurrencyCodeAndQuotes = () =>
                CryptocurrencyQuotesResponse.From(CurrencyCode.Of("BTC"), null);

            createCryptocurrencyQuotesResponseFromBaseCurrencyCodeAndQuotes.Should()
                .ThrowExactly<ArgumentNullException>()
                    .Which.ParamName.Should().Be("quotes");
        }

        [Fact]
        public void Is_successfully_created_from_base_currency_code_and_quotes_when_both_values_are_present()
        {
            var expectedCryptocurrencyQuotesResponse = new CryptocurrencyQuotesResponse
            {
                BaseCurrencyCode = "BTC",
                Quotes = new List<QuoteCurrencyDetails>
                {
                    new QuoteCurrencyDetails { Code = "USD", Rate = 10128.54M }
                }
            };

            var cryptocurrencyQuotesResponse = CryptocurrencyQuotesResponse.From(
                CurrencyCode.Of("BTC"),
                Enumerable.Empty<QuoteCurrency>().Append(
                    QuoteCurrency.Of(CurrencyCode.Of("USD"), CurrencyExchangeRate.Of(10128.54M))));

            cryptocurrencyQuotesResponse.Should().BeEquivalentTo(expectedCryptocurrencyQuotesResponse);
        }
    }
}