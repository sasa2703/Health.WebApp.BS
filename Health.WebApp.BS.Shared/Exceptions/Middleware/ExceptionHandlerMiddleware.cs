using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthManager.WebApp.BS.Shared.Exceptions.Middleware
{
    public class ExceptionHandlerMiddleware
    {
        private readonly RequestDelegate _next;

        private readonly ILogger<ExceptionHandlerMiddleware>? _logger;

        private readonly ExceptionHandlerOptions _options;

        public ExceptionHandlerMiddleware(RequestDelegate next, IOptions<ExceptionHandlerOptions> options, ILogger<ExceptionHandlerMiddleware>? logger = null)
        {
            _next = next;
            _options = options.Value ?? new ExceptionHandlerOptions();
            _logger = (_options.LogExceptions ? logger : null);
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger?.LogError("An exception occured: " + ex.Message + "\nStack trace: " + ex.StackTrace + "\nInner exception: " + ex.InnerException?.Message);
                await HandleExceptionAsync(context, ex);
            }
        }

        internal async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            int statusCode = 500;
            string message = "Unspecified server error";
            if (exception is AppExceptionBase)
            {
                statusCode = (exception as AppExceptionBase).ErrorCode;
                message = exception.Message;
            }

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = statusCode;
            await context.Response.WriteAsync(new ErrorDetails
            {
                StatusCode = context.Response.StatusCode,
                Message = message,
                StackTrace = ((!_options.ShowStackTrace) ? null : exception?.StackTrace),
                InnerExceptionMessage = ((!_options.ShowInnerException) ? null : exception?.InnerException?.Message)
            }.ToString());
        }
    }
}
