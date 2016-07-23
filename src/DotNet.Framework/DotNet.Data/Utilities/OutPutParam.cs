// ===============================================================================
// DotNet.Platform 开发框架 2016 版权所有
// ===============================================================================
using System.Data.Common;

namespace DotNet.Data.Utilities
{
    /// <summary>
    /// 输出参数
    /// </summary>
    public class OutPutParam
    {
        /// <summary>
        /// 初始化 <see cref="T:System.Object"/> 类的新实例。
        /// </summary>
        public OutPutParam()
        {
        }

        /// <summary>
        /// 初始化 <see cref="T:System.Object"/> 类的新实例。
        /// </summary>
        /// <param name="isCursor">是否游标参数</param>
        public OutPutParam(bool isCursor)
        {
            IsCursor = isCursor;
        }

        /// <summary>
        /// 关联的参数对象
        /// </summary>
        public DbParameter InnerParam { get; internal set; }

        /// <summary>
        /// 是否游标参数
        /// </summary>
        public bool IsCursor { get; set; }

        /// <summary>
        /// 参数值
        /// </summary>
        public object Value
        {
            get { return InnerParam.Value; }
        }
    }
}