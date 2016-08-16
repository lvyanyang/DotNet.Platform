using DotNet.Auth.Utility;
using DotNet.Edu.Entity;
using DotNet.Edu.WebUtility;
using DotNet.Extensions;
using DotNet.Helper;
using DotNet.Mvc;
using DotNet.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DotNet.Web.Areas.Students.Controllers
{
    [Authorize]
    public class StudentWebController : JsonController
    {
        /// <summary>
        /// 当前登录学员对象
        /// </summary>
        public static StudentSession CurrentStudent
        {
            get
            {
                var session = EduWebHelper.GetStudentSession();
                return session;
            }
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