// ===============================================================================
// DotNet.Platform 开发框架 2016 版权所有
// ===============================================================================
using DotNet.Data;

namespace DotNet.Auth.Service
{
    public class AuthDatabase : Database
    {
        public AuthDatabase() : base("dotnet.auth") { }
    }

    public class AuthRepository<T> : Repository<T> where T : class, new()
    {
        public AuthRepository() : base("dotnet.auth") { }
    }

    /// <summary>
    /// 权限系统服务
    /// </summary>
    public static class AuthService
    {
        /// <summary>
        /// 系统角色服务
        /// </summary>
        public static RoleService Role { get; } = new RoleService();

        /// <summary>
        /// 系统日志服务
        /// </summary>
        public static LogService Log { get; } = new LogService();

        /// <summary>
        /// 系统序列服务
        /// </summary>
        public static SeqService Seq { get; } = new SeqService();

        /// <summary>
        /// 系统部门服务
        /// </summary>
        public static DepartmentService Department { get; } = new DepartmentService();

        /// <summary>
        /// 系统用户服务
        /// </summary>
        public static UserService User { get; } = new UserService();

        /// <summary>
        /// 系统字典服务
        /// </summary>
        public static DicService Dic { get; } = new DicService();

        /// <summary>
        /// 系统字典明细服务
        /// </summary>
        public static DicDetailService DicDetail { get; } = new DicDetailService();

        /// <summary>
        /// 系统审计日志服务
        /// </summary>
        public static AuditService Audit { get; } = new AuditService();

        /// <summary>
        /// 系统异常服务
        /// </summary>
        public static ExcepService Excep { get; } = new ExcepService();

        /// <summary>
        /// 系统菜单服务
        /// </summary>
        public static MenuService Menu { get; } = new MenuService();

        /// <summary>
        /// 系统消息服务
        /// </summary>
        public static MessageService Message { get; } = new MessageService();

        /// <summary>
        /// 系统参数服务
        /// </summary>
        public static ParamService Param { get; } = new ParamService();

        /// <summary>
        /// 系统数据版本服务
        /// </summary>
        public static VersionService Version { get; } = new VersionService();
    }
}