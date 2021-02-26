using Aliyun.Acs.Core;
using Aliyun.Acs.Core.Http;
using Aliyun.Acs.Core.Profile;
using NewLife.Serialization;
using FurionTemplate.Common.Helper;
using System;
using System.Collections.Generic;

namespace FurionTemplate.Common.Sms
{
    public class AliSms : IAliSms
    {
        private readonly string _domain;
        private readonly string _accessKeyId;
        private readonly string _accessKeySecret;
        private readonly string _signName;


        public AliSms(string domain, string accessKeyId, string accessKeySecret, string signName)
        {
            _domain = domain;
            _accessKeyId = accessKeyId;
            _accessKeySecret = accessKeySecret;
            _signName = signName;

        }

        public AliSms()
        {
            _domain = Appsettings.App(new string[] { "AppSettings", "AliSmsSetting", "Domain" });
            _accessKeyId = Appsettings.App(new string[] { "AppSettings", "AliSmsSetting", "AccessKeyId" });
            _accessKeySecret = Appsettings.App(new string[] { "AppSettings", "AliSmsSetting", "AccessKeySecret" });
            _signName = Appsettings.App(new string[] { "AppSettings", "AliSmsSetting", "SignName" });

        }

        private string CreateOutId()
        {
            var outId = new NewLife.Data.Snowflake().NewId();
            return outId.ToString();
        }

        public CommonResponse SendRegisterCode(string phone, string registerCode, out string outId)
        {
            outId = CreateOutId();
            IClientProfile profile = DefaultProfile.GetProfile("cn-hangzhou", _accessKeyId, _accessKeySecret);
            DefaultAcsClient client = new DefaultAcsClient(profile);
            CommonRequest request = new CommonRequest();
            request.Method = MethodType.POST;
            request.Domain = _domain;
            request.Version = "2017-05-25";
            request.Action = "SendSms";
            // request.Protocol = ProtocolType.HTTP;
            request.AddQueryParameters("PhoneNumbers", phone);
            request.AddQueryParameters("SignName", _signName);
            request.AddQueryParameters("TemplateCode", "SMS_144175855");
            //可选:模板中的变量替换JSON串,如模板内容为"亲爱的${name},您的验证码为${code}"时,此处的值为
            Dictionary<string, string> templateparam = new Dictionary<string, string>
            {
                ["code"] = registerCode
            };

            request.AddQueryParameters("TemplateParam", templateparam.ToJson());
            request.AddQueryParameters("OutId", outId);
            CommonResponse response = client.GetCommonResponse(request);
            Console.WriteLine(System.Text.Encoding.Default.GetString(response.HttpResponse.Content));
            return response;

        }

        public CommonResponse SendLoginCode(string phone, string loginCode, out string outId)
        {

            outId = CreateOutId();
            IClientProfile profile = DefaultProfile.GetProfile("cn-hangzhou", _accessKeyId, _accessKeySecret);
            DefaultAcsClient client = new DefaultAcsClient(profile);
            CommonRequest request = new CommonRequest();
            request.Method = MethodType.POST;
            request.Domain = _domain;
            request.Version = "2017-05-25";
            request.Action = "SendSms";
            // request.Protocol = ProtocolType.HTTP;
            request.AddQueryParameters("PhoneNumbers", phone);
            request.AddQueryParameters("SignName", _signName);
            request.AddQueryParameters("TemplateCode", "SMS_144175857");
            //可选:模板中的变量替换JSON串,如模板内容为"亲爱的${name},您的验证码为${code}"时,此处的值为
            Dictionary<string, string> templateparam = new Dictionary<string, string>
            {
                ["code"] = loginCode
            };
            var a = templateparam.ToJson();
            request.AddQueryParameters("TemplateParam", templateparam.ToJson());
            request.AddQueryParameters("OutId", outId);
            CommonResponse response = client.GetCommonResponse(request);
            Console.WriteLine(System.Text.Encoding.Default.GetString(response.HttpResponse.Content));
            return response;
        }

        public CommonResponse SendAlarmSms(string phoneList, string deviceName, out string outId)
        {
            outId = CreateOutId();
            IClientProfile profile = DefaultProfile.GetProfile("cn-hangzhou", _accessKeyId, _accessKeySecret);
            DefaultAcsClient client = new DefaultAcsClient(profile);
            CommonRequest request = new CommonRequest();
            request.Method = MethodType.POST;
            request.Domain = _domain;
            request.Version = "2017-05-25";
            request.Action = "SendSms";
            // request.Protocol = ProtocolType.HTTP;
            request.AddQueryParameters("PhoneNumbers", phoneList);
            request.AddQueryParameters("SignName", _signName);
            request.AddQueryParameters("TemplateCode", "SMS_187271969");
            //可选:模板中的变量替换JSON串,如模板内容为"亲爱的${name},您的验证码为${code}"时,此处的值为
            Dictionary<string, string> templateparam = new Dictionary<string, string>
            {
                { "DevName", deviceName }
            };
            request.AddQueryParameters("TemplateParam", templateparam.ToJson());
            request.AddQueryParameters("OutId", outId);
            CommonResponse response = client.GetCommonResponse(request);
            Console.WriteLine(System.Text.Encoding.Default.GetString(response.HttpResponse.Content));
            return response;
        }
    }
}
