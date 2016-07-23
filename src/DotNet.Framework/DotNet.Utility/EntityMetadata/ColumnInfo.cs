// ===============================================================================
// DotNet.Platform 开发框架 2016 版权所有
// ===============================================================================
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace DotNet.Entity
{
    /// <summary>
    /// 列信息
    /// </summary>
    public class ColumnInfo
    {
        /// <summary>
        /// 列名
        /// </summary>
        public string ColumnName { get; set; }

        /// <summary>
        /// 标题
        /// </summary>
        public string Caption { get; set; }

        /// <summary>
        /// 是否导出
        /// </summary>
        public bool Exported { get; set; }

        /// <summary>
        /// 是否结果列
        /// </summary>
        public bool ResultColumn { get; set; }

        /// <summary>
        /// 读取属性特性标签获取列信息
        /// </summary>
        /// <param name="pi">属性对象</param>
        /// <returns>返回新创建的列信息</returns>
        public static ColumnInfo FromProperty(PropertyInfo pi)
        {
            if (pi==null)
            {
                throw new ArgumentNullException("pi", "参数pi不能为空");
            }
            // ReSharper disable once PossibleNullReferenceException
            bool explicitAttribute = pi.DeclaringType.GetCustomAttributes(typeof(ExplicitAttribute), true).Length > 0;

            var columnAttribute = pi.GetCustomAttributes(typeof(ColumnAttribute), true);
            if (explicitAttribute)
            {
                if (columnAttribute.Length == 0) return null;
            }
            else
            {
                if (pi.GetCustomAttributes(typeof(IgnoreAttribute), true).Length != 0) return null;
            }

            ColumnInfo ci = new ColumnInfo();
            if (columnAttribute.Length > 0)
            {
                var colattr = columnAttribute[0] as ColumnAttribute;

                // ReSharper disable once PossibleNullReferenceException
                ci.ColumnName = string.IsNullOrEmpty(colattr.Name)? pi.Name : colattr.Name;
                ci.Caption = colattr.Caption;
                ci.Exported = colattr.Exported;
                if ((colattr as ResultColumnAttribute) != null)
                    ci.ResultColumn = true;
            }
            else
            {
                ci.ColumnName = pi.Name;
                ci.Caption = pi.Name;
                ci.Exported = true;
                ci.ResultColumn = false;
            }

            return ci;
        }
    }
}
