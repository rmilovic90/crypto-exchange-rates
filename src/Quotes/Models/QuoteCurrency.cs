using System;

namespace CryptoExchangeRates.Quotes.Models
{
    public sealed class QuoteCurrency
    {
        public static QuoteCurrency Of(CurrencyCode code, CurrencyExchangeRate rate)
        {
            if (code is null)
                throw new ArgumentNullException(nameof(code), $"{nameof(QuoteCurrency)} {nameof(code)} is required");
            if (rate is null)
                throw new ArgumentNullException(nameof(rate), $"{nameof(QuoteCurrency)} {nameof(rate)} is required");

            return new QuoteCurrency(code, rate);
        }

        private QuoteCurrency(CurrencyCode code, CurrencyExchangeRate rate)
        {
            Code = code;
            Rate = rate;
        }

        public CurrencyCode Code { get; }
        public CurrencyExchangeRate Rate { get; }

        public override bool Equals(object obj) =>
            ReferenceEquals(this, obj)
            || obj is QuoteCurrency other
            && Equals(other);

        private bool Equals(QuoteCurrency other) =>
            Code.Equals(other.Code)
            && Rate.Equals(other.Rate);

        public override int GetHashCode()
        {
            unchecked
            {
                return (Code.GetHashCode() * 397) ^ Rate.GetHashCode();
            }
        }

        public override string ToString() =>
            $"{nameof(QuoteCurrency)} {{ {nameof(Code)}: {Code}, {nameof(Rate)}: {Rate} }}";

        public static bool operator ==(QuoteCurrency left, QuoteCurrency right) => Equals(left, right);

        public static bool operator !=(QuoteCurrency left, QuoteCurrency right) => !Equals(left, right);
    }
}