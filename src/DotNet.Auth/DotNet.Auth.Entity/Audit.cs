// ===============================================================================
// DotNet.Platform 开发框架 2016 版权所有
// ===============================================================================

using System;
using DotNet.Entity;

namespace DotNet.Auth.Entity
{
    /// <summary>
    /// 系统审计日志
    /// </summary>
    [Table("Audits", "系统审计日志")]
    public class Audit
    {
        /// <summary>
        /// 主键
        /// </summary>
        [PrimaryKey]
        [Column("主键", false)]
        public string Id { get; set; }

        /// <summary>
        /// 用户主键
        /// </summary>
        [Column("用户主键")]
        public string UserId { get; set; }

        /// <summary>
        /// 用户姓名
        /// </summary>
        [Column("姓名")]
        public string UserName { get; set; }

        /// <summary>
        /// 日期
        /// </summary>
        [Column("日期")]
        public DateTime CreateDateTime { get; set; }

        /// <summary>
        /// 审计类型:1登录2退出
        /// </summary>
        [Column("审计类型")]
        public int Category { get; set; }

        /// <summary>
        /// 审计类型名称
        /// </summary>
        [Ignore]
        public string CategoryName
        {
            get { return Category == 1 ? "登录" : "退出"; }
        }

        /// <summary>
        /// 所在地
        /// </summary>
        [Column("所在地")]
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
        /// 浏览器代理字符串
        /// </summary>
        [Column("浏览器代理字符串")]
        public string UserAgent { get; set; }

        /// <summary>
        /// 复制对象
        /// </summary>
        public Audit Clone()
        {
            return (Audit)MemberwiseClone();
        }
    }
}