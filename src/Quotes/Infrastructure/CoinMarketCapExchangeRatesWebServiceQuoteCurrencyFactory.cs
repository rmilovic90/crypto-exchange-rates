using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using CryptoExchangeRates.Quotes.Models;

namespace CryptoExchangeRates.Quotes.Infrastructure
{
    internal static class CoinMarketCapExchangeRatesWebServiceQuoteCurrencyFactory
    {
        private const string QuotesPropertyName = "quote";
        private const string QuoteCurrencyCrossRatePropertyName = "price";

        public static QuoteCurrency CreateQuoteCurrencyFromGetLatestCryptocurrencyQuotesResponse(
            HttpResponseMessage response)
        {
            if (response is null)
                throw new ArgumentNullException(nameof(response));

            response.EnsureSuccessStatusCode();

            var jsonText = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
            var jsonReader = new Utf8JsonReader(Encoding.UTF8.GetBytes(jsonText), isFinalBlock: true, state: default);

            return CreateQuoteCurrencyFromGetLatestCryptocurrencyQuotesResponseJsonContent(jsonReader);
        }

        private static QuoteCurrency CreateQuoteCurrencyFromGetLatestCryptocurrencyQuotesResponseJsonContent(
            Utf8JsonReader jsonReader)
        {
            CurrencyCode currencyCode = null;
            CurrencyExchangeRate crossRate = null;

            var currentPropertyName = string.Empty;
            while (jsonReader.Read())
            {
                switch (jsonReader.TokenType)
                {
                    case JsonTokenType.PropertyName:
                        var previousPropertyName = currentPropertyName;
                        currentPropertyName = jsonReader.GetString();
                        TryParseQuoteCurrencyCode(previousPropertyName, currentPropertyName, ref currencyCode);
                        continue;
                    case JsonTokenType.Number:
                        TryParseQuoteCurrencyCrossRate(jsonReader.GetDecimal(), currentPropertyName, ref crossRate);
                        break;
                    default:
                        continue;
                }
            }

            return QuoteCurrency.Of(currencyCode, crossRate);
        }

        private static void TryParseQuoteCurrencyCode(
            string previousPropertyName, string currentPropertyName, ref CurrencyCode currencyCode)
        {
            if (previousPropertyName == QuotesPropertyName)
            {
                currencyCode = CurrencyCode.Of(currentPropertyName);
            }
        }

        private static void TryParseQuoteCurrencyCrossRate(
            decimal value, string currentPropertyName, ref CurrencyExchangeRate crossRate)
        {
            if (currentPropertyName == QuoteCurrencyCrossRatePropertyName)
            {
                crossRate = CurrencyExchangeRate.Of(value);
            }
        }
    }
}