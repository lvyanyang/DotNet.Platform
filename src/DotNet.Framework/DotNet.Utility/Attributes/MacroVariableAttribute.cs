// ===============================================================================
// DotNet.Platform 开发框架 2016 版权所有
// ===============================================================================
using System;

namespace DotNet.Utility
{
    /// <summary>
    /// 宏变量
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class MacroVariableAttribute: Attribute
    {
        /// <summary>
        /// 宏变量
        /// </summary>
        public MacroVariableAttribute()
        {

        }

        /// <summary>
        /// 宏变量
        /// </summary>
        public MacroVariableAttribute(string remark)
        {
            this.Remark = remark;
        }

        /// <summary>
        /// 名称 格式$({属性名称}))
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 值
        /// </summary>
        public string Value { get;set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
    }
}