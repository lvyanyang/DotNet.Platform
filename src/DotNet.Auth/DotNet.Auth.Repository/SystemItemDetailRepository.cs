// ===============================================================================
// DotNet.Platform 开发框架 2016 版权所有
// ===============================================================================

using System;
using System.Collections.Generic;
using DotNet.Auth.Entity;
using DotNet.Data.Extensions;
using DotNet.Utility;

namespace DotNet.Auth.Repository
{
    /// <summary>
    /// 系统选项明细数据存储器
    /// </summary>
    public class SystemItemDetailRepository
    {
        private AuthRepository<DicDetail> Repos { get; } = new AuthRepository<DicDetail>();

        /// <summary>
        /// 新建系统选项明细
        /// </summary>
        /// <param name="entity">系统选项明细实体</param>
        public BoolMessage Create(DicDetail entity)
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
        /// 更新系统选项明细
        /// </summary>
        /// <param name="entity">系统选项明细实体</param>
        public BoolMessage Update(DicDetail entity)
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
        /// 删除系统选项明细
        /// </summary>
        /// <param name="id">系统选项明细主键</param>
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
        /// 批量删除系统选项明细
        /// </summary>
        /// <param name="itemIds">数据字典主键数组</param>
        public BoolMessage DeleteByItemArray(string[] itemIds)
        {
            try
            {
                Repos.Delete(p => p.DicId.In(itemIds));
                return BoolMessage.True;
            }
            catch (Exception e)
            {
                return new BoolMessage(false, e.Message);
            }
        }

        /// <summary>
        /// 获取系统选项明细
        /// </summary>
        public IEnumerable<DicDetail> Query()
        {
            return Repos.Query();
        }
    }
}