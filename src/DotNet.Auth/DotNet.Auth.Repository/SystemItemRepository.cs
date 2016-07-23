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
    /// 系统字典数据存储器
    /// </summary>
    public class SystemItemRepository
    {
		private AuthRepository<Dic> Repos { get; } = new AuthRepository<Dic>();

		/// <summary>
        /// 新建系统字典
        /// </summary>
        /// <param name="entity">系统字典实体</param>
        public BoolMessage Create(Dic entity)
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
        /// 更新系统字典
        /// </summary>
        /// <param name="entity">系统字典实体</param>
        public BoolMessage Update(Dic entity)
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
        /// 删除系统字典
        /// </summary>
        /// <param name="id">系统字典主键</param>
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
        /// 批量删除系统字典
        /// </summary>
        /// <param name="ids">系统字典主键数组</param>
        public BoolMessage DeleteByArray(string[] ids)
        {
            try
            {
                Repos.Delete(ids);
                return BoolMessage.True;
            }
            catch (Exception e)
            {
                return new BoolMessage(false, e.Message);
            }
        }


        /// <summary>
        /// 保存系统字典父节点
        /// </summary>
        /// <param name="id">系统字典主键</param>
        /// <param name="newParentId">新父节点主键</param>
        public BoolMessage SaveParent(string id, string newParentId)
        {
            try
            {
                Repos.Update(new Dic { ParentId = newParentId }, p => p.Id == id, p => p.ParentId);
                return BoolMessage.True;
            }
            catch (Exception e)
            {
                return new BoolMessage(false, e.Message);
            }
        }

        /// <summary>
        /// 保存系统字典排序路径
        /// </summary>
        /// <param name="sortPaths">更改的数据</param>
        public BoolMessage SaveSortPath(List<PrimaryKeyValue> sortPaths)
        {
            try
            {
                Repos.BatchUpdate(sortPaths);
                return BoolMessage.True;
            }
            catch (Exception e)
            {
                return new BoolMessage(false, e.Message);
            }
        }

        /// <summary>
        /// 获取系统字典
        /// </summary>
        public IEnumerable<Dic> Query()
        {
            return Repos.Query();
        }
    }
}