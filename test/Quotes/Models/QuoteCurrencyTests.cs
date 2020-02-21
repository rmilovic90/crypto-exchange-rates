using System;
using FluentAssertions;
using Xunit;

using static CryptoExchangeRates.Quotes.CurrencyCodes;
using static CryptoExchangeRates.Quotes.SampleCryptocurrencyExchangeRates;

namespace CryptoExchangeRates.Quotes.Models
{
    public sealed class QuoteCurrencyTests
    {
        [Fact]
        public void Requires_code()
        {
            Action createQuoteCurrency = () =>
                QuoteCurrency.Of(null, CurrencyExchangeRate.Of(BTC_to_USD));

            createQuoteCurrency.Should().ThrowExactly<DomainException>();
        }

        [Fact]
        public void Requires_rate()
        {
            Action createQuoteCurrency = () =>
                QuoteCurrency.Of(CurrencyCode.Of(USD), null);

            createQuoteCurrency.Should().ThrowExactly<DomainException>();
        }

        [Fact]
        public void Is_successfully_created_when_both_code_and_exchange_rate_are_provided()
        {
            Action createQuoteCurrency = () =>
                QuoteCurrency.Of(CurrencyCode.Of(USD), CurrencyExchangeRate.Of(BTC_to_USD));

            createQuoteCurrency.Should().NotThrow();
        }
    }
}