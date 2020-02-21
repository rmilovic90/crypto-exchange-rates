using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CryptoExchangeRates.Quotes.Gateways;
using CryptoExchangeRates.Quotes.Models;
using FluentAssertions;
using Moq;
using Xunit;

using static CryptoExchangeRates.Quotes.CurrencyCodes;
using static CryptoExchangeRates.Quotes.QuoteCurrenciesBuilder;
using static CryptoExchangeRates.Quotes.SampleCryptocurrencyExchangeRates;

namespace CryptoExchangeRates.Quotes.UseCases.GetCurrentQuotesForCryptocurrencyUseCase
{
    public sealed class GetCurrentQuotesForCryptocurrencyTests
    {
        private readonly Mock<IExchangeRatesService> _fakeExchangeRatesService;
        private readonly IGetCurrentQuotesForCryptocurrency _useCase;

        public GetCurrentQuotesForCryptocurrencyTests()
        {
            _fakeExchangeRatesService = new Mock<IExchangeRatesService>();
            _useCase = QuotesServicesFactory.UseCases.GetCurrentQuotesForCryptocurrency.Create(_fakeExchangeRatesService.Object);
        }

        [Fact]
        public void Forbids_use_of_an_absent_request()
        {
            Func<Task> executeGetCurrentQuotesForCryptocurrency = () => _useCase.Execute(null);

            executeGetCurrentQuotesForCryptocurrency.Should().ThrowExactly<ArgumentNullException>();
        }

        [Fact]
        public async Task Returns_current_quotes_for_a_given_cryptocurrency_code()
        {
            var request = new CryptocurrencyQuotesRequest { CryptocurrencyCode = BTC };
            var expectedResponse = new CryptocurrencyQuotesResponse
            {
                BaseCryptocurrencyCode = BTC,
                Quotes = QuoteCurrencyDetailsOf(
                    (USD, BTC_to_USD),
                    (EUR, BTC_to_EUR),
                    (BRL, BTC_to_BRL),
                    (GBP, BTC_to_GBP),
                    (AUD, BTC_to_AUD))
            };

            _fakeExchangeRatesService.Setup(service =>
                    service.GetQuotesFor(CurrencyCode.Of(BTC)))
                .ReturnsAsync(
                    QuoteCurrenciesOf(
                        (USD, BTC_to_USD),
                        (EUR, BTC_to_EUR),
                        (BRL, BTC_to_BRL),
                        (GBP, BTC_to_GBP),
                        (AUD, BTC_to_AUD)));

            var response = await _useCase.Execute(request);

            response.Should().BeEquivalentTo(expectedResponse);
        }

        private static List<QuoteCurrencyDetails> QuoteCurrencyDetailsOf(
            params (string code, decimal exchangeRate)[] quoteCurrencyCodeAndExchangeRatePairs) =>
                quoteCurrencyCodeAndExchangeRatePairs.Select(pair =>
                    new QuoteCurrencyDetails
                    {
                        Code = pair.code,
                        ExchangeRate = pair.exchangeRate
                    }).ToList();
    }
}