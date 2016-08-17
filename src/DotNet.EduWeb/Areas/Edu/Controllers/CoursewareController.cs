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
    public class CoursewareController : EduController
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Grid(string name, string workType, string courseType)
        {
            var list = EduService.Courseware.GetPageList(PageInfo(), name, workType, courseType);
            return View(list);
        }

        public ActionResult Create()
        {
            MarkCreate();
            return EditCore(new Courseware
            {
                Id = StringHelper.Guid(),
                RowIndex = EduService.Courseware.GetNewRowIndex()
            });
        }

        public ActionResult Edit(string id)
        {
            var entity = EduService.Courseware.Get(id);
            if (entity == null) return NotFound(id);
            entity.PeriodMinute = entity.Period / 60;
            return EditCore(entity);
        }

        private ActionResult EditCore(Courseware entity)
        {
            return View("Edit", entity);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Save(Courseware entity)
        {
            var hasResult = EduService.Courseware.ExistsByName(entity.Id, entity.Name);
            if (hasResult.Failure)
            {
                return Json(hasResult);
            }
            entity.Period = entity.PeriodMinute * 60;
            entity.Spell = entity.Name.Spell();
            var result = EduService.Courseware.Save(entity, IsCreate);
            return Json(result);
        }

        [HttpPost]
        public ActionResult Delete(string id)
        {
            EduService.Courseware.Delete(id.SplitToArray());
            return Json(BoolMessage.True);
        }

        public ActionResult Details(string id)
        {
            var entity = EduService.Courseware.Get(id);
            if (entity == null) return NotFound(id);
            return View(entity);
        }

        public ActionResult Export()
        {
            return Export(EduService.Courseware.GetList(null));
        }

        private ActionResult NotFound(string id)
        {
            return NotFound("获取 课件信息 错误", $"无法找到 主键 = {id} 的课件信息");
        }
    }
}
