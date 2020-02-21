using System;

namespace CryptoExchangeRates.Quotes.Models
{
    public sealed class CurrencyCode
    {
        public static CurrencyCode Of(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new DomainException($"{nameof(CurrencyCode)} {nameof(value)} must not be blank.");

            return new CurrencyCode(value.ToUpper());
        }

        private readonly string _value;

        private CurrencyCode(string value)
        {
            _value = value;
        }

        public override bool Equals(object obj) =>
            ReferenceEquals(this, obj)
            || obj is CurrencyCode other
            && Equals(other);

        private bool Equals(CurrencyCode other) =>
            string.Equals(_value, other._value, StringComparison.OrdinalIgnoreCase);

        public override int GetHashCode() => StringComparer.OrdinalIgnoreCase.GetHashCode(_value);

        public override string ToString() => _value;

        public static bool operator ==(CurrencyCode left, CurrencyCode right) => Equals(left, right);

        public static bool operator !=(CurrencyCode left, CurrencyCode right) => !Equals(left, right);

        public static implicit operator string(CurrencyCode currencyCode) => currencyCode._value;
    }
}