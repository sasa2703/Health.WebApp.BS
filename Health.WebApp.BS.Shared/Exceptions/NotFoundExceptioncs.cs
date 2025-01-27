namespace HealthManager.WebApp.BS.Shared.Exceptions
{
    public abstract class NotFoundException : AppExceptionBase
    {
        protected NotFoundException(string? message)
            : base(404, message)
        {
        }

        protected NotFoundException(string? message, Exception? innerException)
            : base(404, message, innerException)
        {
        }
    }
}
