// ===============================================================================
// DotNet.Platform 开发框架 2016 版权所有
// ===============================================================================

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using DevExpress.Spreadsheet;
using DotNet.Collections;
using DotNet.Doc;
using DotNet.Edu.Entity;
using DotNet.Edu.Service;
using DotNet.Extensions;
using DotNet.Helper;
using DotNet.Utility;

namespace DotNet.Edu.Controllers
{
    public class TrainGroupController : EduController
    {
        public ActionResult Index()
        {
            if (!IsSchool) throw new ApplicationException("当前功能只允许培训机构使用");
            return View();
        }

        public ActionResult _Grid(string name, string schoolId, string categoryId,
            string startDate, string endDate, string status)
        {
            if (IsSchool) schoolId = CurrentSchoolId;
            var list = EduService.TrainGroup.GetPageList(PageInfo(), name,
                schoolId, categoryId, startDate.ToDateTimeOrNull(),
                endDate.ToDateTimeOrNull(), status.ToIntOrNull());
            return View(list);
        }


        public ActionResult Create()
        {
            MarkCreate();
            return EditCore(new TrainGroup
            {
                Id = StringHelper.Guid(),
                CreateDateTime = DateTime.Now
            });
        }

        public ActionResult Edit(string id)
        {
            var entity = EduService.TrainGroup.Get(id);
            if (entity == null) return NotFound(id);
            return EditCore(entity);
        }

        private ActionResult EditCore(TrainGroup entity)
        {
            ViewBag.SchoolId = CurrentSchoolId;
            return View("Edit", entity);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Save(TrainGroup entity)
        {
            var hasResult = EduService.TrainGroup.ExistsByName(entity.Id, entity.Name, entity.SchoolId, entity.CategoryId);
            if (hasResult.Failure)
            {
                return Json(hasResult);
            }
            entity.Spell = entity.Name.Spell();
            var result = EduService.TrainGroup.Save(entity, IsCreate);
            return Json(result);
        }

        [HttpPost]
        public ActionResult Delete(string id)
        {
            EduService.TrainGroup.Delete(id.SplitToArray());
            return Json(BoolMessage.True);
        }

        public ActionResult Details(string id)
        {
            var entity = EduService.TrainGroup.Get(id);
            if (entity == null) return NotFound(id);
            return View(entity);
        }


        public ActionResult Export()
        {
            return Export(EduService.TrainGroup.GetList("0"));
        }

        private ActionResult NotFound(string id)
        {
            return NotFound("获取 班级信息信息 错误", $"无法找到 主键 = {id} 的班级信息信息");
        }

        public ActionResult Reservation(string id)
        {
            var entity = EduService.TrainGroup.Get(id);
            if (entity == null) return NotFound(id);
            return View(entity);
        }

        #region Reservation

        [HttpPost]
        public ActionResult Reservation(string trainGroupId, string studentIds)
        {
            var results = EduService.TrainGroup.Reservation(trainGroupId, studentIds.SplitToArray());
            return Json(new { success = true, items = results });
        }

        public ActionResult _ReservationGrid(string groupId, StudentSearchParam searchParam)
        {
            var trainGroup = EduService.TrainGroup.Get(groupId);
            searchParam.SchoolId = trainGroup.SchoolId;
            searchParam.WorkCategoryId = trainGroup.CategoryId;
            searchParam.TrainGroupId = null;
            searchParam.Status = 1;
            ViewBag.trainGroupId = groupId;
            var list = EduService.Student.GetPageList(PageInfo(), searchParam);
            return View(list);
        }

        public ActionResult ReservationImport(string trainGroupId)
        {
            ViewBag.trainGroupId = trainGroupId;
            return View();
        }

        public ActionResult ReservationImportSave(string trainGroupId)
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
            var students = ExcelHelper.Import<StudentSimple>(file, true, DocumentFormat.Xlsx);
            results = EduService.Student.FillSimpleId(students);
            var _results = EduService.TrainGroup.Reservation(trainGroupId, students.Where(p => !String.IsNullOrEmpty(p.Id)).Select(p => p.Id).ToArray());
            results.AddRange(_results);
            return Json(new { success = true, items = results });
        }

        #endregion

        #region UnReservation

        [HttpPost]
        public ActionResult UnReservation(string trainGroupId, string studentIds)
        {
            var results = EduService.TrainGroup.UnReservation(trainGroupId, studentIds.SplitToArray());
            return Json(new { success = true, items = results });
        }

        public ActionResult _UnReservationGrid(string groupId, StudentSearchParam searchParam)
        {
            var list = GetAlreadyReservationStudent(groupId, searchParam);
            return View(list);
        }

        public ActionResult _ReservationDetailsGrid(string groupId, StudentSearchParam searchParam)
        {
            var list = GetAlreadyReservationStudent(groupId, searchParam);
            ViewBag.isView = true;
            return View("_UnReservationGrid", list);
        }

        private PageList<Student> GetAlreadyReservationStudent(string groupId, StudentSearchParam searchParam)
        {
            var trainGroup = EduService.TrainGroup.Get(groupId);
            searchParam.SchoolId = trainGroup.SchoolId;
            searchParam.WorkCategoryId = trainGroup.CategoryId;
            searchParam.TrainGroupId = groupId;
            searchParam.Status = 2;
            ViewBag.trainGroupId = groupId;
            return EduService.Student.GetPageList(PageInfo(), searchParam);
        }

        public ActionResult UnReservationImport(string trainGroupId)
        {
            ViewBag.trainGroupId = trainGroupId;
            return View();
        }

        public ActionResult UnReservationImportSave(string trainGroupId)
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
            var students = ExcelHelper.Import<StudentSimple>(file, true, DocumentFormat.Xlsx);
            results = EduService.Student.FillSimpleId(students);
            var _results = EduService.TrainGroup.UnReservation(trainGroupId, students.Where(p => !String.IsNullOrEmpty(p.Id)).Select(p => p.Id).ToArray());
            results.AddRange(_results);
            return Json(new { success = true, items = results });
        }

        #endregion

    }
}
