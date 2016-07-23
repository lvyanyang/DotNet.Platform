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
    /// 系统菜单数据存储器
    /// </summary>
    public class SystemMenuRepository
    {
        private AuthRepository<Menu> Repos { get; } = new AuthRepository<Menu>();

        /// <summary>
        /// 新建系统菜单
        /// </summary>
        /// <param name="entity">系统菜单实体</param>
        public BoolMessage Create(Menu entity)
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
        /// 更新系统菜单
        /// </summary>
        /// <param name="entity">系统菜单实体</param>
        public BoolMessage Update(Menu entity)
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
        /// 删除系统菜单
        /// </summary>
        /// <param name="id">系统菜单主键</param>
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
        /// 获取系统菜单
        /// </summary>
        public IEnumerable<Menu> Query()
        {
            return Repos.Query();
            //return Repos.Query(Repos.SQL.OrderBy(p=>p.SortIndex));
        }
    }
}