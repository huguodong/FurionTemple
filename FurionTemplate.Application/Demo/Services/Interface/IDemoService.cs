using FurionTemplate.Core.AOP;
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
        string Query(int id);
        string Throw();
    }
}
