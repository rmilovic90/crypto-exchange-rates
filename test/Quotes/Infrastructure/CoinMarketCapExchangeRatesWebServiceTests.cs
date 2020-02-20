using System;
using CryptoExchangeRates.Quotes.Gateways;
using FluentAssertions;
using Xunit;

namespace CryptoExchangeRates.Quotes.Infrastructure
{
    public sealed class CoinMarketCapExchangeRatesWebServiceTests
    {
        private readonly IExchangeRatesService _exchangeRatesService;

        public CoinMarketCapExchangeRatesWebServiceTests()
        {
            _exchangeRatesService = QuotesServicesFactory.Infrastructure.ExchangeRatesService
                .CreateCoinMarketCapExchangeRatesWebService();
        }

        [Fact]
        public void Forbids_use_of_an_absent_base_currency_code_when_retrieving_quotes()
        {
            Action executeGetQuotesForBaseCurrencyCode = () => _exchangeRatesService.GetQuotesFor(null);

            executeGetQuotesForBaseCurrencyCode.Should().ThrowExactly<ArgumentNullException>();
        }
    }
}