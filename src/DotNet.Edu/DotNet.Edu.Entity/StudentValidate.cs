// ===============================================================================
// DotNet.Platform 开发框架 2016 版权所有
// ===============================================================================

using System;
using DotNet.Entity;

namespace DotNet.Edu.Entity
{
    /// <summary>
    /// 学习验证
    /// </summary>
    [Table("学习验证")]    
    public class StudentValidate
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
        public string Name { get; set; }

		/// <summary>
        /// 选项A
        /// </summary>
		[Column("选项A")]
        public string A { get; set; }

		/// <summary>
        /// 选项B
        /// </summary>
		[Column("选项B")]
        public string B { get; set; }

		/// <summary>
        /// 选项C
        /// </summary>
		[Column("选项C")]
        public string C { get; set; }

		/// <summary>
        /// 选项D
        /// </summary>
		[Column("选项D")]
        public string D { get; set; }

		/// <summary>
        /// 正确答案
        /// </summary>
		[Column("正确答案")]
        public string Answer { get; set; }

        /// <summary>
        /// 复制对象
        /// </summary>
        public StudentValidate Clone()
        {
            return (StudentValidate)MemberwiseClone();
        }
    }
}