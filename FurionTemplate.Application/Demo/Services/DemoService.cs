using Furion;
using Furion.DataEncryption;
using Furion.DependencyInjection;
using Furion.FriendlyException;
using FurionTemplate.Application.AOP;
using FurionTemplate.Core;
using FurionTemplate.Core.Enum;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using NewLife.Serialization;
using System.Collections.Generic;
using System.Security.Claims;

namespace FurionTemplate.Application.Demo.Services
{
    /// <summary>
    /// 测试服务
    /// </summary>
    [Injection(Proxy = typeof(RedisAop))]

    public class DemoService : IDemoService, ITransient
    {
        private readonly ILogger<DemoService> _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public DemoService(ILogger<DemoService> logger, IHttpContextAccessor httpContextAccessor)
        {
            this._logger = logger;
            this._httpContextAccessor = httpContextAccessor;
        }

        public string GetString()
        {

            return "Hello World";
        }


        public string Query(int id)
        {
            _logger.LogInformation($"查询了ID为{id}的数据");
            return $"查询了ID为{id}的数据";
        }

        public string Add(AddModel model)
        {
            return model.ToJson();
        }

        public string Throw()
        {
            _logger.LogError("报错了", Oops.Oh(ErrorCodes.x1000));
            throw Oops.Oh(ErrorCodes.x1000).StatusCode(404);
        }

        public string GetToken()
        {

            var accessToken = JWTEncryption.Encrypt(new Dictionary<string, object>()
            {
                { "Uid","123456" },  // 存储Id
                { "UName","admin" }, // 存储用户名
            });
            var user = App.User;
            var a = App.User?.FindFirstValue("UName");
            var c = user.Claims;

            //var Account = userInfo["Account"];

            // 获取刷新 token
            var refreshToken = JWTEncryption.GenerateRefreshToken(accessToken, 30); // 第二个参数是刷新 token 的有效期，默认三十天

            // 设置请求报文头
            _httpContextAccessor.HttpContext.Response.Headers["access-token"] = accessToken;

            _httpContextAccessor.HttpContext.Response.Headers["x-access-token"] = refreshToken;
            return "成功";
        }

        [ULog(OperationType = OperationTypeEnum.Add, LogName = "添加用户")]
        public string AopTest()
        {

            return "请求成功";
        }

    }
}
