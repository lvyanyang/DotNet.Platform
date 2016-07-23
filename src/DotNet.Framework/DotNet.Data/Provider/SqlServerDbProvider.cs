// ===============================================================================
// DotNet.Platform 开发框架 2016 版权所有
// ===============================================================================
using System;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using DotNet.Data.Utilities;
using DotNet.Entity;

namespace DotNet.Data.Provider
{
    /// <summary>
    /// SqlServer数据库引擎
    /// </summary>
    public class SqlServerDbProvider : DbProvider, IDbProvider
    {
        /// <summary>
        /// SqlServer默认实例对象
        /// </summary>
        public static readonly SqlServerDbProvider Instance = new SqlServerDbProvider();

        /// <summary>
        /// 获取Exists语句模板
        /// </summary>
        /// <returns>获取Exists语句模板</returns>
        public override string GetExistsStatement()
        {
            return "IF EXISTS (SELECT 1 FROM {0} WHERE {1}) SELECT 1 ELSE SELECT 0";
        }

        /// <summary>
        /// 获取Insert语句模板
        /// </summary>
        /// <param name="tableInfo">表信息</param>
        /// <param name="cmd">Insert命令对象</param>
        public override void PreExecuteInsert(TableInfo tableInfo, DbCommand cmd)
        {
            if (!tableInfo.AutoIncrement) return;

            string sql = cmd.CommandText;
            int index = sql.IndexOf("VALUES", StringComparison.OrdinalIgnoreCase);
            cmd.CommandText = sql.Insert(index, String.Format(" OUTPUT INSERTED.{0} ",tableInfo.PrimaryKey));
        }

        /// <summary>
        /// 获取分页语句模板
        /// </summary>
        /// <param name="sql">查询语句</param>
        /// <param name="orderBy"></param>
        /// <param name="startIndex"></param>
        /// <param name="endIndex"></param>
        /// <returns>获取分页语句模板</returns>
        public override string GetPageStatement(string sql, string orderBy, int startIndex, int endIndex)
        {
            return string.Format("with grid as (select *,row_number() over ({0}) rownum from ({1}) x ) " +
                                 "select * from grid where rownum between {2} and {3}",
                                 orderBy, sql, startIndex, endIndex);
        }

        /// <summary>
        /// Builds an SQL query suitable for performing page based queries to the database
        /// </summary>
        /// <param name="skip">The number of rows that should be skipped by the query</param>
        /// <param name="take">The number of rows that should be retruend by the query</param>
        /// <param name="parts">The original SQL query after being parsed into it's component parts</param>
        /// <returns>The final SQL query that should be executed.</returns>
        public override string BuildPageQuery(long skip, long take, PagingHelper.SQLParts parts)
        {
            parts.sqlSelectRemoved = PagingHelper.rxOrderBy.Replace(parts.sqlSelectRemoved, "", 1);
            if (PagingHelper.rxDistinct.IsMatch(parts.sqlSelectRemoved))
            {
                parts.sqlSelectRemoved = "peta_inner.* FROM (SELECT " + parts.sqlSelectRemoved + ") peta_inner";
            }
            var sqlPage = string.Format("SELECT * FROM (SELECT ROW_NUMBER() OVER ({0}) peta_rn, {1}) peta_paged WHERE peta_rn>{2} AND peta_rn<={3}",
                                    parts.sqlOrderBy == null ? "ORDER BY (SELECT NULL)" : parts.sqlOrderBy, parts.sqlSelectRemoved, skip, skip + take);
            
            return sqlPage;
        }


        /// <summary>
        /// 获取数据库工厂对象
        /// </summary>
        /// <returns>返回工厂对象</returns>
        protected override DbProviderFactory GetFactory()
        {
            return SqlClientFactory.Instance;
        }
    }
}