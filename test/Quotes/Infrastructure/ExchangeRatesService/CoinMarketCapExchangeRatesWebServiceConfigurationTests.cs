using System;
using FluentAssertions;
using Xunit;

namespace CryptoExchangeRates.Quotes.Infrastructure.ExchangeRatesService
{
    public sealed class CoinMarketCapExchangeRatesWebServiceConfigurationTests
    {
        private const string SampleApiKeyValue = "b54bcf4d-1bca-4e8e-9a24-22ff2c3d462c";

        private static readonly Uri CoinMarketCapExchangeRatesWebServiceBaseUrl =
            new Uri("https://pro-api.coinmarketcap.com");

        [Fact]
        public void Fails_the_validation_when_base_URL_is_absent()
        {
            var configuration = new CoinMarketCapExchangeRatesWebServiceConfiguration
            {
                ApiKey = SampleApiKeyValue
            };

            Action validateConfiguration = () => configuration.Validate();

            validateConfiguration.Should().ThrowExactly<InvalidOperationException>();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("  ")]
        public void Fails_the_validation_when_API_key_is_blank(string apiKey)
        {
            var configuration = new CoinMarketCapExchangeRatesWebServiceConfiguration
            {
                BaseUrl = CoinMarketCapExchangeRatesWebServiceBaseUrl,
                ApiKey = apiKey
            };

            Action validateConfiguration = () => configuration.Validate();

            validateConfiguration.Should().ThrowExactly<InvalidOperationException>();
        }

        [Fact]
        public void Passes_the_validation_when_all_values_are_valid()
        {
            var configuration = new CoinMarketCapExchangeRatesWebServiceConfiguration
            {
                BaseUrl = CoinMarketCapExchangeRatesWebServiceBaseUrl,
                ApiKey = SampleApiKeyValue
            };

            Action validateConfiguration = () => configuration.Validate();

            validateConfiguration.Should().NotThrow();
        }
    }
}