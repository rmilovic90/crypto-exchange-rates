using System;
using System.IO;
using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

namespace CryptoExchangeRates.WebApi.Configuration
{
    internal static class SwaggerConfiguration
    {
        public static void ConfigureSwagger(this IServiceCollection services)
        {
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc(
                    "v1",
                    new OpenApiInfo
                    {
                        Title = "Cryptocurrency Exchange Rates API",
                        Description = "Web API for getting the latest cryptocurrency exchange rates.",
                        Version = "v1"
                    });

                var xmlCommentsFileName = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlCommentsFilePath = Path.Combine(AppContext.BaseDirectory, xmlCommentsFileName);
                options.IncludeXmlComments(xmlCommentsFilePath);
            });
        }

        public static void UseSwaggerWithUI(this IApplicationBuilder app)
        {
            app.UseSwagger(options =>
            {
                options.RouteTemplate = "api-docs/{documentName}/specification.json";
            });

            app.UseSwaggerUI(options =>
            {
                options.RoutePrefix = "api-docs";
                options.SwaggerEndpoint("/api-docs/v1/specification.json", "Cryptocurrency Exchange Rates API V1");
            });
        }
    }
}