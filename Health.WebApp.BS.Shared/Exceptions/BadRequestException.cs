using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthManager.WebApp.BS.Shared.Exceptions
{
    public abstract class BadRequestException : AppExceptionBase
    {
        public BadRequestException(string? message)
            : base(400, message)
        {
        }

        public BadRequestException(string? message, Exception? innerException)
            : base(400, message, innerException)
        {
        }
    }
}
