using System;

namespace CryptoExchangeRates.Quotes.Models
{
    public sealed class QuoteCurrency
    {
        public static QuoteCurrency Of(CurrencyCode code, CurrencyExchangeRate exchangeRate)
        {
            if (code is null)
                throw new ArgumentNullException(nameof(code), $"{nameof(QuoteCurrency)} {nameof(code)} is required");
            if (exchangeRate is null)
                throw new ArgumentNullException(nameof(exchangeRate), $"{nameof(QuoteCurrency)} {nameof(exchangeRate)} is required");

            return new QuoteCurrency(code, exchangeRate);
        }

        private QuoteCurrency(CurrencyCode code, CurrencyExchangeRate exchangeRate)
        {
            Code = code;
            ExchangeRate = exchangeRate;
        }

        public CurrencyCode Code { get; }
        public CurrencyExchangeRate ExchangeRate { get; }

        public override bool Equals(object obj) =>
            ReferenceEquals(this, obj)
            || obj is QuoteCurrency other
            && Equals(other);

        private bool Equals(QuoteCurrency other) =>
            Code.Equals(other.Code)
            && ExchangeRate.Equals(other.ExchangeRate);

        public override int GetHashCode()
        {
            unchecked
            {
                return (Code.GetHashCode() * 397) ^ ExchangeRate.GetHashCode();
            }
        }

        public override string ToString() =>
            $"{nameof(QuoteCurrency)} {{ {nameof(Code)}: {Code}, {nameof(ExchangeRate)}: {ExchangeRate} }}";

        public static bool operator ==(QuoteCurrency left, QuoteCurrency right) => Equals(left, right);

        public static bool operator !=(QuoteCurrency left, QuoteCurrency right) => !Equals(left, right);
    }
}