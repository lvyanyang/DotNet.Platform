// ===============================================================================
// DotNet.Platform 开发框架 2016 版权所有
// ===============================================================================

using System.Web.Mvc;
using DotNet.Auth.Entity;
using DotNet.Auth.Utility;
using DotNet.Extensions;
using DotNet.Helper;
using DotNet.Mvc;
using DotNet.Utility;

namespace DotNet.EduWeb.Controllers
{
    [Authorize]
    public class AuthController : JsonController
    {
        /// <summary>
        /// 当前登录用户主键
        /// </summary>
        public static string CurrentUserId
        {
            get { return CurrentUser.Id; }
        }

        /// <summary>
        /// 当前登录用户账号
        /// </summary>
        public static string CurrentUserAccount
        {
            get { return CurrentUser.Account; }
        }

        /// <summary>
        /// 当前登录用户姓名
        /// </summary>
        public static string CurrentUserName
        {
            get { return CurrentUser.Name; }
        }

        /// <summary>
        /// 当前登录用户部门主键
        /// </summary>
        public static string CurrentDepartmentId
        {
            get { return CurrentDepartment.Id; }
        }

        /// <summary>
        /// 当前登录用户部门名称
        /// </summary>
        public static string CurrentDepartmentName
        {
            get { return CurrentDepartment.Name; }
        }

        /// <summary>
        /// 当前登录会话用户对象
        /// </summary>
        public static SessionUser CurrentSessionUser
        {
            get { return AuthHelper.GetSessionUser(); }
        }

        /// <summary>
        /// 当前登录用户对象
        /// </summary>
        public static User CurrentUser
        {
            get { return CurrentSessionUser.User; }
        }

        /// <summary>
        /// 当前登录用户部门对象
        /// </summary>
        public static Department CurrentDepartment
        {
            get { return CurrentSessionUser.Department; }
        }

        /// <summary>
        /// 获取分页条件
        /// </summary>
        public PaginationCondition PageInfo()
        {
            var pageIndex = WebHelper.GetFormString("page").ToInt(1);
            var pageSize = SystemSetting.GridPageSize;
            var orderName = WebHelper.GetFormString("order");
            var orderDir = WebHelper.GetFormString("dir");
            return new PaginationCondition(pageIndex, pageSize, orderName, orderDir);
        }
    }
}