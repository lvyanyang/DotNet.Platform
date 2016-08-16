using DotNet.Edu.Service;
using DotNet.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DotNet.Web.Areas.Students.Controllers
{
    public class StudentAuditsController : StudentWebController
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Grid(string name, string category, string enabled)
        {
            var list = EduService.StudentAudits.GetPageList(PageInfo(), CurrentStudent.StudentId);
            return View(list);
        }
    }
}