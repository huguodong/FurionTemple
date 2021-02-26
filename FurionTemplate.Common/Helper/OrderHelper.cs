namespace FurionTemplate.Common.Helper
{
    public static class OrderHelper
    {
        /// <summary>
        /// 创建工单ID
        /// </summary>
        /// <param name="workType"></param>
        /// <returns></returns>
        public static string CreateOrderId(int workType)
        {
            //工单类型
            var wt = workType.ToString("00");
            //雪花ID
            var id = new NewLife.Data.Snowflake().NewId();
            return "GD" + wt + id;
        }
    }
}
