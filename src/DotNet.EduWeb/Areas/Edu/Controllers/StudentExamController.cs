// ===============================================================================
// DotNet.Platform 开发框架 2016 版权所有
// ===============================================================================
using System;
using System.Web.Mvc;
using DotNet.Edu.Entity;
using DotNet.Edu.Service;
using DotNet.Utility;
using DotNet.Extensions;

namespace DotNet.EduWeb.Areas.Edu.Controllers
{
    public class StudentExamController : EduController
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult _Grid(StudentSearchParam searchParam)
        {
            if (IsSchool) searchParam.SchoolId = CurrentSchoolId;
            if (IsCompany) searchParam.CompanyId = CurrentCompanyId;
            searchParam.Status = 3;
            var list = EduService.Student.GetPageList(PageInfo(), searchParam);
            return View("_StudentGrid", list);
        }

        [HttpPost]
        public ActionResult ExamPrep(string studentIds)
        {
            var results = EduService.ExamUser.Prep(studentIds.SplitToArray());
            return Json(new { success = true, items = results });
        }
        
        public ActionResult Export(StudentSearchParam searchParam)
        {
            if (IsSchool) searchParam.SchoolId = CurrentSchoolId;
            if (IsCompany) searchParam.CompanyId = CurrentCompanyId;
            searchParam.Status = 3;
            var list = EduService.Student.GetList(searchParam);
            return Export(list);
        }
    }
}