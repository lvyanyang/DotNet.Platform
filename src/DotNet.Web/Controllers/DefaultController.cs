// ===============================================================================
// DotNet.Platform 开发框架 2016 版权所有
// ===============================================================================
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using DotNet.Auth.Controllers;
using DotNet.Auth.Service;
using DotNet.Auth.Utility;
using DotNet.Configuration;
using DotNet.Helper;
using DotNet.Mvc;
using DotNet.Utility;

namespace DotNet.Web.Controllers
{
    public class DefaultController : AuthController
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Menu()
        {
            return Json(CurrentSessionUser.MenuNodes);
        }

        [AllowAnonymous]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult Login(string account, string password)
        {
            var userService = AuthService.User;
            var result = userService.Login(account, password);
            if (result.Success) //登陆成功
            {
                FormsAuthentication.SetAuthCookie(account, false);
                var entity = userService.GetByAccount(account);

                var audit = AuthHelper.BuildAuditEntity(1, entity);
                AuthService.Audit.Create(audit);

                var suser = AuthHelper.BuildSessionUser(AuthService.User.GetByAccount(account));
                AuthHelper.SetSessionUser(suser);

                return Json(new { success = true, message = "登陆成功", url = "/" });
            }
            return Json(result);
        }

        public ActionResult Logout()
        {
            //new OnlineUserService().Logout(CurrentUserId);
            //OrganizeHelper.Logout();
            FormsAuthentication.SignOut();

            var audit = AuthHelper.BuildAuditEntity(2, AuthHelper.GetSessionUser()?.User);
            AuthService.Audit.Create(audit);
            return Redirect(FormsAuthentication.LoginUrl);
        }

        [AllowAnonymous]
        public ActionResult ServerDateTime()
        {
            //return Json(new BoolMessage(true, DateTimeHelper.FormatDateHasMinute(DateTime.Now)));

            var list = new List<JsonMessage>();
            list.Add(new JsonMessage(true, "测试成功"));
            list.Add(new JsonMessage(false, "你的操作台扯淡了"));
            return Json(new { success = true, items = list });
            //System.Threading.Thread.Sleep(3000);
            //return Json(new BoolMessage(true, DateTimeHelper.FormatDateHasMinute(DateTime.Now)));
        }

        [AllowAnonymous]
        public ActionResult Test()
        {
            return View();
        }
    }
}