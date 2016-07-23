// ===============================================================================
// DotNet.Platform 开发框架 2016 版权所有
// ===============================================================================
using System;
using System.IO;
using System.Web;
using System.Web.Mvc;

namespace DotNet.Mvc
{
    /// <summary>
    /// Cdn操作帮助类
    /// </summary>
    public static class CdnHelper
    {
        /// <summary>
        /// 返回Cdn路径
        /// </summary>
        /// <param name="filePath">文件路径</param>
        /// <returns></returns>
        public static string Url(string filePath)
        {
            var setting = CdnSetting.Instance;
            string extName = Path.GetExtension(filePath);
            string name = Path.GetFileNameWithoutExtension(filePath);
            var debugString = setting.Debug ? string.Empty : ".min";
            var path = string.Empty;
            var pathIndex = filePath.LastIndexOf("/", StringComparison.Ordinal);
            if (pathIndex > -1)
            {
                path = filePath.Substring(0, pathIndex + 1);
            }

            return $"{setting.Server}/{path}{name}{debugString}{extName}?v={setting.Version}";
        }

        /// <summary>
        /// 生成脚本标签
        /// </summary>
        /// <param name="filePath">文件路径</param>
        /// <returns></returns>
        public static HtmlString Script(string filePath)
        {
            return MvcHtmlString.Create($"<script src=\"{Url(filePath)}\"></script>");
        }

        /// <summary>
        /// 生成样式标签
        /// </summary>
        /// <param name="filePath">文件路径</param>
        /// <returns></returns>
        public static HtmlString Css(string filePath)
        {
            return MvcHtmlString.Create($"<link href=\"{Url(filePath)}\" rel=\"stylesheet\" />");
        }

        /// <summary>
        /// 生成JQuery脚本引用标签
        /// </summary>
        /// <returns></returns>
        public static HtmlString JQueryScript()
        {
            return MvcHtmlString.Create($"<script src=\"{Url("jquery.js")}\"></script>");
        }

        /// <summary>
        /// 生成Lib脚本引用标签
        /// </summary>
        /// <returns></returns>
        public static HtmlString LibScript()
        {
            return MvcHtmlString.Create($"<script src=\"{Url("lib.js")}\"></script>");
        }

        /// <summary>
        /// 生成Lib样式引用标签
        /// </summary>
        /// <returns></returns>
        public static HtmlString LibCss()
        {
            return MvcHtmlString.Create($"<link href=\"{Url("lib.css")}\" rel=\"stylesheet\" />");
        }
    }
}