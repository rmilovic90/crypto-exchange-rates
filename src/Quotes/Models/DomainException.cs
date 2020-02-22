using System;

namespace CryptoExchangeRates.Quotes.Models
{
    public sealed class DomainException : Exception
    {
        public DomainException(DomainErrors error, string message) : base(message)
        {
            Error = error;
        }

        public DomainErrors Error { get; }
    }
}