// ===============================================================================
// DotNet.Platform 开发框架 2016 版权所有
// ===============================================================================

using System;
using DotNet.Entity;

namespace DotNet.Edu.Entity
{
    /// <summary>
    /// 学习日志
    /// </summary>
    [Table("学习日志")]
    public class LessonLog
    {
        /// <summary>
        /// 主键
        /// </summary>
        [PrimaryKey]
        [Column("主键", false)]
        public string Id { get; set; }

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
        /// 课件名称
        /// </summary>
        [Column("课件名称")]
        public string CoursewareName { get; set; }

        /// <summary>
        /// 从业类型
        /// </summary>
        [Column("从业类型")]
        public string WorkType { get; set; }

        /// <summary>
        /// 从业类型名称
        /// </summary>
        [Column("从业类型名称")]
        public string WorkTypeName { get; set; }

        /// <summary>
        /// 操作时间
        /// </summary>
        [Column("操作时间")]
        public DateTime CreateDateTime { get; set; }

        /// <summary>
        /// 考勤类型 1 签到 2 签退 3 随机验证
        /// </summary>
        [Column("考勤类型")]
        public int RecordType { get; set; }

        /// <summary>
        /// 考勤类型名称
        /// </summary>
        [Ignore]
        public string RecordTypeName
        {
            get
            {
                switch (RecordType)
                {
                    case 1:
                        return "签到";
                    case 2:
                        return "签退";
                    case 3:
                        return "随机验证";
                    default:
                        return "未知";
                }
            }
        }

        /// <summary>
        /// 验证结果
        /// </summary>
        [Column("验证结果")]
        public bool Result { get; set; }

        /// <summary>
        /// 验证结果名称
        /// </summary>
        [Ignore]
        public string ResultName
        {
            get { return Result ? "已通过" : "未通过"; }
        }

        /// <summary>
        /// 复制对象
        /// </summary>
        public LessonLog Clone()
        {
            return (LessonLog)MemberwiseClone();
        }
    }
}