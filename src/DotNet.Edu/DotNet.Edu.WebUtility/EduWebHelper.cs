// ===============================================================================
// DotNet.Platform 开发框架 2016 版权所有
// ===============================================================================
using DotNet.Edu.Entity;
using DotNet.Edu.Service;
using DotNet.Helper;
using System;
using System.Web;

namespace DotNet.Edu.WebUtility
{
    public static class EduWebHelper
    {
        /// <summary>
        /// 生成审计日志对象
        /// </summary>
        /// <param name="student">学员对象</param>
        public static StudentAudits BuildStudentAuditEntity(Student student)
        {
            var request = HttpContext.Current.Request;
            var ipData = IPHelper.GetInternetIP(request);
            return new StudentAudits
            {
                Id = StringHelper.Guid(),
                StudentId = student?.Id,
                StudentName = student?.Name,
                LoginDateTime = DateTime.Now,
                 
                AreaAddress = $"{ipData.Region}{ipData.City} {ipData.Isp}",
                IPAddress = ipData.Ip,

                Browser = WebHelper.GetFormString("browser"),
                Device = WebHelper.GetFormString("device"),
                OS = WebHelper.GetFormString("os"),
                UserAgent = request.UserAgent
            };
        }

        /// <summary>
        /// 生成会话用户对象
        /// </summary>
        /// <param name="student">学员对象</param>
        public static StudentSession BuildStudentSession(Student student)
        {
            var entity = new StudentSession();
            entity.StudentId = student.Id;
            entity.StudentName = student.Name;
            entity.LoginIPAddr = WebHelper.GetFormString("ip", HttpContext.Current.Request.UserHostAddress);
            entity.Device = WebHelper.GetFormString("device");
            entity.LoginDateTime = DateTime.Now;
            entity.Student = student;
            return entity;
        }

        /// <summary>
        /// 清空会话用户
        /// </summary>
        public static void ClearStudentSession()
        {
            var key = "_dotnet_edu_user_";
            HttpContext.Current.Session[key] = null;
        }

        /// <summary>
        /// 设置会话用户
        /// </summary>
        /// <param name="session">用户对象</param>
        public static void SetStudentSession(StudentSession session)
        {
            var key = "_dotnet_edu_user_";
            HttpContext.Current.Session[key] = session;
        }

        /// <summary>
        /// 获取会话用户
        /// </summary>
        public static StudentSession GetStudentSession()
        {
            var key = "_dotnet_edu_user_";
            if (HttpContext.Current.Session[key] == null)
            {
                var account = HttpContext.Current.User.Identity.Name;
                if (string.IsNullOrEmpty(account))
                {
                    return null;
                }
                var suser = BuildStudentSession(EduService.Student.GetByIDCardNoOrMobile(account));
                HttpContext.Current.Session[key] = suser;
            }
            return HttpContext.Current.Session[key] as StudentSession;
        }
    }
}