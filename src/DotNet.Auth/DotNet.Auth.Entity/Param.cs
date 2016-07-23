// ===============================================================================
// DotNet.Platform 开发框架 2016 版权所有
// ===============================================================================

using System;
using DotNet.Entity;

namespace DotNet.Auth.Entity
{
    /// <summary>
    /// 系统参数
    /// </summary>
    [Table("Params", "系统参数")]    
    public class Param
    {
		/// <summary>
        /// 主键
        /// </summary>
        [PrimaryKey]
        [Column("主键",false)]
        public string Id { get; set; }

		/// <summary>
        /// 分类主键
        /// </summary>
		[Column("分类主键")]
        public string CategoryId { get; set; }

		/// <summary>
        /// 分类名称
        /// </summary>
		[Column("分类名称")]
        public string CategoryName { get; set; }

		/// <summary>
        /// 编码
        /// </summary>
		[Column("编码")]
        public string Code { get; set; }

		/// <summary>
        /// 名称
        /// </summary>
		[Column("名称")]
        public string Name { get; set; }

		/// <summary>
        /// 参数值
        /// </summary>
		[Column("参数值")]
        public string Value { get; set; }

		/// <summary>
        /// 创建用户主键
        /// </summary>
		[Column("创建用户主键")]
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
        /// 修改用户主键
        /// </summary>
		[Column("修改用户主键")]
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
        public Param Clone()
        {
            return (Param)MemberwiseClone();
        }
    }
}