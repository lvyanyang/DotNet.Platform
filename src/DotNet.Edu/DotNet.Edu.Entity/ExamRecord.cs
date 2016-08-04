// ===============================================================================
// DotNet.Platform 开发框架 2016 版权所有
// ===============================================================================

using System;
using DotNet.Entity;

namespace DotNet.Edu.Entity
{
    /// <summary>
    /// 考试记录
    /// </summary>
    [Table("考试记录")]    
    public class ExamRecord
    {
		/// <summary>
        /// 考试主键
        /// </summary>
        [PrimaryKey(true)]
        [Column("考试主键",false)]
        public int Id { get; set; }

		/// <summary>
        /// 试卷主键
        /// </summary>
		[Column("试卷主键")]
        public int PaperId { get; set; }

		/// <summary>
        /// 试卷名称
        /// </summary>
		[Column("试卷名称")]
        public string PaperName { get; set; }

		/// <summary>
        /// 用户主键
        /// </summary>
		[Column("用户主键")]
        public int UserId { get; set; }

		/// <summary>
        /// 用户姓名
        /// </summary>
		[Column("用户姓名")]
        public string UserName { get; set; }

		/// <summary>
        /// 姓名简拼
        /// </summary>
		[Column("姓名简拼")]
        public string UserNameSpell { get; set; }

		/// <summary>
        /// 考试类型(初考补考)
        /// </summary>
		[Column("考试类型(初考补考)")]
        public string ExamType { get; set; }

		/// <summary>
        /// 用户类型
        /// </summary>
		[Column("用户类型")]
        public string UserCategoryName { get; set; }

		/// <summary>
        /// 身份证号码
        /// </summary>
		[Column("身份证号码")]
        public string IdNumber { get; set; }

		/// <summary>
        /// 用户考试分数
        /// </summary>
		[Column("用户考试分数")]
        public int? UserScore { get; set; }

		/// <summary>
        /// 用户考试结果
        /// </summary>
		[Column("用户考试结果")]
        public int? UserResult { get; set; }

		/// <summary>
        /// 是否开始考试
        /// </summary>
		[Column("是否开始考试")]
        public int UserIsStart { get; set; }

		/// <summary>
        /// 开始考试时间
        /// </summary>
		[Column("开始考试时间")]
        public DateTime? UserStartDateTime { get; set; }

		/// <summary>
        /// 是否交卷
        /// </summary>
		[Column("是否交卷")]
        public int UserIsCommit { get; set; }

		/// <summary>
        /// 考试提交时间
        /// </summary>
		[Column("考试提交时间")]
        public DateTime? UserCommitDateTime { get; set; }

		/// <summary>
        /// 是否打印
        /// </summary>
		[Column("是否打印")]
        public int IsPrint { get; set; }

		/// <summary>
        /// 打印日期
        /// </summary>
		[Column("打印日期")]
        public DateTime? PrintDateTime { get; set; }

        /// <summary>
        /// 复制对象
        /// </summary>
        public ExamRecord Clone()
        {
            return (ExamRecord)MemberwiseClone();
        }
    }
}