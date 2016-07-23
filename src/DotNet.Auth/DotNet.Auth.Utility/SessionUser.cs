// ===============================================================================
// DotNet.Platform 开发框架 2016 版权所有
// ===============================================================================
using System;
using System.Collections.Generic;
using DotNet.Auth.Entity;
using DotNet.Utility;

namespace DotNet.Auth.Utility
{
    /// <summary>
    /// 会话用户
    /// </summary>
    public class SessionUser
    {
        /// <summary>
        /// 主键
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// 账号
        /// </summary>
        public string Account { get; set; }

        /// <summary>
        /// 姓名
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 部门主键
        /// </summary>
        public string DepartmentId { get; set; }

        /// <summary>
        /// 部门名称
        /// </summary>
        public string DepartmentName { get; set; }

        /// <summary>
        /// 是否管理员
        /// </summary>
        public bool IsAdmin { get; set; }

        /// <summary>
        /// 登陆IP地址
        /// </summary>
        public string LoginIPAddr { get; set; }

        /// <summary>
        /// 登陆时间
        /// </summary>
        public DateTime LoginDateTime { get; set; }

        /// <summary>
        /// 用户实体对象
        /// </summary>
        public User User { get; set; }

        /// <summary>
        /// 部门实体对象
        /// </summary>
        public Department Department { get; set; }

        /// <summary>
        /// 角色列表
        /// </summary>
        public List<Role> Roles { get; set; }

        /// <summary>
        /// 菜单列表
        /// </summary>
        public List<Menu> Menus { get; set; }

        /// <summary>
        /// 菜单节点列表
        /// </summary>
        public List<TreeNode> MenuNodes { get; set; }

        /// <summary>
        /// 是否是企业
        /// </summary>
        public bool IsCompany => User.UserCategoryId.Equals("1");

        /// <summary>
        /// 是否是培训机构
        /// </summary>
        public bool IsSchool => User.UserCategoryId.Equals("2");

        /// <summary>
        /// 是否是管理者
        /// </summary>
        public bool IsManager => User.UserCategoryId.Equals("3");
    }
}