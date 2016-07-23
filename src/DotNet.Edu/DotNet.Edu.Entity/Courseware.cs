// ===============================================================================
// DotNet.Platform 开发框架 2016 版权所有
// ===============================================================================

using System;
using DotNet.Auth.Service;
using DotNet.Entity;
using DotNet.Edu.Utility;
using DotNet.Helper;

namespace DotNet.Edu.Entity
{
    /// <summary>
    /// 课件
    /// </summary>
    [Table("课件")]
    public class Courseware
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
        /// 简拼
        /// </summary>
        [Column("简拼")]
        public string Spell { get; set; }

        /// <summary>
        /// 学时
        /// </summary>
        [Column("学时")]
        public int Period { get; set; }

        /// <summary>
        /// 学时(分钟)
        /// </summary>
        [Ignore]
        public int PeriodMinute { get; set; } = 1;

        /// <summary>
        /// 学时字符串
        /// </summary>
        [Ignore]
        public string PeriodName
        {
            get { return DateTimeHelper.GetTimeStringHMS(Period); }
        }

        /// <summary>
        /// 从业类型
        /// </summary>
        [Column("从业类型")]
        public int WorkType { get; set; }

        /// <summary>
        /// 从业类型名称
        /// </summary>
        [Ignore]
        public string WorkTypeName => AuthService.DicDetail.GetNameByValue(EduDicConst.WorkType, WorkType);

        /// <summary>
        /// 课件类型 1.图片 2.视频
        /// </summary>
        [Column("课件类型")]
        public int CourseType { get; set; }

        /// <summary>
        /// 课件类型名称 1.图片 2.视频
        /// </summary>
		[Ignore]
        public string CourseTypeName => AuthService.DicDetail.GetNameByValue(EduDicConst.CourseType, CourseType);

        /// <summary>
        /// 行号
        /// </summary>
        [Column("行号")]
        public int RowIndex { get; set; }

        /// <summary>
        /// 复制对象
        /// </summary>
        public Courseware Clone()
        {
            return (Courseware)MemberwiseClone();
        }
    }
}