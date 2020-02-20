using System;
using System.Collections.Generic;
using System.Linq;
using CryptoExchangeRates.Quotes.Gateways;
using CryptoExchangeRates.Quotes.Models;
using FluentAssertions;
using Moq;
using Xunit;

namespace CryptoExchangeRates.Quotes.UseCases
{
    public sealed class GetCurrentQuotesForCryptocurrencyTests
    {
        private const string BitCoinCurrencyCode = "BTC";
        private const string UsDollarCurrencyCode = "USD";
        private const string EuroCurrencyCode = "EUR";
        private const string BrazilianRealCurrencyCode = "BRL";
        private const string PoundSterlingCurrencyCode = "GBP";
        private const string AustralianDollarCurrencyCode = "AUD";

        private const decimal SampleBitcoinToUsDollarCrossRate = 10128.54M;
        private const decimal SampleBitcoinToEuroCrossRate = 9395.4M;
        private const decimal SampleBitcoinToBrazilianRealCrossRate = 44307.31M;
        private const decimal SampleBitcoinToPoundSterlingCrossRate = 7823.09M;
        private const decimal SampleBitcoinToAustralianDollarCrossRate = 15162.15M;

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
            Action executeGetCurrentQuotesForCryptocurrency = () => _useCase.Execute(null);

            executeGetCurrentQuotesForCryptocurrency.Should().ThrowExactly<ArgumentNullException>();
        }

        [Fact]
        public void Returns_current_quotes_for_a_given_cryptocurrency_code()
        {
            var request = new CryptocurrencyQuotesRequest { CryptocurrencyCode = BitCoinCurrencyCode };
            var expectedResponse = new CryptocurrencyQuotesResponse
            {
                BaseCurrencyCode = BitCoinCurrencyCode,
                Quotes = QuoteCurrencyDetailsOf(
                    (UsDollarCurrencyCode, SampleBitcoinToUsDollarCrossRate),
                    (EuroCurrencyCode, SampleBitcoinToEuroCrossRate),
                    (BrazilianRealCurrencyCode, SampleBitcoinToBrazilianRealCrossRate),
                    (PoundSterlingCurrencyCode, SampleBitcoinToPoundSterlingCrossRate),
                    (AustralianDollarCurrencyCode, SampleBitcoinToAustralianDollarCrossRate))
            };

            _fakeExchangeRatesService.Setup(service =>
                    service.GetQuotesFor(CurrencyCode.Of(BitCoinCurrencyCode)))
                .Returns(QuoteCurrenciesOf(
                    (UsDollarCurrencyCode, SampleBitcoinToUsDollarCrossRate),
                    (EuroCurrencyCode, SampleBitcoinToEuroCrossRate),
                    (BrazilianRealCurrencyCode, SampleBitcoinToBrazilianRealCrossRate),
                    (PoundSterlingCurrencyCode, SampleBitcoinToPoundSterlingCrossRate),
                    (AustralianDollarCurrencyCode, SampleBitcoinToAustralianDollarCrossRate)));

            var response = _useCase.Execute(request);

            response.Should().BeEquivalentTo(expectedResponse);
        }

        private static List<QuoteCurrencyDetails> QuoteCurrencyDetailsOf(
            params (string code, decimal rate)[] quoteCurrencyCodeAndRatePairs) =>
                quoteCurrencyCodeAndRatePairs.Select(pair =>
                    new QuoteCurrencyDetails
                    {
                        Code = pair.code,
                        Rate = pair.rate
                    }).ToList();

        private static List<QuoteCurrency> QuoteCurrenciesOf(
            params (string code, decimal rate)[] quoteCurrencyCodeAndRatePairs) =>
                quoteCurrencyCodeAndRatePairs.Select(pair =>
                    QuoteCurrency.Of(
                        CurrencyCode.Of(pair.code),
                        CurrencyExchangeRate.Of(pair.rate)))
                .ToList();
    }
}