namespace CryptoExchangeRates.WebApi.Models
{
    public sealed class ErrorResponse
    {
        public int ErrorCode { get; set; }
        public string ErrorMessage { get; set; }
    }
}