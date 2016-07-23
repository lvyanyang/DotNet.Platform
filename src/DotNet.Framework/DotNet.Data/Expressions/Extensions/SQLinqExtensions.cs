// ===============================================================================
// DotNet.Platform 开发框架 2016 版权所有
// ===============================================================================
namespace DotNet.Data.Extensions
{
    /// <summary>
    /// 查询语句扩展
    /// </summary>
    public static class SQLinqExtensions
    {
        /// <summary>
        /// Between查询
        /// </summary>
        /// <param name="column">列名</param>
        /// <param name="start">开始值</param>
        /// <param name="end">结束值</param>
        public static bool Between(this object column, object start, object end)
        {
            return true;
        }

        /// <summary>
        /// In查询(使用Or实现)
        /// </summary>
        /// <param name="column">列名</param>
        /// <param name="values">列值</param>
        public static bool OrArrayString(this object column, string[] values)
        {
            return true;
        }

        /// <summary>
        /// In查询(非参数化)
        /// </summary>
        /// <param name="column">列名</param>
        /// <param name="values">列值</param>
        public static bool InArrayString(this object column, string[] values)
        {
            return true;
        }

        /// <summary>
        /// In查询
        /// </summary>
        /// <param name="column">列名</param>
        /// <param name="values">列值</param>
        public static bool In(this object column, string[] values)
        {
            return true;
        }

        /// <summary>
        /// In查询
        /// </summary>
        /// <param name="column">列名</param>
        /// <param name="values">列值</param>
        public static bool In(this object column, int[] values)
        {
            return true;
        }

        /// <summary>
        /// In查询
        /// </summary>
        /// <param name="column">列名</param>
        /// <param name="values">列值</param>
        public static bool In(this int column, string[] values)
        {
            return true;
        }

        /// <summary>
        /// NotIn查询
        /// </summary>
        /// <param name="column">列名</param>
        /// <param name="values">列值</param>
        public static bool NotIn(this object column, string[] values)
        {
            return true;
        }

        /// <summary>
        /// NotIn查询
        /// </summary>
        /// <param name="column">列名</param>
        /// <param name="values">列值</param>
        public static bool NotIn(this object column, int[] values)
        {
            return true;
        }
    }
}