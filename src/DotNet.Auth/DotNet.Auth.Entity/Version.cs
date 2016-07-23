// ===============================================================================
// DotNet.Platform 开发框架 2016 版权所有
// ===============================================================================

using System;
using DotNet.Entity;

namespace DotNet.Auth.Entity
{
    /// <summary>
    /// 系统数据版本
    /// </summary>
    [Table("Versions", "系统数据版本")]
    public class Version
    {
        /// <summary>
        /// 主键
        /// </summary>
        [PrimaryKey]
        [Column("主键", false)]
        public string Id { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        [Column("名称")]
        public string TableName { get; set; }

        /// <summary>
        /// 主键名
        /// </summary>
        [Column("主键名")]
        public string KeyName { get; set; }

        /// <summary>
        /// 主键值
        /// </summary>
        [Column("主键值")]
        public string KeyValue { get; set; }

        /// <summary>
        /// 操作类型:1.新增2.修改3.删除
        /// </summary>
        [Column("操作类型")]
        public int Category { get; set; }

        /// <summary>
        /// 操作类型名称
        /// </summary>
        [Ignore]
        public string CategoryName
        {
            get
            {
                switch (Category)
                {
                    case 1: return "新增";
                    case 2: return "修改";
                    case 3: return "删除";
                    default:
                        return "未知";
                }
            }
        }

        /// <summary>
        /// 操作前数据
        /// </summary>
        [Column("操作前数据")]
        public string Before { get; set; }

        /// <summary>
        /// 操作后数据
        /// </summary>
        [Column("操作后数据")]
        public string After { get; set; }

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
        /// 复制对象
        /// </summary>
        public Version Clone()
        {
            return (Version)MemberwiseClone();
        }
    }
}