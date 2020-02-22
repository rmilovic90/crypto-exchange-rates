using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using FluentAssertions.Json;
using Newtonsoft.Json.Linq;
using Xunit;

using static CryptoExchangeRates.WebApi.Filters.ExceptionFilter;
using static CryptoExchangeRates.WebApi.Filters.TestController;

namespace CryptoExchangeRates.WebApi.Filters
{
    public sealed class ExceptionFilterTests : IClassFixture<FilterTestsWebApplicationFactory>
    {
        private readonly HttpClient _httpClient;

        public ExceptionFilterTests(FilterTestsWebApplicationFactory webApplicationFactory)
        {
            _httpClient = webApplicationFactory.CreateClient();
        }

        [Fact]
        public async Task returns_400_bad_request_status_response_with_correct_content_when_a_domain_error_occurs()
        {
            var expectedResponseJsonContent = JObject.Parse($@"{{
              ""errorCode"": {SampleDomainErrorCode},
              ""errorMessage"": ""{SampleDomainErrorMessage}""
            }}");

            var response = await _httpClient.GetAsync($"api/test/domain-error");

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            var responseJsonContent = JObject.Parse(await response.Content.ReadAsStringAsync());
            responseJsonContent.Should().BeEquivalentTo(expectedResponseJsonContent);
        }

        [Fact]
        public async Task returns_500_internal_server_error_status_response_with_correct_content_when_an_unhandled_error_occurs()
        {
            var expectedResponseJsonContent = JObject.Parse($@"{{
              ""errorCode"": {UnknownErrorCode},
              ""errorMessage"": ""{UnknownErrorMessage}""
            }}");

            var response = await _httpClient.GetAsync($"api/test/unhandled-error");

            response.StatusCode.Should().Be(HttpStatusCode.InternalServerError);
            var responseJsonContent = JObject.Parse(await response.Content.ReadAsStringAsync());
            responseJsonContent.Should().BeEquivalentTo(expectedResponseJsonContent);
        }
    }
}