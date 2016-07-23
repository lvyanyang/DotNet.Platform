// ===============================================================================
// DotNet.Platform 开发框架 2016 版权所有
// ===============================================================================

using System;
using System.Collections.Generic;
using DotNet.Auth.Entity;
using DotNet.Utility;

namespace DotNet.Auth.Repository
{
    /// <summary>
    /// 系统日志数据存储器
    /// </summary>
    public class SystemLogRepository
    {
        private AuthRepository<Log> Repos { get; } = new AuthRepository<Log>();

        /// <summary>
        /// 新建系统日志
        /// </summary>
        /// <param name="entity">系统日志实体</param>
        public BoolMessage Create(Log entity)
        {
            try
            {
                Repos.Insert(entity);
                return BoolMessage.True;
            }
            catch (Exception e)
            {
                return new BoolMessage(false, e.Message);
            }
        }
        
        /// <summary>
        /// 获取系统日志
        /// </summary>
        public IEnumerable<Log> Query()
        {
            return Repos.Query(Repos.SQL.OrderByDesc(p => p.CreateDateTime));
        }
    }
}