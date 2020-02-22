using CryptoExchangeRates.WebApi.Filters;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace CryptoExchangeRates.WebApi.Configuration
{
    public abstract class StartupBase
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers(options =>
                {
                    options.Filters.Add<ExceptionFilter>();
                })
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.IgnoreNullValues = true;
                });

            ConfigureServicesCustom(services);
        }

        protected abstract void ConfigureServicesCustom(IServiceCollection services);

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });

            ConfigureCustom(app, env);
        }

        protected abstract void ConfigureCustom(IApplicationBuilder app, IWebHostEnvironment env);
    }
}