// ===============================================================================
// DotNet.Platform 开发框架 2016 版权所有
// ===============================================================================
using System.Collections;
using System.ComponentModel;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Web.Routing;
using DotNet.Collections;
using DotNet.Helper;
using DotNet.Extensions;

namespace DotNet.Mvc
{
    /// <summary>
    /// Mvc实体操作帮助类
    /// </summary>
    public static class HtmlExtensions
    {
        //private readonly static Regex HrefRegex = new Regex("\"[^\"]*\"", RegexOptions.IgnoreCase | RegexOptions.Compiled);

        /// <summary>
        /// 生成分页
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="helper">HtmlHelper</param>
        /// <param name="source">数据源</param>
        public static MvcHtmlString RenderPages<T>(this HtmlHelper helper, PageList<T> source)
        {
            if (source == null || source.Count <= 0)
            {
                return MvcHtmlString.Empty;
            }

            var pagetemplate = @"
<div class=""row paginate-area"">
    <div class=""col-sm-4"">
        <div class=""table-info"">{0}</div>
    </div>
    <div class=""col-sm-8"">
        <div class=""table-info-paginate paging_bootstrap"">
            <ul class=""pagination""  style=""margin-right:0"">
                {1}
            </ul>
        </div>
    </div>
</div>";
            int showPages = 11;
            int pageIndex = source.PageIndex;
            int totalPages = source.TotalPages;

            string pagedInfo = (source.RecordEndIndex == 0 && source.RecordEndIndex == 0) ?
                $"共 {source.TotalCount} 条" :
                $"共 {source.TotalCount} 条 当前显示 {source.RecordStartIndex} 到 {source.RecordEndIndex} 条";

            if (totalPages <= showPages)
            {
                return MvcHtmlString.Create(string.Format(pagetemplate, pagedInfo, BuildPage(1, totalPages, pageIndex)));
            }

            var pages = new StringBuilder();

            if (pageIndex <= 7)
            {
                pages.Append(BuildPage(1, 11, pageIndex));
                pages.Append("<li class=\"disabled\"><span>···</span></li>");
                pages.Append(BuildPage(totalPages, totalPages, pageIndex));
            }
            else if (pageIndex >= (totalPages - 7))
            {
                pages.Append(BuildPage(1, 1, pageIndex));
                pages.Append("<li class=\"disabled\"><span>···</span></li>");
                pages.Append(BuildPage(totalPages - 10, totalPages, pageIndex));
            }
            else
            {
                pages.Append(BuildPage(1, 1, pageIndex));
                pages.Append("<li class=\"disabled\"><span>···</span></li>");
                pages.Append(BuildPage(pageIndex - 5, pageIndex + 5, pageIndex));
                pages.Append("<li class=\"disabled\"><span>···</span></li>");
                pages.Append(BuildPage(totalPages, totalPages, pageIndex));
            }

            return MvcHtmlString.Create(string.Format(pagetemplate, pagedInfo, pages));
        }

        /// <summary>
        /// 生成页面链接
        /// </summary>
        private static string BuildPage(int start, int end, int currentPageIndex)
        {
            var sb = new StringBuilder();
            for (int i = start; i <= end; i++)
            {
                sb.Append(currentPageIndex == i
                    ? $"<li class=\"active\" title=\"第{i}页\"><span>{i}</span></li>"
                    : $"<li title=\"第{i}页\"><a data-page=\"{i}\">{i}</a></li>");
            }
            return sb.ToString();
        }

        /// <summary>
        /// 生成排序图标
        /// </summary>
        /// <param name="helper"></param>
        /// <param name="fieldName">字段名称</param>
        public static MvcHtmlString BuildOrderImage(this HtmlHelper helper, string fieldName)
        {
            var order = HttpContext.Current.Request.Params["order"].ToStringOrEmpty();
            if (!fieldName.Equals(order, System.StringComparison.OrdinalIgnoreCase))
            {
                return MvcHtmlString.Create(@"<i class=""glyphicon glyphicon-sort"" style =""opacity: 0.2"" title=""点击排序""></i>");
            }
            var dir = HttpContext.Current.Request.Params["dir"].ToStringOrEmpty();
            if (dir.Equals("desc", System.StringComparison.OrdinalIgnoreCase))
            {
                return MvcHtmlString.Create(@"<i class=""fa fa-sort-amount-desc"" title=""倒序排序""></i>");
            }
            return MvcHtmlString.Create(@"<i class=""fa fa-sort-amount-asc"" title=""升序排序""></i>");
        }

        /// <summary>
        /// 获取Query参数路由
        /// </summary>
        /// <param name="helper"></param>
        /// <param name="addValues">附加的对象</param>
        /// <returns></returns>
        public static RouteValueDictionary GetQueryRouteValuesNoId(this HtmlHelper helper, object addValues = null)
        {
            return WebHelper.GetRouteValues(helper.ViewContext.RouteData.Values, addValues, true, false, "id");
        }

        /// <summary>
        /// 获取Query参数路由
        /// </summary>
        /// <param name="helper"></param>
        /// <param name="addValues">附加的对象</param>
        /// <param name="removeNames">移除的参数数组</param>
        /// <returns></returns>
        public static RouteValueDictionary GetQueryRouteValues(this HtmlHelper helper, object addValues = null, params string[] removeNames)
        {
            return WebHelper.GetRouteValues(helper.ViewContext.RouteData.Values, addValues, true, false, removeNames);
        }

        /// <summary>
        /// 获取Form参数路由
        /// </summary>
        /// <param name="helper"></param>
        /// <param name="addValues">附加的对象</param>
        /// <param name="removeNames">移除的参数数组</param>
        /// <returns></returns>
        public static RouteValueDictionary GetFormRouteValues(this HtmlHelper helper, object addValues = null, params string[] removeNames)
        {
            return WebHelper.GetRouteValues(helper.ViewContext.RouteData.Values, addValues, false, true, removeNames);
        }

        /// <summary>
        /// 获取当前路由参数
        /// </summary>
        /// <param name="helper"></param>
        /// <param name="addValues">附加的对象</param>
        /// <param name="removeNames">移除的参数数组</param>
        /// <returns></returns>
        public static RouteValueDictionary GetCurrentRouteValues(this HtmlHelper helper, object addValues = null, params string[] removeNames)
        {
            return WebHelper.GetRouteValues(helper.ViewContext.RouteData.Values, addValues, true, true, removeNames);
        }

        public static string GetCurrentController(this HtmlHelper helper)
        {
            var dic = helper.ViewContext.RouteData.Values;
            if (dic.ContainsKey("controller"))
            {
                return dic["controller"].ToStringOrEmpty();
            }
            return string.Empty;
        }

        public static string GetCurrentAction(this HtmlHelper helper)
        {
            var dic = helper.ViewContext.RouteData.Values;
            if (dic.ContainsKey("action"))
            {
                return dic["action"].ToStringOrEmpty();
            }
            return string.Empty;
        }
        
        /// <summary>
        /// IIF函数
        /// </summary>
        /// <param name="helper"></param>
        /// <param name="result">判断条件</param>
        /// <param name="trueMsg">符合条件输出的字符串</param>
        /// <param name="falseMsg">不符合条件时输出的字符串</param>
        /// <returns></returns>
        public static MvcHtmlString IIF(this HtmlHelper helper, bool result, string trueMsg, string falseMsg = null)
        {
            if (result)
            {
                return MvcHtmlString.Create(trueMsg);
            }
            return MvcHtmlString.Create(falseMsg);
        }

        /// <summary>
        /// 输出字符串
        /// </summary>
        /// <param name="helper"></param>
        /// <param name="value">输出值</param>
        /// <returns></returns>
        public static MvcHtmlString WriteValue(this HtmlHelper helper, object value)
        {
            if (value.IsEmpty())
            {
                return MvcHtmlString.Create("&nbsp;");
            }
            return MvcHtmlString.Create(value.ToStringOrEmpty());
        }

        /// <summary>
        /// 输出字符串
        /// </summary>
        public static MvcHtmlString WriteValue(this HtmlHelper helper, bool result,string value)
        {
            if (result)
            {
                return MvcHtmlString.Create(value);
            }
            return MvcHtmlString.Empty;
        }

        /// <summary>
        /// 输出active字符串,是否输出,否则返回MvcHtmlString.Empty.
        /// </summary>
        /// <param name="helper"></param>
        /// <param name="isActive">是否输出</param>
        /// <returns></returns>
        public static MvcHtmlString IsActive(this HtmlHelper helper, bool isActive)
        {
            return isActive ? MvcHtmlString.Create("active") : MvcHtmlString.Empty;
        }

        /// <summary>
        /// 输出disabled字符串,是否输出,否则返回MvcHtmlString.Empty.
        /// </summary>
        /// <param name="helper"></param>
        /// <param name="isDisabled">是否输出</param>
        /// <returns></returns>
        public static MvcHtmlString IsDisabled(this HtmlHelper helper, bool isDisabled)
        {
            return isDisabled ? MvcHtmlString.Create("disabled") : MvcHtmlString.Empty;
        }

        /// <summary>
        /// 输出disabled字符串,是否输出,否则返回MvcHtmlString.Empty.
        /// </summary>
        /// <param name="helper"></param>
        /// <param name="isDisabled">是否输出</param>
        /// <returns></returns>
        public static MvcHtmlString IsDisabled(this HtmlHelper helper, bool? isDisabled)
        {
            return isDisabled.HasValue ? IsDisabled(helper, isDisabled.Value) : MvcHtmlString.Empty;
        }

        /// <summary>
        /// 输出checked字符串,是否输出,否则返回MvcHtmlString.Empty.
        /// </summary>
        /// <param name="helper"></param>
        /// <param name="isChecked">是否输出</param>
        /// <returns></returns>
        public static MvcHtmlString IsChecked(this HtmlHelper helper, bool isChecked)
        {
            return isChecked ? MvcHtmlString.Create("checked") : MvcHtmlString.Empty;
        }

        /// <summary>
        /// 输出checked字符串,是否输出,否则返回MvcHtmlString.Empty.
        /// </summary>
        /// <param name="helper"></param>
        /// <param name="isChecked">是否输出</param>
        /// <returns></returns>
        public static MvcHtmlString IsChecked(this HtmlHelper helper, bool? isChecked)
        {
            return isChecked.HasValue ? IsChecked(helper, isChecked.Value) : MvcHtmlString.Empty;
        }

        /// <summary>
        /// 输出 selected 字符串,是否输出,否则返回MvcHtmlString.Empty.
        /// </summary>
        /// <param name="helper"></param>
        /// <param name="isSelected">是否输出</param>
        /// <returns></returns>
        public static MvcHtmlString IsSelected(this HtmlHelper helper, bool isSelected)
        {
            return isSelected ? MvcHtmlString.Create("selected") : MvcHtmlString.Empty;
        }

        /// <summary>
        /// 输出 selected 字符串,是否输出,否则返回MvcHtmlString.Empty.
        /// </summary>
        /// <param name="helper"></param>
        /// <param name="isSelected">是否输出</param>
        /// <returns></returns>
        public static MvcHtmlString IsSelected(this HtmlHelper helper, bool? isSelected)
        {
            return isSelected.HasValue ? IsSelected(helper, isSelected.Value) : MvcHtmlString.Empty;
        }

        /// <summary>
        /// 输出 selected 字符串,是否输出,否则返回MvcHtmlString.Empty.
        /// </summary>
        /// <param name="helper"></param>
        /// <param name="isReadonly">是否输出</param>
        /// <returns></returns>
        public static MvcHtmlString IsReadonly(this HtmlHelper helper, bool isReadonly)
        {
            return isReadonly ? MvcHtmlString.Create("readonly") : MvcHtmlString.Empty;
        }

        /// <summary>
        /// 输出 selected 字符串,是否输出,否则返回MvcHtmlString.Empty.
        /// </summary>
        /// <param name="helper"></param>
        /// <param name="isReadonly">是否输出</param>
        /// <returns></returns>
        public static MvcHtmlString IsReadonly(this HtmlHelper helper, bool? isReadonly)
        {
            return isReadonly.HasValue ? IsReadonly(helper, isReadonly.Value) : MvcHtmlString.Empty;
        }

        /// <summary>
        /// 输出空Option (<option value="">全部</option>)
        /// </summary>
        /// <param name="helper"></param>
        /// <param name="name">默认显示值</param>
        /// <returns></returns>
        public static MvcHtmlString EmptyOption(this HtmlHelper helper, string name = "　")
        {
            return MvcHtmlString.Create($"<option value=\" \">{name}</option>");
        }

        public static MvcHtmlString LabelDefault(this HtmlHelper helper, string lable)
        {
            return LabelCore("label-default", lable);
        }

        public static MvcHtmlString LabelPrimary(this HtmlHelper helper, string lable)
        {
            return LabelCore("label-primary", lable);
        }

        public static MvcHtmlString LabelSuccess(this HtmlHelper helper, string lable)
        {
            return LabelCore("label-success", lable);
        }

        public static MvcHtmlString LabelInfo(this HtmlHelper helper, string lable)
        {
            return LabelCore("label-info", lable);
        }

        public static MvcHtmlString LabelWarning(this HtmlHelper helper, string lable)
        {
            return LabelCore("label-warning", lable);
        }

        public static MvcHtmlString LabelDanger(this HtmlHelper helper, string lable)
        {
            return LabelCore("label-danger", lable);
        }

        private static MvcHtmlString LabelCore(string className, string lable)
        {
            return MvcHtmlString.Create($"<span class=\"label {className}\">{lable}</span>");
        }

        /// <summary>
        /// 输出一个标签
        /// </summary>
        /// <param name="helper"></param>
        /// <param name="result">判断表达式</param>
        /// <param name="trueLable">成功输出的字符串</param>
        /// <param name="falseLable">失败输出的字符串</param>
        /// <returns></returns>
        public static MvcHtmlString BoolLabel(this HtmlHelper helper, bool result, string trueLable = "启用", string falseLable = "禁用")
        {
            if (result)
            {
                return MvcHtmlString.Create("<span class=\"label label-success\">" + trueLable + "</span>");
            }
            return MvcHtmlString.Create("<span class=\"label label-danger\">" + falseLable + "</span>");
        }


        /// <summary>
        /// 输出一个标签
        /// </summary>
        /// <param name="helper"></param>
        /// <param name="result">判断表达式</param>
        /// <param name="trueLable">成功输出的字符串</param>
        /// <param name="falseLable">失败输出的字符串</param>
        /// <returns></returns>
        public static MvcHtmlString BoolLabel(this HtmlHelper helper, bool? result, string trueLable = "启用", string falseLable = "禁用")
        {
            return !result.HasValue ? MvcHtmlString.Create(string.Empty) : BoolLabel(helper, result.Value, trueLable, falseLable);
        }

        /// <summary>
        /// 标记新建记录
        /// </summary>
        /// <param name="helper"></param>
        /// <returns></returns>
        public static MvcHtmlString MarkCreate(this HtmlHelper helper)
        {
            return MvcHtmlString.Create("<input type=\"hidden\" name=\"" + MvcHelper.RecordCreate + "\" value=\"" + helper.ViewData[MvcHelper.RecordCreate] + "\" />");
        }

        /// <summary>
        /// 是否新建记录
        /// </summary>
        /// <param name="helper"></param>
        /// <returns></returns>
        public static bool IsCreate(this HtmlHelper helper)
        {
            return helper.ViewData[MvcHelper.RecordCreate].ToStringOrEmpty().Equals("1");
        }

        /// <summary>
        /// 返回编辑状态字符串
        /// </summary>
        /// <param name="helper"></param>
        /// <param name="joinStr">连接的字符串</param>
        /// <returns></returns>
        public static MvcHtmlString EditStatus(this HtmlHelper helper, string joinStr)
        {
            var v = helper.ViewData[MvcHelper.RecordCreate];
            if (v != null && v.ToString().Equals("1"))
            {
                return MvcHtmlString.Create("新建" + joinStr);
            }
            return MvcHtmlString.Create("修改" + joinStr);
        }
    }
}