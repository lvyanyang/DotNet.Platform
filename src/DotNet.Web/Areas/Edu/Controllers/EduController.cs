// ===============================================================================
// DotNet.Platform 开发框架 2016 版权所有
// ===============================================================================

using DotNet.Auth.Controllers;

namespace DotNet.Edu.Controllers
{
    public class EduController: AuthController
    {
        /// <summary>
        /// 当前用户类型主键
        /// </summary>
        public static string CurrentUserCategoryId
        {
            get { return CurrentUser.UserCategoryId; }
        }

        /// <summary>
        /// 当前用户类型名称
        /// </summary>
        public static string CurrentUserCategoryName
        {
            get { return CurrentUser.UserCategoryName; }
        }

        /// <summary>
        /// 当前企业主键
        /// </summary>
        public static string CurrentCompanyId
        {
            get { return CurrentUser.CompanyId; }
        }

        /// <summary>
        /// 当前企业名称
        /// </summary>
        public static string CurrentCompanyName
        {
            get { return CurrentUser.CompanyName; }
        }

        /// <summary>
        /// 当前培训机构主键
        /// </summary>
        public static string CurrentSchoolId
        {
            get { return CurrentUser.SchoolId; }
        }

        /// <summary>
        /// 当前培训机构名称
        /// </summary>
        public static string CurrentSchoolName
        {
            get { return CurrentUser.SchoolName; }
        }

        /// <summary>
        /// 是否是企业
        /// </summary>
        public static bool IsCompany => CurrentSessionUser.IsCompany;

        /// <summary>
        /// 是否是培训机构
        /// </summary>
        public static bool IsSchool => CurrentSessionUser.IsSchool;

        /// <summary>
        /// 是否是管理者
        /// </summary>
        public static bool IsManager => CurrentSessionUser.IsManager;
    }
}