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
    public class DicController : AuthController
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Tree()
        {
            var nodeList = AuthService.Dic.GetNodeList();
            return Json(nodeList);
        }

        public ActionResult Create(string parentId)
        {
            MarkCreate();
            return EditCore(new Dic
            {
                Id = StringHelper.Guid(),
                ParentId = parentId,
                SortPath = AuthService.Dic.GetNewSortPath(parentId),
            });
        }

        public ActionResult Edit(string id)
        {
            var entity = AuthService.Dic.Get(id);
            if (entity == null) return NotFound(id);
            return EditCore(entity);
        }

        private ActionResult EditCore(Dic entity)
        {
            var parentEntity = AuthService.Dic.Get(entity.ParentId);
            ViewBag.ParentName = parentEntity == null ? "数据字典" : parentEntity.Name;
            return View("Edit", entity);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Save(Dic entity)
        {
            var hasResult = AuthService.Dic.ExistsByCode(entity.Id, entity.Code);
            if (hasResult.Failure)
            {
                return Json(hasResult);
            }
            entity.Spell = entity.Name.Spell();
            AuthHelper.SetEntityUserInfo(entity, IsCreate);
            var result = AuthService.Dic.Save(entity, IsCreate);
            return Json(new EntityMessage(result, new
            {
                id = entity.Id,
                parentId = entity.ParentId,
                text = entity.Name,
                iconCls = "font-icon icon-settings"
            }));
        }

        [HttpPost]
        public ActionResult SaveParent(string id, string newParentId)
        {
            var result = AuthService.Dic.SaveParent(id, newParentId);
            return Json(result);
        }

        [HttpPost]
        public ActionResult SaveSort()
        {
            var list = Request.Form.AllKeys.Select(key => 
                new PrimaryKeyValue(key, nameof(Dic.SortPath), Request.Form[key])).ToArray();
            var result = AuthService.Dic.SaveSortPath(list);
            return Json(result);
        }

        [HttpPost]
        public ActionResult Delete(string id)
        {
            var result = AuthService.Dic.Delete(id);
            return Json(result);
        }

        private ActionResult NotFound(string id)
        {
            return NotFound("获取 数据字典信息 错误", $"无法找到 主键 = {id} 的数据字典信息");
        }
    }
}