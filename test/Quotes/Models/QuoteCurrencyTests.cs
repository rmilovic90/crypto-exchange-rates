using System;
using FluentAssertions;
using Xunit;

namespace CryptoExchangeRates.Quotes.Models
{
    public sealed class QuoteCurrencyTests
    {
        [Fact]
        public void Requires_code()
        {
            Action createQuoteCurrency = () =>
                QuoteCurrency.Of(null, CurrencyExchangeRate.Of(10128.54M));

            createQuoteCurrency.Should().ThrowExactly<ArgumentNullException>()
                .Which.ParamName.Should().Be("code");
        }

        [Fact]
        public void Requires_rate()
        {
            Action createQuoteCurrency = () =>
                QuoteCurrency.Of(CurrencyCode.Of("USD"), null);

            createQuoteCurrency.Should().ThrowExactly<ArgumentNullException>()
                .Which.ParamName.Should().Be("rate");
        }

        [Fact]
        public void Is_successfully_created_when_both_code_and_rate_are_provided()
        {
            Action createQuoteCurrency = () =>
                QuoteCurrency.Of(CurrencyCode.Of("USD"), CurrencyExchangeRate.Of(10128.54M));

            createQuoteCurrency.Should().NotThrow();
        }
    }
}