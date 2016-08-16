// ===============================================================================
// DotNet.Platform 开发框架 2016 版权所有
// ===============================================================================

using System.Web;

namespace DotNet.Utility
{
    /// <summary>
    /// 上传文件设置
    /// </summary>
    public class UploadSetting
    {
        /// <summary>
        /// 上传文件目录(默认为~/upload)
        /// </summary>
        public string UploadFolder { get; set; } = "~/upload";

        /// <summary>
        /// 是否使用绝对路径
        /// </summary>
        public bool IsAbsolute { get; set; }

        /// <summary>
        /// 上传绝对文件目录
        /// </summary>
        public string AbsoluteFolder { get; set; }

        /// <summary>
        /// 上传站点（默认为本站）
        /// </summary>
        public string UploadServer { get; set; }
    }
}