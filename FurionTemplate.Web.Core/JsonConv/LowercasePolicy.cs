using System.Text.Json;

namespace FurionTemplate.Web.Core
{
    /// <summary>
    /// 返回对象全小写
    /// </summary>
    public class LowercasePolicy : JsonNamingPolicy
    {
        public override string ConvertName(string name) =>
            name.ToLower();
    }
}
