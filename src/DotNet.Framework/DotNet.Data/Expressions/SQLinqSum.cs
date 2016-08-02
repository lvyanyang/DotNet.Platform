// ===============================================================================
// DotNet.Platform 开发框架 2016 版权所有
// ===============================================================================
namespace DotNet.Data.Expressions
{
    /// <summary>
    /// Sum语句
    /// </summary>
    /// <typeparam name="T">对象类型</typeparam>
    public class SQLinqSum<T> : ISQLinq
    {
        /// <summary>
        /// 构造Count语句
        /// </summary>
        /// <param name="query">查询对象</param>
        /// <param name="name">列名</param>
        public SQLinqSum(SQLQuery<T> query,string name)
        {
            this.Query = query;
            this.Name = name;
        }

        /// <summary>
        /// 列名
        /// </summary>
        public string Name { get; set; }

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

            string selectCount = $"Sum({Name})";
            result.Select = new [] { selectCount };

            return result;
        }
    }
}
