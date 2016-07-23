// ===============================================================================
// DotNet.Platform 开发框架 2016 版权所有
// ===============================================================================

using System;
using System.Web.Mvc;
using DotNet.Auth.Service;
using DotNet.Extensions;

namespace DotNet.Auth.Controllers
{
    public class ExcepController : AuthController
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Grid(string startDate, string endDate, string ip, string title)
        {
            var list = AuthService.Excep.GetPageList(PageInfo(), startDate.ToDateTimeOrNull(),
                endDate.ToDateTimeOrNull(), ip, title);
            return View(list);
        }
        
        public ActionResult Details(string id)
        {
            var entity = AuthService.Excep.Get(id);
            if (entity == null) return NotFound(id);
            return View(entity);
        }
        
        private ActionResult NotFound(string id)
        {
            return NotFound("获取 系统异常信息 错误", $"无法找到 主键 = {id} 的系统异常信息");
        }
    }
}
