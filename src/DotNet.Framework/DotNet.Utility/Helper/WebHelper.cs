// ===============================================================================
// DotNet.Platform 开发框架 2016 版权所有
// ===============================================================================
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel;
using System.IO;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Routing;
using DotNet.Extensions;
using DotNet.Utility;

namespace DotNet.Helper
{
    /// <summary>
    /// Url操作类
    /// </summary>
    /// <example>
    /// <code>
    /// using ZDX.Helper;
    /// 
    /// Response.Write(string.Format("网站根:{0}<br />", WebHelper.GetRootUrl()));//获取网站根Url 
    /// 
    /// string url = WebHelper.AddOrUpdateUrlParam("http://localhost:2114/Default.aspx", "test", "123");//添加或更新Url参数
    /// Response.Write(string.Format("添加参数test:{0}<br />", url));
    /// 
    /// url = WebHelper.AddOrUpdateUrlParam(url, "abc", "456");//添加或更新Url参数
    /// Response.Write(string.Format("添加参数abc:{0}<br />", url));
    /// 
    /// url = WebHelper.AddOrUpdateUrlParam(url, "test", "张三");//添加或更新Url参数
    /// Response.Write(string.Format("更新参数test:{0}<br />", url));
    /// 
    /// Response.Write("参数键值对<br />");
    /// var dic = WebHelper.GetUrlParamDic(url);//获取Url字符串中的参数信息
    /// foreach (KeyValuePair&lt;string, string&gt; pair in dic)
    /// {
    ///     Response.Write(string.Format("参数名:{0},参数值:{1} <br />", pair.Key, pair.Value));
    /// }
    /// Response.Write(string.Format("网站物理地址:{0}<br />", PathHelper.GetWebSitePhysicsPath()));//获取网站物理目录 
    /// Response.Write(string.Format("转为相对路径:{0}<br />", WebHelper.ConvertRelativeUrl("~/Scripts/Javascript/UI.js")));//转为相对路径 
    /// 
    /// /*
    /// 网站根:http://localhost:2114/
    /// 添加参数test:http://localhost:2114/Default.aspx?test=123
    /// 添加参数abc:http://localhost:2114/Default.aspx?test=123&amp;abc=456
    /// 更新参数test:http://localhost:2114/Default.aspx?test=张三&amp;abc=456
    /// 参数键值对
    /// 参数名:test,参数值:张三 
    /// 参数名:abc,参数值:456 
    /// 网站物理地址:D:\软件开发\ProjectPlatform\WebTest
    /// 转为相对路径:Scripts/Javascript/UI.js
    /// */
    /// </code>
    /// </example>
    public static class WebHelper
    {
        /// <summary>
        /// 获取网站根Url
        /// </summary>
        /// <returns>返回例如:http://localhost:2114/</returns>
        public static string GetRootUrl()
        {
            string protocol = HttpContext.Current.Request.ServerVariables["SERVER_PORT_SECURE"];
            if (protocol == null || protocol == "0")
                protocol = "http://";
            else
                protocol = "https://";

            string port = HttpContext.Current.Request.ServerVariables["SERVER_PORT"];
            if (port == null || port == "80" || port == "443")
                port = "";
            else
                port = ":" + port;

            string siteRoot = protocol + HttpContext.Current.Request.ServerVariables["SERVER_NAME"] + port +
                              HttpContext.Current.Request.ApplicationPath;
            return siteRoot;
        }

        /// <summary>
        /// 添加或更新Url参数
        /// </summary>
        /// <param name="url">Url</param>
        /// <param name="paramName">参数名称</param>
        /// <param name="paramValue">参数值</param>
        /// <exception cref="System.ArgumentNullException">无效的 URI: 此 URI 为空。</exception>
        /// <returns>返回更新后的Url</returns>
        public static string AddOrUpdateUrlParam(string url, string paramName, string paramValue)
        {
            if (string.IsNullOrEmpty(url))
            {
                throw new ArgumentNullException("url", "无效的 URI: 此 URI 为空。");
            }
            Uri uri = new Uri(url);
            bool isAdd = true;

            string keyWord = paramName + "=";
            if (!string.IsNullOrEmpty(uri.Query) && url.IndexOf(keyWord, StringComparison.OrdinalIgnoreCase) > -1)
            {
                isAdd = false;
            }
            if (isAdd)
            {
                string connFix = "?";
                if (!string.IsNullOrEmpty(uri.Query))
                {
                    connFix = "&";
                }
                string eval = HttpContext.Current.Server.UrlEncode(paramValue);
                return String.Concat(url, connFix, paramName, "=", eval);
            }
            //更新Url
            int index = url.IndexOf(keyWord, StringComparison.OrdinalIgnoreCase) + keyWord.Length;
            int index1 = url.IndexOf("&", index, StringComparison.Ordinal);
            if (index1 == -1)
            {
                url = url.Remove(index, url.Length - index);
                url = string.Concat(url, paramValue);
                return url;
            }
            url = url.Remove(index, index1 - index);
            url = url.Insert(index, paramValue);
            return url;
        }

        /// <summary>
        /// 获取Url字符串中的参数信息
        /// </summary>
        /// <param name="url">Url</param>
        /// <exception cref="System.ArgumentNullException">无效的 URI: 此 URI 为空。</exception>
        /// <returns>返回Url中的参数字典</returns>
        public static Dictionary<string, string> GetUrlParamDic(string url)
        {
            if (string.IsNullOrEmpty(url))
            {
                throw new ArgumentNullException("url", "无效的 URI: 此 URI 为空。");
            }

            Dictionary<string, string> dic = new Dictionary<string, string>();
            int questionMarkIndex = url.IndexOf('?');

            if (questionMarkIndex == -1)
            {
                return dic;
            }
            string ps = url.Substring(questionMarkIndex + 1);

            // 开始分析参数对    
            Regex re = new Regex(@"(^|&)?(\w+)=([^&]+)(&|$)?", RegexOptions.Compiled);
            MatchCollection mc = re.Matches(ps);

            foreach (Match m in mc)
            {
                dic.Add(m.Result("$2").ToLower(), m.Result("$3"));
            }
            return dic;
        }

        /// <summary>
        /// 返回上一个页面的地址
        /// </summary>
        /// <returns>上一个页面的地址</returns>
        public static string GetUrlReferrer()
        {
            string retVal = null;
            try
            {
                if (HttpContext.Current.Request.UrlReferrer != null)
                    retVal = HttpContext.Current.Request.UrlReferrer.ToString();
            }
            catch
            {
                return string.Empty;
            }
            return retVal;
        }

        /// <summary>
        /// 获取web客户端ip地址
        /// </summary>
        /// <returns></returns>
        public static string GetClientInnerIP()
        {
            string ip = System.Web.HttpContext.Current.Request.UserHostAddress;
            //或 string ip = System.Web.HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
            return ip;
        }

        /// <summary>
        /// 获得客户端外网IP地址
        /// </summary>
        /// <returns>IP地址</returns>
        public static string GetClientInternetIP()
        {
            string ip;
            using (WebClient webClient = new WebClient())
            {
                var content = webClient.DownloadString("http://www.ip138.com/ips1388.asp"); //站获得IP的网页
                //判断IP是否合法
                ip = new Regex(@"\[((\d{1,3}\.){3}\d{1,3})\]").Match(content).Groups[1].Value;
            }
            return ip;
        }

        /// <summary>
        /// 获取Request.QueryString参数
        /// </summary>
        /// <param name="key">键值</param>
        /// <returns>如果不存在指定key,返回空字符串</returns>
        public static string GetQueryString(string key)
        {
            return GetQueryString(key, string.Empty);
        }

        /// <summary>
        /// 获取Request.QueryString参数
        /// </summary>
        /// <param name="key">键值</param>
        /// <param name="defaultValue">不存在指定key时返回的默认值</param>
        /// <returns>如果不存在指定key,返回默认值</returns>
        public static string GetQueryString(string key, string defaultValue)
        {
            object obj = HttpContext.Current.Request.QueryString[key];
            return obj != null ? obj.ToString() : defaultValue;
        }

        /// <summary>
        /// 获取Request.Form参数
        /// </summary>
        /// <param name="key">键值</param>
        /// <returns>如果不存在指定key,返回空字符串</returns>
        public static string GetFormString(string key)
        {
            return GetFormString(key, string.Empty);
        }

        /// <summary>
        /// 获取Request.Form参数
        /// </summary>
        /// <param name="key">键值</param>
        /// <param name="defaultValue">不存在指定key时返回的默认值</param>
        /// <returns>如果不存在指定key,返回默认值</returns>
        public static string GetFormString(string key, string defaultValue)
        {
            string obj = HttpContext.Current.Request.Form[key];
            return string.IsNullOrEmpty(obj) ? defaultValue : obj;
        }

        /// <summary>
        /// 获取Request.Form参数或者Request.QueryString参数
        /// </summary>
        /// <param name="key">键值</param>
        /// <returns>如果不存在指定key,返回空字符串</returns>
        public static string GetParamString(string key)
        {
            return GetParamString(key, string.Empty);
        }

        /// <summary>
        /// 获取Request.Form参数或者Request.QueryString参数
        /// </summary>
        /// <param name="key">键值</param>
        /// <param name="defaultValue">不存在时返回的默认值</param>
        /// <returns>如果不存在指定key,返回默认值</returns>
        public static string GetParamString(string key, string defaultValue)
        {
            object obj = HttpContext.Current.Request.Params[key];
            return obj != null ? obj.ToString() : defaultValue;
        }

        /// <summary>
        /// 获取Url参数连接符号
        /// </summary>
        /// <param name="url">Url地址</param>
        public static string GetUrlJoinSymbol(string url)
        {
            var fix = "?";
            if (url.IndexOf("?", StringComparison.Ordinal) > -1) //有参数
            {
                fix = "&";
            }
            return fix;
        }

        /// <summary>
        /// 转为相对路径
        /// </summary>
        /// <param name="url">文件的虚拟路径(~/upload/xx.js)</param>
        /// <returns>文件的相对路径如(../upload/xx.js)</returns>
        public static string ConvertRelativeUrl(string url)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 1; i < HttpContext.Current.Request.Url.Segments.Length - 1; i++)
            {
                sb.Append("../");
            }
            return string.Concat(sb.ToString(), url.Replace("~/", ""));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="routeDatas"></param>
        /// <param name="addValues"></param>
        /// <returns></returns>
        public static RouteValueDictionary GetQueryRouteValuesNoId(RouteValueDictionary routeDatas, object addValues = null)
        {
            return GetRouteValues(routeDatas, addValues, true, false, "id");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="routeDatas"></param>
        /// <param name="addValues"></param>
        /// <param name="removeNames"></param>
        /// <returns></returns>
        public static RouteValueDictionary GetQueryRouteValues(RouteValueDictionary routeDatas, object addValues = null, params string[] removeNames)
        {
            return GetRouteValues(routeDatas, addValues, true, false, removeNames);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="routeDatas"></param>
        /// <param name="addValues"></param>
        /// <param name="removeNames"></param>
        /// <returns></returns>
        public static RouteValueDictionary GetFormRouteValues(RouteValueDictionary routeDatas, object addValues = null, params string[] removeNames)
        {
            return GetRouteValues(routeDatas, addValues, false, true, removeNames);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="routeDatas"></param>
        /// <param name="addValues"></param>
        /// <param name="includeQueryString"></param>
        /// <param name="includeForm"></param>
        /// <param name="removeNames"></param>
        /// <returns></returns>
        public static RouteValueDictionary GetRouteValues(RouteValueDictionary routeDatas, object addValues,
            bool includeQueryString, bool includeForm, params string[] removeNames)
        {
            var routeData = new RouteValueDictionary(routeDatas);
            if (includeQueryString)
            {
                var queryString = HttpContext.Current.Request.QueryString;
                foreach (string key in queryString.Keys)
                {
                    if (queryString[key] != null && !string.IsNullOrEmpty(key))
                    {
                        routeData[key] = queryString[key];
                    }
                }
            }
            if (includeForm)
            {
                var formString = HttpContext.Current.Request.Form;
                foreach (string key in formString.Keys)
                {
                    if (formString[key] != null && !string.IsNullOrEmpty(key))
                    {
                        routeData[key] = formString[key];
                    }
                }
            }


            if (addValues != null)
            {
                var pros = TypeDescriptor.GetProperties(addValues);
                foreach (PropertyDescriptor propertyDescriptor in pros)
                {
                    routeData[propertyDescriptor.Name] = propertyDescriptor.GetValue(addValues);
                }
            }

            if (removeNames != null)
            {
                foreach (var item in removeNames)
                {
                    routeData.Remove(item);
                }
            }

            return routeData;
        }

        /// <summary>
        /// 获取Select控件option列表
        /// </summary>
        /// <param name="list">数据源</param>
        /// <param name="selectedValue">选中的值</param>
        /// <param name="isMultiple">是否多选</param>
        /// <returns></returns>
        public static string GetSelectOptions(IEnumerable<Simple> list, object selectedValue = null, bool isMultiple = false)
        {
            var options = new StringBuilder();
            foreach (Simple item in list)
            {
                string display = item.Name;
                string spell = item.Spell;
                string value = item.Id;
                if (isMultiple)
                {
                    var valueArray = new string[0];
                    if (!string.IsNullOrEmpty(selectedValue?.ToString()))
                    {
                        valueArray = StringHelper.ConvertStringToArray(selectedValue.ToString());
                    }
                    var selected = Array.IndexOf(valueArray, value) > -1 ? " selected" : string.Empty;
                    options.AppendFormat($"<option value=\"{value}\" data-spell=\"{spell}\"{selected}>{display}</option>");
                }
                else
                {
                    var selected = value.Equals(selectedValue.ToStringOrEmpty()) ? " selected" : string.Empty;
                    options.AppendFormat($"<option value=\"{value}\" data-spell=\"{spell}\"{selected}>{display}</option>");
                }
            }
            return options.ToString();
        }

        /// <summary>
        /// 获取Select控件option列表
        /// </summary>
        /// <param name="list">数据源</param>
        /// <param name="displayMember">显示字段</param>
        /// <param name="valueMember">值字段</param>
        /// <param name="selectedValue">选中的值</param>
        /// <param name="isMultiple">是否多选</param>
        /// <returns></returns>
        public static string GetSelectOptions(IList list, string displayMember = "Name",
            string valueMember = "Id", object selectedValue = null, bool isMultiple = false)
        {
            var options = new StringBuilder();
            foreach (object item in list)
            {
                string display = ObjectHelper.GetObjectValue(item, displayMember).ToStringOrEmpty();
                string spell = display.Spell().ToLower();
                string value = ObjectHelper.GetObjectValue(item, valueMember).ToStringOrEmpty();
                if (isMultiple)
                {
                    var valueArray = new string[0];
                    if (!string.IsNullOrEmpty(selectedValue?.ToString()))
                    {
                        valueArray = StringHelper.ConvertStringToArray(selectedValue.ToString());
                    }
                    var selected = Array.IndexOf(valueArray, value) > -1 ? " selected" : string.Empty;
                    options.AppendFormat($"<option value=\"{value}\" data-spell=\"{spell}\"{selected}>{display}</option>");
                }
                else
                {
                    var selected = value.Equals(selectedValue.ToStringOrEmpty()) ? " selected" : string.Empty;
                    options.AppendFormat($"<option value=\"{value}\" data-spell=\"{spell}\"{selected}>{display}</option>");
                }
            }
            return options.ToString();
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="list"></param>
        /// <param name="displayMember"></param>
        /// <param name="valueMember"></param>
        /// <param name="groupMember"></param>
        /// <param name="selectedValue"></param>
        /// <returns></returns>
        public static string GetSelectGroupOptions(IList list, string displayMember = "Name",
            string valueMember = "Id", string groupMember = "Category",
            string selectedValue = null)
        {
            var options = new StringBuilder();
            var groupDic = new Dictionary<string, List<object>>();
            foreach (object item in list)
            {
                string group = ObjectHelper.GetObjectValue(item, groupMember).ToStringOrEmpty();
                if (string.IsNullOrEmpty(group))
                {
                    group = "默认";
                }
                if (!groupDic.Keys.Contains(group))
                {
                    groupDic.Add(group, new List<object>());
                }
                groupDic[group].Add(item);
            }
            foreach (var item in groupDic)
            {
                options.Append($"<optgroup label=\"{item.Key}\">{GetSelectOptions(item.Value, displayMember, valueMember, selectedValue)}</optgroup>");
            }

            return options.ToString();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="list"></param>
        /// <param name="displayMember"></param>
        /// <param name="valueMember"></param>
        /// <param name="groupDisplayMember"></param>
        /// <param name="groupValueMember"></param>
        /// <param name="selectedValue"></param>
        /// <returns></returns>
        public static string GetSelectGroupValueOptions(IList list, string displayMember = "Name",
            string valueMember = "Id", string groupDisplayMember = "Category", string groupValueMember = "CategoryId",
            string selectedValue = null)
        {
            var options = new StringBuilder();
            var groupDic = new Dictionary<string, Tuple<string, List<object>>>();
            foreach (object item in list)
            {
                string groupName = ObjectHelper.GetObjectValue(item, groupDisplayMember).ToStringOrEmpty();
                string groupId = ObjectHelper.GetObjectValue(item, groupValueMember).ToStringOrEmpty();
                if (string.IsNullOrEmpty(groupName))
                {
                    groupName = "默认";
                }
                if (!groupDic.Keys.Contains(groupName))
                {
                    groupDic.Add(groupName, new Tuple<string, List<object>>(groupId, new List<object>()));
                }
                groupDic[groupName].Item2.Add(item);
            }
            foreach (var item in groupDic)
            {
                options.Append($"<optgroup label=\"{item.Key}\" value=\"{item.Value.Item1}\">{GetSelectOptions(item.Value.Item2, displayMember, valueMember, selectedValue)}</optgroup>");
            }

            return options.ToString();
        }

        /// <summary>
        /// 获取Select控件option列表(使用数组对象)
        /// </summary>
        /// <param name="array">数组</param>
        /// <param name="selectedValue">选中的值</param>
        /// <returns></returns>
        public static string GetSelectOptionsByArray(Array array, string selectedValue = null)
        {
            var options = new StringBuilder();
            foreach (object item in array)
            {
                string value = item.ToStringOrEmpty();
                string spell = value.Spell().ToLower();
                var selected = value.Equals(selectedValue.ToStringOrEmpty()) ? " selected" : string.Empty;
                options.AppendFormat($"<option value=\"{value}\" data-spell=\"{spell}\"{selected}>{value}</option>");
            }
            return options.ToString();
        }

        /// <summary>
        /// 返回与指定虚拟路径相对应的物理路径。
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static string MapPath(string url)
        {
            if (string.IsNullOrEmpty(url))
            {
                return string.Empty;
            }
            var index = url.IndexOf("?", StringComparison.Ordinal);
            if (index > -1)
            {
                url = url.Substring(0, index);
            }
            return HttpContext.Current.Server.MapPath(url);
        }

        /// <summary>
        /// 上传文件
        /// </summary>
        /// <param name="files">上传文件对象集合</param>
        /// <param name="defaultExtensionName">默认扩展名(如果上传文件没有扩展名,则使用默认扩展名)</param>
        /// <param name="subFolder">子文件夹</param>
        /// <returns>返回上传文件信息</returns>
        public static List<UploadFileInfo> UploadFile(HttpFileCollectionBase files, string defaultExtensionName = ".rar",
            string subFolder = null)
        {
            List<UploadFileInfo> result = new List<UploadFileInfo>();
            if (files == null || files.Count <= 0) return result;
            foreach (string key in files.AllKeys)
            {
                HttpPostedFileBase file = files[key];
                result.Add(UploadFile(file, defaultExtensionName, subFolder));
            }
            return result;
        }

        /// <summary>
        /// 上传文件
        /// </summary>
        /// <param name="postFile">上传文件对象</param>
        /// <param name="defaultExtensionName">默认扩展名(如果上传文件没有扩展名,则使用默认扩展名)</param>
        /// <param name="subFolder">子文件夹</param>
        /// <returns>返回上传文件信息</returns>
        public static UploadFileInfo UploadFile(HttpPostedFileBase postFile, string defaultExtensionName = ".rar", string subFolder = null)
        {
            if (postFile == null || postFile.ContentLength == 0) return null;
            var virtualFolder = UploadFileSetting.UploadFolder;
            if (!string.IsNullOrEmpty(subFolder))
            {
                virtualFolder = Path.Combine(virtualFolder, subFolder);
            }
            string fileName = Path.GetFileNameWithoutExtension(postFile.FileName.Replace("&", "").Replace("?", "")).Replace("&", "").Replace("?", "");
            string extName = Path.GetExtension(postFile.FileName);
            string extensionName = string.IsNullOrEmpty(extName) ? defaultExtensionName : extName;
            string targetFileName = $"{StringHelper.Guid()}{extensionName}";
            string targetFileVirtualPath = Path.Combine(virtualFolder, targetFileName).Replace("\\", "/");
            string targetFilePath = HttpContext.Current.Server.MapPath(targetFileVirtualPath);
            FileHelper.CreateDirectoryByPath(targetFilePath);
            postFile.SaveAs(targetFilePath);

            var uploadFileInfo = new UploadFileInfo();
            uploadFileInfo.Name = fileName;
            uploadFileInfo.Url = targetFileVirtualPath;
            uploadFileInfo.Size = postFile.ContentLength;
            return uploadFileInfo;
        }
    }
}