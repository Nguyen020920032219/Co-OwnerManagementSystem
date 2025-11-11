namespace SU25_Leopard_PE_LyHaiLuan.Business.Models
{
    public class ApiException : Exception
    {
        public int StatusCode { get; set; }
        public string ErrorCode { get; set; }

        public ApiException(int statusCode, string errorCode, string message) : base(message)
        {
            StatusCode = statusCode;
            ErrorCode = errorCode;
        }
    }
}
