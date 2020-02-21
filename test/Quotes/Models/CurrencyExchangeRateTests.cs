using System;
using FluentAssertions;
using Xunit;

namespace CryptoExchangeRates.Quotes.Models
{
    public sealed class CurrencyExchangeRateTests
    {
        [Theory]
        [InlineData("0.0")]
        [InlineData("-0.5")]
        public void Requires_value_higher_than_zero(decimal value)
        {
            Action createCurrencyExchangeRate = () => CurrencyExchangeRate.Of(value);

            createCurrencyExchangeRate.Should().ThrowExactly<DomainException>();
        }

        [Fact]
        public void Is_successfully_created_from_a_non_zero_positive_value()
        {
            Action createCurrencyExchangeRate = () => CurrencyExchangeRate.Of(0.5M);

            createCurrencyExchangeRate.Should().NotThrow();
        }
    }
}