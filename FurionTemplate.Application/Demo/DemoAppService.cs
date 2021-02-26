using FurionTemplate.Application.Demo.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace FurionTemplate.Application.Demo
{


    /// <summary>
    /// 测试
    /// </summary>
    [ApiDescriptionSettings(Tag = "测试组")]
    public class DemoAppService : BaseAppService
    {
        private readonly IDemoService _demoService;

        public DemoAppService(IDemoService demoService)
        {
            this._demoService = demoService;
        }

        /// <summary>
        /// 获取字符串
        /// </summary>
        /// <returns></returns>
        public string GetDemoGet()
        {
            return _demoService.GetString();
        }

        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public string GetQuery(int id)
        {

            return _demoService.Query(id);
        }

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public string PostAdd(AddModel model)
        {
            return _demoService.Add(model);
        }

        /// <summary>
        /// 抛异常
        /// </summary>
        /// <returns></returns>

        public string GetThrow()
        {
            return _demoService.Throw();
        }
        /// <summary>
        /// 获取token
        /// </summary>
        /// <returns></returns>
        public string GetToken()
        {

            return _demoService.GetToken();
        }

        /// <summary>
        /// token访问
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public string NeedToken()
        {

            return _demoService.GetString();
        }

        /// <summary>
        /// 测试Aop
        /// </summary>
        /// <returns></returns>
        public string Aop()
        {
            return _demoService.AopTest();
        }
    }

    public class AddModel
    {

        /// <summary>
        /// Id
        /// </summary>
        [Required(ErrorMessage = "id不能为空")]
        public int? Id { get; set; }


        /// <summary>
        /// 姓名
        /// </summary>
        public string Name { get; set; }

    }
}
