// ===============================================================================
// DotNet.Platform 开发框架 2016 版权所有
// ===============================================================================

using System;
using DotNet.Entity;

namespace DotNet.Auth.Entity
{
    /// <summary>
    /// 系统消息
    /// </summary>
    [Table("系统消息")]
    public class Message
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
        /// 0系统其他为用户主键
        /// </summary>
        [Column("0系统其他为用户主键")]
        public string SendUserId { get; set; }

        /// <summary>
        /// 发送人姓名
        /// </summary>
        [Column("发送人姓名")]
        public string SendUserName { get; set; }

        /// <summary>
        /// 发送时间
        /// </summary>
        [Column("发送时间")]
        public DateTime SendDateTime { get; set; }

        /// <summary>
        /// 接收人主键
        /// </summary>
        [Column("接收人主键")]
        public string ReceiveUserId { get; set; }

        /// <summary>
        /// 接收人姓名
        /// </summary>
        [Column("接收人姓名")]
        public string ReceiveUserName { get; set; }

        /// <summary>
        /// 接收时间
        /// </summary>
        [Column("接收时间")]
        public DateTime? ReadDateTime { get; set; }

        /// <summary>
        /// 是否读取
        /// </summary>
        [Column("状态")]
        public bool IsRead { get; set; }

        /// <summary>
        /// 读取状态
        /// </summary>
        [Ignore]
        public string ReadStatus
        {
            get { return IsRead ? "已读" : "未读"; }
        }

        /// <summary>
        /// 复制对象
        /// </summary>
        public Message Clone()
        {
            return (Message)MemberwiseClone();
        }
    }
}