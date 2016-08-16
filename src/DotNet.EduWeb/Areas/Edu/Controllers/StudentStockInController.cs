// ===============================================================================
// DotNet.Platform 开发框架 2016 版权所有
// ===============================================================================

using System;
using System.Web.Mvc;
using DotNet.Edu.Entity;
using DotNet.Edu.Service;
using DotNet.Utility;
using DotNet.Extensions;
using System.Collections.Generic;

namespace DotNet.Web.Areas.Edu.Controllers
{
    public class StudentStockInController : EduController
    {
        public ActionResult Index()
        {
            if (!IsSchool) throw new ApplicationException("当前功能只允许培训机构使用");
            return View();
        }

        public ActionResult _Grid(StudentSearchParam searchParam)
        {
            searchParam.SchoolId = CurrentSchoolId;
            searchParam.Status = 0;
            var list = EduService.Student.GetPageList(PageInfo(), searchParam);
            return View(list);
        }

        [HttpPost]
        public ActionResult StockIn(string studentIds)
        {
            var results = EduService.Student.StockIn(studentIds.SplitToArray());
            return Json(new { success = true, items = results });
        }
        
        public ActionResult Export(StudentSearchParam searchParam)
        {
            searchParam.SchoolId = CurrentSchoolId;
            searchParam.Status = 0;
            var list = EduService.Student.GetList(searchParam);
            return Export(list);
        }
    }
}