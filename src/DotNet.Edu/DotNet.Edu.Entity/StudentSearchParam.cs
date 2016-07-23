// ===============================================================================
// DotNet.Platform 开发框架 2016 版权所有
// ===============================================================================

using System;

namespace DotNet.Edu.Entity
{
    /// <summary>
    /// 学员查询参数
    /// </summary>
    public class StudentSearchParam
    {
        /// <summary>
        /// 姓名
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 身份证号码
        /// </summary>
        public string IDCardNo { get; set; }

        /// <summary>
        /// 工作单位
        /// </summary>
        public string CompanyId { get; set; }

        /// <summary>
        /// 报名开始日期
        /// </summary>
        public DateTime? RegStartDate { get; set; }

        /// <summary>
        /// 报名结束日期
        /// </summary>
        public DateTime? RegEndDate { get; set; }

        /// <summary>
        /// 所属班级
        /// </summary>
        public string TrainGroupId { get; set; }

        /// <summary>
        /// 报名开始日期
        /// </summary>
        public DateTime? CreateStartDate { get; set; }

        /// <summary>
        /// 报名结束日期
        /// </summary>
        public DateTime? CreateEndDate { get; set; }

        /// <summary>
        /// 学校
        /// </summary>
        public string SchoolId { get; set; }

        /// <summary>
        /// 从业类型
        /// </summary>
        public string WorkCategoryId { get; set; }

        /// <summary>
        /// 状态 0 预报名 1 报名中 2 学习中 3 准备考试 4 考试中 5 完成
        /// </summary>
        public int? Status { get; set; }
    }
}