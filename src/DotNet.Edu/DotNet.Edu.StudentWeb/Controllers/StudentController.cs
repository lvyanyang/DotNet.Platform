using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DotNet.Edu.Service;

namespace DotNet.Edu.StudentWeb.Controllers
{
    public class StudentController : StudentWebController
    {
        public ActionResult TrainHistory()
        {
            var list = EduService.Student.GetTrainHistoryList(CurrentStudent.StudentId);
            return View(list);
        }
    }
}