// ===============================================================================
// DotNet.Platform 开发框架 2016 版权所有
// ===============================================================================

using System;
using DotNet.Auth.Service;
using DotNet.Edu.Utility;
using DotNet.Entity;
using DotNet.Helper;

namespace DotNet.Edu.Entity
{
    /// <summary>
    /// 学员课件学时
    /// </summary>
    [Table("学员课件学时")]
    public class StudentCoursewarePeriod
    {
        /// <summary>
        /// 学员主键
        /// </summary>
        [Column("学员主键")]
        public string StudentId { get; set; }

        /// <summary>
        /// 课件主键
        /// </summary>
        [Column("课件主键")]
        public string CoursewareId { get; set; }

        /// <summary>
        /// 学时
        /// </summary>
        [Column("学时")]
        public int Period { get; set; }

        /// <summary>
        /// 复制对象
        /// </summary>
        public StudentCoursewarePeriod Clone()
        {
            return (StudentCoursewarePeriod)MemberwiseClone();
        }
    }

    public class StudentCoursewarePeriodView
    {
        /// <summary>
        /// 课件主键
        /// </summary>
        public string CoursewareId { get; set; }

        /// <summary>
        /// 课件名称
        /// </summary>
        public string CoursewareName { get; set; }

        /// <summary>
        /// 课件学时
        /// </summary>
        public int CoursewarePeriod { get; set; }

        /// <summary>
        /// 课件学时字符串
        /// </summary>
        public string CoursewarePeriodName
        {
            get { return DateTimeHelper.GetTimeStringHMS(CoursewarePeriod); }
        }

        /// <summary>
        /// 从业类型
        /// </summary>
        public string WorkType { get; set; }

        /// <summary>
        /// 从业类型名称
        /// </summary>
        public string WorkTypeName => AuthService.DicDetail.GetNameByValue(EduDicConst.WorkType, WorkType);

        /// <summary>
        /// 课件类型 1.图片 2.视频
        /// </summary>
        public string CourseType { get; set; }

        /// <summary>
        /// 课件类型名称 1.图片 2.视频
        /// </summary>
        public string CourseTypeName => AuthService.DicDetail.GetNameByValue(EduDicConst.CourseType, CourseType);

        /// <summary>
        /// 已学学时
        /// </summary>
        public int LearnPeriod { get; set; }

        /// <summary>
        /// 已学学时字符串
        /// </summary>
        public string LearnPeriodName
        {
            get { return DateTimeHelper.GetTimeStringHMS(LearnPeriod); }
        }
    }
}