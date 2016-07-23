// ===============================================================================
// DotNet.Platform 开发框架 2016 版权所有
// ===============================================================================
namespace DotNet.Data.Expressions
{
    /// <summary>
    /// 查询语句接口
    /// </summary>
    public interface ISQLinq
    {
        /// <summary>
        /// 转为查询语句结果对象
        /// </summary>
        /// <param name="existingParameterCount">已经存在的参数个数</param>
        /// <returns>返回查询语句结果对象</returns>
        ISQLinqResult ToResult(int existingParameterCount = 0);
    }
}
