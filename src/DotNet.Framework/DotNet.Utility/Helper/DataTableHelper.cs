// ===============================================================================
// DotNet.Platform 开发框架 2016 版权所有
// ===============================================================================
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using DotNet.Entity;
using DotNet.Extensions;

namespace DotNet.Helper
{
    /// <summary>
    /// DataTable操作类帮助类
    /// </summary>
    public static class DataTableHelper
    {
        /// <summary>
        /// 复制数据行到新的DataTable实例
        /// </summary>
        /// <param name="copyRows">复制的行数组</param>
        /// <returns>返回新的包含指定行的DataTable实例</returns>
        public static DataTable CopyDataRowToNewDataTable(DataRow[] copyRows)
        {
            if (copyRows.Length == 0)
            {
                return null;
            }
            DataTable sourceTable = copyRows[0].Table;

            //DataTable的数据结构拷贝到新的DataTable实例中
            DataTable dt = sourceTable.Clone();

            //把上面得到的那个符合条件的行的数组全部复制到新到DataTable的实例中
            foreach (DataRow t in copyRows)
            {
                //把数据复制到新的DataTable的实例中
                dt.ImportRow(t);
            }
            return dt;
        }

        /// <summary>
        /// 复制一个新的数据行对象
        /// </summary>
        /// <param name="sourceRow">源数据行</param>
        /// <returns>返回新的数据行对象</returns>
        public static DataRow CopyDataRow(DataRow sourceRow)
        {
            DataTable table = sourceRow.Table;
            var row = table.NewRow();
            foreach (DataColumn column in table.Columns)
            {
                row[column] = sourceRow[column];
            }
            return row;
        }

        /// <summary>
        /// 复制数据表
        /// </summary>
        /// <param name="table">源数据表</param>
        /// <returns>返回新创建的DataTable实例</returns>
        public static DataTable CopyDataTable(DataTable table)
        {
            if (table == null || table.Rows.Count == 0)
            {
                return null;
            }
            return CopyDataRowToNewDataTable(table.Select());
        }

        /// <summary>
        /// 复制数据表结构
        /// </summary>
        /// <param name="source">源数据表</param>
        /// <param name="target">目标数据表</param>
        public static void CopyDataTableSchema(DataTable source, DataTable target)
        {
            foreach (DataColumn column in source.Columns)
            {
                DataColumn newColumn = new DataColumn();
                newColumn.ColumnName = column.ColumnName;
                newColumn.Caption = column.Caption;
                newColumn.DataType = column.DataType;
                newColumn.DefaultValue = column.DefaultValue;
                newColumn.AllowDBNull = column.AllowDBNull;
                newColumn.AutoIncrement = column.AutoIncrement;
                target.Columns.Add(newColumn);
            }
        }

        /// <summary>
        /// 过滤数据表
        /// </summary>
        /// <param name="table">源数据表</param>
        /// <param name="filter">过滤表达式</param>
        public static DataTable FilterDataTable(DataTable table, string filter)
        {
            var rows = table.Select(filter);
            return CopyDataRowToNewDataTable(rows);
        }

        /// <summary>
        /// 更新源数据行(必须保证两个对象的列结构一致,否则会出错)
        /// </summary>
        /// <param name="sourceRow">源数据行</param>
        /// <param name="newRow">新数据行</param>
        public static void UpdateRow(DataRow sourceRow, DataRow newRow)
        {
            foreach (DataColumn col in sourceRow.Table.Columns)
            {
                sourceRow[col.ColumnName] = newRow[col.ColumnName];
            }
        }

        /// <summary>
        /// 数据行中是否有指定列名
        /// </summary>
        /// <param name="dataRow">数据行</param>
        /// <param name="columnName">列名</param>
        public static bool HasColumnName(DataRow dataRow, string columnName)
        {
            DataColumnCollection collection = dataRow.Table.Columns;
            if (collection.Count > 0)
            {
                foreach (DataColumn item in collection)
                {
                    if (columnName.Equals(item.ColumnName, StringComparison.OrdinalIgnoreCase))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// 根据实体属性创建表(没有数据)
        /// </summary>
        /// <exception cref="System.ArgumentNullException">参数instance不能为空</exception>
        /// <returns>返回新的DataTable对象</returns>
        public static DataTable CreateTable(object instance)
        {
            if (instance == null)
            {
                throw new ArgumentNullException("instance", "参数instance不能为空");
            }
            return CreateTable(instance.GetType());
        }

        /// <summary>
        /// 根据实体属性创建表(没有数据)
        /// </summary>
        /// <returns>返回新的DataTable对象</returns>
        public static DataTable CreateTable(Type type)
        {
            DataSet ds = new DataSet();
            DataTable table = new DataTable();
            ds.Tables.Add(table);
            ds.DataSetName = string.Concat(type.Name, "s");
            table.TableName = type.Name;
            foreach (PropertyInfo info in type.GetProperties())
            {
                DataColumn dc = new DataColumn {ColumnName = info.Name };
                //if (info.PropertyType != typeof(Nullable<>))
                //{
                //    dc.DataType = info.PropertyType;
                //}
                table.Columns.Add(dc);
            }
            return table;
        }


        /// <summary>
        /// 把实体对象转为数据行对象
        /// </summary>
        /// <param name="table">源表</param>
        /// <param name="entity">实体对象</param>
        /// <returns>返回新的数据行对象。</returns>
        public static DataRow ConvertToRow(DataTable table, object entity)
        {
            if (entity == null) return null;
            Type entityType = entity.GetType();
            DataRow row = table.NewRow();
            foreach (DataColumn column in table.Columns)
            {
                string colName = column.ColumnName;
                var pro = entityType.GetProperty(colName);
                if (pro == null) continue;
                row[colName] = pro.GetValue(entity, null);
            }
            return row;
        }

        /// <summary>
        /// 根据DataTable创建数据列表
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="table">数据表</param>
        /// <param name="createInstance">创建实例函数</param>
        /// <returns>返回新创建的列表,包含table中的所有数据</returns>
        public static List<T> ConvertToList<T>(DataTable table, Func<T> createInstance = null) where T : class
        {
            var list = new List<T>();
            if (createInstance == null)
            {
                createInstance = ReflectionHelper.CreateInstance<T>;
            }
            foreach (DataRow row in table.Rows)
            {
                var instance = createInstance();
                WriteToEntity(row, instance);
                list.Add(instance);
            }
            return list;
        }
        /// <summary>
        /// 根据DataTable创建数据列表(列名为字段说明)
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="table">数据表</param>
        /// <param name="createInstance">创建实例函数</param>
        public static List<T> ConvertToListByCaption<T>(DataTable table, Func<T> createInstance = null) where T : class,new()
        {
            var list = new List<T>();
            if (createInstance == null)
            {
                createInstance = ReflectionHelper.CreateInstance<T>;
            }
            var metadata = EntityMetadata.ForType(typeof(T));
            foreach (DataRow row in table.Rows)
            {
                var instance = createInstance();
                ConvertToEntityByCaption(row, instance, metadata);
                list.Add(instance);
            }
            return list;
        }

        /// <summary>
        /// 数据行转为实体对象,列名使用备注字段
        /// </summary>
        /// <param name="row">数据行</param>
        /// <param name="entity">实体对象</param>
        /// <param name="metadata">实体元数据</param>
        /// <returns>返回新的实体对象</returns>
        public static T ConvertToEntityByCaption<T>(DataRow row, T entity,EntityMetadata metadata) where T : class
        {
            foreach (DataColumn column in row.Table.Columns)
            {
                string colName = column.ColumnName;
                if (string.IsNullOrEmpty(colName))
                {
                    continue;
                }
                var colObject = metadata.Columns.FirstOrDefault(p =>
                    p.Key.Equals(colName)|| p.Value.ColumnInfo.Caption.Equals(colName));
                if (colObject.Key == null) continue;
                string name = colObject.Key;
                object value = row[column];
                colObject.Value.SetValue(entity, value);
            }
            return entity;
        }

        /// <summary>
        /// 使用行对象中的数据填充对象属性值,行对象中的列名不区分大小写
        /// </summary>
        /// <param name="row">行对象</param>
        /// <param name="entity">对象</param>
        public static void WriteToEntity(DataRow row, object entity)
        {
            Type entityType = entity.GetType();
            var pros = entityType.GetProperties();
            foreach (PropertyInfo pro in pros)
            {
                string colName = pro.Name;
                if (!HasColumnName(row,colName))
                {
                    continue;
                }
                object value = row[colName];
                if (!ObjectHelper.IsNullableType(pro.PropertyType) && (value == DBNull.Value || value == null))
                {
                    throw new Exception(string.Format("属性{0}不允许为空,但给定的值为空值,无法赋值", pro.Name));
                }
                pro.SetValue(entity, ObjectHelper.ConvertObjectValue(value, pro.PropertyType), null);
            }
        }

        /// <summary>
        /// 把数据行对象转为对象实例
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="row">行对象</param>
        /// <param name="createInstance">创建实例函数</param>
        /// <returns>返回新创建的实例对象</returns>
        public static T ConvertToObject<T>(DataRow row, Func<T> createInstance = null) where T : class
        {
            if (createInstance == null)
            {
                createInstance = ReflectionHelper.CreateInstance<T>;
            }
            var instance = createInstance();
            WriteToEntity(row, instance);
            return instance;
        }

        /// <summary>
        /// 把数据行转成格式为"列名(描述)=列值"的字符串
        /// </summary>
        /// <param name="row">数据行</param>
        /// <returns>数据行字符串</returns>
        public static string DataRowToString(DataRow row)
        {
            DataTable dt = row.Table;
            StringBuilder sb = new StringBuilder();
            foreach (DataColumn item in dt.Columns)
            {
                sb.AppendFormat("{0}({1})={2}  ", item.ColumnName, item.Caption, row[item]);
            }
            return sb.ToString();
        }

        /// <summary>
        /// 把数据列表转为DataTable
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="list">数据列表</param>
        /// <returns>新创建的DataTable</returns>
        public static DataTable ConvertToDataTable<T>(IEnumerable<T> list) where T : class,new()
        {
            var table = CreateTable(typeof(T));
            foreach (T entity in list)
            {
                var row = ConvertToRow(table, entity);
                table.Rows.Add(row);
            }
            return table;
        }

        /// <summary>
        /// 比较DataRow
        /// </summary>
        /// <param name="sourceRow">原来的Row</param>
        /// <param name="newRow">新的Row</param>
        /// <returns>如果Row值发生变化返回true</returns>
        public static bool CompareDataRow(DataRow sourceRow, DataRow newRow)
        {
            var cols = sourceRow.Table.Columns;
            foreach (DataColumn col in cols)
            {
                var oldValue = sourceRow[col];
                var newValue = newRow[col];
                if (oldValue != newValue)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// DataTable分页,起始页码为1
        /// </summary>
        /// <param name="sourceTable">源表</param>
        /// <param name="pageIndex">页码索引,起始页为1</param>
        /// <param name="pageSize">每页数量</param>
        /// <returns>返回分页后的DataTable</returns>
        public static DataTable GetPagedTable(DataTable sourceTable, int pageIndex, int pageSize)
        {
            if (pageIndex < 0)
            {
                pageIndex = 1;
            }
            if (pageIndex == 0)
            {
                return sourceTable;
            }
            if (sourceTable.Rows.Count == 0)
            {
                return sourceTable;
            }

            DataTable newdt = sourceTable.Clone();
            int rowbegin = (pageIndex - 1) * pageSize;
            int rowend = pageIndex * pageSize;

            if (rowbegin >= sourceTable.Rows.Count)
                return sourceTable;

            if (rowend > sourceTable.Rows.Count)
                rowend = sourceTable.Rows.Count;
            for (int i = rowbegin; i <= rowend - 1; i++)
            {
                DataRow dr = sourceTable.Rows[i];
                newdt.ImportRow(dr);
            }
            return newdt;
        }

        /// <summary>
        /// 获取总页数
        /// </summary>
        /// <param name="sourceTable">源表</param>
        /// <param name="pageSize">每页数量</param>
        /// <returns>返回总页数</returns>
        public static int GetPageCount(DataTable sourceTable, int pageSize)
        {
            int sumCount = sourceTable.Rows.Count;
            int page = sumCount / pageSize;
            if (sumCount % pageSize > 0)
            {
                page = page + 1;
            }
            return page;
        }

        /// <summary>
        /// 获取表首列值数组
        /// </summary>
        /// <param name="table">数据表</param>
        /// <returns>返回首列值数组</returns>
        public static string[] GetFirstColumnValues(DataTable table)
        {
            var ids = new string[table.Rows.Count];
            table.ForEach((index, row) => ids[index] = row[0].ToStringOrEmpty());
            return ids;
        }
    }
}