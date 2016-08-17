using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using DotNet.Auth.Service;
using DotNet.Auth.Utility;

namespace DotNet.EduWeb.Controllers
{
    public class AccountController : AuthController
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
    }
}