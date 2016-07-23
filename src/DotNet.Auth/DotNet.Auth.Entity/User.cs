// ===============================================================================
// DotNet.Platform 开发框架 2016 版权所有
// ===============================================================================
using System;
using DotNet.Entity;

namespace DotNet.Auth.Entity
{
    /// <summary>
    /// 系统用户
    /// </summary>
    [Table("Users", "系统用户")]
    public class User
    {
        /// <summary>
        /// 主键
        /// </summary>
        [PrimaryKey]
        [Column("主键", false)]
        public string Id { get; set; }

        /// <summary>
        /// 帐号
        /// </summary>
        [Column("帐号")]
        public string Account { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        [Column("密码")]
        public string Password { get; set; }

        /// <summary>
        /// 姓名
        /// </summary>
        [Column("姓名")]
        public string Name { get; set; }

        /// <summary>
        /// 简拼
        /// </summary>
        [Column("简拼")]
        public string Spell { get; set; }

        /// <summary>
        /// 部门主键
        /// </summary>
        [Column("部门Id")]
        public string DepartmentId { get; set; }

        /// <summary>
        /// 部门名称
        /// </summary>
        [Column("部门名称")]
        public string DepartmentName { get; set; }

        /// <summary>
        /// 企业主键
        /// </summary>
        [Column("企业主键")]
        public string CompanyId { get; set; }

        /// <summary>
        /// 企业称
        /// </summary>
        [Column("企业名称")]
        public string CompanyName { get; set; }

        /// <summary>
        /// 培训机构主键
        /// </summary>
        [Column("培训机构主键")]
        public string SchoolId { get; set; }

        /// <summary>
        /// 培训机构名称
        /// </summary>
        [Column("培训机构名称")]
        public string SchoolName { get; set; }

        /// <summary>
        /// 用户类型主键
        /// </summary>
        [Column("用户类型主键")]
        public string UserCategoryId { get; set; }

        /// <summary>
        /// 用户类型名称
        /// </summary>
        [Column("用户类型名称")]
        public string UserCategoryName { get; set; }

        /// <summary>
        /// 允许开始时间
        /// </summary>
        [Column("允许开始时间")]
        public DateTime? AllowStartDateTime { get; set; }

        /// <summary>
        /// 允许结束时间
        /// </summary>
        [Column("允许结束时间")]
        public DateTime? AllowEndDateTime { get; set; }

        /// <summary>
        /// 第一次登录时间
        /// </summary>
        [Column("第一次登录时间")]
        public DateTime? FirstVisitDateTime { get; set; }

        /// <summary>
        /// 最后一次登录时间
        /// </summary>
        [Column("最后一次登录时间")]
        public DateTime? LastVisitDateTime { get; set; }

        /// <summary>
        /// 登录次数
        /// </summary>
        [Column("登录次数")]
        public int LoginCount { get; set; }

        /// <summary>
        /// 管理员
        /// </summary>
        [Column("管理员")]
        public bool IsAdmin { get; set; }

        /// <summary>
        /// 启用
        /// </summary>
        [Column("启用")]
        public bool IsEnabled { get; set; } = true;

        /// <summary>
        /// 审核
        /// </summary>
        [Column("审核")]
        public bool IsAudit { get; set; } = true;

        /// <summary>
        /// 电子邮件
        /// </summary>
        [Column("电子邮件")]
        public string Email { get; set; }

        /// <summary>
        /// 密码提示问题
        /// </summary>
        [Column("密码提示问题")]
        public string HintQuestion { get; set; }

        /// <summary>
        /// 密码提示答案
        /// </summary>
        [Column("密码提示答案")]
        public string HintAnswer { get; set; }

        /// <summary>
        /// 序号
        /// </summary>
        [Column("序号")]
        public int RowIndex { get; set; }

        /// <summary>
        /// 默认角色Id
        /// </summary>
        [Column("默认角色Id")]
        public string DefaultRoleId { get; set; }

        /// <summary>
        /// 默认角色名称
        /// </summary>
        [Column("默认角色名称")]
        public string DefaultRoleName { get; set; }

        /// <summary>
        /// 创建用户Id
        /// </summary>
        [Column("创建用户Id")]
        public string CreateUserId { get; set; }

        /// <summary>
        /// 创建用户姓名
        /// </summary>
        [Column("创建用户姓名")]
        public string CreateUserName { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        [Column("创建时间")]
        public DateTime? CreateDateTime { get; set; }

        /// <summary>
        /// 修改用户Id
        /// </summary>
        [Column("修改用户Id")]
        public string ModifyUserId { get; set; }

        /// <summary>
        /// 修改用户姓名
        /// </summary>
        [Column("修改用户姓名")]
        public string ModifyUserName { get; set; }

        /// <summary>
        /// 修改时间
        /// </summary>
        [Column("修改时间")]
        public DateTime? ModifyDateTime { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [Column("备注")]
        public string Note { get; set; }

        /// <summary>
        /// 复制对象
        /// </summary>
        public User Clone()
        {
            return (User)this.MemberwiseClone();
        }
    }
}