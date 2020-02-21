﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using CryptoExchangeRates.Quotes.Gateways;
using CryptoExchangeRates.Quotes.Models;

using static CryptoExchangeRates.Quotes.Infrastructure.CoinMarketCapExchangeRatesWebServiceQuoteCurrencyFactory;

namespace CryptoExchangeRates.Quotes.Infrastructure
{
    internal sealed class CoinMarketCapExchangeRatesWebService : IExchangeRatesService
    {
        private const string ApiKeyHeaderName = "X-CMC_PRO_API_KEY";
        private const string GetLatestCryptocurrencyQuotesRequestUrlPathTemplate =
            "/v1/cryptocurrency/quotes/latest?symbol={0}&convert={1}";

        private static readonly MediaTypeWithQualityHeaderValue JsonMediaTypeHeaderValue =
            new MediaTypeWithQualityHeaderValue("application/json");
        private static readonly string[] SupportedQuoteCurrencyCodes = { "USD", "EUR", "BRL", "GBP", "AUD" };

        private readonly IHttpClientFactory _httpClientFactory;
        private readonly CoinMarketCapExchangeRatesWebServiceConfiguration _configuration;

        public CoinMarketCapExchangeRatesWebService(
            IHttpClientFactory httpClientFactory,
            CoinMarketCapExchangeRatesWebServiceConfiguration configuration)
        {
            _httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            _configuration.Validate();
        }

        public IReadOnlyList<QuoteCurrency> GetQuotesFor(CurrencyCode baseCryptocurrencyCode)
        {
            if (baseCryptocurrencyCode is null)
                throw new ArgumentNullException(nameof(baseCryptocurrencyCode));

            var httpClient = CreateHttpClient();

            var responses = Task.WhenAll(
                PrepareGetLatestCryptocurrencyQuotesRequests(baseCryptocurrencyCode, httpClient)).GetAwaiter().GetResult();

            return responses.Select(CreateQuoteCurrencyFromGetLatestCryptocurrencyQuotesResponse)
                .ToList().AsReadOnly();
        }

        private HttpClient CreateHttpClient()
        {
            var httpClient = _httpClientFactory.CreateClient();
            httpClient.BaseAddress = _configuration.BaseUrl;
            ConfigureHeaders(httpClient);

            return httpClient;
        }

        private void ConfigureHeaders(HttpClient httpClient)
        {
            AddApiKeyHeader(httpClient);
            AddAcceptTypeHeader(httpClient);
        }

        private void AddApiKeyHeader(HttpClient httpClient)
        {
            httpClient.DefaultRequestHeaders.Add(ApiKeyHeaderName, _configuration.ApiKey);
        }

        private void AddAcceptTypeHeader(HttpClient httpClient)
        {
            httpClient.DefaultRequestHeaders.Accept.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(JsonMediaTypeHeaderValue);
        }

        private IEnumerable<Task<HttpResponseMessage>> PrepareGetLatestCryptocurrencyQuotesRequests(
            CurrencyCode baseCryptocurrencyCode, HttpClient httpClient) =>
                SupportedQuoteCurrencyCodes.Select(code => httpClient.GetAsync(
                    FormatGetLatestCryptocurrencyQuotesRequestUrlPathFor(
                        baseCryptocurrencyCode,
                        CurrencyCode.Of(code))));

        private string FormatGetLatestCryptocurrencyQuotesRequestUrlPathFor(
            CurrencyCode baseCryptocurrencyCode, CurrencyCode quoteCurrencyCode) =>
                Uri.EscapeUriString(
                    string.Format(
                        GetLatestCryptocurrencyQuotesRequestUrlPathTemplate,
                        baseCryptocurrencyCode,
                        quoteCurrencyCode));
    }
}