// ===============================================================================
// DotNet.Platform 开发框架 2016 版权所有
// ===============================================================================
using System;
using System.Reflection;
using DotNet.Helper;

namespace DotNet.Entity
{
    /// <summary>
    /// 实体列
    /// </summary>
	public class EntityColumn
    {
        private ColumnInfo _columnInfo;

        /// <summary>
        /// 列信息
        /// </summary>
	    public ColumnInfo ColumnInfo
        {
            get { return _columnInfo ?? (_columnInfo = ColumnInfo.FromProperty(Property.Property)); }
        }

        ///// <summary>
        ///// 列属性对象
        ///// </summary>
        //public PropertyInfo Property { get; internal set; }

        public FastProperty Property { get; internal set; }

        /// <summary>
        /// 列名称
        /// </summary>
	    public string ColumnName { get { return ColumnInfo.ColumnName; } }

        /// <summary>
        /// 设置属性值
        /// </summary>
        /// <param name="target"></param>
        /// <param name="val"></param>
	    public virtual void SetValue(object target, object val) { Property.Set(target, val); }

        /// <summary>
        /// 获取属性值
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
		public virtual object GetValue(object target) { return Property.Get(target); }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
		public virtual object ChangeType(object val) { return Convert.ChangeType(val, Property.Property.PropertyType); }
    }
}
