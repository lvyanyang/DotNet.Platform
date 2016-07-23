// ===============================================================================
// DotNet.Platform 开发框架 2016 版权所有
// ===============================================================================

using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using DotNet.Auth.Entity;
using DotNet.Utility;

namespace DotNet.Auth.Repository
{
    /// <summary>
    /// 系统用户数据存储器
    /// </summary>
    public class SystemUserRepository
    {
        private AuthRepository<User> Repos { get; } = new AuthRepository<User>();

        /// <summary>
        /// 新建系统用户
        /// </summary>
        /// <param name="entity">系统用户实体</param>
        public BoolMessage Create(User entity)
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
        /// 更新系统用户
        /// </summary>
        /// <param name="entity">系统用户实体</param>
        /// <param name="condition">更新条件,如果不指定则使用主键条件</param>
        /// <param name="cols">更新字段,如果不指定则更新所有字段</param>
        public BoolMessage Update(User entity,
            Expression<Func<User, bool>> condition = null, string[] cols = null)
        {
            try
            {
                if (condition == null)
                {
                    condition = p => p.Id == entity.Id;
                }
                Repos.Update(entity, condition, cols);
                return BoolMessage.True;
            }
            catch (Exception e)
            {
                return new BoolMessage(false, e.Message);
            }
        }

        /// <summary>
        /// 删除系统用户
        /// </summary>
        /// <param name="id">系统用户主键</param>
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
        /// 获取系统用户
        /// </summary>
        public IEnumerable<User> Query()
        {
            return Repos.Query();
        }
    }
}