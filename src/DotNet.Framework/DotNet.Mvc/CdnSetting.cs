// ===============================================================================
// DotNet.Platform 开发框架 2016 版权所有
// ===============================================================================
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using DotNet.Configuration;

namespace DotNet.Mvc
{
    /// <summary>
    /// cdn配置信息
    /// </summary>
    public class CdnSetting
    {
        private static CdnSetting _instance;

        /// <summary>
        /// 实例对象
        /// </summary>
        public static CdnSetting Instance
        {
            get { return _instance ?? (_instance = ConfigManager.GetSetting("CdnSetting", () => new CdnSetting())); }
        }

        /// <summary>
        /// 服务器
        /// </summary>
        public string Server { get; set; }

        /// <summary>
        /// 是否调试
        /// </summary>
        public bool Debug { get; set; }

        /// <summary>
        /// 版本
        /// </summary>
        public string Version { get; set; }
    }
}
