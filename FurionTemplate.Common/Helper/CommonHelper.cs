using Furion;
using FurionTemplate.Core.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace FurionTemplate.Common.Helper
{
    public static class CommonHelper
    {

        /// <summary>
        /// 根据反射获取类的属性列表
        /// </summary>
        /// <param name="tp"></param>
        /// <returns></returns>
        public static List<string> GetProperties(Type tp)
        {
            List<string> result = new List<string>();
            var list = tp.GetProperties();
            foreach (var item in list)
            {
                result.Add(item.Name.ToLower());
            }
            return result;
        }

        /// <summary>
        /// 更新实体模型,右边合并到左边
        /// </summary>
        /// <typeparam name="T">源实体类型</typeparam>
        /// <typeparam name="S">最终合并后返回的实体类型</typeparam>
        /// <param name="tModel">源数据实体</param>
        /// <param name="outModel">最终合并后返回的实体</param>
        /// <returns>最终实体</returns>
        public static S EntityMerge<T, S>(T tModel, S outModel)
        {
            Type type = tModel.GetType();
            Type outType = outModel.GetType();
            var properties = type.GetProperties();
            var outProperties = outType.GetProperties();
            foreach (var property in properties)
            {
                foreach (var item in outProperties)
                {
                    if (property.Name == item.Name)
                    {
                        var value = property.GetValue(tModel);
                        item.SetValue(outModel, value, null);
                        break;
                    }
                }
            }
            return outModel;
        }

        /// <summary>
        /// 比较数组
        /// </summary>
        /// <param name="first"></param>
        /// <param name="second"></param>
        /// <param name="less_list"></param>
        /// <param name="add_list"></param>
        public static void GetListDifferent(int[] first, int[] second, ref int[] less_list, ref int[] add_list)
        {
            less_list = first.Except(second).ToArray();
            add_list = second.Except(first).ToArray();
        }




        /// <summary>
        /// 解析toke返回用户信息
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public static TokenModel GetUserFromToken()
        {
            TokenModel tokenModel = new TokenModel
            {
                Uid = App.User?.FindFirstValue("Uid"),
                UName = App.User?.FindFirstValue("UName")

            };
            return tokenModel;


        }


        public static string GetSql(KeyValuePair<string, List<SqlSugar.SugarParameter>> queryString)
        {
            var sql = queryString.Key;//sql语句
            var par = queryString.Value;//参数

            //字符串替换MethodConst1x会替换掉MethodConst1所有要从后往前替换,不能用foreach,后续可以优化
            for (int i = par.Count - 1; i >= 0; i--)
            {
                if (par[i].ParameterName.StartsWith("@") && par[i].ParameterName.Contains("UnionAll"))
                {
                    sql = sql.Replace(par[i].ParameterName, par[i].Value.ToString());
                }
            }

            for (int i = par.Count - 1; i >= 0; i--)
            {
                if (par[i].ParameterName.StartsWith("@Method"))
                {
                    sql = sql.Replace(par[i].ParameterName, par[i].Value.ToString());
                }
            }
            for (int i = par.Count - 1; i >= 0; i--)
            {
                if (par[i].ParameterName.StartsWith("@Const"))
                {
                    sql = sql.Replace(par[i].ParameterName, par[i].Value.ToString());
                }
            }
            for (int i = par.Count - 1; i >= 0; i--)
            {
                if (par[i].ParameterName.StartsWith("@"))
                {
                    sql = sql.Replace(par[i].ParameterName, par[i].Value.ToString());
                }
            }
            return sql;
        }
    }
}
