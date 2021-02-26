using BaiduHelper;
using FurionTemplate.Common.Helper;
using System.Collections.Generic;
using System.Linq;

namespace FurionTemplate.Common.BaiDu
{
    public static class LotHubHelper
    {
        private static readonly string accessKey = Appsettings.App(new string[] { "AppSettings", "BaiDuYunSettings", "AccessKey" });
        private static readonly string accessKeySecret = Appsettings.App(new string[] { "AppSettings", "BaiDuYunSettings", "AccessKeySecret" });
        private static readonly string endpointName = Appsettings.App(new string[] { "AppSettings", "BaiDuYunSettings", "LotHubSettings", "EndpointName" });
        private static readonly string userName = Appsettings.App(new string[] { "AppSettings", "BaiDuYunSettings", "LotHubSettings", "UserName" });
        private static readonly string passWord = Appsettings.App(new string[] { "AppSettings", "BaiDuYunSettings", "LotHubSettings", "PassWord" });
        private static readonly string policyName = Appsettings.App(new string[] { "AppSettings", "BaiDuYunSettings", "LotHubSettings", "PolicyName" });
        private static readonly BaiduIotHubHelper helper_shiny = new BaiduIotHubHelper(accessKey, accessKeySecret, userName, passWord);

        /// <summary>
        /// 添加主题
        /// </summary>
        /// <param name="topicName"></param>
        /// <returns></returns>
        public static bool AddTopic(string topicName)
        {
            var result = helper_shiny.AddPermission(endpointName, policyName, new List<Operation> { Operation.PUBLISH, Operation.SUBSCRIBE }, topicName);
            return result.createTime != null;

        }

        /// <summary>
        /// 修改主题
        /// </summary>
        /// <param name="topicName"></param>
        /// <returns></returns>
        public static bool UpdateTopic(string oldName, string newName)
        {
            var permissionList = helper_shiny.GetPermissionList(endpointName, policyName);
            var topic = permissionList.result.Where(it => it.topic == oldName).FirstOrDefault();
            if (topic != null)
            {
                var result = helper_shiny.SetPermission(endpointName, topic.permissionUuid, new List<Operation> { Operation.PUBLISH, Operation.SUBSCRIBE }, newName);
                return result.permissionUuid != null;
            }
            else
            {
                return AddTopic(newName);
            }

        }
    }
}
