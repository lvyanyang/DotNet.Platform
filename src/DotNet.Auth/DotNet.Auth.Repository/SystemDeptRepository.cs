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
    /// 系统部门数据存储器
    /// </summary>
    public class SystemDeptRepository
    {
        private AuthRepository<Department> Repos { get; } = new AuthRepository<Department>();

        /// <summary>
        /// 新建系统部门
        /// </summary>
        /// <param name="entity">系统部门实体</param>
        public BoolMessage Create(Department entity)
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
        /// 更新系统部门
        /// </summary>
        /// <param name="entity">系统部门实体</param>
        public BoolMessage Update(Department entity)
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
        /// 批量删除系统部门
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
                Repos.Update(new Department { ParentId = newParentId }, 
                    p => p.Id == id, p => p.ParentId);
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
        /// 删除系统部门
        /// </summary>
        /// <param name="id">系统部门主键</param>
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
        /// 获取系统部门
        /// </summary>
        public IEnumerable<Department> Query()
        {
            return Repos.Query();
        }
    }
}