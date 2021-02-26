using SqlSugar;

namespace FurionTemplate.Core.Enity
{
    public class TestTable
    {
        /// <summary>
        /// 用户表
        /// </summary>
        public TestTable()
        {
        }

        /// <summary>
        /// 用户ID
        /// </summary>
        [SugarColumn(IsPrimaryKey = true)]
        public string UserId { get; set; }
    }

}
