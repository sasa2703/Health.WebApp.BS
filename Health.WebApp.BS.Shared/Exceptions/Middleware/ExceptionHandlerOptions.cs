using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthManager.WebApp.BS.Shared.Exceptions.Middleware
{
    public class ExceptionHandlerOptions
    {
        public const string ExceptionHandler = "ExceptionHandler";

        public bool LogExceptions { get; set; }

        public bool ShowStackTrace { get; set; }

        public bool ShowInnerException { get; set; }
    }
}
