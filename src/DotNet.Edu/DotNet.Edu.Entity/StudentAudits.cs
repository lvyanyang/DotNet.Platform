// ===============================================================================
// DotNet.Platform 开发框架 2016 版权所有
// ===============================================================================

using System;
using DotNet.Entity;

namespace DotNet.Edu.Entity
{
    /// <summary>
    /// 学员登录日志
    /// </summary>
    [Table("学员登录日志")]    
    public class StudentAudits
    {
		/// <summary>
        /// 主键
        /// </summary>
        [PrimaryKey]
        [Column("主键",false)]
        public string Id { get; set; }

		/// <summary>
        /// 学员主键
        /// </summary>
		[Column("学员主键")]
        public string StudentId { get; set; }

		/// <summary>
        /// 学员姓名
        /// </summary>
		[Column("学员姓名")]
        public string StudentName { get; set; }

		/// <summary>
        /// 登录时间
        /// </summary>
		[Column("登录时间")]
        public DateTime LoginDateTime { get; set; }

		/// <summary>
        /// 所在地区
        /// </summary>
		[Column("所在地区")]
        public string AreaAddress { get; set; }

		/// <summary>
        /// IP地址
        /// </summary>
		[Column("IP地址")]
        public string IPAddress { get; set; }

		/// <summary>
        /// 浏览器
        /// </summary>
		[Column("浏览器")]
        public string Browser { get; set; }

		/// <summary>
        /// 设备
        /// </summary>
		[Column("设备")]
        public string Device { get; set; }

		/// <summary>
        /// 操作系统
        /// </summary>
		[Column("操作系统")]
        public string OS { get; set; }

		/// <summary>
        /// 用户代理字符串
        /// </summary>
		[Column("用户代理字符串")]
        public string UserAgent { get; set; }

        /// <summary>
        /// 复制对象
        /// </summary>
        public StudentAudits Clone()
        {
            return (StudentAudits)MemberwiseClone();
        }
    }
}