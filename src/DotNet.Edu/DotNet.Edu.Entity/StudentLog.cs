// ===============================================================================
// DotNet.Platform 开发框架 2016 版权所有
// ===============================================================================

using System;
using DotNet.Entity;

namespace DotNet.Edu.Entity
{
    /// <summary>
    /// 学员日志
    /// </summary>
    [Table("学员日志")]    
    public class StudentLog
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
        /// 姓名
        /// </summary>
		[Column("姓名")]
        public string StudentName { get; set; }

		/// <summary>
        /// 操作日期
        /// </summary>
		[Column("操作日期")]
        public DateTime CreateDateTime { get; set; }

		/// <summary>
        /// 消息
        /// </summary>
		[Column("消息")]
        public string Message { get; set; }

        /// <summary>
        /// 复制对象
        /// </summary>
        public StudentLog Clone()
        {
            return (StudentLog)MemberwiseClone();
        }
    }
}