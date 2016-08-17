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

namespace DotNet.EduWeb.Controllers
{
    public class DepartmentController : AuthController
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Tree()
        {
            var nodes = AuthService.Department.GetNodeList();
            return Json(nodes);
        }

        public ActionResult Create(string parentId)
        {
            MarkCreate();
            return EditCore(new Department
            {
                Id = StringHelper.Guid(),
                ParentId = parentId,
                SortPath = AuthService.Department.GetNewSortPath(parentId),
            });
        }

        public ActionResult Edit(string id)
        {
            var entity = AuthService.Department.Get(id);
            if (entity == null) return NotFound(id);
            return EditCore(entity);
        }

        private ActionResult EditCore(Department entity)
        {
            var parentEntity = AuthService.Department.Get(entity.ParentId);
            ViewBag.ParentName = parentEntity == null ? "组织机构" : parentEntity.Name;
            return View("Edit", entity);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Save(Department entity)
        {
            var hasResult = AuthService.Department.ExistsByName(entity.Id, entity.ParentId, entity.Name);
            if (hasResult.Failure)
            {
                return Json(hasResult);
            }
            entity.Spell = entity.Name.Spell();
            AuthHelper.SetEntityUserInfo(entity, IsCreate);
            var result = AuthService.Department.Save(entity,IsCreate);
            return Json(new EntityMessage(result, new
            {
                id = entity.Id,
                parentId = entity.ParentId,
                text = entity.Name,
                iconCls = "font-icon icon-users"
            }));
        }

        [HttpPost]
        public ActionResult SaveParent(string id, string newParentId)
        {
            var result = AuthService.Department.SaveParent(id, newParentId);
            return Json(result);
        }

        [HttpPost]
        public ActionResult SaveSort()
        {
            var list = Request.Form.AllKeys.Select(key => 
                            new PrimaryKeyValue(key, nameof(Department.SortPath), 
                                Request.Form[key])).ToArray();
            var result = AuthService.Department.SaveSortPath(list);
            return Json(result);
        }

        [HttpPost]
        public ActionResult Delete(string id)
        {
            var result = AuthService.Department.Delete(id);
            return Json(result);
        }

        private ActionResult NotFound(string id)
        {
            return NotFound("获取 部门信息 错误", $"无法找到 主键 = {id} 的部门信息");
        }
    }
}