// ===============================================================================
// DotNet.Platform 开发框架 2016 版权所有
// ===============================================================================

using System;
using DotNet.Entity;

namespace DotNet.Edu.Entity
{
    /// <summary>
    /// 课件明细
    /// </summary>
    [Table("课件明细")]    
    public class CoursewareDetails
    {
		/// <summary>
        /// 主键
        /// </summary>
        [PrimaryKey]
        [Column("主键",false)]
        public string Id { get; set; }

		/// <summary>
        /// 课件主键
        /// </summary>
		[Column("课件主键")]
        public string CourseId { get; set; }

		/// <summary>
        /// 课件Url
        /// </summary>
		[Column("课件Url")]
        public string Url { get; set; }

		/// <summary>
        /// 行号
        /// </summary>
		[Column("行号")]
        public int RowIndex { get; set; }

        /// <summary>
        /// 复制对象
        /// </summary>
        public CoursewareDetails Clone()
        {
            return (CoursewareDetails)MemberwiseClone();
        }
    }
}