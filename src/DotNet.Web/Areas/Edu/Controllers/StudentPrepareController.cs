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

namespace DotNet.Edu.Controllers
{
    public class StudentPrepareController : EduController
    {
        public ActionResult Index()
        {
            if (!IsCompany) throw new ApplicationException("当前功能只允许企业使用");
            return View();
        }

        public ActionResult _Grid(StudentSearchParam searchParam)
        {
            if (searchParam.Status.IsEmpty()) searchParam.Status = 0;
            searchParam.CompanyId = CurrentCompanyId;
            ViewBag.status = searchParam.Status;
            var list = EduService.Student.GetPageList(PageInfo(), searchParam);
            return View(list);
        }

        [HttpPost]
        public ActionResult Delete(string studentIds)
        {
            var results = EduService.Student.CompanyStudentDelete(studentIds.SplitToArray());
            return Json(new { success = true, items = results });
        }
        
        public ActionResult Export(StudentSearchParam searchParam)
        {
            searchParam.CompanyId = CurrentCompanyId;
            var list = EduService.Student.GetList(searchParam);
            return Export(list);
        }
    }
}