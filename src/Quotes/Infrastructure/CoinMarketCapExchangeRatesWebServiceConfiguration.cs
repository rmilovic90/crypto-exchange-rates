﻿using System;

namespace CryptoExchangeRates.Quotes.Infrastructure
{
    public sealed class CoinMarketCapExchangeRatesWebServiceConfiguration
    {
        public Uri BaseUrl { get; set; }
        public string ApiKey { get; set; }

        public void Validate()
        {
            if (BaseUrl is null)
                throw new InvalidConfigurationException(
                    $"{nameof(CoinMarketCapExchangeRatesWebServiceConfiguration)} {nameof(BaseUrl)} is required");
            if (string.IsNullOrWhiteSpace(ApiKey))
                throw new InvalidConfigurationException(
                    $"{nameof(CoinMarketCapExchangeRatesWebServiceConfiguration)} {nameof(ApiKey)} can't be blank");
        }
    }
}