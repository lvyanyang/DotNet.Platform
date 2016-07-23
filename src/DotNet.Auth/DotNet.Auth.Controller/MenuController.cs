// ===============================================================================
// DotNet.Platform 开发框架 2016 版权所有
// ===============================================================================
using System.Linq;
using System.Web.Mvc;
using DotNet.Auth.Entity;
using DotNet.Auth.Service;
using DotNet.Auth.Utility;
using DotNet.Extensions;
using DotNet.Helper;
using DotNet.Mvc;
using DotNet.Utility;

namespace DotNet.Auth.Controllers
{
    public class MenuController : AuthController
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Tree(string name)
        {
            var list = AuthService.Menu.GetList(name);
            return Json(new EasyUIGrid(list.Count, list));
        }

        public ActionResult Create(string parentid)
        {
            MarkCreate();
            return EditCore(new Menu
            {
                Id = StringHelper.Guid(),
                ParentId = parentid,
                SortPath = AuthService.Menu.GetNewSortPath(parentid),
            });
        }

        public ActionResult Edit(string id)
        {
            var entity = AuthService.Menu.Get(id);
            if (entity == null) return NotFound(id);
            return EditCore(entity);
        }

        private ActionResult EditCore(Menu entity)
        {
            var parentEntity = AuthService.Menu.Get(entity.ParentId);
            ViewBag.ParentName = parentEntity == null ? "系统菜单" : parentEntity.Name;
            return View("Edit", entity);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Save(Menu entity)
        {
            var hasResult = AuthService.Menu.ExistsByCode(entity.Id, entity.Code);
            if (hasResult.Failure)
            {
                return Json(hasResult);
            }
            entity.Spell = entity.Name.Spell();
            AuthHelper.SetEntityUserInfo(entity, IsCreate);
            var result = AuthService.Menu.Save(entity, IsCreate);
            return Json(new EntityMessage(result, new
            {
                id = entity.Id,
                parentId = entity.ParentId,
                text = entity.Name,
                iconCls = string.IsNullOrEmpty(entity.IconCls) ? "font-icon icon-badge" : entity.IconCls
            }));
        }

        [HttpPost]
        public ActionResult SaveParent(string id, string newParentId)
        {
            var result = AuthService.Menu.SaveParent(id, newParentId);
            return Json(result);
        }

        [HttpPost]
        public ActionResult SaveSort()
        {
            var list = Request.Form.AllKeys.Select(key =>
                            new PrimaryKeyValue(key, nameof(Department.SortPath),
                                Request.Form[key])).ToArray();
            var result = AuthService.Menu.SaveSortPath(list);
            return Json(result);
        }

        [HttpPost]
        public ActionResult Delete(string id)
        {
            var result = AuthService.Menu.Delete(id);
            return Json(result);
        }

        public ActionResult Details(string id)
        {
            var entity = AuthService.Menu.Get(id);
            if (entity == null) return NotFound(id);
            return View(entity);
        }

        private ActionResult NotFound(string id)
        {
            return NotFound("获取 菜单信息 错误", $"无法找到 主键 = {id} 的菜单信息");
        }
    }
}