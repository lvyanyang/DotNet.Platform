// ===============================================================================
// DotNet.Platform 开发框架 2016 版权所有
// ===============================================================================
using System.Collections.Generic;
using System.Data;
using System.IO;
using DevExpress.Spreadsheet;
using DevExpress.Spreadsheet.Export;
using DotNet.Entity;
using DotNet.Helper;

namespace DotNet.Doc
{
    /// <summary>
    /// Excel操作类(包括Excel2003和Excel2010)
    /// </summary>
    public static class ExcelHelper
    {
        /// <summary>
        /// 导入Excel文件
        /// </summary>
        /// <param name="fileName">文件名</param>
        /// <param name="firstRowIsHead">是否首行包含列名</param>
        public static DataTable Import(string fileName, bool firstRowIsHead)
        {
            return ImportCore(fileName, firstRowIsHead);
        }

        /// <summary>
        /// 导入Excel文件
        /// </summary>
        /// <param name="fileName">文件名</param>
        /// <param name="firstRowIsHead">是否首行包含列名</param>
        public static List<T> Import<T>(string fileName, bool firstRowIsHead) where T:class,new()
        {
            var dt = ImportCore(fileName, firstRowIsHead);
            return DataTableHelper.ConvertToListByCaption<T>(dt);
        }

        /// <summary>
        /// 导入Excel文件
        /// </summary>
        /// <param name="buffer">二进制文件</param>
        /// <param name="firstRowIsHead">是否首行包含列名</param>
        /// <param name="format">Excel格式</param>
        public static DataTable Import(byte[] buffer, bool firstRowIsHead, DocumentFormat format)
        {
            if (buffer == null || buffer.Length == 0) return null;
            Workbook book = new Workbook();
            book.LoadDocument(buffer, format);
            var sheet = book.Worksheets[0];
            Range range = sheet.Cells.CurrentRegion;
            DataTable table = sheet.CreateDataTable(range, firstRowIsHead);
            DataTableExporter exporter = sheet.CreateDataTableExporter(range, table, firstRowIsHead);
            exporter.Export();
            return table;
        }

        /// <summary>
        /// 导入Excel文件
        /// </summary>
        /// <param name="buffer">二进制文件</param>
        /// <param name="firstRowIsHead">是否首行包含列名</param>
        /// <param name="format">Excel格式</param>
        public static List<T> Import<T>(byte[] buffer, bool firstRowIsHead, DocumentFormat format) where T : class, new()
        {
            if (buffer == null || buffer.Length == 0) return null;
            Workbook book = new Workbook();
            book.LoadDocument(buffer, format);
            var sheet = book.Worksheets[0];
            Range range = sheet.Cells.CurrentRegion;
            DataTable table = sheet.CreateDataTable(range, firstRowIsHead);
            DataTableExporter exporter = sheet.CreateDataTableExporter(range, table, firstRowIsHead);
            exporter.Export();
            return DataTableHelper.ConvertToListByCaption<T>(table);
        }

        /// <summary>
        /// 内部导入
        /// </summary>
        /// <param name="fileName">文件名</param>
        /// <param name="firstRowIsHead">是否首行包含列名</param>
        private static DataTable ImportCore(string fileName, bool firstRowIsHead)
        {
            Workbook book = new Workbook();
            book.LoadDocument(fileName);
            var sheet = book.Worksheets[0];
            Range range = sheet.Cells.CurrentRegion;
            DataTable table = sheet.CreateDataTable(range, firstRowIsHead);
            DataTableExporter exporter = sheet.CreateDataTableExporter(range, table, firstRowIsHead);
            exporter.Export();
            return table;
        }

        /// <summary>
        /// 获取Excel格式
        /// </summary>
        /// <param name="fileName">文件名</param>
        public static DocumentFormat GetFormat(string fileName)
        {
            string fileExt = Path.GetExtension(fileName);
            if (string.IsNullOrEmpty(fileExt))
            {
                return DocumentFormat.Undefined;
            }
            switch (fileExt.ToLower())
            {
                case ".xls":
                    return DocumentFormat.Xls;
                case ".xlsx":
                    return DocumentFormat.Xlsx;
                case ".csv":
                    return DocumentFormat.Csv;
                case ".txt":
                    return DocumentFormat.Text;
                default:
                    return DocumentFormat.Undefined;
            }
        }

        /// <summary>
        /// 导出Excel
        /// </summary>
        /// <param name="table">导出的数据</param>
        /// <param name="metadata">导出配置</param>
        /// <returns>返回字节数组</returns>
        public static byte[] Export(DataTable table, EntityMetadata metadata)
        {
            Workbook workbook = new Workbook();
            Worksheet sheet = workbook.Worksheets[0];
            sheet.Name = metadata.TableInfo.Caption ?? metadata.TableInfo.TableName;
            // ReSharper disable once AssignNullToNotNullAttribute
            table.PrimaryKey = null;
            foreach (var item in metadata.Columns)
            {
                var colName = item.Key;
                var col = item.Value;
                if (col.ColumnInfo.Exported)
                {
                    var column = table.Columns[colName];
                    if (column == null)
                    {
                        continue;
                    }
                    column.Caption = col.ColumnInfo.Caption ?? colName;
                }
                else
                {
                    table.Columns.Remove(colName);
                }
            }
            sheet.Import(table, true, 0, 0);
            sheet.Columns.AutoFit(0, sheet.Cells.CurrentRegion.ColumnCount - 1);
            return workbook.SaveDocument(DocumentFormat.Xlsx);
        }

        /// <summary>
        /// 导出Excel
        /// </summary>
        /// <param name="table">导出的数据</param>
        /// <returns>返回字节数组</returns>
        public static byte[] Export<T>(DataTable table) where T : class,new()
        {
            var metadata = EntityMetadata.ForType(typeof(T));
            return Export(table, metadata);
        }

        /// <summary>
        /// 导出Excel
        /// </summary>
        /// <typeparam name="T">实体对象</typeparam>
        /// <param name="data">导出的数据</param>
        /// <returns>返回字节数组</returns>
        public static byte[] Export<T>(IEnumerable<T> data) where T : class,new()
        {
            var metadata = EntityMetadata.ForType(typeof(T));
            var table = DataTableHelper.ConvertToDataTable(data);
            return Export(table, metadata);
        }
    }
}