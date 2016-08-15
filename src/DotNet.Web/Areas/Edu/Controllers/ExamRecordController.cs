// ===============================================================================
// DotNet.Platform 开发框架 2016 版权所有
// ===============================================================================

using System;
using System.Web.Mvc;
using DotNet.Edu.Service;

namespace DotNet.Edu.Controllers
{
    public class ExamRecordController : EduController
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Grid(string name)
        {
            var list = EduService.ExamRecord.GetPageList(PageInfo(),name);
            return View(list);
        }
 
        public ActionResult Details(string id)
        {
            var entity = EduService.ExamRecord.Get(id);
            if (entity == null) return NotFound(id);
            return View(entity);
        }

        //public ActionResult Export()
        //{
        //    return Export(EduService.ExamRecord.GetList());
        //}

        private ActionResult NotFound(string id)
        {
            return NotFound("获取 考试记录信息 错误", $"无法找到 主键 = {id} 的考试记录信息");
        }
    }
}
