using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace CryptoExchangeRates.WebApi.ApiResources
{
    public sealed class ApiResourcesSmokeTests : IClassFixture<SmokeTestsWebApplicationFactory>
    {
        private readonly HttpClient _httpClient;

        public ApiResourcesSmokeTests(SmokeTestsWebApplicationFactory webApplicationFactory)
        {
            _httpClient = webApplicationFactory.CreateClient();
        }

        [Fact]
        public async Task returns_200_ok_status_response_when_calling_get_latest_quotes_for_cryptocurrency()
        {
            var response = await _httpClient.GetAsync("api/v1.0/BTC/quotes");

            response.EnsureSuccessStatusCode();
        }
    }
}