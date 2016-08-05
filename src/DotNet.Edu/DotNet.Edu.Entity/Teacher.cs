// ===============================================================================
// DotNet.Platform 开发框架 2016 版权所有
// ===============================================================================

using System;
using DotNet.Entity;

namespace DotNet.Edu.Entity
{
    /// <summary>
    /// 教师
    /// </summary>
    [Table("教师")]    
    public class Teacher
    {
		/// <summary>
        /// 主键
        /// </summary>
        [PrimaryKey]
        [Column("主键",false)]
        public string Id { get; set; }

		/// <summary>
        /// 编号
        /// </summary>
		[Column("编号")]
        public string Code { get; set; }

		/// <summary>
        /// 姓名
        /// </summary>
		[Column("姓名")]
        public string Name { get; set; }

		/// <summary>
        /// 性别
        /// </summary>
		[Column("性别")]
        public string Sex { get; set; }

		/// <summary>
        /// 身份证号码
        /// </summary>
		[Column("身份证号码")]
        public string IdNumber { get; set; }

		/// <summary>
        /// 培训机构
        /// </summary>
		[Column("培训机构",Exported =false)]
        public string SchoolId { get; set; }

		/// <summary>
        /// 培训机构名称
        /// </summary>
		[Column("培训机构名称")]
        public string SchoolName { get; set; }

		/// <summary>
        /// 联系电话
        /// </summary>
		[Column("联系电话")]
        public string Phone { get; set; }

		/// <summary>
        /// 创建时间
        /// </summary>
		[Column("创建时间")]
        public DateTime CreateDateTime { get; set; }

        /// <summary>
        /// 复制对象
        /// </summary>
        public Teacher Clone()
        {
            return (Teacher)MemberwiseClone();
        }
    }
}