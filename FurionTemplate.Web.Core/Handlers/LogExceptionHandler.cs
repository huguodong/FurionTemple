using Furion.DependencyInjection;
using Furion.FriendlyException;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace FurionTemplate.Web.Core.Handlers
{
    public class LogExceptionHandler : IGlobalExceptionHandler, ISingleton
    {
        private readonly IHostEnvironment _hostEnvironment;
        private readonly ILogger<LogExceptionHandler> _logger;

        public LogExceptionHandler(IHostEnvironment hostEnvironment,
                                      ILogger<LogExceptionHandler> logger)
        {
            this._hostEnvironment = hostEnvironment;
            this._logger = logger;
        }

        public Task OnExceptionAsync(ExceptionContext context)
        {

            // 写日志
            return Task.CompletedTask;
        }
    }
}
