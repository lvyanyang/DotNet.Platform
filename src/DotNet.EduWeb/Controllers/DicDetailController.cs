// ===============================================================================
// DotNet.Platform 开发框架 2016 版权所有
// ===============================================================================
using System.Web.Mvc;
using DotNet.Auth.Entity;
using DotNet.Auth.Service;
using DotNet.Auth.Utility;
using DotNet.Extensions;
using DotNet.Helper;

namespace DotNet.EduWeb.Controllers
{
    public class DicDetailController : AuthController
    {
        public ActionResult Grid(string parentid, string name, string isEnabled)
        {
            var list = AuthService.DicDetail
                .GetPageList(PageInfo(), parentid, name, isEnabled.ToBoolOrNull());
            return View(list);
        }

        public ActionResult Create(string parentid)
        {
            MarkCreate();
            return EditCore(new DicDetail
            {
                Id = StringHelper.Guid(),
                DicId = parentid,
                RowIndex = AuthService.DicDetail.GetNewRowIndex(parentid)
            });
        }

        public ActionResult Edit(string id)
        {
            var entity = AuthService.DicDetail.Get(id);
            return EditCore(entity);
        }

        private ActionResult EditCore(DicDetail entity)
        {
            var itemEntity = AuthService.Dic.Get(entity.DicId);
            ViewBag.ItemName = itemEntity?.Name;
            return View("Edit", entity);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Save(DicDetail entity)
        {
            var hasResult = AuthService.DicDetail.ExistsByName(entity.DicId, entity.Id, entity.Name);
            if (hasResult.Failure)
            {
                return Json(hasResult);
            }
            entity.Spell = entity.Name.Spell();
            AuthHelper.SetEntityUserInfo(entity, IsCreate);
            var result = AuthService.DicDetail.Save(entity, IsCreate);
            return Json(result);
        }

        [HttpPost]
        public ActionResult Delete(string id)
        {
            var result = AuthService.DicDetail.Delete(id.SplitToArray());
            return Json(result);
        }

        public ActionResult Details(string id)
        {
            var entity = AuthService.DicDetail.Get(id);
            ViewBag.ItemName = AuthService.Dic.Get(entity.DicId)?.Name;
            return View(entity);
        }
    }
}