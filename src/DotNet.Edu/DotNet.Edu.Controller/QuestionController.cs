// ===============================================================================
// DotNet.Platform 开发框架 2016 版权所有
// ===============================================================================

using System;
using System.Collections.Generic;
using System.Web.Mvc;
using DevExpress.Spreadsheet;
using DotNet.Doc;
using DotNet.Edu.Entity;
using DotNet.Edu.Service;
using DotNet.Extensions;
using DotNet.Helper;
using DotNet.Utility;

namespace DotNet.Edu.Controllers
{
    public class QuestionController : EduController
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Grid(string name, string questType, string workType, string questUnit)
        {
            var list = EduService.Question.GetPageList(PageInfo(), name,questType, workType, questUnit);
            return View(list);
        }

        public ActionResult Create()
        {
            MarkCreate();
            return EditCore(new Question
            {
                Id = StringHelper.Guid()
            });
        }

        public ActionResult Edit(string id)
        {
            var entity = EduService.Question.Get(id);
            if (entity == null) return NotFound(id);
            return EditCore(entity);
        }

        private ActionResult EditCore(Question entity)
        {
            return View("Edit", entity);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Save(Question entity)
        {
            var hasResult = EduService.Question.ExistsByName(entity.Id, entity.Name);
            if (hasResult.Failure)
            {
                return Json(hasResult);
            }
            entity.CreateDateTime = DateTime.Now;
            var result = EduService.Question.Save(entity, IsCreate);
            return Json(result);
        }

        [HttpPost]
        public ActionResult Delete(string id)
        {
            EduService.Question.Delete(id.SplitToArray());
            return Json(BoolMessage.True);
        }

        public ActionResult Details(string id)
        {
            var entity = EduService.Question.Get(id);
            if (entity == null) return NotFound(id);
            return View(entity);
        }

        public ActionResult Export(string name, string questType, string workType, string questUnit)
        {
            return Export(EduService.Question.GetList(name,questType, workType, questUnit));
        }

        private ActionResult NotFound(string id)
        {
            return NotFound("获取 题库信息 错误", $"无法找到 主键 = {id} 的题库信息");
        }

        public ActionResult Import()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ImportSave()
        {
            var results = new List<BoolMessage>(new[] { new BoolMessage(false, "请选择有效的文件") });
            if (Request.Files.Count == 0 || Request.Files[0] == null)
            {
                return Json(new { success = false, items = results });
            }
            var file = FileHelper.ConvertToBytes(Request.Files[0].InputStream);
            if (file == null || file.Length == 0)
            {
                return Json(new { success = false, items = results });
            }
            var questions = ExcelHelper.Import<Question>(file, true, ExcelHelper.GetFormat(Request.Files[0].FileName));
            results = EduService.Question.Import(questions);
            return Json(new { success = true, items = results });
        }
    }
}
