using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DotNet.Edu.Entity;
using DotNet.Edu.Service;
using DotNet.Extensions;
using DotNet.Helper;
using DotNet.Utility;

namespace DotNet.Edu.StudentWeb.Controllers
{
    public class StudentController : StudentWebController
    {
        public ActionResult TrainHistory()
        {
            var list = EduService.Student.GetTrainHistoryList(CurrentStudent.StudentId);
            return View(list);
        }

        public ActionResult LearnHistory()
        {
            return View();
        }

        public ActionResult LearnGrid()
        {
            var list = EduService.LessonLog.GetPageList(PageInfo(), CurrentStudent.StudentId);
            return View(list);
        }

        public ActionResult PeriodDetails()
        {
            ViewBag.learnPeriod = CurrentStudent.Student.TotalPeriod;
            return View();
        }

        public ActionResult PeriodDetailsGrid(string startDate, string endDate)
        {
            var list = EduService.PeriodDetails.GetPageList(PageInfo(), CurrentStudent.StudentId, 
                startDate.ToDateTimeOrNull(),endDate.ToDateTimeOrNull());
            return View(list);
        }

        public ActionResult LessonNote()
        {
            return View();
        }

        public ActionResult LessonNoteGrid()
        {
            var list = EduService.LessonNote.GetPageList(PageInfo(), CurrentStudent.StudentId);
            return View(list);
        }

        public ActionResult Notice()
        {
            return View();
        }

        public ActionResult NoticeGrid(string title, string startDate, string endDate)
        {
            var list = EduService.Notice.GetPageList(PageInfo(), title,  
                startDate.ToDateTimeOrNull(),  endDate.ToDateTimeOrNull());
            return View(list);
        }

        public ActionResult NoticeDetails(string id)
        {
            var entity = EduService.Notice.Get(id);
            return View(entity);
        }

        public ActionResult LessonNoteLi()
        {
            var list = EduService.LessonNote.GetTopList(CurrentStudent.StudentId, 10);
            return View(list);
        }

        public ActionResult LessonNoteSave(string message, string coursewareId, string coursewareName)
        {
            var entity = new LessonNote();
            entity.Id = StringHelper.Guid();
            entity.StudentId = CurrentStudent.Student.Id;
            entity.CoursewareId = coursewareId;
            entity.CoursewareName = coursewareName;
            entity.CreateDateTime = DateTime.Now;
            entity.Message = message;
            var result = EduService.LessonNote.Create(entity);
            return Json(result);
        }

        public ActionResult LessonNoteDelete(string id)
        {
            var result = EduService.LessonNote.Delete(id);
            return Json(result);
        }

        public ActionResult LearningValidateModal()
        {
            var entity = EduService.StudentValidate.GetRandom();
            var list = new List<string>();
            if (entity.A.IsNotEmpty()) list.Add(entity.A);
            if (entity.B.IsNotEmpty()) list.Add(entity.B);
            if (entity.C.IsNotEmpty()) list.Add(entity.C);
            if (entity.D.IsNotEmpty()) list.Add(entity.D);
            ViewBag.options = RandomHelper.RandomArray(list.ToArray());
            return View(entity);
        }

        [HttpPost]
        public ActionResult LearningValidate(string validateId, string coursewareId, string answer, int recordType)
        {
            var result = false;
            var validEntity = EduService.StudentValidate.Get(validateId);
            if (validEntity.Answer.Equals(answer, StringComparison.OrdinalIgnoreCase))
            {
                result = true;
            }
            var log = new LessonLog();
            log.Id = StringHelper.Guid();
            log.StudentId = CurrentStudent.StudentId;
            log.CoursewareId = coursewareId;
            log.CoursewareName = EduService.Courseware.GetName(coursewareId);
            log.WorkType = CurrentStudent.Student.WorkCategoryId;
            log.WorkTypeName = CurrentStudent.Student.WorkCategoryName;
            log.CreateDateTime = DateTime.Now;
            log.RecordType = recordType;
            log.Result = result;
            EduService.LessonLog.Create(log);
            return Json(new BoolMessage(result));
        }

        public ActionResult ValidateImage(string validateId)
        {
            var validEntity = EduService.StudentValidate.Get(validateId);
            var msg = validEntity.Name;
            ValidateCodeDrawHelper v = new ValidateCodeDrawHelper();
            v.FontSize = 28;
            v.Padding = 10;
            var bmp = v.CreateImage(msg);
            return File(ImageHelper.ToArray(bmp), "image/png");
        }
    }
}