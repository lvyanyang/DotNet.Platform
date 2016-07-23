// ===============================================================================
// DotNet.Platform 开发框架 2016 版权所有
// ===============================================================================

using DotNet.Auth.Service;
using DotNet.Extensions;

namespace DotNet.Auth.Utility
{
    /// <summary>
    /// 系统配置参数
    /// </summary>
    public static class SystemSetting
    {
        /// <summary>
        /// 系统标题
        /// </summary>
        public static string SystemTitle => AuthService.Param.Get("SystemTitle", "应用程序开发框架");

        /// <summary>
        /// 系统版权
        /// </summary>
        public static string Copyright => AuthService.Param.Get("Copyright", "DotNet开发框架");

        /// <summary>
        /// 系统版本
        /// </summary>
        public static string Version => AuthService.Param.Get("Version", "开发版");

        /// <summary>
        /// 表格每页记录数
        /// </summary>
        public static int GridPageSize => AuthService.Param.Get("GridPageSize", "10").ToInt();

    }
}