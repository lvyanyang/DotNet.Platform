// ===============================================================================
// DotNet.Platform 开发框架 2016 版权所有
// ===============================================================================
using System.Collections.Generic;
using DotNet.Extensions;
using DotNet.Utility;

namespace DotNet.Helper
{
    /// <summary>
    /// 宏变量操作类
    /// </summary>
    public static class MacroHelper
    {
        /// <summary>
        /// 转换宏字符串
        /// </summary>
        /// <param name="macroString">宏字符串</param>
        /// <param name="macroObjects">宏对象</param>
        /// <returns>返回替换宏之后的字符串</returns>
        public static string Convert(string macroString, params object[] macroObjects)
        {
            if (macroObjects == null || macroObjects.Length == 0)
            {
                throw new System.ArgumentNullException("macroObjects", "请指定宏对象");
            }
            var str = macroString;
            foreach (var macroObject in macroObjects)
            {
                var attrs = GetMacroItems(macroObject);
                foreach (var attr in attrs)
                {
                    str = str.Replace(attr.Name, attr.Value);
                }
            }
            return str;
        }

        /// <summary>
        /// 获取指定对象宏信息列表
        /// </summary>
        /// <param name="macroObject">宏对象</param>
        public static MacroVariableAttribute[] GetMacroItems(object macroObject)
        {
            var list = new List<MacroVariableAttribute>();
            foreach (var propertyInfo in macroObject.GetType().GetProperties())
            {
                var attr = AssemblyHelper.GetAttribute<MacroVariableAttribute>(propertyInfo);
                if (attr == null) continue;
                attr.Name = string.Format("$({0})", propertyInfo.Name);
                attr.Value = propertyInfo.GetValue(macroObject, null).ToStringOrEmpty();
                list.Add(attr);
            }
            return list.ToArray();
        }
    }
}