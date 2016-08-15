// ===============================================================================
// DotNet.Platform 开发框架 2016 版权所有
// ===============================================================================

using System;
using System.Web.Mvc;
using DotNet.Auth.Utility;
using DotNet.Edu.Entity;
using DotNet.Edu.Service;
using DotNet.Extensions;
using DotNet.Helper;
using DotNet.Utility;

namespace DotNet.Edu.Controllers
{
    public class NoticeController : EduController
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Grid(string title, string startDate, string endDate)
        {
            var list = EduService.Notice.GetPageList(PageInfo(), title, startDate.ToDateTimeOrNull(), endDate.ToDateTimeOrNull());
            return View(list);
        }

        public ActionResult Create()
        {
            MarkCreate();
            return EditCore(new Notice
            {
                Id = StringHelper.Guid()
            });
        }

        public ActionResult Edit(string id)
        {
            var entity = EduService.Notice.Get(id);
            if (entity == null) return NotFound(id);
            return EditCore(entity);
        }

        private ActionResult EditCore(Notice entity)
        {
            return View("Edit", entity);
        }

        [HttpPost]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult Save(Notice entity)
        {
            var hasResult = EduService.Notice.ExistsByName(entity.Id, entity.Title);
            if (hasResult.Failure)
            {
                return Json(hasResult);
            }
            if (IsCreate)
            {
                entity.CreateDateTime = DateTime.Now;
            }
            var result = EduService.Notice.Save(entity, IsCreate);
            return Json(result);
        }

        [HttpPost]
        public ActionResult Delete(string id)
        {
            EduService.Notice.Delete(id.SplitToArray());
            return Json(BoolMessage.True);
        }

        public ActionResult Details(string id)
        {
            var entity = EduService.Notice.Get(id);
            if (entity == null) return NotFound(id);
            return View(entity);
        }

        public ActionResult Export(string title, string startDate, string endDate)
        {
            return Export(EduService.Notice.GetList(title, startDate.ToDateTimeOrNull(), endDate.ToDateTimeOrNull()));
        }

        private ActionResult NotFound(string id)
        {
            return NotFound("获取 通知公告信息 错误", $"无法找到 主键 = {id} 的通知公告信息");
        }
    }
}
