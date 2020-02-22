using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CryptoExchangeRates.WebApi.Configuration
{
    public sealed class Startup : StartupBase
    {
        private readonly IConfiguration _configuration;

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        protected override void ConfigureServicesCustom(IServiceCollection services)
        {
            services.ConfigureSwagger();

            services.RegisterCustomServices(_configuration);
        }

        protected override void ConfigureCustom(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseSwaggerWithUI();
        }
    }
}