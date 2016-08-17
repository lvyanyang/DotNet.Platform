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
    public class CompanyController : EduController
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Grid(string name, string enabled)
        {
            var list = EduService.Company.GetPageList(PageInfo(), name, enabled.ToBoolOrNull());
            return View(list);
        }

        public ActionResult Create()
        {
            MarkCreate();
            return EditCore(new Company
            {
                Id = StringHelper.Guid(),
                CreateDateTime = DateTime.Now
            });
        }

        public ActionResult Edit(string id)
        {
            var entity = EduService.Company.Get(id);
            if (entity == null) return NotFound(id);
            return EditCore(entity);
        }

        private ActionResult EditCore(Company entity)
        {
            return View("Edit", entity);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Save(Company entity)
        {
            var hasResult = EduService.Company.ExistsByName(entity.Id, entity.Name);
            if (hasResult.Failure)
            {
                return Json(hasResult);
            }
            entity.Spell = entity.Name.Spell();
            var result = EduService.Company.Save(entity, IsCreate);
            return Json(result);
        }

        [HttpPost]
        public ActionResult Delete(string id)
        {
            EduService.Company.Delete(id.SplitToArray());
            return Json(BoolMessage.True);
        }

        public ActionResult Details(string id)
        {
            var entity = EduService.Company.Get(id);
            if (entity == null) return NotFound(id);
            return View(entity);
        }

        public ActionResult Export()
        {
            return Export(EduService.Company.GetList());
        }

        private ActionResult NotFound(string id)
        {
            return NotFound("获取 企业信息信息 错误", $"无法找到 主键 = {id} 的企业信息信息");
        }
    }
}
