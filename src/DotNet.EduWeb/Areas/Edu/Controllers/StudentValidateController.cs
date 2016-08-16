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

namespace DotNet.Web.Areas.Edu.Controllers
{
    public class StudentValidateController : EduController
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Grid(string name)
        {
            var list = EduService.StudentValidate.GetPageList(PageInfo(), name);
            return View(list);
        }

        public ActionResult Create()
        {
            MarkCreate();
            return EditCore(new StudentValidate
            {
                Id = StringHelper.Guid()
            });
        }

        public ActionResult Edit(string id)
        {
            var entity = EduService.StudentValidate.Get(id);
            if (entity == null) return NotFound(id);
            return EditCore(entity);
        }

        private ActionResult EditCore(StudentValidate entity)
        {
            return View("Edit", entity);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Save(StudentValidate entity)
        {
            var hasResult = EduService.StudentValidate.ExistsByName(entity.Id, entity.Name);
            if (hasResult.Failure)
            {
                return Json(hasResult);
            }
            var result = EduService.StudentValidate.Save(entity, IsCreate);
            return Json(result);
        }

        [HttpPost]
        public ActionResult Delete(string id)
        {
            EduService.StudentValidate.Delete(id.SplitToArray());
            return Json(BoolMessage.True);
        }

        public ActionResult Details(string id)
        {
            var entity = EduService.StudentValidate.Get(id);
            if (entity == null) return NotFound(id);
            return View(entity);
        }

        public ActionResult Export()
        {
            return Export(EduService.StudentValidate.GetList());
        }

        private ActionResult NotFound(string id)
        {
            return NotFound("获取 学习验证信息 错误", $"无法找到 主键 = {id} 的学习验证信息");
        }
    }
}
