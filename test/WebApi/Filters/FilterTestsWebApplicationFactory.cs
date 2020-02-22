using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using StartupBase = CryptoExchangeRates.WebApi.Configuration.StartupBase;

namespace CryptoExchangeRates.WebApi.Filters
{
    public sealed class FilterTestsWebApplicationFactory : WebApplicationFactory<StartupBase>
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.UseEnvironment("Test");

            builder.ConfigureServices(services =>
            {
                services.AddControllers()
                    .AddApplicationPart(GetType().Assembly);
            });
        }
    }
}