// ===============================================================================
// DotNet.Platform 开发框架 2016 版权所有
// ===============================================================================

using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using DevExpress.Spreadsheet;
using DotNet.Doc;
using DotNet.Edu.Entity;
using DotNet.Edu.Service;
using DotNet.Extensions;
using DotNet.Helper;
using DotNet.Utility;

namespace DotNet.EduWeb.Areas.Edu.Controllers
{
    public class StudentController : EduController
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Grid(StudentSearchParam searchParam)
        {
            if (IsCompany) searchParam.CompanyId = CurrentCompanyId;
            if (IsSchool) searchParam.SchoolId = CurrentSchoolId;
            ViewBag.status = searchParam.Status;
            var list = EduService.Student.GetPageList(PageInfo(), searchParam);
            return View(list);
        }

        public ActionResult _LogGrid(string studentId)
        {
            var list = EduService.StudentLog.GetPageList(PageInfo(), studentId);
            return View(list);
        }

        public ActionResult Create()
        {
            MarkCreate();
            var entity = new Student();
            entity.Id = StringHelper.Guid();
            if (IsCompany)
            {
                entity.CompanyId = CurrentCompanyId;
            }
            return EditCore(entity);
        }

        public ActionResult Edit(string id)
        {
            var entity = EduService.Student.Get(id);
            if (entity == null) return NotFound(id);
            return EditCore(entity);
        }

        private ActionResult EditCore(Student entity)
        {
            return View("Edit", entity);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Save(Student entity)
        {
            var hasResult = EduService.Student.ExistsByIDCardNo(entity.Id, entity.Name);
            if (hasResult.Failure)
            {
                return Json(hasResult);
            }
            entity.Spell = entity.Name.Spell();
            var result = EduService.Student.Save(entity, IsCreate);
            return Json(result);
        }

        [HttpPost]
        public ActionResult Delete(string studentIds)
        {
            var results = EduService.Student.SchoolStudentDelete(studentIds.SplitToArray());
            return Json(new { success = true, items = results });
        }

        public ActionResult Details(string id)
        {
            var entity = EduService.Student.Get(id);
            if (entity == null) return NotFound(id);
            return View(entity);
        }

        public ActionResult Export(StudentSearchParam searchParam)
        {
            if (IsCompany) searchParam.CompanyId = CurrentCompanyId;
            if (IsSchool) searchParam.SchoolId = CurrentSchoolId;
            var list = EduService.Student.GetList(searchParam);
            return Export(list);
        }

        private ActionResult NotFound(string id)
        {
            return NotFound("获取 学员信息 错误", $"无法找到 主键 = {id} 的学员信息");
        }

        public ActionResult Import()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ImportSave()
        {
            var results = new List<BoolMessage>(new[] { new BoolMessage(false, "请选择有效的文件") });
            if (Request.Files.Count == 0 || Request.Files[0] == null)
            {
                return Json(new { success = false, items = results });
            }
            var file = FileHelper.ConvertToBytes(Request.Files[0].InputStream);
            if (file == null || file.Length == 0)
            {
                return Json(new { success = false, items = results });
            }
            var students = ExcelHelper.Import<Student>(file, true, ExcelHelper.GetFormat(Request.Files[0].FileName));
            results = EduService.Student.Import(students);
            return Json(new { success = true, items = results });
        }
    }
}
