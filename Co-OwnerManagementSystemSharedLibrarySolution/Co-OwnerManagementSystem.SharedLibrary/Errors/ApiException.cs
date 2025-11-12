namespace Co_OwnerManagementSystem.SharedLibrary.Errors
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
