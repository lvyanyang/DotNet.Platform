// ===============================================================================
// DotNet.Platform 开发框架 2016 版权所有
// ===============================================================================

using System.Web.Mvc;
using DotNet.Auth.WebUtility;
using DotNet.Edu.Entity;
using DotNet.Edu.Service;
using DotNet.Edu.Utility;
using DotNet.Extensions;
using DotNet.Helper;
using System;
using DotNet.Auth.Utility;

namespace DotNet.Edu.WebUtility
{
    public static class HtmlExtensions
    {
        /// <summary>
        /// 获取用户分类下拉选项
        /// </summary>
        /// <param name="helper"></param>
        /// <param name="selected">选中的值</param>
        public static MvcHtmlString UserCategoryOption(this HtmlHelper helper, string selected = null)
        {
            return MvcHtmlString.Create(AuthWebHelper.BuildItemOption("UserCategory", selected));
        }

        /// <summary>
        /// 获取培训业务分类下拉选项
        /// </summary>
        /// <param name="helper"></param>
        /// <param name="selected">选中的值</param>
        public static MvcHtmlString TrainCategoryOption(this HtmlHelper helper, string selected = null)
        {
            return MvcHtmlString.Create(AuthWebHelper.BuildItemOption("TrainCategory", selected));
        }

        /// <summary>
        /// 获取课件类型下拉选项
        /// </summary>
        /// <param name="helper"></param>
        /// <param name="selected">选中的值</param>
        public static MvcHtmlString CourseTypeOption(this HtmlHelper helper, string selected = null)
        {
            return MvcHtmlString.Create(AuthWebHelper.BuildItemOption(EduDicConst.CourseType, selected));
        }

        /// <summary>
        /// 获取题目类型下拉选项
        /// </summary>
        /// <param name="helper"></param>
        /// <param name="selected">选中的值</param>
        public static MvcHtmlString QuestTypeOption(this HtmlHelper helper, string selected = null)
        {
            return MvcHtmlString.Create(AuthWebHelper.BuildItemOption(EduDicConst.QuestType, selected));
        }

        /// <summary>
        /// 获取题目单元下拉选项
        /// </summary>
        /// <param name="helper"></param>
        /// <param name="selected">选中的值</param>
        public static MvcHtmlString QuestUnitOption(this HtmlHelper helper, string selected = null)
        {
            return MvcHtmlString.Create(AuthWebHelper.BuildItemOption(EduDicConst.QuestUnit, selected));
        }

        /// <summary>
        /// 获取准驾车型下拉选项
        /// </summary>
        /// <param name="helper"></param>
        /// <param name="selected">选中的值</param>
        public static MvcHtmlString DrivingCategoryOption(this HtmlHelper helper, string selected = null)
        {
            return MvcHtmlString.Create(AuthWebHelper.BuildItemOption("DrivingCategory", selected));
        }

        /// <summary>
        /// 获取培训原因下拉选项
        /// </summary>
        /// <param name="helper"></param>
        /// <param name="selected">选中的值</param>
        public static MvcHtmlString TrainReasonCategoryOption(this HtmlHelper helper, string selected = null)
        {
            return MvcHtmlString.Create(AuthWebHelper.BuildItemOption("TrainReasonCategory", selected));
        }

        /// <summary>
        /// 获取学员状态下拉选项
        /// </summary>
        /// <param name="helper"></param>
        /// <param name="selected">选中的值</param>
        public static MvcHtmlString StudentStatusOption(this HtmlHelper helper, string selected = null)
        {
            return MvcHtmlString.Create(AuthWebHelper.BuildItemOption("StudentStatus", selected));
        }

        /// <summary>
        /// 获取培训机构下拉选项
        /// </summary>
        /// <param name="helper"></param>
        /// <param name="selected">选中的值</param>
        /// <param name="isMultiple">是否多选</param>
        public static MvcHtmlString SchoolOption(this HtmlHelper helper, object selected = null, bool isMultiple = false)
        {
            var list = EduService.School.GetSimpleList();
            return MvcHtmlString.Create(WebHelper.GetSelectOptions(list, selected, isMultiple));
        }

        /// <summary>
        /// 获取教师下拉选项
        /// </summary>
        /// <param name="helper"></param>
        /// <param name="schoolId"></param>
        /// <param name="selected">选中的值</param>
        /// <param name="isMultiple">是否多选</param>
        public static MvcHtmlString TeacherOption(this HtmlHelper helper, string schoolId,object selected = null, bool isMultiple = false)
        {
            var list = EduService.Teacher.GetSimpleList(schoolId);
            return MvcHtmlString.Create(WebHelper.GetSelectOptions(list, selected, isMultiple));
        }

        /// <summary>
        /// 获取班级下拉选项
        /// </summary>
        /// <param name="helper"></param>
        /// <param name="selected">选中的值</param>
        /// <param name="isMultiple">是否多选</param>
        public static MvcHtmlString TrainGroupOption(this HtmlHelper helper, object selected = null, bool isMultiple = false)
        {
            var list = EduService.TrainGroup.GetSimpleList();
            return MvcHtmlString.Create(WebHelper.GetSelectOptions(list, selected, isMultiple));
        }

        /// <summary>
        /// 获取企业下拉选项
        /// </summary>
        /// <param name="helper"></param>
        /// <param name="selected">选中的值</param>
        /// <param name="isMultiple">是否多选</param>
        public static MvcHtmlString CompanyOption(this HtmlHelper helper, object selected = null, bool isMultiple = false)
        {
            var list = EduService.Company.GetSimpleList();
            return MvcHtmlString.Create(WebHelper.GetSelectOptions(list, selected, isMultiple));
        }

        /// <summary>
        /// 获取学员状态Label
        /// </summary>
        /// <param name="helper"></param>
        /// <param name="record">学员记录</param>
        public static MvcHtmlString StudentStatusLabel(this HtmlHelper helper, Student record)
        {
            switch (record.Status)
            {
                case 0:
                    return Mvc.HtmlExtensions.LabelWarning(helper, record.StatusName);
                case 1:
                    return Mvc.HtmlExtensions.LabelInfo(helper, record.StatusName);
                case 2:
                    return Mvc.HtmlExtensions.LabelSuccess(helper, record.StatusName);
            }
            return Mvc.HtmlExtensions.LabelPrimary(helper, record.StatusName);
        }

        public static MvcHtmlString IsActiveStudentMenu(this HtmlHelper helper, string action, string controller)
        {
            var dic = helper.ViewContext.RouteData.Values;
            string _controller = string.Empty;
            string _action = string.Empty;
            if (dic.ContainsKey("controller"))
            {
                _controller = dic["controller"].ToStringOrEmpty();
            }
            if (dic.ContainsKey("action"))
            {
                _action = dic["action"].ToStringOrEmpty();
            }
            if (_controller.Equals(controller, StringComparison.OrdinalIgnoreCase) &&
                _action.Equals(action, StringComparison.OrdinalIgnoreCase))
            {
                return MvcHtmlString.Create("h");
            }
            return MvcHtmlString.Empty;
        }
    }
}