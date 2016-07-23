// ===============================================================================
// DotNet.Platform 开发框架 2016 版权所有
// ===============================================================================
namespace DotNet.Data.Expressions
{
    /// <summary>
    /// SQL原生语句
    /// </summary>
    public class SQLStatement
    {
        /// <summary>
        /// 初始化SQL原生语句。
        /// </summary>
        /// <param name="statement">SQL语句</param>
        public SQLStatement(string statement)
        {
            Statement = statement;
        }

        /// <summary>
        /// SQL语句
        /// </summary>
        public string Statement { get; set; }
    }
}