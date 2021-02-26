using FurionTemplate.Core.AOP;
using FurionTemplate.Core.Enum;
using Masuit.Tools;
using System.Collections.Generic;

namespace FurionTemplate.Common.Helper
{
    public static class SerilogHelper
    {
        /// <summary>
        /// 格式化系统日志内容
        /// </summary>
        /// <param name="context"></param>
        /// <param name="logMessage"></param>
        /// <param name="param"></param>
        public static void InitSystem(ref string logMessage, ref List<object> param, string logName = null)
        {
            logMessage = null;
            if (logName == null)
            {
                logName = "系统异常";
            }
            param = new List<object>
            {
                logName,
                LogTypeEnum.System
            };
            logMessage += "【日志名称】:{LogName} \r\n";
            logMessage += "【日志类型】:{LogType} \r\n";

        }

        /// <summary>
        /// 格式化设备报警日志内容
        /// </summary>
        /// <param name="context"></param>
        /// <param name="logMessage"></param>
        /// <param name="param"></param>
        public static void InitDevice(ref string logMessage, ref List<object> param, string logName = null)
        {
            logMessage = null;

            if (logName == null)
            {
                logName = "设备异常";
            }
            param = new List<object>
            {
                logName,
                LogTypeEnum.Device
            };
            logMessage += "【日志名称】:{LogName} \r\n";
            logMessage += "【日志类型】:{LogType} \r\n";

        }


        /// <summary>
        /// 格式化操作日志内容
        /// </summary>
        /// <param name="uLogModel"></param>
        /// <param name="logMessage"></param>
        /// <param name="param"></param>
        public static void InitOperation(ULogModel uLogModel, ref string logMessage, ref List<object> param)
        {
            var uLogAttribute = uLogModel.ULogAttribute;
            var result = uLogModel.ReturnValue.ToJsonString();
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
        }
    }

}
