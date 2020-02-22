using CryptoExchangeRates.Quotes.UseCases.GetCurrentQuotesForCryptocurrencyUseCase;
using CryptoExchangeRates.WebApi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CryptoExchangeRates.WebApi.Controllers
{
    [Route("api/v1.0/{baseCryptocurrencyCode}/quotes")]
    [ApiController]
    [Produces("application/json")]
    public sealed class CryptocurrencyQuotesController : ControllerBase
    {
        /// <summary>
        /// Gets the latest quotes for a base cryptocurrency code.
        /// </summary>
        /// <param name="baseCryptocurrencyCode"></param>
        /// <returns>The latest quotes for a base cryptocurrency code.</returns>
        /// <response code="200">Returns the latest quotes for a base cryptocurrency code</response>
        /// <response code="400">If the base cryptocurrency code is invalid</response>
        /// <response code="500">If an unknown error occurs</response>
        [HttpGet(Name = "GetCryptocurrencyQuotes")]
        [ProducesResponseType(typeof(CryptocurrencyQuotesResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponse),StatusCodes.Status500InternalServerError)]
        public IActionResult Get(string baseCryptocurrencyCode) => Ok(new CryptocurrencyQuotesResponse());
    }
}