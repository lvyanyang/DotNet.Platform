// ===============================================================================
// DotNet.Platform 开发框架 2016 版权所有
// ===============================================================================

using System;
using DotNet.Entity;

namespace DotNet.Edu.Entity
{
    /// <summary>
    /// 通知公告
    /// </summary>
    [Table("通知公告")]    
    public class Notice
    {
		/// <summary>
        /// 主键
        /// </summary>
        [PrimaryKey]
        [Column("主键",false)]
        public string Id { get; set; }

		/// <summary>
        /// 标题
        /// </summary>
		[Column("标题")]
        public string Title { get; set; }

		/// <summary>
        /// 消息内容
        /// </summary>
		[Column("消息内容")]
        public string Message { get; set; }

		/// <summary>
        /// 创建时间
        /// </summary>
		[Column("创建时间")]
        public DateTime CreateDateTime { get; set; }

        /// <summary>
        /// 复制对象
        /// </summary>
        public Notice Clone()
        {
            return (Notice)MemberwiseClone();
        }
    }
}