using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DotNet.Edu.Entity;
using DotNet.Edu.Service;
using DotNet.Utility;

namespace DotNet.Edu.StudentWeb.Controllers
{
    public class StudentLearningController : StudentWebController
    {
        public ActionResult Begin(string coursewareId)
        {
            var session = CurrentStudent;
            var student = session.Student;
            PeriodDetails entity = new PeriodDetails();
            entity.CoursewareId = coursewareId;
            entity.SchoolId = student.SchoolId;
            entity.SchoolName = student.SchoolName;
            entity.StudentId = student.Id;
            entity.StudentName = student.Name;
            entity.StudyCategory = session.Device;
            entity.IPAddress = session.LoginIPAddr;
            entity.SignInDateTime = DateTime.Now;
            session.Learning = entity;
            return Json(BoolMessage.True);
        }

        public ActionResult Commit()
        {
            var session = CurrentStudent;
            if (session.Learning != null)
            {
                var entity = session.Learning;
                entity.SignOutDateTime = DateTime.Now;
                entity.Period = Convert.ToInt32((entity.SignOutDateTime - entity.SignInDateTime).TotalSeconds);
                entity.CreateDateTime = DateTime.Now;

                EduService.PeriodDetails.Create(entity);
                EduService.StudentCoursewarePeriod.Save(entity.StudentId,entity.CoursewareId,entity.Period);
                session.Student.TotalPeriod = EduService.StudentCoursewarePeriod.GetStudentPeriod(entity.StudentId);
                EduService.Student.UpdateTotalPeriod(entity.StudentId, session.Student.TotalPeriod);
            }

            return Json(BoolMessage.True);
        }
    }
}