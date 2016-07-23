// ===============================================================================
// DotNet.Platform 开发框架 2016 版权所有
// ===============================================================================
using System;
using System.Collections.Generic;
using System.Linq;
using DotNet.Helper;
using DotNet.Utility;

namespace DotNet.Entity
{
    /// <summary>
    /// 实体元数据
    /// </summary>
    public class EntityMetadata
    {
        private static Cache<Type, EntityMetadata> _datas = new Cache<Type, EntityMetadata>();

        /// <summary>
        /// 初始化实体元数据
        /// </summary>
        public EntityMetadata()
        {
        }

        /// <summary>
        /// 使用类型初始化实体元数据
        /// </summary>
        /// <param name="t">实体类型</param>
        public EntityMetadata(Type t)
        {
            TableInfo = TableInfo.FromPoco(t);
            Columns = new Dictionary<string, EntityColumn>(StringComparer.OrdinalIgnoreCase);
            EntityType = t;
            foreach (var pi in t.GetProperties())
            {
                #region 主键信息

                var pkAttr = AssemblyHelper.GetAttribute<PrimaryKeyAttribute>(pi);
                if (pkAttr != null)
                {
                    TableInfo.PrimaryKey = string.IsNullOrEmpty(pkAttr.Name) ? pi.Name : pkAttr.Name;
                    TableInfo.SequenceName = pkAttr.SequenceName;
                    TableInfo.AutoIncrement = pkAttr.AutoIncrement;
                    TableInfo.PrimaryKeyProperty = new FastProperty(pi);
                }

                #endregion

                #region 父级信息

                var parentkeyAttr = AssemblyHelper.GetAttribute<ParentKeyAttribute>(pi);
                if (parentkeyAttr != null)
                {
                    TableInfo.ParentKey = string.IsNullOrEmpty(parentkeyAttr.Name) ? pi.Name : parentkeyAttr.Name;
                    TableInfo.ParentKeyProperty = new FastProperty(pi);
                }

                #endregion

                #region 显示字段信息

                var textkeyAttr = AssemblyHelper.GetAttribute<TextKeyAttribute>(pi);
                if (textkeyAttr != null)
                {
                    TableInfo.TextKey = string.IsNullOrEmpty(textkeyAttr.Name) ? pi.Name : textkeyAttr.Name;
                    TableInfo.TextKeyProperty = new FastProperty(pi);
                }

                #endregion

                #region 排序路径

                var sortpathAttr = AssemblyHelper.GetAttribute<SortPathAttribute>(pi);
                if (sortpathAttr != null)
                {
                    TableInfo.SortPath = string.IsNullOrEmpty(sortpathAttr.Name) ? pi.Name : sortpathAttr.Name;
                    TableInfo.SortPathProperty = new FastProperty(pi);
                }

                #endregion

                ColumnInfo ci = ColumnInfo.FromProperty(pi);
                if (ci == null) continue;

                var pc = new EntityColumn();
                pc.Property = new FastProperty(pi);
                Columns.Add(pc.ColumnName, pc);
            }
            QueryColumns = (from c in Columns select c.Key).ToArray();
        }

        /// <summary>
        /// 实体类型
        /// </summary>
        public Type EntityType { get; private set; }

        /// <summary>
        /// 查询列
        /// </summary>
        public string[] QueryColumns { get; private set; }

        /// <summary>
        /// 表信息
        /// </summary>
        public TableInfo TableInfo { get; private set; }

        /// <summary>
        /// 列信息
        /// </summary>
        public Dictionary<string, EntityColumn> Columns { get; private set; }

        /// <summary>
        /// 使用指定的实体类型获取实体元数据(从缓存)
        /// </summary>
        /// <param name="t">实体类型</param>
        /// <returns>返回实体元数据</returns>
        public static EntityMetadata ForType(Type t)
        {
            return _datas.Get(t, () => new EntityMetadata(t));
        }

        /// <summary>
        /// 获取实体主键值
        /// </summary>
        /// <param name="poco">实体对象</param>
        /// <returns>返回实体主键值</returns>
        public object GetPrimaryKeyValue(object poco)
        {
            if (string.IsNullOrEmpty(TableInfo.PrimaryKey))
            {
                throw new ArgumentException($"请指定实体{EntityType.FullName}({TableInfo.Caption})的主键");
            }
            string primaryKeyName = TableInfo.PrimaryKey;
            if (!string.IsNullOrEmpty(primaryKeyName))
            {
                var pc = this.Columns[primaryKeyName];
                return pc.GetValue(poco);
            }
            return null;
        }

        /// <summary>
        /// 获取实体主键值
        /// </summary>
        /// <param name="poco">实体对象</param>
        /// <param name="primaryKeyValue">主键值</param>
        /// <returns>返回实体主键值</returns>
        public void SetPrimaryKeyValue(object poco, object primaryKeyValue)
        {
            string primaryKeyName = TableInfo.PrimaryKey;
            if (!string.IsNullOrEmpty(primaryKeyName))
            {
                var pc = this.Columns[primaryKeyName];
                pc.SetValue(poco, pc.ChangeType(primaryKeyValue));
            }
        }
    }
}