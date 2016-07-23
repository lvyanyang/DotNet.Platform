// ===============================================================================
// DotNet.Platform 开发框架 2016 版权所有
// ===============================================================================
using System.Collections.Generic;

namespace DotNet.Data.Expressions
{
    /// <summary>
    /// 语句结果
    /// </summary>
    public interface ISQLinqResult
    {
        /// <summary>
        /// 参数字典
        /// </summary>
        IDictionary<string, object> Parameters { get; set; }

        /// <summary>
        /// 转为SQL语句
        /// </summary>
        /// <returns>返回SQL语句</returns>
        string ToSQL();
    }
}
