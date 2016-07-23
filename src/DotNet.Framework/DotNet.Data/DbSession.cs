// ===============================================================================
// DotNet.Platform 开发框架 2016 版权所有
// ===============================================================================

using System;
using System.Runtime.Remoting.Messaging;

namespace DotNet.Data
{
    /// <summary>
    /// 数据库会话
    /// </summary>
    public static class DbSession
    {
        private const string dbContext = "DotNet.Database.Context";

        /// <summary>
        /// 开启事务
        /// </summary>
        public static void Begin(Database database)
        {
            var dbObject = CallContext.GetData(dbContext);
            if (dbObject != null)
            {
                throw new InvalidOperationException("当前线程已经开启事务,请勿重复开启");
            }
            CallContext.SetData(dbContext, database);
            database.BeginTransaction();
        }

        /// <summary>
        /// 提交事务
        /// </summary>
        public static void Commit()
        {
            var db = GetInstance();
            if (db == null)
            {
                throw new InvalidOperationException("当前线程没有开启事务");
            }
            try
            {
                db.CommitTransaction();
            }
            finally
            {
                CallContext.SetData(dbContext, null);
                db.Dispose();
            }
        }

        /// <summary>
        /// 回滚事务
        /// </summary>
        public static void Rollback()
        {
            var db = GetInstance();
            if (db == null)
            {
                throw new InvalidOperationException("当前线程没有开启事务");
            }
            try
            {
                db.RollbackTransaction();
            }
            finally
            {
                CallContext.SetData(dbContext,null);
                db.Dispose();
            }
        }

        /// <summary>
        /// 获取会话数据库实例
        /// </summary>
        public static Database GetInstance()
        {
            var dbObject = CallContext.GetData(dbContext);
            return dbObject as Database;
        }
    }
}