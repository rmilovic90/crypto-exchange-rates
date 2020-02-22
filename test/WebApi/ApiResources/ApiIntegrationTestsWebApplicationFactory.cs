using CryptoExchangeRates.Quotes.UseCases.GetCurrentQuotesForCryptocurrencyUseCase;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using StartupBase = CryptoExchangeRates.WebApi.Configuration.StartupBase;

namespace CryptoExchangeRates.WebApi.ApiResources
{
    public sealed class ApiIntegrationTestsWebApplicationFactory : WebApplicationFactory<StartupBase>
    {
        public ApiIntegrationTestsWebApplicationFactory()
        {
            GetLatestQuotesForCryptocurrencyUseCaseMock = new Mock<IGetCurrentQuotesForCryptocurrency>();
        }

        public Mock<IGetCurrentQuotesForCryptocurrency> GetLatestQuotesForCryptocurrencyUseCaseMock { get; }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.UseEnvironment("Test")
                .ConfigureTestServices(services =>
                {
                    services.AddSingleton(GetLatestQuotesForCryptocurrencyUseCaseMock.Object);
                });
        }
    }
}