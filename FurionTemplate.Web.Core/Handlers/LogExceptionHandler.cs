using Furion.DependencyInjection;
using Furion.FriendlyException;
using FurionTemplate.Core.Enum;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
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
            string logMessage = string.Empty;
            var logName = "系统异常";
            var param = new List<object>
            {
                logName,
                LogTypeEnum.System
            };
            logMessage += "【日志名称】:{LogName} \r\n";
            logMessage += "【日志类型】:{LogType} \r\n";
            _logger.LogError(context.Exception, logMessage, param.ToArray());
            return Task.CompletedTask;
        }
    }
}
