// ===============================================================================
// DotNet.Platform 开发框架 2016 版权所有
// ===============================================================================
using System;
using System.Web;

namespace DotNet.Helper
{
    /// <summary>
    /// Cookie操作类
    /// </summary>
    public static class CookieHelper
    {
        #region 创建Cookie

        /// <summary>
        /// 创建永久Cookie
        /// </summary>
        /// <param name="cookieName">Cookie名称</param>
        /// <param name="cookieValue">Cookie值</param>
        public static void CreateCookie(string cookieName, string cookieValue)
        {
            CreateCookie(cookieName, cookieValue, DateTime.MaxValue, null, null);
        }

        /// <summary>
        /// 创建Cookie值
        /// </summary>
        /// <param name="cookieName">Cookie名称</param>
        /// <param name="cookieValue">Cookie值</param>
        /// <param name="cookieTime">Cookie有效时间</param>
        /// <param name="domain">域名</param>
        /// <param name="path">路径</param>
        public static void CreateCookie(string cookieName, string cookieValue, DateTime? cookieTime, string domain,
                                        string path)
        {
            if (GetCookie(cookieName) != null)
            {
                RemoveCookie(cookieName);
            }
            if (cookieValue != null)
            {
                var cookie = new HttpCookie(cookieName) {Value = cookieValue};
                if (cookieTime != null) cookie.Expires = cookieTime.Value;
                if (domain != null) cookie.Domain = domain;
                if (path != null) cookie.Path = path;
                HttpContext.Current.Response.Cookies.Add(cookie);
            }
        }

        #endregion

        #region 获取Cookie

        /// <summary>
        /// 获取Cookie
        /// </summary>
        /// <param name="cookieName">Cookie名称</param>
        /// <param name="domain">域名</param>
        /// <param name="path">路径</param>
        /// <returns>返回Cookie值,如果没有找到指定名称的Cookie则返回空字符串</returns>
        public static string GetCookie(string cookieName, string domain, string path)
        {
            string cookieValue = string.Empty;
            HttpCookie cookie = HttpContext.Current.Request.Cookies[cookieName];
            if (null != cookie)
            {
                if (domain != null) cookie.Domain = domain;
                if (path != null) cookie.Path = path;
                cookieValue = cookie.Value;
            }
            return cookieValue;
        }

        /// <summary>
        /// 获取Cookie
        /// </summary>
        /// <param name="cookieName">Cookie名称</param>
        /// <returns>返回Cookie值,如果没有找到指定名称的Cookie则返回空字符串</returns>
        public static string GetCookie(string cookieName)
        {
            return GetCookie(cookieName, string.Empty, string.Empty);
        }

        #endregion

        #region 删除Cookie

        /// <summary>
        /// 删除Cookie
        /// </summary>
        /// <param name="cookieName">Cookie名称</param>
        /// <param name="domain">域名</param>
        /// <param name="path">路径</param>
        /// <returns>删除成功返回true,如果没有找到指定名称的Cookie也返回false</returns>
        public static bool RemoveCookie(string cookieName, string domain, string path)
        {
            if (HttpContext.Current.Request.Cookies[cookieName] != null)
            {
                var myCookie = new HttpCookie(cookieName) {Expires = DateTime.Now.AddDays(-1)};
                if (domain != null) myCookie.Domain = domain;
                if (path != null) myCookie.Path = path;
                HttpContext.Current.Response.Cookies.Add(myCookie);
                return true;
            }
            return false;
        }

        /// <summary>
        /// 删除Cookie
        /// </summary>
        /// <param name="cookieName">Cookie名称</param>
        /// <returns>删除成功返回true,如果没有找到指定名称的Cookie也返回false</returns>
        public static bool RemoveCookie(string cookieName)
        {
            return RemoveCookie(cookieName, string.Empty, string.Empty);
        }

        #endregion
    }
}