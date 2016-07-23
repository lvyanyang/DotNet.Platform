// ===============================================================================
// DotNet.Platform 开发框架 2016 版权所有
// ===============================================================================

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Web.Mvc;
using DotNet.Auth.Entity;
using DotNet.Edu.Entity;
using DotNet.Edu.Service;
using DotNet.Extensions;
using DotNet.Helper;
using DotNet.Utility;

namespace DotNet.Edu.Controllers
{
    public class CoursewareDetailsController : EduController
    {
        public ActionResult Index(string coursewareId)
        {
           var entity = EduService.Courseware.Get(coursewareId);
            return View(entity);
        }

        [HttpPost]
        public ActionResult Save(string coursewareId)
        {
            var infos = WebHelper.UploadFile(Request.Files, ".jpg", $"courseware/{coursewareId}");
            foreach (var info in infos)
            {
                var entity = new CoursewareDetails();
                entity.Id = StringHelper.Guid();
                entity.CourseId = coursewareId;
                entity.Url = info.Url;
                EduService.CoursewareDetails.Create(entity);
            }
            return Json(new { success = true });
        }

        public ActionResult _Image(string coursewareId)
        {
            var list = EduService.CoursewareDetails.GetList(coursewareId);
            return View(list);
        }

        public ActionResult _Video(string coursewareId)
        {
            var list = EduService.CoursewareDetails.GetList(coursewareId);
            return View(list);
        }

        [HttpPost]
        public ActionResult Delete(string id)
        {
            var result = EduService.CoursewareDetails.Delete(id);
            return Json(result);
        }

        public ActionResult Export(string coursewareId)
        {
            return Export(EduService.CoursewareDetails.GetList(coursewareId));
        }

        [HttpPost]
        public ActionResult SaveRowIndex()
        {
            var list = Request.Form.AllKeys.Select(key =>
                            new PrimaryKeyValue(key, nameof(CoursewareDetails.RowIndex),
                                Request.Form[key])).ToArray();
            var result = EduService.CoursewareDetails.SaveRowIndex(list);
            return Json(result);
        }
    }
}
