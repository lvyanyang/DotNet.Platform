// ===============================================================================
// DotNet.Platform 开发框架 2016 版权所有
// ===============================================================================

using System;
using DotNet.Entity;

namespace DotNet.Auth.Entity
{
    /// <summary>
    /// 系统异常
    /// </summary>
    [Table("Exceptions", "系统异常")]
    public class Excep
    {
        /// <summary>
        /// 主键
        /// </summary>
        [PrimaryKey]
        [Column("主键", false)]
        public string Id { get; set; }

        /// <summary>
        /// 标题
        /// </summary>
        [Column("标题")]
        public string Title { get; set; }

        /// <summary>
        /// 内容
        /// </summary>
        [Column("内容")]
        public string MessageContent { get; set; }

        /// <summary>
        /// IP地址
        /// </summary>
        [Column("IP地址")]
        public string IPAddress { get; set; }

        /// <summary>
        /// 操作时间
        /// </summary>
        [Column("操作时间")]
        public DateTime CreateDateTime { get; set; }

        /// <summary>
        /// 复制对象
        /// </summary>
        public Excep Clone()
        {
            return (Excep)MemberwiseClone();
        }
    }
}