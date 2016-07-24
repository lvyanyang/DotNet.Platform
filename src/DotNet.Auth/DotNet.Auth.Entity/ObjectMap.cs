// ===============================================================================
// DotNet.Platform 开发框架 2016 版权所有
// ===============================================================================

using System;
using DotNet.Entity;

namespace DotNet.Edu.Entity
{
    /// <summary>
    /// 对象资源映射
    /// </summary>
    [Table("对象资源映射")]    
    public class ObjectMap
    {
		/// <summary>
        /// 对象主键
        /// </summary>
		[Column("对象主键")]
        public string ObjectId { get; set; }

		/// <summary>
        /// 对象名称
        /// </summary>
		[Column("对象名称")]
        public string ObjectName { get; set; }

		/// <summary>
        /// 目标主键
        /// </summary>
		[Column("目标主键")]
        public string TargetId { get; set; }

		/// <summary>
        /// 目标名称
        /// </summary>
		[Column("目标名称")]
        public string TargetName { get; set; }

        /// <summary>
        /// 复制对象
        /// </summary>
        public ObjectMap Clone()
        {
            return (ObjectMap)MemberwiseClone();
        }
    }
}