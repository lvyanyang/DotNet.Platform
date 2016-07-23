// ===============================================================================
// DotNet.Platform 开发框架 2016 版权所有
// ===============================================================================
using System.Web.Mvc;
using DotNet.Auth.Service;
using DotNet.Helper;

namespace DotNet.Auth.WebUtility
{
    public static class HtmlExtensions
    {
        /// <summary>
        /// 获取角色分类下拉选项
        /// </summary>
        /// <param name="helper"></param>
        /// <param name="selected">选中的值</param>
        public static MvcHtmlString RoleCategoryOption(this HtmlHelper helper, string selected = null)
        {
            return MvcHtmlString.Create(AuthWebHelper.BuildItemOption("RoleCategory", selected));
        }

        /// <summary>
        /// 获取参数分类下拉选项
        /// </summary>
        /// <param name="helper"></param>
        /// <param name="selected">选中的值</param>
        public static MvcHtmlString ParamCategoryOption(this HtmlHelper helper, string selected = null)
        {
            return MvcHtmlString.Create(AuthWebHelper.BuildItemOption("ParamCategory", selected));
        }

        /// <summary>
        /// 获取角色下拉选项
        /// </summary>
        /// <param name="helper"></param>
        /// <param name="selected">选中的值</param>
        /// <param name="isMultiple">是否多选</param>
        public static MvcHtmlString SystemRoleOption(this HtmlHelper helper, object selected = null, bool isMultiple = false)
        {
            var list = AuthService.Role.GetSimpleList();
            return MvcHtmlString.Create(WebHelper.GetSelectOptions(list, selected, isMultiple));
        }

        /// <summary>
        /// 获取用户下拉选项
        /// </summary>
        /// <param name="helper"></param>
        /// <param name="selected">选中的值</param>
        /// <param name="isMultiple">是否多选</param>
        public static MvcHtmlString SystemUserOption(this HtmlHelper helper, object selected = null, bool isMultiple = false)
        {
            var list = AuthService.User.GetSimpleList();
            return MvcHtmlString.Create(WebHelper.GetSelectOptions(list, selected, isMultiple));
        }

        /// <summary>
        /// 获取部门下拉选项
        /// </summary>
        /// <param name="helper"></param>
        /// <param name="selected">选中的值</param>
        /// <param name="isMultiple">是否多选</param>
        public static MvcHtmlString SystemDeptOption(this HtmlHelper helper, string selected = null, bool isMultiple = false)
        {
            var list = AuthService.Department.GetSimpleList();
            return MvcHtmlString.Create(WebHelper.GetSelectOptions(list, selected, isMultiple));
        }

        /// <summary>
        /// 密码提示问题下拉选项
        /// </summary>
        /// <param name="helper"></param>
        /// <param name="selected">选中的值</param>
        /// <param name="isMultiple">是否多选</param>
        public static MvcHtmlString HintQuestionOption(this HtmlHelper helper, string selected = null, bool isMultiple = false)
        {
            return MvcHtmlString.Create(AuthWebHelper.BuildItemOption("HintQuestion", selected));
        }
    }
}