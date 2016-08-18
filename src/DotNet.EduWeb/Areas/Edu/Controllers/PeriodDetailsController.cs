// ===============================================================================
// DotNet.Platform 开发框架 2016 版权所有
// ===============================================================================

using System;
using System.Web.Mvc;
using DotNet.Edu.Entity;
using DotNet.Edu.Service;
using DotNet.Edu.Utility;
using DotNet.Extensions;
using DotNet.Helper;
using DotNet.Utility;

namespace DotNet.EduWeb.Areas.Edu.Controllers
{
    public class PeriodDetailsController : EduController
    {
        private readonly PeriodDetailsService service = new PeriodDetailsService();

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Grid(string studentName, string startDate, string endDate)
        {
            var list = EduService.PeriodDetails.GetPageList(PageInfo(), studentName,
                startDate.ToDateTimeOrNull(), endDate.ToDateTimeOrNull());
            return View(list);
        }

        public ActionResult StudentPeriodGrid(string studentId)
        {
            var list = service.GetStudentPageList(PageInfo(), studentId);
            return View(list); 
        }

        public ActionResult Details(string id)
        {
            var entity = service.Get(id);
            return entity == null ? NotFound(id) : View(entity);
        }
        
        private ActionResult NotFound(string id)
        {
            return NotFound("获取 学时明细信息 错误", $"无法找到 主键 = {id} 的学时明细信息");
        }
    }
}
