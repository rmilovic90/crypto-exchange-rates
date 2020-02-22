using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using CryptoExchangeRates.Quotes.Gateways;
using CryptoExchangeRates.Quotes.Models;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;
using WireMock.Server;
using Xunit;

using static CryptoExchangeRates.Quotes.CurrencyCodes;
using static CryptoExchangeRates.Quotes.SampleCryptocurrencyExchangeRates;

namespace CryptoExchangeRates.Quotes.Infrastructure.ExchangeRatesService
{
    public sealed class CoinMarketCapExchangeRatesWebServiceTests : IDisposable
    {
        private const string ApiKeyHeaderName = "X-CMC_PRO_API_KEY";
        private const string SampleApiKeyValue = "b54bcf4d-1bca-4e8e-9a24-22ff2c3d462c";
        private const string GetLatestCryptocurrencyQuotesRequestUrlPath = "/v1/cryptocurrency/quotes/latest";
        private const string BaseCryptocurrencyCodeParameterName = "symbol";
        private const string QuoteCurrencyCodeParameterName = "convert";
        private const string ContentTypeResponseHeaderName = "Content-Type";
        private const string Utf8JsonContentType = "application/json; charset=utf-8";

        private readonly WireMockServer _mockServer;

        private readonly IExchangeRatesService _exchangeRatesService;

        public CoinMarketCapExchangeRatesWebServiceTests()
        {
            _mockServer = WireMockServer.Start();

            var configuration = new CoinMarketCapWebServiceConfiguration
            {
                BaseUrl = new Uri(_mockServer.Urls.First()),
                ApiKey = SampleApiKeyValue
            };
            _exchangeRatesService = QuotesServicesFactory.Infrastructure.ExchangeRatesService
                .CreateCoinMarketCapExchangeRatesWebService(CreateHttpClientFactory(), configuration);
        }

        [Fact]
        public void Forbids_use_of_an_absent_base_cryptocurrency_code_when_returning_quotes()
        {
            Func<Task> executeGetQuotesForBaseCryptocurrencyCode = () => _exchangeRatesService.GetQuotesFor(null);

            executeGetQuotesForBaseCryptocurrencyCode.Should().ThrowExactly<ArgumentNullException>();
        }

        [Fact]
        public async Task Sets_given_cryptocurrency_code_in_URL_of_get_latest_cryptocurrency_quotes_requests()
        {
            ConfigureGetLatestCryptocurrencyQuotesRequestsToRespondSuccessfully();

            await _exchangeRatesService.GetQuotesFor(CurrencyCode.Of(BTC));

            var foundRequests = _mockServer.FindLogEntries(
                Request.Create()
                    .WithPath(GetLatestCryptocurrencyQuotesRequestUrlPath)
                    .WithParam(BaseCryptocurrencyCodeParameterName, BTC)
                    .UsingGet());

            foundRequests.Should().HaveCount(5);
        }

        [Fact]
        public async Task Includes_specific_quote_currency_codes_in_URL_of_get_latest_cryptocurrency_quotes_requests()
        {
            ConfigureGetLatestCryptocurrencyQuotesRequestsToRespondSuccessfully();

            await _exchangeRatesService.GetQuotesFor(CurrencyCode.Of(BTC));

            var foundLatestQuotesForBitcoinToUsDollarRequests = _mockServer.FindLogEntries(
                Request.Create()
                    .WithPath(GetLatestCryptocurrencyQuotesRequestUrlPath)
                    .WithParam(QuoteCurrencyCodeParameterName, USD)
                    .UsingGet());
            var foundLatestQuotesForBitcoinToEuroRequests = _mockServer.FindLogEntries(
                Request.Create()
                    .WithPath(GetLatestCryptocurrencyQuotesRequestUrlPath)
                    .WithParam(QuoteCurrencyCodeParameterName, EUR)
                    .UsingGet());
            var foundLatestQuotesForBitcoinToBrazilianRealRequests = _mockServer.FindLogEntries(
                Request.Create()
                    .WithPath(GetLatestCryptocurrencyQuotesRequestUrlPath)
                    .WithParam(QuoteCurrencyCodeParameterName, BRL)
                    .UsingGet());
            var foundLatestQuotesForBitcoinToPoundSterlingRequests = _mockServer.FindLogEntries(
                Request.Create()
                    .WithPath(GetLatestCryptocurrencyQuotesRequestUrlPath)
                    .WithParam(QuoteCurrencyCodeParameterName, GBP)
                    .UsingGet());
            var foundLatestQuotesForBitcoinToAustralianDollarRequests = _mockServer.FindLogEntries(
                Request.Create()
                    .WithPath(GetLatestCryptocurrencyQuotesRequestUrlPath)
                    .WithParam(QuoteCurrencyCodeParameterName, AUD)
                    .UsingGet());

            foundLatestQuotesForBitcoinToUsDollarRequests.Should().HaveCount(1);
            foundLatestQuotesForBitcoinToEuroRequests.Should().HaveCount(1);
            foundLatestQuotesForBitcoinToBrazilianRealRequests.Should().HaveCount(1);
            foundLatestQuotesForBitcoinToPoundSterlingRequests.Should().HaveCount(1);
            foundLatestQuotesForBitcoinToAustralianDollarRequests.Should().HaveCount(1);
        }

        [Fact]
        public async Task Sets_authentication_header_with_configured_api_key_for_get_latest_cryptocurrency_quotes_request()
        {
            ConfigureGetLatestCryptocurrencyQuotesRequestsToRespondSuccessfully();

            await _exchangeRatesService.GetQuotesFor(CurrencyCode.Of(BTC));

            var foundRequests = _mockServer.FindLogEntries(
                Request.Create()
                    .WithPath(GetLatestCryptocurrencyQuotesRequestUrlPath)
                    .WithHeader(ApiKeyHeaderName, SampleApiKeyValue)
                    .UsingGet());

            foundRequests.Should().HaveCount(5);
        }

        [Fact]
        public void Fails_to_return_quotes_for_base_cryptocurrency_code_when_get_latest_cryptocurrency_quotes_request_responds_with_bad_request_status()
        {
            ConfigureGetLatestCryptocurrencyQuotesRequestsToRespondWithBadRequestStatus();

            Func<Task> executeGetQuotesForBaseCryptocurrencyCode = () => _exchangeRatesService.GetQuotesFor(CurrencyCode.Of(BTC));

            executeGetQuotesForBaseCryptocurrencyCode.Should().ThrowExactly<HttpRequestException>();
        }

        [Fact]
        public void Fails_to_return_quotes_for_base_cryptocurrency_code_when_get_latest_cryptocurrency_quotes_request_responds_with_unauthorized_status()
        {
            ConfigureGetLatestCryptocurrencyQuotesRequestsToRespondWithUnauthorizedStatus();

            Func<Task> executeGetQuotesForBaseCryptocurrencyCode = () => _exchangeRatesService.GetQuotesFor(CurrencyCode.Of(BTC));

            executeGetQuotesForBaseCryptocurrencyCode.Should().ThrowExactly<HttpRequestException>();
        }

        [Fact]
        public void Fails_to_return_quotes_for_base_cryptocurrency_code_when_get_latest_cryptocurrency_quotes_request_responds_with_forbidden_status()
        {
            ConfigureGetLatestCryptocurrencyQuotesRequestsToRespondWithForbiddenStatus();

            Func<Task> executeGetQuotesForBaseCryptocurrencyCode = () => _exchangeRatesService.GetQuotesFor(CurrencyCode.Of(BTC));

            executeGetQuotesForBaseCryptocurrencyCode.Should().ThrowExactly<HttpRequestException>();
        }

        [Fact]
        public void Fails_to_return_quotes_for_base_cryptocurrency_code_when_get_latest_cryptocurrency_quotes_request_responds_with_too_many_requests_status()
        {
            ConfigureGetLatestCryptocurrencyQuotesRequestsToRespondWithTooManyRequestsStatus();

            Func<Task> executeGetQuotesForBaseCryptocurrencyCode = () => _exchangeRatesService.GetQuotesFor(CurrencyCode.Of(BTC));

            executeGetQuotesForBaseCryptocurrencyCode.Should().ThrowExactly<HttpRequestException>();
        }

        [Fact]
        public void Fails_to_return_quotes_for_base_cryptocurrency_code_when_get_latest_cryptocurrency_quotes_request_responds_with_internal_server_error_status()
        {
            ConfigureGetLatestCryptocurrencyQuotesRequestsToRespondWithIntervalServerErrorStatus();

            Func<Task> executeGetQuotesForBaseCryptocurrencyCode = () => _exchangeRatesService.GetQuotesFor(CurrencyCode.Of(BTC));

            executeGetQuotesForBaseCryptocurrencyCode.Should().ThrowExactly<HttpRequestException>();
        }

        [Fact]
        public async Task Returns_quotes_for_base_cryptocurrency_code_when_get_latest_cryptocurrency_quotes_request_responds_with_ok_status()
        {
            ConfigureGetLatestCryptocurrencyQuotesRequestsToRespondSuccessfully();

            var expectedQuotes = QuoteCurrenciesBuilder.QuoteCurrenciesOf(
                (USD, BTC_to_USD),
                (EUR, BTC_to_EUR),
                (BRL, BTC_to_BRL),
                (GBP, BTC_to_GBP),
                (AUD, BTC_to_AUD));

            var quotes = await _exchangeRatesService.GetQuotesFor(CurrencyCode.Of(BTC));

            quotes.Should().BeEquivalentTo(expectedQuotes);
        }

        public void Dispose()
        {
            _mockServer.Stop();
        }

        private static IHttpClientFactory CreateHttpClientFactory()
        {
            var services = new ServiceCollection();
            services.AddHttpClient();

            var serviceProviderFactory = new DefaultServiceProviderFactory();
            var serviceProvider = serviceProviderFactory.CreateServiceProvider(services);

            return serviceProvider.GetRequiredService<IHttpClientFactory>();
        }

        private void ConfigureGetLatestCryptocurrencyQuotesRequestsToRespondSuccessfully()
        {
            ConfigureGetLatestCryptocurrencyQuotesRequestsToRespondSuccessfullyFor(
                BTC, USD, BTC_to_USD);
            ConfigureGetLatestCryptocurrencyQuotesRequestsToRespondSuccessfullyFor(
                BTC, EUR, BTC_to_EUR);
            ConfigureGetLatestCryptocurrencyQuotesRequestsToRespondSuccessfullyFor(
                BTC, BRL, BTC_to_BRL);
            ConfigureGetLatestCryptocurrencyQuotesRequestsToRespondSuccessfullyFor(
                BTC, GBP, BTC_to_GBP);
            ConfigureGetLatestCryptocurrencyQuotesRequestsToRespondSuccessfullyFor(
                BTC, AUD, BTC_to_AUD);
        }

        private void ConfigureGetLatestCryptocurrencyQuotesRequestsToRespondSuccessfullyFor(
            string baseCryptocurrencyCode, string quoteCurrencyCode, decimal baseToQuoteCurrencyExchangeRate)
        {
            _mockServer.Given(
                    Request.Create()
                        .WithPath(GetLatestCryptocurrencyQuotesRequestUrlPath)
                        .WithParam(BaseCryptocurrencyCodeParameterName, baseCryptocurrencyCode)
                        .WithParam(QuoteCurrencyCodeParameterName, quoteCurrencyCode)
                        .WithHeader(ApiKeyHeaderName, SampleApiKeyValue)
                        .UsingGet())
                .RespondWith(
                Response.Create()
                    .WithStatusCode(HttpStatusCode.OK)
                    .WithHeader(ContentTypeResponseHeaderName, Utf8JsonContentType)
                    .WithBody($@"{{
                        ""status"": {{
                            ""timestamp"": ""2020-02-21T11:54:28.769Z"",
                            ""error_code"": 0,
                            ""error_message"": null,
                            ""elapsed"": 13,
                            ""credit_count"": 1,
                            ""notice"": null
                        }},
                        ""data"": {{
                            ""{baseCryptocurrencyCode}"": {{
                                ""id"": 1,
                                ""name"": ""Bitcoin"",
                                ""symbol"": ""{baseCryptocurrencyCode}"",
                                ""slug"": ""bitcoin"",
                                ""num_market_pairs"": 7728,
                                ""date_added"": ""2013-04-28T00:00:00.000Z"",
                                ""tags"": [
                                    ""mineable""
                                ],
                                ""max_supply"": 21000000,
                                ""circulating_supply"": 18229337,
                                ""total_supply"": 18229337,
                                ""platform"": null,
                                ""cmc_rank"": 1,
                                ""last_updated"": ""2020-02-21T11:53:38.000Z"",
                                ""quote"": {{
                                    ""{quoteCurrencyCode}"": {{
                                        ""price"": {baseToQuoteCurrencyExchangeRate},
                                        ""volume_24h"": 42937215991.6813,
                                        ""percent_change_1h"": 0.105442,
                                        ""percent_change_24h"": 0.684771,
                                        ""percent_change_7d"": -5.23679,
                                        ""market_cap"": 176471375918.7279,
                                        ""last_updated"": ""2020-02-21T11:53:38.000Z""
                                    }}
                                }}
                            }}
                        }}
                    }}"));
        }

        private void ConfigureGetLatestCryptocurrencyQuotesRequestsToRespondWithBadRequestStatus()
        {
            _mockServer.Given(
                    Request.Create()
                        .WithPath(GetLatestCryptocurrencyQuotesRequestUrlPath)
                        .WithParam(BaseCryptocurrencyCodeParameterName, BTC)
                        .WithParam(QuoteCurrencyCodeParameterName, USD)
                        .WithHeader(ApiKeyHeaderName, SampleApiKeyValue)
                        .UsingGet())
                .RespondWith(
                    Response.Create()
                        .WithStatusCode(HttpStatusCode.BadRequest)
                        .WithHeader(ContentTypeResponseHeaderName, Utf8JsonContentType)
                        .WithBody(@"{
                            ""status"": {
                                ""timestamp"": ""2018-06-02T22:51:28.209Z"",
                                ""error_code"": 400,
                                ""error_message"": ""Invalid value for \""id\"""",
                                ""elapsed"": 10,
                                ""credit_count"": 0
                            }
                        }"));
        }

        private void ConfigureGetLatestCryptocurrencyQuotesRequestsToRespondWithUnauthorizedStatus()
        {
            _mockServer.Given(
                    Request.Create()
                        .WithPath(GetLatestCryptocurrencyQuotesRequestUrlPath)
                        .WithParam(BaseCryptocurrencyCodeParameterName, BTC)
                        .WithParam(QuoteCurrencyCodeParameterName, USD)
                        .WithHeader(ApiKeyHeaderName, SampleApiKeyValue)
                        .UsingGet())
                .RespondWith(
                    Response.Create()
                        .WithStatusCode(HttpStatusCode.Unauthorized)
                        .WithHeader(ContentTypeResponseHeaderName, Utf8JsonContentType)
                        .WithBody(@"{
                            ""status"": {
                                ""timestamp"": ""2018-06-02T22:51:28.209Z"",
                                ""error_code"": 1002,
                                ""error_message"": ""API key missing."",
                                ""elapsed"": 10,
                                ""credit_count"": 0
                            }
                        }"));
        }

        private void ConfigureGetLatestCryptocurrencyQuotesRequestsToRespondWithForbiddenStatus()
        {
            _mockServer.Given(
                    Request.Create()
                        .WithPath(GetLatestCryptocurrencyQuotesRequestUrlPath)
                        .WithParam(BaseCryptocurrencyCodeParameterName, BTC)
                        .WithParam(QuoteCurrencyCodeParameterName, USD)
                        .WithHeader(ApiKeyHeaderName, SampleApiKeyValue)
                        .UsingGet())
                .RespondWith(
                    Response.Create()
                        .WithStatusCode(HttpStatusCode.Forbidden)
                        .WithHeader(ContentTypeResponseHeaderName, Utf8JsonContentType)
                        .WithBody(@"{
                            ""status"": {
                                ""timestamp"": ""2018-06-02T22:51:28.209Z"",
                                ""error_code"": 1006,
                                ""error_message"": ""Your API Key subscription plan doesn't support this endpoint."",
                                ""elapsed"": 10,
                                ""credit_count"": 0
                            }
                        }"));
        }

        private void ConfigureGetLatestCryptocurrencyQuotesRequestsToRespondWithTooManyRequestsStatus()
        {
            _mockServer.Given(
                    Request.Create()
                        .WithPath(GetLatestCryptocurrencyQuotesRequestUrlPath)
                        .WithParam(BaseCryptocurrencyCodeParameterName, BTC)
                        .WithParam(QuoteCurrencyCodeParameterName, USD)
                        .WithHeader(ApiKeyHeaderName, SampleApiKeyValue)
                        .UsingGet())
                .RespondWith(
                    Response.Create()
                        .WithStatusCode(HttpStatusCode.TooManyRequests)
                        .WithHeader(ContentTypeResponseHeaderName, Utf8JsonContentType)
                        .WithBody(@"{
                            ""status"": {
                                ""timestamp"": ""2018-06-02T22:51:28.209Z"",
                                ""error_code"": 1008,
                                ""error_message"": ""You've exceeded your API Key's HTTP request rate limit. Rate limits reset every minute."",
                                ""elapsed"": 10,
                                ""credit_count"": 0
                            }
                        }"));
        }

        private void ConfigureGetLatestCryptocurrencyQuotesRequestsToRespondWithIntervalServerErrorStatus()
        {
            _mockServer.Given(
                    Request.Create()
                        .WithPath(GetLatestCryptocurrencyQuotesRequestUrlPath)
                        .WithParam(BaseCryptocurrencyCodeParameterName, BTC)
                        .WithParam(QuoteCurrencyCodeParameterName, USD)
                        .WithHeader(ApiKeyHeaderName, SampleApiKeyValue)
                        .UsingGet())
                .RespondWith(
                    Response.Create()
                        .WithStatusCode(HttpStatusCode.InternalServerError)
                        .WithHeader(ContentTypeResponseHeaderName, Utf8JsonContentType)
                        .WithBody(@"{
                            ""status"": {
                                ""timestamp"": ""2018-06-02T22:51:28.209Z"",
                                ""error_code"": 500,
                                ""error_message"": ""An internal server error occurred"",
                                ""elapsed"": 10,
                                ""credit_count"": 0
                            }
                        }"));
        }
    }
}