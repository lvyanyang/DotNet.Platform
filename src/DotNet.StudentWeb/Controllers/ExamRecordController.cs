using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DotNet.Edu.Service;

namespace DotNet.StudentWeb.Controllers
{
    public class ExamRecordController : StudentWebController
    {
        // GET: ExamRecord
        public ActionResult Index()
        {
            var list = EduService.ExamRecord.GetList(CurrentStudent.Student.IDCardNo);
            return View(list);
        }
    }
}