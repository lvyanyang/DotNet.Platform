// ===============================================================================
// DotNet.Platform 开发框架 2016 版权所有
// ===============================================================================
using System;
using System.Collections.Generic;
using System.Web;
using DotNet.Auth.Entity;
using DotNet.Auth.Service;
using DotNet.Extensions;
using DotNet.Helper;

namespace DotNet.Auth.Utility
{
    public class AuthHelper
    {
        /// <summary>
        /// 设置对象的用户信息(包括创建用户信息和修改用户信息)
        /// </summary>
        /// <param name="entity">待修改的对象</param>
        /// <param name="isCreate">是否新建</param>
        public static void SetEntityUserInfo(dynamic entity, bool isCreate)
        {
            var type = entity?.GetType();
            var currentDate = DateTime.Now;
            var currentUser = GetSessionUser();
            if (isCreate)
            {
                type?.GetProperty("CreateUserId")?.SetValue(entity, currentUser.Id);
                type?.GetProperty("CreateUserName")?.SetValue(entity, currentUser.Name);
                type?.GetProperty("CreateDateTime")?.SetValue(entity, currentDate);
                type?.GetProperty("CreateDate")?.SetValue(entity, currentDate);
                type?.GetProperty("CreateDepartmentId")?.SetValue(entity, currentUser.DepartmentId);
                type?.GetProperty("CreateDepartmentName")?.SetValue(entity, currentUser.DepartmentName);
            }
            else
            {
                type?.GetProperty("ModifyUserId")?.SetValue(entity, currentUser.Id);
                type?.GetProperty("ModifyUserName")?.SetValue(entity, currentUser.Name);
                type?.GetProperty("ModifyDateTime")?.SetValue(entity, currentDate);
                type?.GetProperty("ModifyDate")?.SetValue(entity, currentDate);
                type?.GetProperty("ModifyDepartmentId")?.SetValue(entity, currentUser.DepartmentId);
                type?.GetProperty("ModifyDepartmentName")?.SetValue(entity, currentUser.DepartmentName);
            }
        }

        /// <summary>
        /// 生成异常对象
        /// </summary>
        /// <param name="e">异常对象</param>
        public static Excep BuildExcep(Exception e)
        {
            return new Excep
            {
                Id = StringHelper.Guid(),
                Title = e.Message,
                MessageContent = StringHelper.BuildExceptionDetails(e),
                CreateDateTime = DateTime.Now
            };
        }


        /// <summary>
        /// 生成审计日志对象
        /// </summary>
        /// <param name="category">审计类型:1登录2退出</param>
        /// <param name="user">用户对象</param>
        public static Audit BuildAuditEntity(int category, User user)
        {
            var request = HttpContext.Current.Request;
            return new Audit
            {
                Id = StringHelper.Guid(),
                UserId = user?.Id,
                UserName = user?.Name,
                CreateDateTime = DateTime.Now,
                Category = category,
                AreaAddress = WebHelper.GetFormString("area"),
                IPAddress = WebHelper.GetFormString("ip", request.UserHostAddress),
                Browser = WebHelper.GetFormString("browser"),
                Device = WebHelper.GetFormString("device"),
                OS = WebHelper.GetFormString("os"),
                UserAgent = request.UserAgent
            };
        }

        /// <summary>
        /// 生成会话用户对象
        /// </summary>
        /// <param name="user">用户对象</param>
        public static SessionUser BuildSessionUser(User user)
        {
            var entity = new SessionUser();
            entity.Id = user.Id;
            entity.Account = user.Account;
            entity.Name = user.Name;
            entity.DepartmentId = user.DepartmentId;
            entity.DepartmentName = user.DepartmentName;
            entity.IsAdmin = user.IsAdmin;
            entity.LoginIPAddr = HttpContext.Current.Request.UserHostAddress;
            entity.LoginDateTime = DateTime.Now;
            entity.User = user;
            if (!string.IsNullOrEmpty(user.DefaultRoleId))
            {
                entity.Menus = AuthService.Menu.GetMenuListByRoleId(user.DefaultRoleId);
                entity.Roles = new List<Role>();

                var roleEntity = AuthService.Role.Get(user.DefaultRoleId);
                if (roleEntity != null)
                {
                    entity.Roles.Add(roleEntity);
                }
            }
            if (user.IsAdmin)
            {
                entity.Menus = AuthService.Menu.GetList();
            }
            entity.MenuNodes = TreeHelper.BuildTree(entity.Menus, "0", null, "font-icon fa fa-file-text-o", (n, e) =>
            {
                if (e.IconCls.IsNotEmpty())
                {
                    n.IconCls = e.IconCls;
                }
                if (e.IsExpand)
                {
                    n.State = DotNet.Utility.TreeNodeState.Open;
                }
                n.Url = e.Url;
            });
            return entity;
        }

        /// <summary>
        /// 设置会话用户
        /// </summary>
        /// <param name="user">用户对象</param>
        public static void SetSessionUser(SessionUser user)
        {
            var key = "_dotnet_user_";
            HttpContext.Current.Session[key] = user;
        }

        /// <summary>
        /// 获取会话用户
        /// </summary>
        public static SessionUser GetSessionUser()
        {
            var key = "_dotnet_user_";
            if (HttpContext.Current.Session[key] == null)
            {
                var account = HttpContext.Current.User.Identity.Name;
                if (string.IsNullOrEmpty(account))
                {
                    return null;
                }
                var suser = BuildSessionUser(AuthService.User.GetByAccount(account));
                HttpContext.Current.Session[key] = suser;
            }
            return HttpContext.Current.Session[key] as SessionUser;
        }
    }
}