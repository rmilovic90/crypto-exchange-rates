using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using CryptoExchangeRates.Quotes.UseCases.GetCurrentQuotesForCryptocurrencyUseCase;
using FluentAssertions;
using Moq;
using Newtonsoft.Json.Linq;
using Xunit;

using static CryptoExchangeRates.WebApi.ApiResources.CryptocurrencyQuotes.CurrencyCodes;
using static CryptoExchangeRates.WebApi.ApiResources.CryptocurrencyQuotes.SampleCryptocurrencyExchangeRates;

namespace CryptoExchangeRates.WebApi.ApiResources.CryptocurrencyQuotes
{
    public sealed class CryptocurrencyQuotesApiResourceTests : IClassFixture<ApiIntegrationTestsWebApplicationFactory>
    {
        private readonly Mock<IGetCurrentQuotesForCryptocurrency> _getCurrentQuotesForCryptocurrencyUseCaseMock;

        private readonly HttpClient _httpClient;

        public CryptocurrencyQuotesApiResourceTests(ApiIntegrationTestsWebApplicationFactory webApplicationFactory)
        {
            _getCurrentQuotesForCryptocurrencyUseCaseMock = webApplicationFactory.GetLatestQuotesForCryptocurrencyUseCaseMock;

            _httpClient = webApplicationFactory.CreateClient();
        }

        [Fact]
        public async Task returns_200_ok_status_response_with_correct_content_when_calling_get_latest_quotes_for_cryptocurrency()
        {
            var expectedResponseJsonContent = JObject.Parse($@"{{
                  ""baseCryptocurrencyCode"": ""{BTC}"",
                  ""quotes"": [
                    {{
                      ""code"": ""{USD}"",
                      ""exchangeRate"": {BTC_to_USD}
                    }},
                    {{
                      ""code"": ""{EUR}"",
                      ""exchangeRate"": {BTC_to_EUR}
                    }},
                    {{
                      ""code"": ""{BRL}"",
                      ""exchangeRate"": {BTC_to_BRL}
                    }},
                    {{
                      ""code"": ""{GBP}"",
                      ""exchangeRate"": {BTC_to_GBP}
                    }},
                    {{
                      ""code"": ""{AUD}"",
                      ""exchangeRate"": {BTC_to_AUD}
                    }}
                  ]
                }}");

            ConfigureGetCurrentQuotesForCryptocurrencyUseCaseResponse();

            var response = await _httpClient.GetAsync($"api/v1.0/{BTC}/quotes");

            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var responseJsonContent = JObject.Parse(await response.Content.ReadAsStringAsync());
            responseJsonContent.Should().BeEquivalentTo(expectedResponseJsonContent);
        }

        private void ConfigureGetCurrentQuotesForCryptocurrencyUseCaseResponse()
        {
            _getCurrentQuotesForCryptocurrencyUseCaseMock.Setup(useCase =>
                    useCase.Execute(It.Is<CryptocurrencyQuotesRequest>(request =>
                        request.CryptocurrencyCode == BTC)))
                .ReturnsAsync(new CryptocurrencyQuotesResponse
                {
                    BaseCryptocurrencyCode = BTC,
                    Quotes = new List<QuoteCurrencyDetails>
                    {
                        new QuoteCurrencyDetails { Code = USD, ExchangeRate = BTC_to_USD },
                        new QuoteCurrencyDetails { Code = EUR, ExchangeRate = BTC_to_EUR },
                        new QuoteCurrencyDetails { Code = BRL, ExchangeRate = BTC_to_BRL },
                        new QuoteCurrencyDetails { Code = GBP, ExchangeRate = BTC_to_GBP },
                        new QuoteCurrencyDetails { Code = AUD, ExchangeRate = BTC_to_AUD }
                    }
                });
        }
    }
}