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
    /// 系统序列数据存储器
    /// </summary>
    public class SystemSeqRepository
    {
        private AuthRepository<Seq> Repos { get; } = new AuthRepository<Seq>();

        /// <summary>
        /// 新建系统序列
        /// </summary>
        /// <param name="entity">系统序列实体</param>
        public BoolMessage Create(Seq entity)
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
        /// 更新系统序列
        /// </summary>
        /// <param name="entity">系统序列实体</param>
        public BoolMessage Update(Seq entity)
        {
            try
            {
                Repos.Update(entity);
                return BoolMessage.True;
            }
            catch (Exception e)
            {
                return new BoolMessage(false, e.Message);
            }
        }

        /// <summary>
        /// 删除系统序列
        /// </summary>
        /// <param name="id">系统序列主键</param>
        public BoolMessage Delete(string id)
        {
            try
            {
                Repos.Delete(id);
                return BoolMessage.True;
            }
            catch (Exception e)
            {
                return new BoolMessage(false, e.Message);
            }
        }

        /// <summary>
        /// 获取系统序列
        /// </summary>
        public IEnumerable<Seq> Query()
        {
            return Repos.Query();
            //return Repos.Query(Repos.SQL.OrderBy(p=>p.SortIndex));
        }
    }
}