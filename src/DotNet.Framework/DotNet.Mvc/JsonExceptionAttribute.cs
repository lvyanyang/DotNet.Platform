// ===============================================================================
// DotNet.Platform 开发框架 2016 版权所有
// ===============================================================================
using System;
using System.Web.Mvc;

namespace DotNet.Mvc
{
    /// <summary>
    /// 全局Json错误异常捕获
    /// </summary>
    public class JsonExceptionAttribute : HandleErrorAttribute
    {
        /// <summary>
        /// 在发生异常时调用。
        /// </summary>
        /// <param name="filterContext">筛选器上下文。</param>
        public override void OnException(ExceptionContext filterContext)
        {
            var requestedWith = filterContext.HttpContext.Request.Headers["X-Requested-With"];
            //var requestType = filterContext.HttpContext.Request.RequestType;
            if (!string.IsNullOrEmpty(requestedWith) && requestedWith.Equals("XMLHttpRequest"))
            {
                var httpException = filterContext.Exception;
                if (httpException != null)
                {
                    filterContext.Result = new JsonNetResult(new JsonMessage(false, httpException.Message.Replace("\r", "").Replace("\n", ""))); // new BaseController().InternalError(httpException.Message);
                    filterContext.ExceptionHandled = true;
                    filterContext.HttpContext.Response.Clear();
                    filterContext.HttpContext.Response.StatusCode = 500;
                    filterContext.HttpContext.Response.TrySkipIisCustomErrors = true;
                    filterContext.HttpContext.Response.ContentType = "application/json";
                }
            }
            else
            {
                base.OnException(filterContext);
            }
        }
    }
}
