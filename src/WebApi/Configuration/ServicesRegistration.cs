using System.Net.Http;
using CryptoExchangeRates.Quotes;
using CryptoExchangeRates.Quotes.Gateways;
using CryptoExchangeRates.Quotes.Infrastructure.ExchangeRatesService;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CryptoExchangeRates.WebApi.Configuration
{
    internal static class ServicesRegistration
    {
        public static void RegisterCustomServices(this IServiceCollection services, IConfiguration configuration)
        {
            var getCurrentQuotesForCryptocurrencyUseCase = QuotesServicesFactory.UseCases
                .GetCurrentQuotesForCryptocurrency
                .Create(CreateCoinMarketCapExchangeRatesWebService(services, configuration));

            services.AddSingleton(getCurrentQuotesForCryptocurrencyUseCase);
        }

        private static IExchangeRatesService CreateCoinMarketCapExchangeRatesWebService(
            IServiceCollection services, IConfiguration configuration) =>
                QuotesServicesFactory.Infrastructure
                    .ExchangeRatesService
                    .CreateCoinMarketCapExchangeRatesWebService(
                        services.GetHttpClientFactory(),
                        configuration.GetCoinMarketCapWebServiceConfiguration());

        private static IHttpClientFactory GetHttpClientFactory(this IServiceCollection services)
        {
            services.AddHttpClient();

            var serviceProvider = services.BuildServiceProvider();

            return serviceProvider.GetRequiredService<IHttpClientFactory>();
        }

        private static CoinMarketCapWebServiceConfiguration GetCoinMarketCapWebServiceConfiguration(
            this IConfiguration configuration)
        {
            var coinMarketCapWebServiceConfiguration = new CoinMarketCapWebServiceConfiguration();
            configuration.Bind(nameof(CoinMarketCapWebServiceConfiguration), coinMarketCapWebServiceConfiguration);

            return coinMarketCapWebServiceConfiguration;
        }
    }
}