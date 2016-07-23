// ===============================================================================
// DotNet.Platform 开发框架 2016 版权所有
// ===============================================================================
using System.Collections.Generic;

namespace DotNet.Data.Expressions.Compiler
{
    /// <summary>
    /// 
    /// </summary>
    public class SqlExpressionCompilerResult
    {
        /// <summary>
        /// 
        /// </summary>
        public SqlExpressionCompilerResult()
        {
            this.Parameters = new Dictionary<string, object>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        public SqlExpressionCompilerResult(string sql, IDictionary<string, object> parameters)
        {
            this.SQL = sql;
            this.Parameters = parameters;
        }

        /// <summary>
        /// 
        /// </summary>
        public string SQL { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public IDictionary<string, object> Parameters { get; set; }
    }
}
