// ===============================================================================
// DotNet.Platform 开发框架 2016 版权所有
// ===============================================================================
namespace DotNet.Utility
{
    /// <summary>
    /// 分页条件
    /// </summary>
    public class PaginationCondition
    {
        /// <summary>
        /// 构造默认分页条件
        /// </summary>
        public PaginationCondition()
        {

        }

        /// <summary>
        /// 按指定条件构造分页条件
        /// </summary>
        /// <param name="pageIndex">页码索引,从1开始</param>
        /// <param name="pageSize">每页记录数</param>
        public PaginationCondition(int pageIndex, int pageSize)
        {
            PageIndex = pageIndex;
            PageSize = pageSize;
        }

        /// <summary>
        /// 按指定条件构造分页条件
        /// </summary>
        /// <param name="pageIndex">页码索引,从1开始</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="orderName">排序字段名称</param>
        /// <param name="orderDir">排序方式(asc/desc)</param>
        public PaginationCondition(int pageIndex, int pageSize, string orderName, string orderDir)
        {
            PageIndex = pageIndex;
            PageSize = pageSize;
            OrderName = orderName;
            OrderDir = orderDir;
        }

        /// <summary>
        /// 页码索引,从1开始
        /// </summary>
        public int PageIndex { get; set; }

        /// <summary>
        /// 每页记录数
        /// </summary>
        public int PageSize { get; set; }

        /// <summary>
        /// 排序字段名称
        /// </summary>
        public string OrderName { get; set; }

        /// <summary>
        /// 排序方式(asc/desc)
        /// </summary>
        public string OrderDir { get; set; }

        /// <summary>
        /// 指定默认的排序信息,如果已经指定了,则不设置.
        /// </summary>
        /// <param name="orderName">排序字段名称</param>
        /// <param name="orderDir">排序方式(asc/desc)</param>
        public void SetDefaultOrder(string orderName, string orderDir)
        {
            if (string.IsNullOrEmpty(OrderName))
            {
                OrderName = orderName;
            }
            if (string.IsNullOrEmpty(OrderDir))
            {
                OrderDir = orderDir;
            }
        }

        /// <summary>
        /// 指定默认的排序信息,如果已经指定了,则不设置.
        /// </summary>
        /// <param name="orderName">排序字段名称</param>
        /// <param name="isDesc">是否倒序</param>
        public void SetDefaultOrder(string orderName, bool isDesc = true)
        {
            if (string.IsNullOrEmpty(OrderName))
            {
                OrderName = orderName;
            }
            if (string.IsNullOrEmpty(OrderDir))
            {
                OrderDir = isDesc? "desc": "asc";
            }
        }
    }
}