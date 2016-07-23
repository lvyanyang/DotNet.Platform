// ===============================================================================
// DotNet.Platform 开发框架 2016 版权所有
// ===============================================================================
using System;
using System.Data;
using System.Data.Common;
using DotNet.Entity;
using DotNet.Helper;

namespace DotNet.Data.Provider
{
    /// <summary>
    /// Oracle数据库引擎
    /// 连接串 user id=sa;password=1;data source=//192.168.1.1:1521/oraxajp.local
    /// </summary>
    public class OracleDbProvider : DbProvider, IDbProvider
    {
        /// <summary>
        /// Oracle默认实例对象
        /// </summary>
        public static readonly OracleDbProvider Instance = new OracleDbProvider();

        /// <summary>
        /// 参数前缀
        /// </summary>
        public override string ParameterPrefix
        {
            get { return ":"; }
        }

        /// <summary>
        /// 执行命令之前调用,可以修改DbCommand对象
        /// </summary>
        /// <param name="cmd">命令对象</param>
        public override void PreExecute(IDbCommand cmd)
        {
            cmd.GetType().GetProperty("BindByName").SetValue(cmd, true, null);
        }

        /// <summary>
        /// 获取Exists语句模板
        /// </summary>
        /// <returns>获取Exists语句模板</returns>
        public override string GetExistsStatement()
        {
            return "SELECT 1 FROM {0} WHERE {1}";
        }

        /// <summary>
        /// 获取Insert语句模板
        /// </summary>
        /// <param name="tableInfo">表信息</param>
        /// <param name="cmd">Insert命令对象</param>
        public override void PreExecuteInsert(TableInfo tableInfo, DbCommand cmd)
        {
            if (!tableInfo.AutoIncrement) return;

            cmd.CommandText += string.Format(" returning {0} into :newid", tableInfo.PrimaryKey);
            var param = cmd.CreateParameter();
            param.ParameterName = ":newid";
            param.Value = DBNull.Value;
            param.Direction = ParameterDirection.ReturnValue;
            param.DbType = DbType.Int64;
            cmd.Parameters.Add(param);
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
            string result = @"SELECT * FROM(SELECT grid.*, rownum FROM ({0} {1}) grid WHERE ROWNUM <= {2}) WHERE rownum >= {3}";
            return string.Format(result, sql, orderBy, endIndex, startIndex);
        }

        /// <summary>
        /// 添加游标参数（针对Oracle数据库）
        /// </summary>
        /// <param name="param">参数对象</param>
        public override void SetCursorParam(DbParameter param)
        {
            param.Direction = System.Data.ParameterDirection.Output;
            var dbTypeProperty = param.GetType().GetProperty("OracleDbType");
            var typeValue = EnumHelper.ToEnum("Oracle.ManagedDataAccess.Client.OracleDbType,Oracle.ManagedDataAccess", "RefCursor");
            dbTypeProperty.SetValue(param, typeValue, null);
        }

        /// <summary>
        /// 获取数据库工厂对象
        /// </summary>
        /// <returns>返回工厂对象</returns>
        protected override DbProviderFactory GetFactory()
        {
            return GetFactory("Oracle.ManagedDataAccess.Client.OracleClientFactory,Oracle.ManagedDataAccess");
        }
    }
}