// ===============================================================================
// DotNet.Platform 开发框架 2016 版权所有
// ===============================================================================
using System;
using System.IO;
using System.Web;

namespace DotNet.Configuration
{
    /// <summary>
    /// 系统目录
    /// </summary>
    public static class SystemDirectory
    {
        private static string _extensionDirectory = Path.Combine(RootDirectory, "Extension");
        private static string _libDirectory = Path.Combine(RootDirectory, "Lib");
        private static string _dataDirectory = IsWebApp ? Path.Combine(RootDirectory,"App_Data") : Path.Combine(RootDirectory, "Data");
        private static string _logDirectory = Path.Combine(RootDirectory, "Log");
        private static string _tempDirectory = Path.Combine(RootDirectory, "Temp");
        private static string _backupDirectory = Path.Combine(RootDirectory, "Backup");
        private static string _configDirectory = Path.Combine(RootDirectory, "Config");
        private static string _pluginDirectory = Path.Combine(RootDirectory, "Plugin");
        private static string _reportDirectory = Path.Combine(RootDirectory, "Report");

        #region 目录

        /// <summary>
        /// 项目根目录,对于Web项目是网站根目录,对于Win项目是启动exe所在目录
        /// </summary>
        public static string RootDirectory
        {
            get { return AppDomain.CurrentDomain.BaseDirectory; }
        }

        /// <summary>
        /// 类库输出目录,对于Web项目是网站bin目录,对于Win项目是启动exe所在目录
        /// </summary>
        public static string OutDirectory
        {
            get
            {
                return IsWebApp ?
                    Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "bin") : AppDomain.CurrentDomain.BaseDirectory;
            }
        }

        /// <summary>
        /// 是否是Windows应用程序
        /// </summary>
        public static bool IsWinApp
        {
            get { return HttpContext.Current == null; }
        }

        /// <summary>
        /// 是否是Web应用程序
        /// </summary>
        public static bool IsWebApp
        {
            get { return HttpContext.Current != null; }
        }

        /// <summary>
        /// 扩展目录
        /// </summary>
        public static string ExtensionDirectory
        {
            get { return _extensionDirectory; }
            set { _extensionDirectory = value; }
        }

        /// <summary>
        /// 类库目录
        /// </summary>
        public static string LibDirectory
        {
            get { return _libDirectory; }
            set { _libDirectory = value; }
        }

        /// <summary>
        /// 数据目录
        /// </summary>
        public static string DataDirectory
        {
            get { return _dataDirectory; }
            set { _dataDirectory = value; }
        }

        /// <summary>
        /// 日志目录
        /// </summary>
        public static string LogDirectory
        {
            get { return _logDirectory; }
            set { _logDirectory = value; }
        }

        /// <summary>
        /// 临时目录
        /// </summary>
        public static string TempDirectory
        {
            get { return _tempDirectory; }
            set { _tempDirectory = value; }
        }

        /// <summary>
        /// 备份目录
        /// </summary>
        public static string BackupDirectory
        {
            get { return _backupDirectory; }
            set { _backupDirectory = value; }
        }

        /// <summary>
        /// 配置目录
        /// </summary>
        public static string ConfigDirectory
        {
            get { return _configDirectory; }
            set { _configDirectory = value; }
        }

        /// <summary>
        /// 插件目录
        /// </summary>
        public static string PluginDirectory
        {
            get { return _pluginDirectory; }
            set { _pluginDirectory = value; }
        }

        /// <summary>
        /// 报表目录
        /// </summary>
        public static string ReportDirectory
        {
            get { return _reportDirectory; }
            set { _reportDirectory = value; }
        }

        #endregion
    }
}