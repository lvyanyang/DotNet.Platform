// ===============================================================================
// DotNet.Platform 开发框架 2016 版权所有
// ===============================================================================

using System;
using DotNet.Entity;

namespace DotNet.Edu.Entity
{
    /// <summary>
    /// 学时明细
    /// </summary>
    [Table("学时明细")]
    public class PeriodDetails
    {
        /// <summary>
        /// 主键
        /// </summary>
        [PrimaryKey]
        [Column("主键", false)]
        public string Id { get; set; }

        /// <summary>
        /// 课件主键
        /// </summary>
        [Column("课件主键")]
        public string CoursewareId { get; set; }

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
        /// 学员主键
        /// </summary>
        [Column("学员主键")]
        public string StudentId { get; set; }

        /// <summary>
        /// 学员姓名
        /// </summary>
        [Column("学员姓名")]
        public string StudentName { get; set; }

        /// <summary>
        /// 学习方式
        /// </summary>
        [Column("学习方式")]
        public string StudyCategory { get; set; }

        /// <summary>
        /// IP地址
        /// </summary>
        [Column("IP地址")]
        public string IPAddress { get; set; }

        /// <summary>
        /// 签到时间
        /// </summary>
        [Column("签到时间")]
        public DateTime SignInDateTime { get; set; }

        /// <summary>
        /// 签退时间
        /// </summary>
        [Column("签退时间")]
        public DateTime SignOutDateTime { get; set; }

        /// <summary>
        /// 时长
        /// </summary>
        [Column("时长")]
        public int Period { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        [Column("创建时间")]
        public DateTime CreateDateTime { get; set; }

        /// <summary>
        /// 复制对象
        /// </summary>
        public PeriodDetails Clone()
        {
            return (PeriodDetails)MemberwiseClone();
        }
    }
}