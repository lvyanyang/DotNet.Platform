// ===============================================================================
// DotNet.Platform 开发框架 2016 版权所有
// ===============================================================================
namespace DotNet.Data.Expressions
{
    /// <summary>
    /// Count语句
    /// </summary>
    /// <typeparam name="T">对象类型</typeparam>
    public class SQLinqCount<T> : ISQLinq
    {
        /// <summary>
        /// 构造Count语句
        /// </summary>
        /// <param name="query">查询对象</param>
        public SQLinqCount(SQLQuery<T> query)
        {
            this.Query = query;
        }

        /// <summary>
        /// 查询对象
        /// </summary>
        public SQLQuery<T> Query { get; private set; }

        /// <summary>
        /// 转为Count语句结果对象
        /// </summary>
        /// <param name="existingParameterCount">已经存在的参数个数</param>
        /// <returns>返回Count语句结果对象</returns>
        public ISQLinqResult ToResult(int existingParameterCount = 0)
        {
            var result = (SQLinqSelectResult)this.Query.ToResult(existingParameterCount);

            const string selectCount = "COUNT(1)";
            result.Select = new [] { selectCount };

            return result;
        }
    }
}
