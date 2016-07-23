// ===============================================================================
// DotNet.Platform 开发框架 2016 版权所有
// ===============================================================================
using System.Collections.Generic;

namespace DotNet.Data.Expressions.Compiler
{
    /// <summary>
    /// 
    /// </summary>
    public class SqlExpressionCompilerSelectorResult
    {
        /// <summary>
        /// 
        /// </summary>
        public SqlExpressionCompilerSelectorResult()
        {
            this.Select = new List<string>();
            this.Parameters = new Dictionary<string, object>();
        }

        /// <summary>
        /// 
        /// </summary>
        public IList<string> Select { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public IDictionary<string, object> Parameters { get; set; }
    }
}
