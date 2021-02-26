using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace FurionTemplate.Core.Repository
{
    /// <summary>
    /// 基类接口,其他接口继承该接口
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public interface IBaseRepository<TEntity> where TEntity : class
    {

        /// <summary>
        /// 条件查询列表
        /// </summary>
        /// <param name="whereExpression"></param>
        /// <param name="orderbyExpression"></param>
        /// <param name="orderByType"></param>
        /// <returns></returns>
        Task<List<TEntity>> QueryList(Expression<Func<TEntity, bool>> whereExpression, Expression<Func<TEntity, object>> orderbyExpression = null, OrderByType orderByType = OrderByType.Asc);

        /// <summary>
        /// 条件查询列表可以选择指定字段
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="whereExpression"></param>
        /// <param name="selectColumns"></param>
        /// <returns></returns>
        Task<List<T>> QueryListSelectColumns<T>(Expression<Func<TEntity, bool>> whereExpression = null, Expression<Func<TEntity, T>> selectColumns = null);

        /// <summary>
        /// 条件查询可以选择指定字段
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="whereExpression"></param>
        /// <param name="selectColumns"></param>
        /// <returns></returns>
        Task<T> QuerySelectColumns<T>(Expression<Func<TEntity, bool>> whereExpression = null, Expression<Func<TEntity, T>> selectColumns = null);
        /// <summary>
        /// 查询数量
        /// </summary>
        /// <param name="whereExpression"></param>
        /// <returns></returns>
        Task<int> QueryCount(Expression<Func<TEntity, bool>> whereExpression);

        /// <summary>
        /// 条件查询一条返回实体
        /// </summary>
        /// <param name="whereExpression"></param>
        /// <returns></returns>
        Task<TEntity> Query(Expression<Func<TEntity, bool>> whereExpression);

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="whereExpression"></param>
        /// <param name="selectExpression"></param>
        /// <param name="orderbyExpression"></param>
        /// <param name="orderByType"></param>
        /// <returns></returns>
        Task<List<TEntity>> PageList(int pageIndex, int pageSize, Expression<Func<TEntity, bool>> whereExpression = null, Expression<Func<TEntity, TEntity>> selectExpression = null, Expression<Func<TEntity, object>> orderbyExpression = null, OrderByType orderByType = OrderByType.Asc);

        /// <summary>
        /// 分页查询返回int类型字段
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="whereExpression"></param>
        /// <param name="selectExpression"></param>
        /// <param name="orderbyExpression"></param>
        /// <param name="orderByType"></param>
        /// <returns></returns>
        Task<List<int>> PageIntList(int pageIndex, int pageSize, Expression<Func<TEntity, bool>> whereExpression = null, Expression<Func<TEntity, int>> selectExpression = null, Expression<Func<TEntity, object>> orderbyExpression = null, OrderByType orderByType = OrderByType.Asc);
        /// <summary>
        /// 根据ID查询
        /// </summary>
        /// <param name="objId"></param>
        /// <returns></returns>
        Task<TEntity> GetById(object objId);

        /// <summary>
        /// 查询所有
        /// </summary>
        /// <returns></returns>
        Task<List<TEntity>> GetList();

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<bool> Add(TEntity model);

        /// <summary>
        /// 批量插入
        /// </summary>
        /// <param name="model"></param>
        /// <param name="insertColumns"></param>
        /// <returns></returns>
        Task<int> AddList(List<TEntity> model, Expression<Func<TEntity, object>> insertColumns);

        /// <summary>
        /// 批量插入
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<int> AddList(List<TEntity> model);
        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<bool> Update(TEntity model);

        /// <summary>
        /// 更新部分字段
        /// </summary>
        /// <param name="model"></param>
        /// <param name="updateColumns"></param>
        /// <returns></returns>
        Task<bool> UpdateColumns(TEntity model, Expression<Func<TEntity, object>> updateColumns);

        /// <summary>
        /// 批量修改
        /// </summary>
        /// <param name="model"></param>
        /// <param name="updateColumns"></param>
        /// <returns></returns>
        Task<int> UpdateList(List<TEntity> model, Expression<Func<TEntity, object>> updateColumns);
        /// <summary>
        /// 批量修改不根据主键
        /// </summary>
        /// <param name="model"></param>
        /// <param name="whereExpression"></param>
        /// <param name="updateColumns"></param>
        /// <returns></returns>
        Task<int> UpdateList(List<TEntity> model, Expression<Func<TEntity, object>> whereExpression, Expression<Func<TEntity, object>> updateColumns);
        /// <summary>
        /// 删除一条信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<bool> DeleteById(object id);

        /// <summary>
        /// 根据主键数组删除
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        Task<int> DeleteByIds(object[] ids);
        /// <summary>
        /// 获取最后一条记录
        /// </summary>
        /// <param name="field"></param>
        /// <returns></returns>
        Task<TEntity> GetLast(Expression<Func<TEntity, object>> field);


        /// <summary>
        /// 添加或更新
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<bool> AddOrUpdate(TEntity model);
    }
}
