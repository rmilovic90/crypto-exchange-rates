﻿using System;
using FluentAssertions;
using Xunit;

using static CryptoExchangeRates.Quotes.CurrencyCodes;
using static CryptoExchangeRates.Quotes.Models.DomainErrors;

namespace CryptoExchangeRates.Quotes.Models
{
    public sealed class CurrencyCodeTests
    {
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("  ")]
        public void Requires_non_blank_value(string value)
        {
            Action createCurrencyCode = () => CurrencyCode.Of(value);

            createCurrencyCode.Should().ThrowExactly<DomainException>()
                .Which.Error.Should().Be(MissingCurrencyCode);
        }

        [Theory]
        [InlineData("123")]
        [InlineData("1TC")]
        [InlineData("BT$")]
        [InlineData("BTC2")]
        public void Forbids_improperly_formatted_value(string value)
        {
            Action createCurrencyCode = () => CurrencyCode.Of(value);

            createCurrencyCode.Should().ThrowExactly<DomainException>()
                .Which.Error.Should().Be(CurrencyCodeInvalidFormat);
        }

        [Fact]
        public void Is_successfully_created_from_a_non_blank_value()
        {
            Action createCurrencyCode = () => CurrencyCode.Of(BTC);

            createCurrencyCode.Should().NotThrow();
        }

        [Fact]
        public void Uppercase_the_value()
        {
            string currencyCodeValue = CurrencyCode.Of("bTc");

            currencyCodeValue.Should().Be(BTC);
        }
    }
}