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
        /// <summary>
        /// Cdn服务器
        /// </summary>
        public string CdnServer { get; set; }

        /// <summary>
        /// Cdn是否调试
        /// </summary>
        public bool CdnDebug { get; set; }

        /// <summary>
        /// Cdn版本
        /// </summary>
        public string CdnVersion { get; set; }

        /// <summary>
        /// Local是否调试
        /// </summary>
        public bool LocalDebug { get; set; } = true;

        /// <summary>
        /// Local版本
        /// </summary>
        public string LocalVersion { get; set; }
    }
}
