// ===============================================================================
// DotNet.Platform 开发框架 2016 版权所有
// ===============================================================================
using System;

namespace DotNet.Entity
{	
    /// <summary>
    /// 表字段
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class TableAttribute : Attribute
    {
        /// <summary>
        /// 初始化表字段新实例。
        /// </summary>
        public TableAttribute()
        {
        }

        /// <summary>
        /// 初始化表字段新实例。
        /// </summary>
        /// <param name="caption">备注</param>
        public TableAttribute(string caption)
        {
            Caption = caption;
        }

        /// <summary>
        /// 初始化表字段新实例。
        /// </summary>
        /// <param name="name">表名</param>
        /// <param name="caption">备注</param>
        public TableAttribute(string name,string caption)
        {
            Name = name;
            Caption = caption;
        }

        public TableAttribute(string name, string caption, string schema)
        {
            Name = name;
            Caption = caption;
            Schema = schema;
        }

        /// <summary>
        /// 表名
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 标题
        /// </summary>
        public string Caption { get; set; }

        /// <summary>
        /// 架构
        /// </summary>
        public string Schema { get; set; }
    }

    /// <summary>
    /// 主键字段
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class PrimaryKeyAttribute : Attribute
    {
        /// <summary>
        /// 初始化主键字段新实例。
        /// </summary>
        public PrimaryKeyAttribute()
        {
        }

        /// <summary>
        /// 初始化主键字段新实例。
        /// </summary>
        public PrimaryKeyAttribute(bool autoIncrement)
        {
            AutoIncrement = autoIncrement;
        }

        /// <summary>
        /// 初始化主键字段新实例。
        /// </summary>
        public PrimaryKeyAttribute(string name, bool autoIncrement, string sequenceName)
        {
            Name = name;
            SequenceName = sequenceName;
            AutoIncrement = autoIncrement;
        }

        /// <summary>
        /// 主键名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 序列名称(针对Oracle数据库)
        /// </summary>
        public string SequenceName { get; set; }

        /// <summary>
        /// 是否自增
        /// </summary>
        public bool AutoIncrement { get; set; }
    }

    /// <summary>
    /// 父级字段
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class ParentKeyAttribute : Attribute
    {
        /// <summary>
        /// 初始化父级字段。
        /// </summary>
        public ParentKeyAttribute()
        {
        }

        /// <summary>
        /// 字段名称
        /// </summary>
        public string Name { get; set; }
    }

    /// <summary>
    /// 显示字段
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class TextKeyAttribute : Attribute
    {
        /// <summary>
        /// 初始化父级字段。
        /// </summary>
        public TextKeyAttribute()
        {
        }

        /// <summary>
        /// 字段名称
        /// </summary>
        public string Name { get; set; }
    }

    /// <summary>
    /// 排序字段
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class SortPathAttribute : Attribute
    {
        /// <summary>
        /// 初始化父级字段。
        /// </summary>
        public SortPathAttribute()
        {
        }

        /// <summary>
        /// 排序字段
        /// </summary>
        public string Name { get; set; }
    }

    /// <summary>
    /// 显式标记字段,指定此字段后要求所有列全部显式标记ColumnAttribute特性,否则将忽略.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class ExplicitAttribute : Attribute
    {}

    /// <summary>
    /// 列字段
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class ColumnAttribute : Attribute
    {
        private bool _exported = true;

        /// <summary>
        /// 初始化列字段新实例。
        /// </summary>
        public ColumnAttribute()
        {
        }

        /// <summary>
        /// 初始化列字段新实例。
        /// </summary>
        public ColumnAttribute(string caption)
        {
            Caption = caption;
        }

        /// <summary>
        /// 初始化列字段新实例。
        /// </summary>
        public ColumnAttribute(string caption,bool exported)
        {
            Caption = caption;
            Exported = exported;
        }

        /// <summary>
        /// 列名
        /// </summary>
        public string Name { get; set; }


        /// <summary>
        /// 是否导出
        /// </summary>
        public bool Exported
        {
            get { return _exported; }
            set { _exported = value; }
        }

        /// <summary>
        /// 备注
        /// </summary>
        public string Caption { get; set; }
    }

    /// <summary>
    /// 结果字段,只针对查询有效.对于Insert和Update无效.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class ResultColumnAttribute : ColumnAttribute
    {
        /// <summary>
        /// 初始化结果字段新实例
        /// </summary>
        public ResultColumnAttribute()
        {
            
        }
    }
    
    /// <summary>
    /// 忽略字段
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class IgnoreAttribute : Attribute
    {}
}