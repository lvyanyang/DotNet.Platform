// ===============================================================================
// DotNet.Platform 开发框架 2016 版权所有
// ===============================================================================
using System;
using System.IO;
using System.Web;
using System.Web.Mvc;
using DotNet.Configuration;
using DotNet.Helper;

namespace DotNet.Mvc
{
    /// <summary>
    /// Cdn操作帮助类
    /// </summary>
    public static class CdnHelper
    {
        /// <summary>
        /// 获取本地服务Url
        /// </summary>
        private static string GetLocalServer()
        {
            var url = HttpContext.Current.Request.Url;
            return $"{url.Scheme}://{url.Host}:{url.Port}";
        }

        /// <summary>
        /// 获取配置
        /// </summary>
        private static CdnSetting GetUploadSetting()
        {
            var setting = ConfigManager.GetSetting("Cdn", () => new CdnSetting());
            if (string.IsNullOrEmpty(setting.CdnServer))
            {
                setting.CdnServer = GetLocalServer();
            }
            return setting;
        }

        /// <summary>
        /// 返回资源路径
        /// </summary>
        /// <param name="virtualPath">虚拟路径</param>
        /// <param name="isCdn">是否Cdn</param>
        private static string Url(string virtualPath, bool isCdn)
        {
            CdnSetting setting = GetUploadSetting();
            virtualPath = VirtualPathUtility.ToAbsolute(virtualPath);
            string extName = VirtualPathUtility.GetExtension(virtualPath);
            string name = Path.GetFileNameWithoutExtension(virtualPath);
            string debugString = isCdn
                ? (setting.CdnDebug ? string.Empty : ".min")
                : (setting.LocalDebug ? string.Empty : ".min");
            string fileName = $"{name}{debugString}{extName}";
            string dir = VirtualPathUtility.GetDirectory(virtualPath);
            dir = VirtualPathUtility.AppendTrailingSlash(dir);
            string version = isCdn ? setting.CdnVersion : setting.LocalVersion;
            if (string.IsNullOrEmpty(version))
            {
                version = StringHelper.Guid();
            }
            string server = isCdn ? setting.CdnServer : GetLocalServer();
            if (!string.IsNullOrEmpty(server))
            {
                server = VirtualPathUtility.RemoveTrailingSlash(server);
            }
            return $"{server}{dir}{fileName}?v={version}";
        }

        /// <summary>
        /// 返回Cdn资源路径
        /// </summary>
        /// <param name="virtualPath">虚拟路径</param>
        /// <returns></returns>
        public static string CdnUrl(string virtualPath)
        {
            return Url(virtualPath, true);
        }

        /// <summary>
        /// 返回Local资源路径
        /// </summary>
        /// <param name="virtualPath">虚拟路径</param>
        /// <returns></returns>
        public static string LocalUrl(string virtualPath)
        {
            return Url(virtualPath, false);
        }

        /// <summary>
        /// 生成Cdn脚本标签
        /// </summary>
        /// <param name="virtualPath">虚拟路径</param>
        /// <returns></returns>
        public static MvcHtmlString CdnJavaScript(string virtualPath)
        {
            return MvcHtmlString.Create($"<script src=\"{CdnUrl(virtualPath)}\"></script>");
        }

        /// <summary>
        /// 生成Local脚本标签
        /// </summary>
        /// <param name="virtualPath">虚拟路径</param>
        /// <returns></returns>
        public static MvcHtmlString LocalJavaScript(string virtualPath)
        {
            return MvcHtmlString.Create($"<script src=\"{LocalUrl(virtualPath)}\"></script>");
        }

        /// <summary>
        /// 生成Cdn样式标签
        /// </summary>
        /// <param name="virtualPath">虚拟路径</param>
        /// <returns></returns>
        public static MvcHtmlString CdnCss(string virtualPath)
        {
            return MvcHtmlString.Create($"<link href=\"{CdnUrl(virtualPath)}\" rel=\"stylesheet\" />");
        }

        /// <summary>
        /// 生成Local样式标签
        /// </summary>
        /// <param name="virtualPath">虚拟路径</param>
        /// <returns></returns>
        public static MvcHtmlString LocalCss(string virtualPath)
        {
            return MvcHtmlString.Create($"<link href=\"{LocalUrl(virtualPath)}\" rel=\"stylesheet\" />");
        }

        /// <summary>
        /// 根据后缀名自动判断是css还是js的Cdn资源
        /// </summary>
        /// <param name="virtualPath">虚拟路径</param>
        public static MvcHtmlString CdnImport(string virtualPath)
        {
            string extName = VirtualPathUtility.GetExtension(virtualPath);
            if (string.IsNullOrEmpty(extName))
            {
                throw new ArgumentException("无效文件后缀名");
            }
            return extName.Equals(".css", StringComparison.OrdinalIgnoreCase) ? CdnCss(virtualPath) : CdnJavaScript(virtualPath);
        }

        /// <summary>
        /// 根据后缀名自动判断是css还是js的Local资源
        /// </summary>
        /// <param name="virtualPath">虚拟路径</param>
        public static MvcHtmlString LocalImport(string virtualPath)
        {
            string extName = VirtualPathUtility.GetExtension(virtualPath);
            if (string.IsNullOrEmpty(extName))
            {
                throw new ArgumentException("无效文件后缀名");
            }
            return extName.Equals(".css", StringComparison.OrdinalIgnoreCase) ? LocalCss(virtualPath) : LocalJavaScript(virtualPath);
        }

    }
}