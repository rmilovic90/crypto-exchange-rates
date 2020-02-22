using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using CryptoExchangeRates.Quotes.Models;

namespace CryptoExchangeRates.Quotes.Infrastructure.ExchangeRatesService
{
    internal static class CoinMarketCapExchangeRatesWebServiceQuoteCurrencyFactory
    {
        private const string QuotesPropertyName = "quote";
        private const string QuoteCurrencyCrossRatePropertyName = "price";

        // NOTE: Parsing of the CoinMarketCap API cryptocurrency quotes response is done this way because
        // JSON content has dynamic property names (based on quote currency code). I could not create the
        // static type to deserialize it into.
        public static async Task<QuoteCurrency> CreateQuoteCurrencyFromGetLatestCryptocurrencyQuotesResponse(
            HttpResponseMessage response)
        {
            if (response is null)
                throw new ArgumentNullException(nameof(response));

            response.EnsureSuccessStatusCode();

            var jsonText = await response.Content.ReadAsStringAsync();

            return CreateQuoteCurrencyFromGetLatestCryptocurrencyQuotesResponseJsonContent(jsonText);
        }

        private static QuoteCurrency CreateQuoteCurrencyFromGetLatestCryptocurrencyQuotesResponseJsonContent(
            string jsonText)
        {
            var jsonReader = new Utf8JsonReader(Encoding.UTF8.GetBytes(jsonText), isFinalBlock: true, state: default);

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