// ===============================================================================
// DotNet.Platform 开发框架 2016 版权所有
// ===============================================================================
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using DotNet.Collections;
using DotNet.Utility;

namespace DotNet.Data
{
    /// <summary>
    /// 数据存储器接口
    /// </summary>
    /// <typeparam name="T">实体类型</typeparam>
    public interface IRepository<T> where T : class, new()
    {
        /// <summary>
        /// 创建查询对象
        /// </summary>
        SQLQuery<T> SQL { get; }

        /// <summary>
        /// 是否存在指定条件的记录(参数对象)
        /// </summary>
        /// <param name="whereCondition">查询条件字符串(不带where,例如: name=@name and id=@id)</param>
        /// <param name="args">条件参数对象</param>
        /// <returns>找到符合条件的记录返回true,否则返回false</returns>
        bool Exists(string whereCondition, object args = null);

        /// <summary>
        /// 是否存在指定主键值的记录(主键值)
        /// </summary>
        /// <param name="primaryKey">主键值</param>
        /// <returns>存在返回true,否则返回false</returns>
        bool Exists(object primaryKey);

        /// <summary>
        /// 是否存在指定条件的记录(查询表达式)
        /// </summary>
        /// <param name="expression">查询表达式</param>
        /// <returns>存在返回true,否则返回false</returns>
        bool Exists(Expression<Func<T, bool>> expression);

        /// <summary>
        /// 插入数据(需要写入的列和值对象,支持字典类型和对象类型,不检查主键)
        /// </summary>
        /// <param name="args">参数对象(需要写入的列和值对象,支持字典类型和对象类型,不检查主键字段)</param>
        /// <returns>返回受影响的行数</returns>
        int Insert(object args);

        /// <summary>
        /// 插入数据(实体对象,检查主键)
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <returns>返回新增后记录的主键值</returns>
        object Insert(T entity);

        /// <summary>
        /// 更新数据(需要写入的列和值对象,支持字典类型和对象类型)
        /// </summary>
        /// <param name="valueArgs">更新数据参数(需要更新的列和值对象,支持字典类型和对象类型,不检查主键字段)</param>
        /// <param name="whereCondition">查询条件字符串(不带where,例如: name=@name and id=@id)</param>
        /// <param name="args">条件参数对象</param>
        /// <returns>返回受影响的行数</returns>
        int Update(object valueArgs, string whereCondition, object args = null);

        /// <summary>
        /// 更新数据
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <param name="columns">指定需要更新的列名数组,如果为空则更新所有的列</param>
        /// <param name="whereCondition">查询条件字符串(不带where,例如: name=@name and id=@id)</param>
        /// <param name="args">条件参数对象</param>
        /// <returns>返回受影响的行数</returns>
        int Update(T entity, string[] columns, string whereCondition, object args = null);

        /// <summary>
        /// 更新实体数据
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <param name="columns">指定需要更新的列名数组,如果为空则更新所有的列</param>
        /// <returns>返回受影响的行数</returns>
        int Update(T entity, string[] columns);

        /// <summary>
        /// 更新实体数据
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <param name="columns">指定需要更新的列名数组,如果为空则更新所有的列</param>
        /// <returns>返回受影响的行数</returns>
        int Update(T entity, params Expression<Func<T, object>>[] columns);

        /// <summary>
        /// 更新实体数据(查询表达式)
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <param name="expression">查询表达式</param>
        /// <param name="columns">指定需要更新的列名数组,如果为空则更新所有的列</param>
        /// <returns>返回受影响的行数</returns>
        int Update(T entity, Expression<Func<T, bool>> expression, string[] columns);

        /// <summary>
        /// 更新实体数据(查询表达式)
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <param name="expression">查询表达式</param>
        /// <param name="columns">指定需要更新的列名数组,如果为空则更新所有的列</param>
        /// <returns>返回受影响的行数</returns>
        int Update(T entity, Expression<Func<T, bool>> expression, params Expression<Func<T, object>>[] columns);

        /// <summary>
        /// 批量更新(建议外部开启事务)
        /// </summary>
        /// <param name="changedData">改变的数据</param>
        int BatchUpdate(IEnumerable<PrimaryKeyValue> changedData);

        /// <summary>
        /// 删除数据(参数对象)
        /// </summary>
        /// <param name="whereCondition">查询条件字符串(不带where,例如: name=@name and id=@id)</param>
        /// <param name="args">参数对象</param>
        /// <returns>返回受影响的行数</returns>
        int Delete(string whereCondition, object args);

        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="expression">查询表达式</param>
        /// <returns>返回受影响的行数</returns>
        int Delete(Expression<Func<T, bool>> expression);

        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="ids">主键数组</param>
        /// <returns>返回受影响的行数</returns>
        int Delete(Array ids);

        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="id">整形主键</param>
        /// <returns>返回受影响的行数</returns>
        int Delete(int id);

        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="id">字符串主键</param>
        /// <returns>返回受影响的行数</returns>
        int Delete(string id);

        /// <summary>
        /// 删除所有数据
        /// </summary>
        /// <returns>返回受影响的行数</returns>
        int DeleteAll();

        /// <summary>
        /// 获取实体对象(主键值)
        /// </summary>
        /// <param name="id">主键值</param>
        /// <param name="columns">查询列数组,如果为null查询所有列</param>
        /// <returns>返回实体对象</returns>
        T Get(object id, string[] columns);

        /// <summary>
        /// 获取实体对象(主键值)
        /// </summary>
        /// <param name="id">主键值</param>
        /// <param name="columns">查询列数组,如果为null查询所有列</param>
        /// <returns>返回实体对象</returns>
        T Get(object id, params Expression<Func<T, object>>[] columns);

        /// <summary>
        /// 获取实体对象(查询表达式)
        /// </summary>
        /// <param name="linq">查询表达式</param>
        /// <returns>返回实体对象</returns>
        T Get(SQLQuery<T> linq);

        /// <summary>
        /// 查询数据(查询表达式)
        /// </summary>
        /// <param name="linq">查询表达式</param>
        /// <returns>返回记录集合</returns>
        IEnumerable<T> Query(SQLQuery<T> linq);

        /// <summary>
        /// 查询数据(默认排序规则)
        /// </summary>
        /// <param name="expression">查询表达式</param>
        /// <param name="columns">查询列</param>
        /// <returns>返回记录集合</returns>
        IEnumerable<T> Query(Expression<Func<T, bool>> expression, params Expression<Func<T, object>>[] columns);

        /// <summary>
        /// 查询数据
        /// </summary>
        /// <returns>返回记录集合</returns>
        IEnumerable<T> Query();

        /// <summary>
        /// 查询数据(查询表达式)
        /// </summary>
        /// <param name="linq">查询表达式</param>
        /// <returns>返回记录集合</returns>
        List<T> Fetch(SQLQuery<T> linq);

        /// <summary>
        /// 查询数据(默认排序规则)
        /// </summary>
        /// <param name="expression">查询表达式</param>
        /// <param name="columns">查询列</param>
        /// <returns>返回记录集合</returns>
        List<T> Fetch(Expression<Func<T, bool>> expression, params Expression<Func<T, object>>[] columns);

        /// <summary>
        /// 查询数据
        /// </summary>
        /// <returns>返回记录集合</returns>
        List<T> Fetch();

        /// <summary>
        /// 查询分页数据
        /// </summary>
        /// <param name="linq">查询表达式</param>
        /// <returns>返回分页列表</returns>
        PageList<T> Page(SQLQuery<T> linq);
    }
}