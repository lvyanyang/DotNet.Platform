// ===============================================================================
// DotNet.Platform 开发框架 2016 版权所有
// ===============================================================================

using System.Collections.Generic;
using System.Web.Mvc;
using DotNet.Auth.Entity;
using DotNet.Auth.Service;
using DotNet.Auth.Utility;
using DotNet.Extensions;
using DotNet.Helper;

namespace DotNet.Web.Areas.Auth.Controllers
{
    public class UserController : AuthController
    {
        public ActionResult Grid(string parentid, string name, string isEnabled)
        {
            var list = AuthService.User.GetPageList(PageInfo(), name,
                parentid, isEnabled.ToBoolOrNull());
            return View(list);
        }

        public ActionResult Create(string parentid)
        {
            MarkCreate();
            return EditCore(new User
            {
                Id = StringHelper.Guid(),
                DepartmentId = parentid,
                RowIndex = AuthService.User.GetNewRowIndex()
            });
        }

        public ActionResult Edit(string id)
        {
            var entity = AuthService.User.Get(id);
            if (entity == null) return NotFound(id);
            return EditCore(entity);
        }

        private ActionResult EditCore(User entity)
        {
            return View("Edit", entity);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Save(User entity)
        {
            var hasResult = AuthService.User.ExistsByAccount(entity.Id, entity.Name);
            if (hasResult.Failure)
            {
                return Json(hasResult);
            }
            entity.Spell = entity.Name.Spell();
            AuthHelper.SetEntityUserInfo(entity, IsCreate);
            var result = AuthService.User.Save(entity, IsCreate);
            return Json(result);
        }

        [HttpPost]
        public ActionResult Delete(string id)
        {
            var result = AuthService.User.Delete(id.SplitToArray());
            return Json(result);
        }

        public ActionResult Details(string id)
        {
            var entity = AuthService.User.Get(id);
            if (entity == null) return NotFound(id);
            return View(entity);
        }

        private ActionResult NotFound(string id)
        {
            return NotFound("获取 系统用户信息 错误", $"无法找到 主键 = {id} 的系统用户信息");
        }

        public ActionResult ModifyUserPassword()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ModifyUserPassword(string currentPwd, string newPwd)
        {
            var userService = AuthService.User;
            if (!userService.ValidUserPassword(CurrentUserId, currentPwd))
            {
                return Json(false, "当前密码输入不正确,请重新输入");
            }
            var result = userService.UpdatePassword(new[] { CurrentUserId }, newPwd);
            return Json(result);
        }

        public ActionResult ResetUser(string ids)
        {
            var list = new List<User>();
            foreach (var id in ids.SplitToArray())
            {
                list.Add(AuthService.User.Get(id));
            }
            return View(list);
        }

        public ActionResult ResetUserPassword(string ids)
        {
            ViewBag.ids = ids;
            return View();
        }

        [HttpPost]
        public ActionResult ResetUserPassword(string ids, string newPwd)
        {
            var result = AuthService.User.UpdatePassword(ids.SplitToArray(), newPwd);
            return Json(result);
        }
    }
}