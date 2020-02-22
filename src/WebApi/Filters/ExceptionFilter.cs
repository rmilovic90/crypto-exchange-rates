using System.Net;
using CryptoExchangeRates.Quotes.Models;
using CryptoExchangeRates.WebApi.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Hosting;

namespace CryptoExchangeRates.WebApi.Filters
{
    public sealed class ExceptionFilter : IActionFilter, IOrderedFilter
    {
        public const int UnknownErrorCode = 0;
        public const string UnknownErrorMessage = "An unexpected error occurred.";

        private readonly IWebHostEnvironment _environment;

        public ExceptionFilter(IWebHostEnvironment environment)
        {
            _environment = environment;
        }

        public int Order { get; } = int.MaxValue - 10;

        public void OnActionExecuting(ActionExecutingContext context) { }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            if (context.Exception is DomainException exception)
            {
                context.Result = new ObjectResult(AsErrorResponse(exception))
                {
                    StatusCode = (int) HttpStatusCode.BadRequest
                };
                context.ExceptionHandled = true;
            }
            else if (context.Exception != null && !_environment.IsDevelopment())
            {
                context.Result = new ObjectResult(UnknownErrorResponse)
                {
                    StatusCode = (int) HttpStatusCode.InternalServerError
                };
                context.ExceptionHandled = true;
            }
        }

        private ErrorResponse AsErrorResponse(DomainException exception) =>
            new ErrorResponse
            {
                ErrorCode = (int) exception.Error,
                ErrorMessage = exception.Message
            };

        private ErrorResponse UnknownErrorResponse =>
            new ErrorResponse
            {
                ErrorCode = UnknownErrorCode,
                ErrorMessage = UnknownErrorMessage
            };
    }
}