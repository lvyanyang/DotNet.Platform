// ===============================================================================
// DotNet.Platform 开发框架 2016 版权所有
// ===============================================================================

using System;
using System.Web.Mvc;
using DotNet.Auth.Entity;
using DotNet.Auth.Service;
using DotNet.Auth.Utility;
using DotNet.Extensions;
using DotNet.Helper;
using DotNet.Utility;

namespace DotNet.Auth.Controllers
{
    public class ParamController : AuthController
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Grid(string name, string code, string category)
        {
            var list = AuthService.Param.GetPageList(PageInfo(), name, code,category);
            return View(list);
        }

        public ActionResult Create()
        {
            MarkCreate();
            return EditCore(new Param
            {
                Id = StringHelper.Guid()
            });
        }

        public ActionResult Edit(string id)
        {
            var entity = AuthService.Param.GetById(id);
            if (entity == null) return NotFound(id);
            return EditCore(entity);
        }

        private ActionResult EditCore(Param entity)
        {
            return View("Edit", entity);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Save(Param entity)
        {
            var hasResult = AuthService.Param.ExistsByName(entity.Id, entity.Name);
            if (hasResult.Failure)
            {
                return Json(hasResult);
            }
            AuthHelper.SetEntityUserInfo(entity, IsCreate);
            var result = AuthService.Param.Save(entity, IsCreate);
            return Json(result);
        }

        [HttpPost]
        public ActionResult Delete(string id)
        {
            AuthService.Param.Delete(id.SplitToArray());
            return Json(BoolMessage.True);
        }

        public ActionResult Details(string id)
        {
            var entity = AuthService.Param.GetById(id);
            if (entity == null) return NotFound(id);
            return View(entity);
        }

        public ActionResult Export()
        {
            return Export(AuthService.Param.GetList());
        }

        private ActionResult NotFound(string id)
        {
            return NotFound("获取 系统参数信息 错误", $"无法找到 主键 = {id} 的系统参数信息");
        }
    }
}
