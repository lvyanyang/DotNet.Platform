// ===============================================================================
// DotNet.Platform 开发框架 2016 版权所有
// ===============================================================================

using System;
using System.Web.Mvc;
using DotNet.Edu.Entity;
using DotNet.Edu.Service;
using DotNet.Extensions;
using DotNet.Helper;
using DotNet.Utility;

namespace DotNet.EduWeb.Areas.Edu.Controllers
{
    public class TeacherController : EduController
    {
        public ActionResult Index()
        {
            if (!IsSchool) throw new ApplicationException("当前功能只允许培训机构使用");
            return View();
        }

        public ActionResult Grid(string name)
        {
            var list = EduService.Teacher.GetPageList(PageInfo(), CurrentSchoolId, name);
            return View(list);
        }

        public ActionResult Create()
        {
            MarkCreate();
            return EditCore(new Teacher
            {
                Id = StringHelper.Guid()
            });
        }

        public ActionResult Edit(string id)
        {
            var entity = EduService.Teacher.Get(id);
            if (entity == null) return NotFound(id);
            return EditCore(entity);
        }

        private ActionResult EditCore(Teacher entity)
        {
            return View("Edit", entity);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Save(Teacher entity)
        {
            var hasResult = EduService.Teacher.ExistsByName(entity.Id, entity.Name);
            if (hasResult.Failure)
            {
                return Json(hasResult);
            }
            if (IsCreate)
            {
                entity.CreateDateTime = DateTime.Now;
            }
            var result = EduService.Teacher.Save(entity, IsCreate);
            return Json(result);
        }

        [HttpPost]
        public ActionResult Delete(string id)
        {
            EduService.Teacher.Delete(id.SplitToArray());
            return Json(BoolMessage.True);
        }

        public ActionResult Details(string id)
        {
            var entity = EduService.Teacher.Get(id);
            if (entity == null) return NotFound(id);
            return View(entity);
        }

        public ActionResult Export()
        {
            return Export(EduService.Teacher.GetList(CurrentSchoolId));
        }

        private ActionResult NotFound(string id)
        {
            return NotFound("获取 教师信息 错误", $"无法找到 主键 = {id} 的教师信息");
        }
    }
}
