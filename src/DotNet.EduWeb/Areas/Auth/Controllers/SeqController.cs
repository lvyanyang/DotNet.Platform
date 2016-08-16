// ===============================================================================
// DotNet.Platform 开发框架 2016 版权所有
// ===============================================================================

using System;
using System.Web.Mvc;
using DotNet.Auth.Entity;
using DotNet.Auth.Service;
using DotNet.Extensions;
using DotNet.Helper;
using DotNet.Utility;

namespace DotNet.Web.Areas.Auth.Controllers
{
    public class SeqController : AuthController
    {
        public ActionResult Index()
        {
            return View();
        }

        //public ActionResult InitCache()
        //{
            
        //}

        public ActionResult Grid(string name)
        {
            var list = AuthService.Seq.GetPageList(PageInfo(), name);
            return View(list);
        }

        public ActionResult Create()
        {
            MarkCreate();
            return EditCore(new Seq
            {
                Id = StringHelper.Guid()
            });
        }

        public ActionResult Edit(string id)
        {
            var entity = AuthService.Seq.Get(id);
            if (entity == null) return NotFound(id);
            return EditCore(entity);
        }

        private ActionResult EditCore(Seq entity)
        {
            return View("Edit", entity);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Save(Seq entity)
        {
            var hasResult = AuthService.Seq.ExistsByName(entity.Id, entity.Name);
            if (hasResult.Failure)
            {
                return Json(hasResult);
            }
            var result = AuthService.Seq.Save(entity, IsCreate);
            return Json(result);
        }

        [HttpPost]
        public ActionResult Delete(string id)
        {
            AuthService.Seq.Delete(id.SplitToArray());
            return Json(BoolMessage.True);
        }

        public ActionResult Details(string id)
        {
            var entity = AuthService.Seq.Get(id);
            if (entity == null) return NotFound(id);
            return View(entity);
        }

        public ActionResult Export()
        {
            return Export(AuthService.Seq.GetList());
        }

        private ActionResult NotFound(string id)
        {
            return NotFound("获取 系统序列信息 错误", $"无法找到 主键 = {id} 的系统序列信息");
        }
    }
}
