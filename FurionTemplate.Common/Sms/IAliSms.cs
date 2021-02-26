using Aliyun.Acs.Core;

namespace FurionTemplate.Common.Sms
{
    public interface IAliSms
    {
        /// <summary>
        /// 发送报警短信
        /// </summary>
        /// <param name="phoneList"></param>
        /// <param name="deviceName"></param>
        /// <param name="outId"></param>
        /// <returns></returns>
        CommonResponse SendAlarmSms(string phoneList, string deviceName, out string outId);
        /// <summary>
        /// 发送登录验证短信
        /// </summary>
        /// <param name="phone"></param>
        /// <param name="loginCode"></param>
        /// <param name="outId"></param>
        /// <returns></returns>
        CommonResponse SendLoginCode(string phone, string loginCode, out string outId);

        /// <summary>
        /// 发送注册验证短信
        /// </summary>
        /// <param name="phone"></param>
        /// <param name="registerCode"></param>
        /// <param name="outId"></param>
        /// <returns></returns>
        CommonResponse SendRegisterCode(string phone, string registerCode, out string outId);
    }
}
