// ===============================================================================
// DotNet.Platform 开发框架 2016 版权所有
// ===============================================================================
using System;
using DotNet.Entity;

namespace DotNet.Auth.Entity
{
    /// <summary>
    /// 系统部门
    /// </summary>
    [Table("Departments", "系统部门")]
    public class Department
    {
        /// <summary>
        /// 主键
        /// </summary>
        [PrimaryKey]
        [Column("主键", false)]
        public string Id { get; set; }

        /// <summary>
        /// 父Id
        /// </summary>
        [ParentKey]
        [Column("父Id")]
        public string ParentId { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        [TextKey]
        [Column("名称")]
        public string Name { get; set; }

        /// <summary>
        /// 简拼
        /// </summary>
        [Column("简拼")]
        public string Spell { get; set; }

        /// <summary>
        /// 部门领导主键
        /// </summary>
        [Column("部门领导Id")]
        public string DepartmentLeaderId { get; set; }

        /// <summary>
        /// 部门领导名称
        /// </summary>
        [Column("部门领导名称")]
        public string DepartmentLeaderName { get; set; }

        /// <summary>
        /// 分管领导主键
        /// </summary>
        [Column("分管领导Id")]
        public string ChargeLeaderId { get; set; }

        /// <summary>
        /// 分管领导名称
        /// </summary>
        [Column("分管领导名称")]
        public string ChargeLeaderName { get; set; }

        /// <summary>
        /// 主管领导主键
        /// </summary>
        [Column("主管领导Id")]
        public string MainLeaderId { get; set; }

        /// <summary>
        /// 主管领导名称
        /// </summary>
        [Column("主管领导名称")]
        public string MainLeaderName { get; set; }

        /// <summary>
        /// 排序路径
        /// </summary>
        [SortPath]
        [Column("排序路径")]
        public string SortPath { get; set; }

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
        public Department Clone()
        {
            return (Department)this.MemberwiseClone();
        }
    }
}