// ===============================================================================
// DotNet.Platform 开发框架 2016 版权所有
// ===============================================================================

using System.Web.Mvc;
using DotNet.Auth.Service;

namespace DotNet.Auth.Controllers
{
    public class VersionController : AuthController
    {
        public ActionResult Grid(string tableName, string pkValue)
        {
            var list = AuthService.Version.GetPageList(PageInfo(), tableName, pkValue);
            return View(list);
        }

        public ActionResult Details(string id)
        {
            var entity = AuthService.Version.Get(id);
            if (entity == null) return NotFound(id);
            return View(entity);
        }

        private ActionResult NotFound(string id)
        {
            return NotFound("获取 系统数据版本信息 错误", $"无法找到 主键 = {id} 的系统数据版本信息");
        }
    }
}
