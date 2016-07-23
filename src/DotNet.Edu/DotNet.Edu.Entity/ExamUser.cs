// ===============================================================================
// DotNet.Platform 开发框架 2016 版权所有
// ===============================================================================

using System;
using DotNet.Entity;

namespace DotNet.Edu.Entity
{
    /// <summary>
    /// 考试用户
    /// </summary>
    [Table("考试用户")]    
    public class ExamUser
    {
		/// <summary>
        /// 主键
        /// </summary>
        [PrimaryKey(true)]
        [Column("主键",false)]
        public int Id { get; set; }

		/// <summary>
        /// 账号
        /// </summary>
		[Column("账号")]
        public string LoginName { get; set; }

		/// <summary>
        /// 密码
        /// </summary>
		[Column("密码")]
        public string Password { get; set; }

		/// <summary>
        /// 姓名
        /// </summary>
		[Column("姓名")]
        public string UserName { get; set; }

		/// <summary>
        /// 姓名简拼
        /// </summary>
		[Column("姓名简拼")]
        public string UserNameSpell { get; set; }

		/// <summary>
        /// 身份证号码
        /// </summary>
		[Column("身份证号码")]
        public string IdNumber { get; set; }

		/// <summary>
        /// 是否管理员
        /// </summary>
		[Column("是否管理员")]
        public bool IsAdmin { get; set; }

		/// <summary>
        /// 是否预约
        /// </summary>
		[Column("是否预约")]
        public bool IsPrep { get; set; }

		/// <summary>
        /// 预约考试类型
        /// </summary>
		[Column("预约考试类型")]
        public string ExamType { get; set; }

        /// <summary>
        /// 复制对象
        /// </summary>
        public ExamUser Clone()
        {
            return (ExamUser)MemberwiseClone();
        }
    }
}