using DotNet.Edu.Service;
using DotNet.Edu.WebUtility;
using DotNet.Extensions;
using DotNet.Helper;
using System;
using System.Web.Mvc;
using System.Web.Security;
using DotNet.Mvc;

namespace DotNet.StudentWeb.Controllers
{
    public class DefaultController : StudentWebController
    {
        public ActionResult Index()
        {
            var session = EduWebHelper.GetStudentSession();
            var list = EduService.StudentCoursewarePeriod.GetList(session.StudentId, session.Student.WorkCategoryId);
            ViewBag.totalPeriod = EduService.Courseware.GetTotalPeriod(session.Student.WorkCategoryId);
            ViewBag.learnPeriod = session.Student.TotalPeriod;
            ViewBag.lastPeriodDetails = EduService.PeriodDetails.GetLast();
            return View(list);
        }

        public ActionResult Learning(string coursewareId)
        {
            CurrentStudent.Learning = null;
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

        public ActionResult ModifyUserPassword()
        {
            return View();
        }

        public ActionResult VideoPlay(string url)
        {
            ViewBag.url = UploadHelper.GetUploadUrl(url);
            return View();
        }

        [HttpPost]
        public ActionResult ModifyUserPassword(string currentPwd, string newPwd)
        {
            if (!EduService.Student.ValidUserPassword(CurrentStudent.StudentId, currentPwd))
            {
                return Json(false, "当前密码输入不正确,请重新输入");
            }
            var result = EduService.Student.UpdatePassword(CurrentStudent.StudentId, newPwd);
            return Json(result);
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

        [AllowAnonymous]
        public ActionResult VideoImage(string name)
        {
            ValidateCodeDrawHelper v = new ValidateCodeDrawHelper();
            v.FontSize = 28;
            var bmp = v.CreateImage(name);
            return File(ImageHelper.ToArray(bmp), "image/png");
        }

        //[AllowAnonymous]
        //public ActionResult UploadTest()
        //{
        //    return View();
        //}

        //[AllowAnonymous]
        //public ActionResult UploadSave()
        //{
        //    var a = UploadHelper.Upload(Request.Files[0],"test/a");
        //    var u = UploadHelper.GetWebUrl(a.Url);
        //    return View();
        //}
    }
}