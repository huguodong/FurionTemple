using FurionTemplate.Application.AOP;
using FurionTemplate.Core.Enum;

namespace FurionTemplate.Application.Demo.Services
{
    public interface IDemoService
    {
        string Add(AddModel model);
        [ULog(OperationType = OperationTypeEnum.Add, LogName = "添加用户")]
        string AopTest();
        string GetString();
        string GetToken();
        [Caching(AbsoluteExpiration = 1)]
        string Query(int id);
        string Throw();
    }
}
