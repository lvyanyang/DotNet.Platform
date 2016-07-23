// ===============================================================================
// DotNet.Platform 开发框架 2016 版权所有
// ===============================================================================
using System;

namespace DotNet.Utility
{
    /// <summary>
    /// 枚举项描述信息
    /// </summary>
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
    public class EnumCaptionAttribute : Attribute
    {
        private readonly string _caption;

        /// <summary>
        /// 指定描述信息初始化枚举项描述信息
        /// </summary>
        /// <param name="caption">枚举项描述</param>
        public EnumCaptionAttribute(string caption)
        {
            this._caption = caption;
        }

        /// <summary>
        /// 枚举项名称
        /// </summary>
        public string Name { get; internal set; }

        /// <summary>
        /// 枚举项描述
        /// </summary>
        public string Caption
        {
            get { return _caption; }
        }
    }
}