// ===============================================================================
// DotNet.Platform 开发框架 2016 版权所有
// ===============================================================================
using DotNet.Data;

namespace DotNet.Edu.Service
{
    public class EduDatabase : Database
    {
        public EduDatabase() : base("dotnet.edu") { }
    }

    public class EduRepository<T> : Repository<T> where T : class, new()
    {
        public EduRepository() : base("dotnet.edu") { }
    }

    /// <summary>
    /// 在线教育系统服务
    /// </summary>
    public static class EduService
    {
        /// <summary>
        /// 培训机构服务
        /// </summary>
        public static SchoolService School { get; } = new SchoolService();

        /// <summary>
        /// 企业信息服务
        /// </summary>
        public static CompanyService Company { get; } = new CompanyService();

        /// <summary>
        /// 班级信息服务
        /// </summary>
        public static TrainGroupService TrainGroup { get; } = new TrainGroupService();

        /// <summary>
        /// 学员信息服务
        /// </summary>
        public static StudentService Student { get; } = new StudentService();

        /// <summary>
        /// 学员日志服务
        /// </summary>
        public static StudentLogService StudentLog { get; } = new StudentLogService();

        /// <summary>
        /// 考试学员服务
        /// </summary>
        public static ExamUserService ExamUser { get; } = new ExamUserService();

        /// <summary>
        /// 课件服务
        /// </summary>
        public static CoursewareService Courseware { get; } = new CoursewareService();

        /// <summary>
        /// 课件明细服务
        /// </summary>
        public static CoursewareDetailsService CoursewareDetails { get; } = new CoursewareDetailsService();

        /// <summary>
        /// 题库服务
        /// </summary>
        public static QuestionService Question { get; } = new QuestionService();

        /// <summary>
        /// 学员登录日志服务
        /// </summary>
        public static StudentAuditsService StudentAudits { get; } = new StudentAuditsService();

        ///// <summary>
        ///// 学时明细服务
        ///// </summary>
        //public static PeriodDetailsService PeriodDetails { get; } = new PeriodDetailsService();
    }
}