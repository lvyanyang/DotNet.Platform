using DotNet.Edu.Service;
using System.Web.Mvc;

namespace DotNet.StudentWeb.Controllers
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