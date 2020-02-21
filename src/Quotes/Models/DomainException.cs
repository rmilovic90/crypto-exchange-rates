using System;

namespace CryptoExchangeRates.Quotes.Models
{
    public sealed class DomainException : Exception
    {
        public DomainException(string message) : base(message) { }
    }
}