using System.Threading.Tasks;
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
        private readonly IGetCurrentQuotesForCryptocurrency _getCurrentQuotesForCryptocurrencyUseCase;

        public CryptocurrencyQuotesController(IGetCurrentQuotesForCryptocurrency getCurrentQuotesForCryptocurrencyUseCase)
        {
            _getCurrentQuotesForCryptocurrencyUseCase = getCurrentQuotesForCryptocurrencyUseCase;
        }

        /// <summary>
        /// Gets the latest quotes for a cryptocurrency.
        /// </summary>
        /// <param name="baseCryptocurrencyCode"></param>
        /// <returns>The latest quotes for a cryptocurrency.</returns>
        /// <response code="200">Returns the latest quotes for a cryptocurrency</response>
        /// <response code="400">If the base cryptocurrency code is invalid</response>
        /// <response code="500">If an unknown error occurs</response>
        [HttpGet(Name = "GetCryptocurrencyQuotes")]
        [ProducesResponseType(typeof(CryptocurrencyQuotesResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponse),StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Get(string baseCryptocurrencyCode)
        {
            var cryptocurrencyQuotesRequest = new CryptocurrencyQuotesRequest
            {
                CryptocurrencyCode = baseCryptocurrencyCode
            };

            var cryptocurrencyQuotesResponse =
                await _getCurrentQuotesForCryptocurrencyUseCase.Execute(cryptocurrencyQuotesRequest);

            return Ok(cryptocurrencyQuotesResponse);
        }
    }
}