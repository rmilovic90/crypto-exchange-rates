using System;
using CryptoExchangeRates.Quotes.Models;
using Microsoft.AspNetCore.Mvc;

namespace CryptoExchangeRates.WebApi.Filters
{
    [Route("api/test")]
    [ApiController]
    public sealed class TestController : ControllerBase
    {
        private const DomainErrors SampleDomainError = DomainErrors.CurrencyCodeInvalidFormat;
        public const int SampleDomainErrorCode = (int) SampleDomainError;
        public const string SampleDomainErrorMessage = "CurrencyCode value must have valid format (3 letters).";

        [HttpGet("unhandled-error")]
        public IActionResult GetUnhandledError()
        {
            throw new Exception("Some error occurred.");
        }

        [HttpGet("domain-error")]
        public IActionResult GetDomainError()
        {
            throw new DomainException(SampleDomainError, SampleDomainErrorMessage);
        }
    }
}