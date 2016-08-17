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
    public class SchoolController : EduController
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Grid(string name, string enabled)
        {
            string schoolId = null;
            if (IsSchool) schoolId = CurrentSchoolId;
            var list = EduService.School.GetPageList(PageInfo(), schoolId, name, enabled.ToBoolOrNull());
            return View(list);
        }

        public ActionResult Create()
        {
            MarkCreate();
            return EditCore(new School
            {
                Id = StringHelper.Guid(),
                CreateDateTime = DateTime.Now
            });
        }

        public ActionResult Edit(string id)
        {
            var entity = EduService.School.Get(id);
            if (entity == null) return NotFound(id);
            return EditCore(entity);
        }

        private ActionResult EditCore(School entity)
        {
            return View("Edit", entity);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Save(School entity)
        {
            var hasResult = EduService.School.ExistsByCode(entity.Id, entity.Code);
            if (hasResult.Failure)
            {
                return Json(hasResult);
            }
            entity.Spell = entity.Name.Spell();
            var result = EduService.School.Save(entity, IsCreate);
            return Json(result);
        }

        [HttpPost]
        public ActionResult Delete(string id)
        {
            EduService.School.Delete(id.SplitToArray());
            return Json(BoolMessage.True);
        }

        public ActionResult Details(string id)
        {
            var entity = EduService.School.Get(id);
            if (entity == null) return NotFound(id);
            return View(entity);
        }

        public ActionResult Export()
        {
            return Export(EduService.School.GetList());
        }

        private ActionResult NotFound(string id)
        {
            return NotFound("获取 培训机构信息 错误", $"无法找到 主键 = {id} 的培训机构信息");
        }
    }
}
