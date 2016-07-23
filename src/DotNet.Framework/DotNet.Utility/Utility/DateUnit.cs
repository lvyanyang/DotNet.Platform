// ===============================================================================
// DotNet.Platform 开发框架 2016 版权所有
// ===============================================================================
namespace DotNet.Utility
{
    /// <summary>
    /// 日期单位
    /// </summary>
    public enum DateUnit
    {
        /// <summary>
        /// 年
        /// </summary>
        [EnumCaption("年")]
        Year,

        /// <summary>
        /// 月
        /// </summary>
        [EnumCaption("月")]
        Month,
        
        /// <summary>
        /// 日
        /// </summary>
        [EnumCaption("天")]
        Day,

        /// <summary>
        /// 周
        /// </summary>
        [EnumCaption("星期")]
        Week,

        /// <summary>
        /// 时
        /// </summary>
        [EnumCaption("小时")]
        Hour,

        /// <summary>
        /// 分
        /// </summary>
        [EnumCaption("分钟")]
        Minute,

        /// <summary>
        /// 秒
        /// </summary>
        [EnumCaption("秒")]
        Second
    }
}
