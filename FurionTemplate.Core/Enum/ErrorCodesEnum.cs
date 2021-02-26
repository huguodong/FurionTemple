using Furion.FriendlyException;

namespace FurionTemplate.Core
{
    [ErrorCodeType]
    public enum ErrorCodes
    {
        [ErrorCodeItemMetadata("{0} 不能小于 {1}")]
        z1000,

        [ErrorCodeItemMetadata("数据不存在")]
        x1000,

        [ErrorCodeItemMetadata("服务器运行异常", ErrorCode = "Error")]
        SERVER_ERROR
    }

}
