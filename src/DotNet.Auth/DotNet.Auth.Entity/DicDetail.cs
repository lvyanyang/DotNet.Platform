// ===============================================================================
// DotNet.Platform 开发框架 2016 版权所有
// ===============================================================================
using System;
using DotNet.Entity;

namespace DotNet.Auth.Entity
{
    /// <summary>
    /// 系统字典明细
    /// </summary>
    [Table("DicDetails", "系统字典明细")]
    public class DicDetail
    {
        /// <summary>
        /// 主键
        /// </summary>
        [PrimaryKey]
        [Column("主键", false)]
        public string Id { get; set; }

        /// <summary>
        /// 字典Id
        /// </summary>
        [Column("字典Id")]
        public string DicId { get; set; }

        /// <summary>
        /// 项名称
        /// </summary>
        [Column("项名称")]
        public string Name { get; set; }

        /// <summary>
        /// 项值
        /// </summary>
        [Column("项值")]
        public string Value { get; set; }

        /// <summary>
        /// 项简拼
        /// </summary>
        [Column("项简拼")]
        public string Spell { get; set; }

        /// <summary>
        /// 启用
        /// </summary>
        [Column("启用")]
        public bool IsEnabled { get; set; } = true;

        /// <summary>
        /// 序号
        /// </summary>
        [Column("序号")]
        public int RowIndex { get; set; }

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
        public DicDetail Clone()
        {
            return (DicDetail)this.MemberwiseClone();
        }
    }
}