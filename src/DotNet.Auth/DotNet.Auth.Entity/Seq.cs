// ===============================================================================
// DotNet.Platform 开发框架 2016 版权所有
// ===============================================================================
using System;
using DotNet.Entity;

namespace DotNet.Auth.Entity
{
    /// <summary>
    /// 系统序列
    /// </summary>
    [Table("Seqs","系统序列")]
    public class Seq
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
        public string Name { get; set; }

        /// <summary>
        /// 值
        /// </summary>
        [Column("值")]
        public int Value { get; set; } = 1;

        /// <summary>
        /// 步长
        /// </summary>
        [Column("步长")]
        public int Step { get; set; } = 1;

        /// <summary>
        /// 备注
        /// </summary>
        [Column("备注")]
        public string Note { get; set; }

        /// <summary>
        /// 复制对象
        /// </summary>
        public Seq Clone()
        {
            return (Seq)this.MemberwiseClone();
        }
    }
}