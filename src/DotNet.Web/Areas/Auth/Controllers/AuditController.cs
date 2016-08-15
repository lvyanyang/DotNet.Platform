// ===============================================================================
// DotNet.Platform 开发框架 2016 版权所有
// ===============================================================================

using System;
using System.Web.Mvc;
using DotNet.Auth.Service;
using DotNet.Extensions;

namespace DotNet.Auth.Controllers
{
    public class AuditController : AuthController
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Grid(string startDate, string endDate, string ip)
        {
            var list = AuthService.Audit.GetPageList(PageInfo(), startDate.ToDateTimeOrNull(),
                endDate.ToDateTimeOrNull(), ip, null);
            return View(list);
        }

        public ActionResult Details(string id)
        {
            var entity = AuthService.Audit.Get(id);
            if (entity == null) return NotFound(id);
            return View(entity);
        }

        private ActionResult NotFound(string id)
        {
            return NotFound("获取 系统审计日志信息 错误", $"无法找到 主键 = {id} 的系统审计日志信息");
        }
    }
}
