// ===============================================================================
// DotNet.Platform 开发框架 2016 版权所有
// ===============================================================================
using System;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Reflection;
using DotNet.Data.Utilities;
using DotNet.Entity;

namespace DotNet.Data
{
    /// <summary>
    /// 数据库引擎基类
    /// </summary>
    public abstract class DbProvider : IDbProvider
    {
        private DbProviderFactory _providerFactory;

        /// <summary>
        /// 数据对象创建工厂
        /// </summary>
        public DbProviderFactory ProviderFactory
        {
            get { return _providerFactory ?? (_providerFactory = GetFactory()); }
        }

        /// <summary>
        /// 参数前缀
        /// </summary>
        public virtual string ParameterPrefix
        {
            get { return "@"; }
        }

        /// <summary>
        /// 获取Exists语句模板
        /// </summary>
        /// <returns>获取Exists语句模板</returns>
        public abstract string GetExistsStatement();

        /// <summary>
        /// 获取Insert语句模板
        /// </summary>
        /// <param name="tableInfo">表信息</param>
        /// <param name="cmd">Insert命令对象</param>
        public abstract void PreExecuteInsert(TableInfo tableInfo, DbCommand cmd);

        /// <summary>
        /// 获取分页语句模板
        /// </summary>
        /// <param name="sql">查询语句</param>
        /// <param name="orderBy"></param>
        /// <param name="startIndex"></param>
        /// <param name="endIndex"></param>
        /// <returns>获取分页语句模板</returns>
        public abstract string GetPageStatement(string sql, string orderBy, int startIndex, int endIndex);

        /// <summary>
		/// Builds an SQL query suitable for performing page based queries to the database
		/// </summary>
		/// <param name="skip">The number of rows that should be skipped by the query</param>
		/// <param name="take">The number of rows that should be retruend by the query</param>
		/// <param name="parts">The original SQL query after being parsed into it's component parts</param>
		/// <returns>The final SQL query that should be executed.</returns>
		public virtual string BuildPageQuery(long skip, long take, PagingHelper.SQLParts parts)
        {
            var sql = string.Format("{0}\nLIMIT {1} OFFSET {2}", parts.sql, take, skip);
            return sql;
        }

        /// <summary>
        /// 数据类型转换
        /// </summary>
        /// <param name="value">参数值</param>
        /// <returns>返回数据库对应的数据类型</returns>
        public virtual object ConvertParameterValue(object value)
        {
            //把布尔值转换为整形
            if (value is bool)
            {
                return ((bool)value) ? 1 : 0;
            }

            return value;
        }

        /// <summary>
        /// 执行命令之前调用,可以修改DbCommand对象
        /// </summary>
        /// <param name="cmd">命令对象</param>
        public virtual void PreExecute(IDbCommand cmd)
        {
        }

        /// <summary>
        /// 添加游标参数（针对Oracle数据库）
        /// </summary>
        /// <param name="param">参数对象</param>
        public virtual void SetCursorParam(DbParameter param)
        {
            
        }

        /// <summary>
        /// 获取数据库工厂对象
        /// </summary>
        /// <returns>返回工厂对象</returns>
        protected abstract DbProviderFactory GetFactory();

        /// <summary>
        /// 获取数据库工厂对象
        /// </summary>
        /// <param name="factoryClassName">工厂类名</param>
        /// <returns>返回工厂对象</returns>
        protected DbProviderFactory GetFactory(string factoryClassName)
        {
            if (string.IsNullOrEmpty(factoryClassName))
            {
                throw new ArgumentException("请输入正确的工厂类名");
            }
            Type providerType = Type.GetType(factoryClassName);
            if (null == providerType) throw new ArgumentException("请输入正确的工厂类名");

            FieldInfo providerInstance = providerType.GetField("Instance",
                BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.Static);
            if (providerInstance == null)
            {
                throw new ArgumentException("不存在名称为Instance的字段");
            }
            return (DbProviderFactory)providerInstance.GetValue(null);
        }
    }
}
