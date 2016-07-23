// ===============================================================================
// DotNet.Platform 开发框架 2016 版权所有
// ===============================================================================

using System;
using System.Collections.Generic;
using DotNet.Utility;
using SystemException = DotNet.Auth.Entity.SystemException;

namespace DotNet.Auth.Repository
{
    /// <summary>
    /// 系统异常数据存储器
    /// </summary>
    public class SystemExceptionRepository
    {
        private AuthRepository<SystemException> Repos { get; } = new AuthRepository<SystemException>();

        /// <summary>
        /// 新建系统异常
        /// </summary>
        /// <param name="entity">系统异常实体</param>
        public BoolMessage Create(SystemException entity)
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
        /// 获取系统异常
        /// </summary>
        public IEnumerable<SystemException> Query()
        {
            return Repos.Query(Repos.SQL.OrderByDesc(p => p.CreateDateTime));
        }
    }
}