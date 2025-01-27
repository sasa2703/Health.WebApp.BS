using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthManager.WebApp.BS.Shared.Exceptions
{
    public abstract class UnauthorizedException : AppExceptionBase
    {
        public UnauthorizedException(string? message)
            : base(401, message)
        {
        }

        public UnauthorizedException(string? message, Exception? innerException)
            : base(401, message, innerException)
        {
        }
    }
}
