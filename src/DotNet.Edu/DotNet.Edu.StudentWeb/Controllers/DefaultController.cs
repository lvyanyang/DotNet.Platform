using DotNet.Edu.Service;
using DotNet.Edu.WebUtility;
using DotNet.Extensions;
using DotNet.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace DotNet.Edu.StudentWeb.Controllers
{
    public class DefaultController : StudentWebController
    {
        public ActionResult Index()
        {
            var session = EduWebHelper.GetStudentSession();
            var list = EduService.StudentCoursewarePeriod.GetList(session.StudentId, session.Student.WorkCategoryId);
            ViewBag.totalPeriod = EduService.Courseware.GetTotalPeriod(session.Student.WorkCategoryId);
            ViewBag.learnPeriod = session.Student.TotalPeriod;
            return View(list);
        }

        public ActionResult Learning(string coursewareId)
        {
            ViewBag.Courseware = EduService.Courseware.Get(coursewareId);
            var detailsList = EduService.CoursewareDetails.GetList(coursewareId);
            return View(detailsList);
        }

        [AllowAnonymous]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult Login(string account, string password, string vcode)
        {
            var validateCode = Session["validateCode"].ToStringOrEmpty();
            if (!vcode.Equals(validateCode, StringComparison.OrdinalIgnoreCase))
            {
                return Json(new { success = false, message = "验证码错误" });
            }
            Session["validateCode"] = null;
            var studentService = EduService.Student;
            var result = studentService.Login(account, password);
            if (result.Success) //登陆成功
            {
                FormsAuthentication.SetAuthCookie(account, false);
                var entity = studentService.GetByIDCardNoOrMobile(account);

                var audit = EduWebHelper.BuildStudentAuditEntity(entity);
                new StudentAuditsService().Create(audit);

                var suser = EduWebHelper.BuildStudentSession(entity);
                EduWebHelper.SetStudentSession(suser);

                return Json(new { success = true, message = "", url = "/" });
            }
            return Json(result);
        }

        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            EduWebHelper.ClearStudentSession();
            return Redirect(FormsAuthentication.LoginUrl);
        }

        [AllowAnonymous]
        public ActionResult CaptchaImage()
        {
            var validateCode = RandomHelper.GenerateRandomString(4);
            Session["validateCode"] = validateCode;
            ValidateCodeDrawHelper v = new ValidateCodeDrawHelper();
            v.FontSize = 28;
            var bmp = v.CreateImage(validateCode);
            return File(ImageHelper.ToArray(bmp), "image/png");
        }
    }
}