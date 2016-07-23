// ===============================================================================
// DotNet.Platform 开发框架 2016 版权所有
// ===============================================================================
using System.Collections.Generic;
using System.Text;

namespace DotNet.Data.Expressions
{
    /// <summary>
    /// Select语句结果对象
    /// </summary>
    public class SQLinqSelectResult : ISQLinqResult
    {
        /// <summary>
        /// 查询列
        /// </summary>
        public string[] Select { get; set; }

        /// <summary>
        /// 是否去重
        /// </summary>
        public bool? Distinct { get; set; }

        /// <summary>
        /// 提取记录
        /// </summary>
        public int? Take { get; set; }

        /// <summary>
        /// 跳过记录
        /// </summary>
        public int? Skip { get; set; }

        /// <summary>
        /// 页码索引,从1开始
        /// </summary>
        public int PageIndex { get; set; }

        /// <summary>
        /// 表名
        /// </summary>
        public string Table { get; set; }

        /// <summary>
        /// Where条件
        /// </summary>
        public string Where { get; set; }

        /// <summary>
        /// Having条件
        /// </summary>
        public string Having { get; set; }

        /// <summary>
        /// 分组字段
        /// </summary>
        public string[] GroupBy { get; set; }

        /// <summary>
        /// 排序字段
        /// </summary>
        public string[] OrderBy { get; set; }

        /// <summary>
        /// 参数
        /// </summary>
        public IDictionary<string, object> Parameters { get; set; }

        /// <summary>
        /// 转为SQL语句
        /// </summary>
        /// <returns>返回SQL语句</returns>
        public string ToSQL()
        {
            var orderby = ConcatFieldArray(this.OrderBy);
            var groupby = ConcatFieldArray(this.GroupBy);

            var sb = new StringBuilder();
            sb.Append("SELECT ");
            if (this.Distinct == true)
            {
                sb.Append("DISTINCT ");
            }
            sb.Append(ConcatFieldArray(this.Select));
            sb.AppendFormat(" FROM {0}", this.Table);
            if (!string.IsNullOrEmpty(this.Where))
            {
                sb.Append(" WHERE ");
                sb.Append(this.Where);
            }

            if (!string.IsNullOrEmpty(groupby))
            {
                sb.Append(" GROUP BY ");
                sb.Append(groupby);
            }

            if (!string.IsNullOrEmpty(this.Having))
            {
                sb.Append(" HAVING ");
                sb.Append(this.Having);
            }

            if (orderby.Length > 0)
            {
                sb.AppendFormat(" ORDER BY {0}", orderby);
            }
            return sb.ToString();
        }

        /// <summary>
        /// 转为分页SQL语句
        /// </summary>
        /// <returns>返回分页SQL语句</returns>
        public string ToPageSQL()
        {
            var orderby = ConcatFieldArray(this.OrderBy);
            var groupby = ConcatFieldArray(this.GroupBy);

            var sb = new StringBuilder();
            if (this.Distinct == true)
            {
                sb.Append("DISTINCT ");
            }

            // SELECT
            sb.Append(ConcatFieldArray(this.Select));

            if (this.Skip != null)
            {
                if (sb.Length > 0)
                {
                    sb.Append(",");
                }
                sb.Append(string.Format(" ROW_NUMBER() OVER (ORDER BY {0}) AS [SQLinq_row_number]", orderby));
            }

            sb.Append(" FROM ");

            if (this.Distinct == true && this.Skip != null && this.Take != null)
            {
                sb.Append("(SELECT DISTINCT ");
                sb.Append(ConcatFieldArray(this.Select));
                sb.Append(" FROM ");
                sb.Append(this.Table);
                sb.Append(") AS d");
            }
            else
            {
                sb.Append(this.Table);
            }

            if (!string.IsNullOrEmpty(this.Where))
            {
                sb.Append(" WHERE ");
                sb.Append(this.Where);
            }

            if (!string.IsNullOrEmpty(groupby))
            {
                sb.Append(" GROUP BY ");
                sb.Append(groupby);
            }

            if (!string.IsNullOrEmpty(this.Having))
            {
                sb.Append(" HAVING ");
                sb.Append(this.Having);
            }


            var sqlOrderBy = string.Empty;
            if (orderby.Length > 0)
            {
                sqlOrderBy = " ORDER BY " + orderby;
            }

            if (this.Skip != null)
            {
                // paging support
                var start = (this.Skip + 1).ToString();
                var end = (this.Skip + (this.Take ?? 0)).ToString();
                if (this.Take == null)
                {
                    if (this.Distinct == true)
                    {
                        return string.Format(@"WITH SQLinq_data_set AS (SELECT {0}) SELECT * FROM SQLinq_data_set WHERE [SQLinq_row_number] >= {1} ORDER BY [SQLinq_row_number]", sb.ToString(), start);
                    }
                    else
                    {
                        return string.Format(@"WITH SQLinq_data_set AS (SELECT {0}) SELECT * FROM SQLinq_data_set WHERE [SQLinq_row_number] >= {1}", sb.ToString(), start);
                    }
                }

                return string.Format(@"WITH SQLinq_data_set AS (SELECT {0}) SELECT * FROM SQLinq_data_set WHERE [SQLinq_row_number] BETWEEN {1} AND {2}", sb.ToString(), start, end);
            }
            else if (this.Take != null)
            {
                var sbQuery = sb.ToString();
                if (sbQuery.ToLower().StartsWith("distinct "))
                {
                    return "SELECT DISTINCT TOP " + this.Take.ToString() + " " + sbQuery.Substring(9) + sqlOrderBy;
                }
                else
                {
                    return "SELECT TOP " + this.Take.ToString() + " " + sbQuery + sqlOrderBy;
                }
            }

            return "SELECT " + sb.ToString() + sqlOrderBy;
        }

        /// <summary>
        /// 使用逗号分隔指定的数组
        /// </summary>
        /// <param name="fields">字段数组</param>
        /// <returns>返回使用逗号分隔的字符串</returns>
        public static string ConcatFieldArray(string[] fields)
        {
            if (fields == null) return string.Empty;
            if (fields.Length == 0) return string.Empty;

            var sb = new StringBuilder();
            for (var s = 0; s < fields.Length; s++)
            {
                if (s > 0)
                {
                    sb.Append(", ");
                }
                sb.Append(fields[s]);
            }
            return sb.ToString();
        }
    }
}
