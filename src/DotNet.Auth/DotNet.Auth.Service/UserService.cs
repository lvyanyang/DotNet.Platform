// ===============================================================================
// DotNet.Platform 开发框架 2016 版权所有
// ===============================================================================
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using DotNet.Auth.Entity;
using DotNet.Collections;
using DotNet.Data.Extensions;
using DotNet.Extensions;
using DotNet.Helper;
using DotNet.Utility;

namespace DotNet.Auth.Service
{
    /// <summary>
    /// 系统用户服务
    /// </summary>
    public class UserService
    {
        private static readonly Cache<string, User> Cache = new Cache<string, User>();

        /// <summary>
        /// 构造服务
        /// </summary>
        internal UserService()
        {
            InitCache();
        }

        /// <summary>
        /// 初始化缓存
        /// </summary>
        public void InitCache()
        {
            var repos = new AuthRepository<User>();
            Cache.Clear().Set(repos.Query().ToDictionary(p => p.Id, p => p));
        }

        /// <summary>
        /// 用户登录
        /// </summary>
        /// <param name="account">账号</param>
        /// <param name="password">密码(明文)</param>
        /// <returns>操作成功返回True</returns>
        public BoolMessage Login(string account, string password)
        {
            account = account.Trim();
            var entity = GetByAccount(account);
            if (entity == null)
            {
                return new BoolMessage(false, "无效的账号");
            }

            #region 检测条件

            var encryptedPwd = GenerateEncryptPassword(password);
            if (!entity.Password.Equals(encryptedPwd))
            {
                return new BoolMessage(false, "账号密码错误");
            }

            if (!entity.IsEnabled)
            {
                return new BoolMessage(false, "账号已经被禁用,请联系管理员");
            }

            if (!entity.IsAudit)
            {
                return new BoolMessage(false, "账号没有通过审核,请联系管理员");
            }

            if (IsUserExpire(entity))
            {
                return new BoolMessage(false, "账号已过期,请联系管理员");
            }

            #endregion

            #region 更新状态

            entity.LastVisitDateTime = DateTime.Now;
            entity.LoginCount += 1;
            if (!entity.FirstVisitDateTime.HasValue)
            {
                entity.FirstVisitDateTime = DateTime.Now;
            }

            var cols = new[]
            {
                nameof(entity.FirstVisitDateTime),
                nameof(entity.LastVisitDateTime),
                nameof(entity.LoginCount),
            };
            var repos = new AuthRepository<User>();
            repos.Update(entity, cols);

            #endregion

            return new BoolMessage(true);
        }

        /// <summary>
        /// 用户注销
        /// </summary>
        /// <param name="userId">主键</param>
        /// <returns>操作成功返回True</returns>
        public BoolMessage Logout(int userId)
        {
            //var entity = Repository.Get(p => p.Id.Equals(userId) && p.Deleted == false, p => p.Id);
            //if (entity == null)
            //{
            //    return new BoolMessage(false, "无效的用户主键");
            //}
            //entity.IsOnline = false;
            //return Repository.Update(entity, p => p.Id == entity.Id, p => p.IsOnline);

            //SystemOnlineUserManage.Current.LogoutUser(userId);
            //SystemLogManage.Current.Message("用户退出系统成功", Constant.LogonLogCategory);
            return new BoolMessage(true);
        }

        /// <summary>
        /// 生成用户加密后密码
        /// </summary>
        /// <param name="password">密码(明文)</param>
        /// <returns>返回加密后的密码</returns>
        public string GenerateEncryptPassword(string password)
        {
            //if (string.IsNullOrEmpty(account))
            //{
            //    throw new ArgumentNullException(nameof(account), "请指定用户账号");
            //}
            return StringHelper.EncryptString(password); //StringHelper.EncryptString($"{account}-@{password}");
        }

        /// <summary>
        /// 用户是否过期
        /// </summary>
        /// <param name="entity">对象</param>
        /// <returns></returns>
        public bool IsUserExpire(User entity)
        {
            return (entity.AllowStartDateTime.HasValue && entity.AllowStartDateTime.Value >= DateTime.Now)
                   || (entity.AllowEndDateTime.HasValue && entity.AllowEndDateTime.Value <= DateTime.Now);
        }

        /// <summary>
        /// 检测是否存在指定账号对象
        /// </summary>
        /// <param name="id">主键</param>
        /// <param name="account">账号</param>
        /// <returns>存在返回false</returns>
        public BoolMessage ExistsByAccount(string id, string account)
        {
            var has = Cache.ValueList().Contains(p => p.Account.Equals(account) && !p.Id.Equals(id));
            return has ? new BoolMessage(false, "输入用户账号已经存在") : BoolMessage.True;
        }

        /// <summary>
        /// 添加对象
        /// </summary>
        /// <param name="entity">实体(密码明文)</param>
        public BoolMessage Create(User entity)
        {
            var repos = new AuthRepository<User>();
            entity.Password = GenerateEncryptPassword(entity.Password);
            repos.Insert(entity);
            Cache.Set(entity.Id, entity);
            return BoolMessage.True;
        }

        /// <summary>
        /// 更新对象
        /// </summary>
        /// <param name="entity">实体(不更新密码)</param>
        public BoolMessage Update(User entity)
        {
            #region 更新列
            var cols = new[]
            {
                nameof(entity.Account),
                nameof(entity.Name),
                nameof(entity.Spell),
                nameof(entity.AllowStartDateTime),
                nameof(entity.AllowEndDateTime),
                nameof(entity.IsAdmin),
                nameof(entity.IsEnabled),
                nameof(entity.IsAudit),
                nameof(entity.HintQuestion),
                nameof(entity.HintAnswer),
                nameof(entity.RowIndex),
                nameof(entity.Email),
                nameof(entity.DefaultRoleId),
                nameof(entity.DefaultRoleName),
                nameof(entity.Note),
                nameof(entity.DepartmentId),
                nameof(entity.DepartmentName),
                nameof(entity.UserCategoryId),
                nameof(entity.UserCategoryName),
                nameof(entity.CompanyId),
                nameof(entity.CompanyName),
                nameof(entity.SchoolId),
                nameof(entity.SchoolName),
                nameof(entity.ModifyUserId),
                nameof(entity.ModifyUserName),
                nameof(entity.ModifyDateTime),
            };
            #endregion

            var repos = new AuthRepository<User>();
            repos.Update(entity, cols);
            var e = Cache.Get(entity.Id);
            if (e != null)
            {
                ObjectHelper.SetObjectValue(entity, e, cols);
            }
            return BoolMessage.True;
        }

        /// <summary>
        /// 保存对象
        /// </summary>
        /// <param name="entity">实体</param>
        /// <param name="isCreate">是否新增</param>
        public BoolMessage Save(User entity, bool isCreate)
        {
            return isCreate ? Create(entity) : Update(entity);
        }

        /// <summary>
        /// 删除对象
        /// </summary>
        /// <param name="ids">主键数组</param>
        public BoolMessage Delete(string[] ids)
        {
            var repos = new AuthRepository<User>();
            repos.Delete(ids);
            Cache.Remove(ids);
            return BoolMessage.True;
        }

        /// <summary>
        /// 获取对象
        /// </summary>
        /// <param name="id">主键</param>
        public User Get(string id)
        {
            return Cache.Get(id);
        }

        /// <summary>
        /// 获取用户对象根据姓名
        /// </summary>
        /// <param name="name">姓名</param>
        public User GetByName(string name)
        {
            return Cache.ValueList().FirstOrDefault(p => p.Name.Equals(name));
        }

        /// <summary>
        /// 根据账号获取用户对象
        /// </summary>
        /// <param name="account">账号</param>
        public User GetByAccount(string account)
        {
            return Cache.ValueList().FirstOrDefault(p => p.Account.Equals(account));
        }

        /// <summary>
        /// 获取用户是否是管理员
        /// </summary>
        /// <param name="id">用户主键</param>
        /// <returns>如果指定用户是管理员返回true</returns>
        public bool IsAdmin(string id)
        {
            var entity = Get(id);
            return entity != null && entity.IsAdmin;
        }

        ///// <summary>
        ///// 获取用户的部门领导对象
        ///// </summary>
        ///// <param name="userId">用户主键</param>
        ///// <returns>返回指定用户部门领导对象</returns>
        //public SystemUser GetDeptLeader(string userId)
        //{
        //    return GetLeader(userId, "DepartmentLeaderId");
        //}

        ///// <summary>
        ///// 指定用户是否是部门领导
        ///// </summary>
        ///// <param name="userId">用户主键</param>
        ///// <returns>如果是部门领导返回true</returns>
        //public bool IsDeptLeader(string userId)
        //{
        //    return IsLeader(userId, "DepartmentLeaderId");
        //}

        ///// <summary>
        ///// 获取用户的分管领导对象
        ///// </summary>
        ///// <param name="userId">用户主键</param>
        ///// <returns>返回指定用户分管领导对象</returns>
        //public SystemUser GetChargeLeader(string userId)
        //{
        //    return GetLeader(userId, "ChargeLeaderId");
        //}

        ///// <summary>
        ///// 指定用户是否是分管领导
        ///// </summary>
        ///// <param name="userId">用户主键</param>
        ///// <returns>如果是分管领导返回true</returns>
        //public bool IsChargeLeader(string userId)
        //{
        //    return IsLeader(userId, "ChargeLeaderId");
        //}

        ///// <summary>
        ///// 获取用户的主管领导对象
        ///// </summary>
        ///// <param name="userId">用户主键</param>
        ///// <returns>返回指定用户主管领导对象</returns>
        //public SystemUser GetMainLeader(string userId)
        //{
        //    return GetLeader(userId, "MainLeaderId");
        //}

        ///// <summary>
        ///// 指定用户是否是主管领导
        ///// </summary>
        ///// <param name="userId">用户主键</param>
        ///// <returns>如果是主管领导返回true</returns>
        //public bool IsMainLeader(string userId)
        //{
        //    return IsLeader(userId, "MainLeaderId");
        //}

        /// <summary>
        /// 验证用户密码
        /// </summary>
        /// <param name="userId">用户主键</param>
        /// <param name="password">密码</param>
        public bool ValidUserPassword(string userId, string password)
        {
            var entity = Get(userId);
            if (entity == null)
            {
                throw new ArgumentException("无效的用户主键", nameof(userId));
            }
            return entity.Password.Equals(GenerateEncryptPassword(password));
        }

        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="userIds">用户主键数组</param>
        /// <param name="newPassword">新密码(明文)</param>
        public BoolMessage UpdatePassword(string[] userIds, string newPassword)
        {
            if (userIds == null || userIds.Length == 0) throw new InvalidOperationException("请指定用户主键数组");
            var pwd = GenerateEncryptPassword(newPassword);
            foreach (var id in userIds)
            {
                var entity = Get(id);
                if (entity != null)
                {
                    entity.Password = pwd;
                }
            }
            var repos = new AuthRepository<User>();
            repos.UpdateInclude(
                new User { Password = pwd}, p => p.Id.In(userIds),
                p => p.Password);
            return new BoolMessage(true, "密码修改成功");
        }

        /// <summary>
        /// 更新用户部门信息
        /// </summary>
        /// <param name="userIds">用户主键数组</param>
        /// <param name="deptId">部门主键</param>
        /// <param name="deptName">部门名称</param>
        public BoolMessage UpdateUserDept(string[] userIds, string deptId, string deptName)
        {
            if (userIds == null || userIds.Length == 0) throw new InvalidOperationException("请指定用户主键数组");
            foreach (var id in userIds)
            {
                var entity = Get(id);
                if (entity != null)
                {
                    entity.DepartmentId = deptId;
                    entity.DepartmentName = deptName;
                }
            }
            var repos = new AuthRepository<User>();
            repos.UpdateInclude(
                new User { DepartmentId = deptId, DepartmentName = deptName },p => p.Id.In(userIds),
                p => p.DepartmentId, 
                p => p.DepartmentName);
            return BoolMessage.True;
        }

        /// <summary>
        /// 获取新建序号
        /// </summary>
        public int GetNewRowIndex()
        {
            return Cache.ValueList().Max(p => p.RowIndex, 0) + 1;
        }

        /// <summary>
        /// 获取启用的简单对象集合(已排序)
        /// </summary>
        public List<Simple> GetSimpleList()
        {
            return Cache.ValueList()
                .Where(p => p.IsEnabled).ToList()
                .OrderByAsc(p => p.RowIndex)
                .Select(p => new Simple(p.Id, p.Name, p.Spell))
                .ToList();
        }

        /// <summary>
        /// 获取启用的对象集合(已排序)
        /// </summary>
        public List<User> GetList()
        {
            return Cache.ValueList()
                .Where(p => p.IsEnabled).ToList()
                .OrderByAsc(p => p.RowIndex);
        }

        /// <summary>
        /// 获取对象集合
        /// </summary>
        /// <param name="deptIds">部门主键</param>
        /// <param name="isEnabled">启用状态(null为全部,true为启用,false为禁用)</param>
        /// <param name="isOrderBy">是否排序</param>
        public List<User> GetList(string[] deptIds, bool? isEnabled = true, bool isOrderBy = true)
        {
            var list = Cache.ValueList()
                .Where(p => p.DepartmentId.InArray(deptIds));
            if (isEnabled.HasValue)
            {
                list = list.Where(p => p.IsEnabled == isEnabled);
            }
            return isOrderBy ? list.ToList().OrderByAsc(p => p.RowIndex) : list.ToList();
        }

        /// <summary>
        /// 获取对象分页集合
        /// </summary>
        /// <param name="pageCondition">分页对象</param>
        /// <param name="name">用户账号/姓名/简拼</param>
        /// <param name="departmentId">部门主键</param>
        /// <param name="isEnabled">启用</param>
        public PageList<User> GetPageList(PaginationCondition pageCondition,
            string name, string departmentId, bool? isEnabled)
        {
            pageCondition.SetDefaultOrder(nameof(User.RowIndex));
            var repos = new AuthRepository<User>();
            var query = repos.PageQuery(pageCondition);
            if (name.IsNotEmpty())
            {
                name = name.Trim();
                query.Where(p => p.Account.Contains(name) || p.Name.Contains(name) || p.Spell.Contains(name));
            }
            if (isEnabled.HasValue)
            {
                query.Where(p => p.IsEnabled == isEnabled.Value);
            }
            if (departmentId.IsNotEmpty() && !departmentId.Equals(TreeHelper.RootTreeNodeId))
            {
                var childs = AuthService.Department.GetChildNodeList(departmentId, true);
                if (childs != null && childs.Count > 0)
                {
                    var deptIds = childs.Select(p => p.Id).ToArray();
                    query.Where(p => p.DepartmentId.In(deptIds));
                }
            }
            return repos.Page(query);
        }
    }
}