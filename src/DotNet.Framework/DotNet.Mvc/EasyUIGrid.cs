// ===============================================================================
// DotNet.Platform 开发框架 2016 版权所有
// ===============================================================================
using Newtonsoft.Json;

namespace DotNet.Mvc
{
    /// <summary>
    /// Json表格数据消息
    /// </summary>
    public class EasyUIGrid
    {
        /// <summary>
        /// 构造Json表格数据消息
        /// </summary>
        public EasyUIGrid()
        {

        }

        /// <summary>
        /// 构造Json表格数据消息
        /// </summary>
        /// <param name="total">总行数</param>
        /// <param name="rows">数据行对象</param>
        public EasyUIGrid(int total, object rows)
        {
            if (total == 0)
            {
                total = 1;
            }
            this.Total = total;
            this.Rows = rows;
        }

        /// <summary>
        /// 总行数
        /// </summary>
        [JsonProperty("total")]
        public int Total { get; set; }

        /// <summary>
        /// 数据行对象
        /// </summary>
        [JsonProperty("rows")]
        public object Rows { get; set; }
    }
}