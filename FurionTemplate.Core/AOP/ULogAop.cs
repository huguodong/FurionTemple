using Furion;
using Furion.Reflection;
using FurionTemplate.Core.Enum;
using Microsoft.Extensions.Logging;
using NewLife.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace FurionTemplate.Core.AOP
{
    public class ULogAop : AspectDispatchProxy, IGlobalDispatchProxy
    {

        /// <summary>
        /// 当前服务实例
        /// </summary>
        public object Target { get; set; }

        /// <summary>
        /// 服务提供器，可以用来解析服务，如：Services.GetService()
        /// </summary>
        public IServiceProvider Services { get; set; }

        /// <summary>
        /// 拦截方法
        /// </summary>
        /// <param name="method"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public override object Invoke(MethodInfo method, object[] args)
        {

            if (method.ReturnType == typeof(void) || method.ReturnType == typeof(Task))
            {
                return method.Invoke(Target, args);
            }
            else
            {
                var result = method.Invoke(Target, args);
                RecordLog(method, result);
                return result;
            }
        }

        /// <summary>
        /// 异步无返回值
        /// </summary>
        /// <param name="method"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public override async Task InvokeAsync(MethodInfo method, object[] args)
        {
            var task = method.Invoke(Target, args) as Task;
            await task;
        }

        /// <summary>
        /// 异步带返回值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="method"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public override async Task<T> InvokeAsyncT<T>(MethodInfo method, object[] args)
        {
            var taskT = method.Invoke(Target, args) as Task<T>;
            var result = await taskT;
            RecordLog(method, result);
            return result;
        }

        /// <summary>
        /// 记录日志
        /// </summary>
        /// <param name="method"></param>
        /// <param name="returnValue"></param>
        private void RecordLog(MethodInfo method, object returnValue)
        {
            if (method.GetCustomAttributes(true).FirstOrDefault(x => x.GetType() == typeof(ULogAttribute)) is ULogAttribute uLogAttribute)
            {
                CreateLog(uLogAttribute, returnValue);
            }
        }

        /// <summary>
        /// 创建日志内容
        /// </summary>
        /// <param name="uLogAttribute"></param>
        /// <param name="returnValue"></param>
        private void CreateLog(ULogAttribute uLogAttribute, object returnValue = null)
        {
            string logMessage = string.Empty;
            List<object> param = null;
            var result = returnValue.ToJson();
            var logName = uLogAttribute.LogName;
            var logType = uLogAttribute.LogType;
            var operation = uLogAttribute.OperationType;
            param = new List<object>
            {
                logName,
                logType,
                operation,
                result
            };
            logMessage += "【日志名称】:{LogName} \r\n";
            logMessage += "【日志类型】:{LogType} \r\n";
            logMessage += "【操作类型】:{OperationType} \r\n";
            logMessage += "【执行结果】:{Result} \r\n";
            var logger = App.GetService<ILogger<ULogAop>>();
            logger.LogInformation(logMessage, param.ToArray());

        }
    }

    public class ULogAttribute : Attribute
    {
        /// <summary>
        /// 操作名称
        /// </summary>
        public string LogName { get; set; }
        /// <summary>
        /// 日志类型
        /// </summary>
        public LogTypeEnum LogType { get; set; } = LogTypeEnum.Operation;
        /// <summary>
        /// 操作类型
        /// </summary>
        public OperationTypeEnum OperationType { get; set; }
    }

    /// <summary>
    /// 用户操作
    /// </summary>
    public class ULogModel
    {
        public ULogAttribute ULogAttribute { get; set; }
        public object ReturnValue { get; set; }
    }
}
