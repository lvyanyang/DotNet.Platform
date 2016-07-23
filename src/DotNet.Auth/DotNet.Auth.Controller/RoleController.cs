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
    public class RoleController : AuthController
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Grid(string name, string category, string enabled)
        {
            var list = AuthService.Role.GetPageList(PageInfo(), name, category, enabled.ToBoolOrNull());
            return View(list);
        }

        public ActionResult Create()
        {
            MarkCreate();
            return EditCore(new Role
            {
                Id = StringHelper.Guid(),
                RowIndex = AuthService.Role.GetNewRowIndex()
            });
        }

        public ActionResult Edit(string id)
        {
            var entity = AuthService.Role.Get(id);
            if (entity == null) return NotFound(id);
            return EditCore(entity);
        }

        private ActionResult EditCore(Role entity)
        {
            return View("Edit", entity);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Save(Role entity)
        {
            var hasResult = AuthService.Role.ExistsByName(entity.Id, entity.Name);
            if (hasResult.Failure)
            {
                return Json(hasResult);
            }
            entity.Spell = entity.Name.Spell();
            AuthHelper.SetEntityUserInfo(entity, IsCreate);
            var result = AuthService.Role.Save(entity, IsCreate);
            return Json(result);
        }

        [HttpPost]
        public ActionResult Delete(string id)
        {
            AuthService.Role.Delete(id.SplitToArray());
            return Json(BoolMessage.True);
        }

        public ActionResult Details(string id)
        {
            var entity = AuthService.Role.Get(id);
            if (entity == null) return NotFound(id);
            return View(entity);
        }

        public ActionResult Export()
        {
            return Export(AuthService.Role.GetList());
        }

        private ActionResult NotFound(string id)
        {
            return NotFound("获取 角色信息 错误", $"无法找到 主键 = {id} 的角色信息");
        }
    }
}