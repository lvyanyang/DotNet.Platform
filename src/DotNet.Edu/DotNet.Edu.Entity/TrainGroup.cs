// ===============================================================================
// DotNet.Platform 开发框架 2016 版权所有
// ===============================================================================

using System;
using DotNet.Entity;

namespace DotNet.Edu.Entity
{
    /// <summary>
    /// 班级信息
    /// </summary>
    [Table("班级信息")]    
    public class TrainGroup
    {
		/// <summary>
        /// 主键
        /// </summary>
        [PrimaryKey]
        [Column("主键",false)]
        public string Id { get; set; }

		/// <summary>
        /// 名称
        /// </summary>
		[Column("名称")]
        public string Name { get; set; }

		/// <summary>
        /// 简拼
        /// </summary>
		[Column("简拼")]
        public string Spell { get; set; }

		/// <summary>
        /// 业务类型主键
        /// </summary>
		[Column("业务类型主键")]
        public string CategoryId { get; set; }

		/// <summary>
        /// 业务类型名称
        /// </summary>
		[Column("业务类型名称")]
        public string CategoryName { get; set; }

		/// <summary>
        /// 培训学校主键
        /// </summary>
		[Column("培训学校主键")]
        public string SchoolId { get; set; }

		/// <summary>
        /// 培训学校名称
        /// </summary>
		[Column("培训学校名称")]
        public string SchoolName { get; set; }

        /// <summary>
        /// 教室主键
        /// </summary>
		[Column("教室主键")]
        public string TeacherId { get; set; }

        /// <summary>
        /// 教室名称
        /// </summary>
        [Column("教室名称")]
        public string TeacherName { get; set; }

        /// <summary>
        /// 开始日期
        /// </summary>
        [Column("开始日期")]
        public DateTime? StartDate { get; set; }

		/// <summary>
        /// 结束日期
        /// </summary>
		[Column("结束日期")]
        public DateTime? EndDate { get; set; }

		/// <summary>
        /// 人数
        /// </summary>
		[Column("人数")]
        public int Num { get; set; }

        /// <summary>
        /// 状态 0 未开班 1 已开班
        /// </summary>
        [Column("状态")]
        public int Status { get; set; }

        /// <summary>
        /// 状态名称
        /// </summary>
        [Ignore]
        public string StatusName => Status == 0 ? "未开班" : "已开班";

        /// <summary>
        /// 创建时间
        /// </summary>
        [Column("创建时间")]
        public DateTime CreateDateTime { get; set; }

		/// <summary>
        /// 备注
        /// </summary>
		[Column("备注")]
        public string Note { get; set; }

        /// <summary>
        /// 复制对象
        /// </summary>
        public TrainGroup Clone()
        {
            return (TrainGroup)MemberwiseClone();
        }
    }
}