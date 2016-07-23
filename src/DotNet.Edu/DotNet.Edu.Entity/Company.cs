// ===============================================================================
// DotNet.Platform 开发框架 2016 版权所有
// ===============================================================================

using System;
using DotNet.Entity;

namespace DotNet.Edu.Entity
{
    /// <summary>
    /// 企业信息
    /// </summary>
    [Table("企业信息")]    
    public class Company
    {
		/// <summary>
        /// 主键
        /// </summary>
        [PrimaryKey]
        [Column("主键",false)]
        public string Id { get; set; }

		/// <summary>
        /// 简拼
        /// </summary>
		[Column("简拼")]
        public string Spell { get; set; }

		/// <summary>
        /// 名称
        /// </summary>
		[Column("名称")]
        public string Name { get; set; }

		/// <summary>
        /// 联系人
        /// </summary>
		[Column("联系人")]
        public string LinkMan { get; set; }

		/// <summary>
        /// 联系电话
        /// </summary>
		[Column("联系电话")]
        public string TelPhone { get; set; }

		/// <summary>
        /// 手机号码
        /// </summary>
		[Column("手机号码")]
        public string MobilePhone { get; set; }

		/// <summary>
        /// 传真
        /// </summary>
		[Column("传真")]
        public string Fax { get; set; }

		/// <summary>
        /// 地址
        /// </summary>
		[Column("地址")]
        public string Address { get; set; }

		/// <summary>
        /// 创建时间
        /// </summary>
		[Column("创建时间")]
        public DateTime CreateDateTime { get; set; }

		/// <summary>
        /// 备注
        /// </summary>
		[Column("备注")]
        public string Note { get; set; }

        /// <summary>
        /// 启用
        /// </summary>
        [Column("启用")]
        public bool IsEnabled { get; set; } = true;

        /// <summary>
        /// 复制对象
        /// </summary>
        public Company Clone()
        {
            return (Company)MemberwiseClone();
        }
    }
}