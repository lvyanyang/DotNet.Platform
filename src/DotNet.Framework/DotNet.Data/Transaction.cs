// ===============================================================================
// DotNet.Platform 开发框架 2016 版权所有
// ===============================================================================
using System;

namespace DotNet.Data
{
    /// <summary>
    /// 包装的事务对象
    /// </summary>
    public class Transaction : IDisposable
    {
        private Database _db;

        /// <summary>
        /// 初始化包装的事务对象
        /// </summary>
        /// <param name="db">数据库对象</param>
        public Transaction(Database db)
        {
            _db = db;
            _db.BeginTransaction();
        }

        /// <summary>
        /// 执行与释放或重置非托管资源相关的应用程序定义的任务。(回滚事务)
        /// </summary>
        public void Dispose()
        {
            if (_db != null)
                _db.RollbackTransaction();
        }

        /// <summary>
        /// 提交事务
        /// </summary>
        public void Commit()
        {
            _db.CommitTransaction();
            _db = null;
        }

        /// <summary>
        /// 回滚事务
        /// </summary>
        public void Rollback()
        {
            if (_db != null)
                _db.RollbackTransaction();
        }
    }
}
