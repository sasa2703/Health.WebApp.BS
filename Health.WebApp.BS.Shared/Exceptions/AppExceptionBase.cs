namespace HealthManager.WebApp.BS.Shared.Exceptions
{
    public abstract class AppExceptionBase : Exception
    {
        public int ErrorCode { get; private set; }

        protected AppExceptionBase(int errorCode, string? message)
            : base(message)
        {
            ErrorCode = errorCode;
        }

        protected AppExceptionBase(int errorCode, string? message, Exception? innerException)
            : base(message, innerException)
        {
            ErrorCode = errorCode;
        }
    }
}
