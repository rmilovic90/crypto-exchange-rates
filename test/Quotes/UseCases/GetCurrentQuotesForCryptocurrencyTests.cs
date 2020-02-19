using System;
using FluentAssertions;
using Xunit;

namespace CryptoExchangeRates.Quotes.UseCases
{
    public sealed class GetCurrentQuotesForCryptocurrencyTests
    {
        private readonly IGetCurrentQuotesForCryptocurrency _useCase;

        public GetCurrentQuotesForCryptocurrencyTests()
        {
            _useCase = QuotesServicesFactory.UseCases.GetCurrentQuotesForCryptocurrency.Create();
        }

        [Fact]
        public void Forbids_use_of_an_absent_request()
        {
            Action executeGetCurrentQuotesForCryptocurrency = () => _useCase.Execute(null);

            executeGetCurrentQuotesForCryptocurrency.Should()
                .ThrowExactly<ArgumentNullException>();
        }
    }
}