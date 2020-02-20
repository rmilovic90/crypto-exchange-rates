using System;
using System.Globalization;

namespace CryptoExchangeRates.Quotes.Models
{
    public sealed class CurrencyExchangeRate
    {
        public static CurrencyExchangeRate Of(decimal value)
        {
            if (value <= Decimal.Zero)
                throw new ArgumentException(
                    $"{nameof(CurrencyExchangeRate)} {nameof(value)} must be higher than {decimal.Zero}",
                    nameof(value));

            return new CurrencyExchangeRate(value);
        }

        private readonly decimal _value;

        private CurrencyExchangeRate(decimal value)
        {
            _value = value;
        }

        public override bool Equals(object obj) =>
            ReferenceEquals(this, obj)
            || obj is CurrencyExchangeRate other
            && Equals(other);

        private bool Equals(CurrencyExchangeRate other) => _value == other._value;

        public override int GetHashCode() => _value.GetHashCode();

        public override string ToString() => _value.ToString(CultureInfo.InvariantCulture);

        public static bool operator ==(CurrencyExchangeRate left, CurrencyExchangeRate right) => Equals(left, right);

        public static bool operator !=(CurrencyExchangeRate left, CurrencyExchangeRate right) => !Equals(left, right);

        public static implicit operator decimal(CurrencyExchangeRate currencyExchangeRate) => currencyExchangeRate._value;
    }
}