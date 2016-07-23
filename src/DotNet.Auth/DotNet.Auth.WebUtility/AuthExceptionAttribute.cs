// ===============================================================================
// DotNet.Platform 开发框架 2016 版权所有
// ===============================================================================

using DotNet.Mvc;
using System.Web.Mvc;
using DotNet.Auth.Service;
using DotNet.Auth.Utility;

namespace DotNet.Auth.WebUtility
{
    public class AuthExceptionAttribute : JsonExceptionAttribute
    {
        /// <summary>
        /// 在发生异常时调用。
        /// </summary>
        /// <param name="filterContext">筛选器上下文。</param>
        public override void OnException(ExceptionContext filterContext)
        {
            //记录异常信息
            var entity = AuthHelper.BuildExcep(filterContext.Exception);
            if (filterContext.HttpContext.Request.Url != null)
            {
                entity.MessageContent += $"<br> Url:{filterContext.HttpContext.Request.Url.PathAndQuery}";
            }
            entity.IPAddress = filterContext.HttpContext.Request.UserHostAddress;
            AuthService.Excep.Create(entity);

            base.OnException(filterContext);
        }

    }
}