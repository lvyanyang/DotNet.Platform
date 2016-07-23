// ===============================================================================
// DotNet.Platform 开发框架 2016 版权所有
// ===============================================================================
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using DotNet.Auth.Entity;
using DotNet.Auth.Service;
using DotNet.Entity;
using DotNet.Extensions;
using DotNet.Helper;

namespace DotNet.Auth.WebUtility
{
    public static class AuthWebHelper
    {
        /// <summary>
        /// 生成数据字典下拉选项
        /// </summary>
        /// <param name="dicCode">数据字典编码</param>
        /// <param name="selected">选中的值</param>
        /// <param name="isMultiple">是否多选</param>
        /// <returns></returns>
        public static string BuildItemOption(string dicCode, string selected = null, bool isMultiple = false)
        {
            var dicId = AuthService.Dic.GetByCode(dicCode)?.Id;
            if (string.IsNullOrEmpty(dicId))
            {
                return string.Empty;
            }
            var simpleList = AuthService.DicDetail.GetSimpleList(dicId);
            return WebHelper.GetSelectOptions(simpleList, selected, isMultiple);
        }

        /// <summary>
        /// 生成复选框组
        /// </summary>
        /// <param name="itemCode">字典编码</param>
        /// <param name="controlName">控件名称</param>
        /// <param name="isDisabled">是否禁用</param>
        /// <param name="selected">选中的值</param>
        /// <returns></returns>
        public static string BuildItemCheckBox(string itemCode, string controlName, bool isDisabled = false, string selected = null)
        {
            return BuildItemBox(itemCode, controlName, true, isDisabled, selected);
        }

        /// <summary>
        /// 生成单选框组
        /// </summary>
        /// <param name="itemCode">字典编码</param>
        /// <param name="controlName">控件名称</param>
        /// <param name="isDisabled">是否禁用</param>
        /// <param name="selected">选中的值</param>
        /// <returns></returns>
        public static string BuildItemRadioBox(string itemCode, string controlName, bool isDisabled = false, string selected = null)
        {
            return BuildItemBox(itemCode, controlName, false, isDisabled, selected);
        }

        private static string BuildItemBox(string itemCode, string controlName, bool isCheckbox, bool isDisabled = false, string selected = null)
        {
            var entity = AuthService.Dic.GetByCode(itemCode);
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(itemCode), "数据字典编码不允许为空");
            }
            var detailsList = AuthService.DicDetail.GetList(entity.Id);
            var options = new StringBuilder();
            string listClass = "radio-list";
            string inlineClass = "radio-inline";
            string controlClass = "iradiobox-control";
            string controlType = "radio";
            if (isCheckbox)
            {
                listClass = "checkbox-list";
                inlineClass = "checkbox-inline";
                controlClass = "icheckbox-control";
                controlType = "checkbox";
            }

            options.AppendFormat($"<div class=\"{listClass}\">");
            string[] valueArray = null;
            if (!string.IsNullOrEmpty(selected))
            {
                valueArray = StringHelper.ConvertStringToArray(selected);
            }
            foreach (DicDetail item in detailsList)
            {
                string name = item.Name;
                string value = item.Value.ToStringOrEmpty();
                //bool isChecked = selected.ToStringOrEmpty().Contains(value.ToStringOrEmpty());
                bool isChecked = valueArray != null && valueArray.Length > 0
                                 && valueArray.Any(p => p.Equals(value.ToStringOrEmpty()));
                string _checked = isChecked ? "checked" : string.Empty;
                string _disabled = isDisabled ? "disabled" : string.Empty;
                options.AppendFormat($"<label class=\"{inlineClass}\">");
                options.AppendFormat($"<input class=\"{controlClass}\" name=\"{controlName}\" value=\"{value}\" type=\"{controlType}\" {_checked} {_disabled} />");
                options.AppendFormat($"<span> {name}</span>");
                options.AppendFormat("</label>");
            }
            options.Append("</div>");
            return options.ToString();
        }
        
    }
}