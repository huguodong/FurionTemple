using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace FurionTemplate.Core.Repository
{
    public class BaseRepository<TEntity> : IBaseRepository<TEntity> where TEntity : class, new()
    {
        private readonly ISqlSugarRepository<TEntity> Repository; // 仓储对象：封装简单的CRUD
        private readonly ISqlSugarClient Db; // 核心对象：拥有完整的SqlSugar全部功能
        public BaseRepository(ISqlSugarRepository<TEntity> sqlSugarRepository)
        {
            Repository = sqlSugarRepository;
            Db = Repository.Context;    // 推荐操作
        }

        /// <summary>
        /// 写入实体数据
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<bool> Add(TEntity model)
        {
            var result = await Db.Insertable(model).IgnoreColumns(true).ExecuteCommandAsync();
            return result > 0;


        }

        public virtual async Task<bool> AddOrUpdate(TEntity model)
        {
            var result = await Db.Saveable(model).ExecuteCommandAsync();
            return result > 0;

        }



        public async Task<int> AddList(List<TEntity> model)
        {
            return await Db.Insertable(model.ToArray()).ExecuteCommandAsync();
        }

        public async Task<int> AddList(List<TEntity> model, Expression<Func<TEntity, object>> insertColumns)
        {
            return await Db.Insertable(model.ToArray()).InsertColumns(insertColumns).ExecuteCommandAsync();
        }

        /// <summary>
        /// 根据主键删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<bool> DeleteById(object id)
        {
            var result = await Db.Deleteable<TEntity>().In(id).ExecuteCommandAsync();
            return result > 0;
        }


        /// <summary>
        /// 根据主键数组删除
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<int> DeleteByIds(object[] ids)
        {

            return await Db.Deleteable<TEntity>().In(ids).With(SqlWith.RowLock).ExecuteCommandAsync();
        }

        /// <summary>
        /// 根据ID查询一条数据
        /// </summary>
        /// <param name="objId"></param>
        /// <returns></returns>
        public virtual async Task<TEntity> GetById(object objId)
        {
            return await Db.Queryable<TEntity>().InSingleAsync(objId);
        }

        /// <summary>
        /// 获取最后一条记录
        /// </summary>
        /// <param name="field"></param>
        /// <returns></returns>
        public async Task<TEntity> GetLast(Expression<Func<TEntity, object>> field)
        {
            return await Db.Queryable<TEntity>().OrderBy(field, SqlSugar.OrderByType.Desc).FirstAsync();
        }

        /// <summary>
        /// 获取所有
        /// </summary>
        /// <returns></returns>
        public virtual async Task<List<TEntity>> GetList()
        {
            return await Db.Queryable<TEntity>().ToListAsync();
        }
        /// <summary>
        /// 分页
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="whereExpression"></param>
        /// <param name="selectExpression"></param>
        /// <param name="orderbyExpression"></param>
        /// <param name="orderByType"></param>
        /// <returns></returns>
        public async Task<List<TEntity>> PageList(int pageIndex, int pageSize, Expression<Func<TEntity, bool>> whereExpression = null, Expression<Func<TEntity, TEntity>> selectExpression = null, Expression<Func<TEntity, object>> orderbyExpression = null, OrderByType orderByType = OrderByType.Asc)
        {
            return await Repository.AsQueryable()
                                  .WhereIF(whereExpression != null, whereExpression)
                                  .OrderByIF(orderbyExpression != null, orderbyExpression, orderByType)
                                  .Select(selectExpression)
                                  .ToPageListAsync(pageIndex, pageSize);
        }

        public async Task<List<int>> PageIntList(int pageIndex, int pageSize, Expression<Func<TEntity, bool>> whereExpression = null, Expression<Func<TEntity, int>> selectExpression = null, Expression<Func<TEntity, object>> orderbyExpression = null, OrderByType orderByType = OrderByType.Asc)
        {
            return await Repository.AsQueryable().WhereIF(whereExpression != null, whereExpression)
                         .OrderByIF(orderbyExpression != null, orderbyExpression, orderByType)
                         .Select(selectExpression)
                         .ToPageListAsync(pageIndex, pageSize);
        }

        /// <summary>
        /// 条件查询返回第一个
        /// </summary>
        /// <param name="whereExpression"></param>
        /// <returns></returns>
        public virtual async Task<TEntity> Query(Expression<Func<TEntity, bool>> whereExpression)
        {
            return await Db.Queryable<TEntity>().WhereIF(whereExpression != null, whereExpression).FirstAsync();
        }

        public async Task<int> QueryCount(Expression<Func<TEntity, bool>> whereExpression)
        {
            return await Db.Queryable<TEntity>().WhereIF(whereExpression != null, whereExpression).CountAsync();
        }

        /// <summary>
        /// 条件查询返回列表
        /// </summary>
        /// <param name="whereExpression"></param>
        /// <param name="orderbyExpression"></param>
        /// <param name="orderByType"></param>
        /// <returns></returns>
        public virtual async Task<List<TEntity>> QueryList(Expression<Func<TEntity, bool>> whereExpression = null, Expression<Func<TEntity, object>> orderbyExpression = null, OrderByType orderByType = OrderByType.Asc)
        {
            return await Db.Queryable<TEntity>()
                   .WhereIF(whereExpression != null, whereExpression).OrderByIF(orderbyExpression != null, orderbyExpression, orderByType)
                   .ToListAsync();
        }

        public async Task<List<T>> QueryListSelectColumns<T>(Expression<Func<TEntity, bool>> whereExpression = null, Expression<Func<TEntity, T>> selectColumns = null)
        {

            return await Db.Queryable<TEntity>()
                       .WhereIF(whereExpression != null, whereExpression)
                       .Select(selectColumns)
                       .ToListAsync();

        }


        public async Task<T> QuerySelectColumns<T>(Expression<Func<TEntity, bool>> whereExpression = null, Expression<Func<TEntity, T>> selectColumns = null)
        {
            return await Db.Queryable<TEntity>()
                       .WhereIF(whereExpression != null, whereExpression)
                       .Select(selectColumns)
                       .FirstAsync();
        }

        /// <summary>
        /// 更新实体数据
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<bool> Update(TEntity model)
        {

            //这种方式会以主键为条件
            var i = await Db.Updateable(model).IgnoreColumns(ignoreAllNullColumns: true).ExecuteCommandAsync();
            return i > 0;
        }

        public async Task<bool> UpdateColumns(TEntity model, Expression<Func<TEntity, object>> updateColumns)
        {
            var i = await Db.Updateable(model).UpdateColumns(updateColumns).ExecuteCommandAsync();
            return i > 0;
        }
        public async Task<int> UpdateList(List<TEntity> model, Expression<Func<TEntity, object>> updateColumns)
        {
            return await Db.Updateable(model.ToArray()).UpdateColumns(updateColumns).ExecuteCommandAsync();
        }

        public async Task<int> UpdateList(List<TEntity> model, Expression<Func<TEntity, object>> whereExpression, Expression<Func<TEntity, object>> updateColumns)
        {
            return await Db.Updateable(model.ToArray())
                           .WhereColumns(whereExpression)
                           .UpdateColumns(updateColumns)
                           .ExecuteCommandAsync();
        }



    }
}
