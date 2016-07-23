// ===============================================================================
// DotNet.Platform 开发框架 2016 版权所有
// ===============================================================================
namespace DotNet.Data.Utilities
{
	/// <summary>
    /// 包装一个DBType.AnsiString类型的字符串
	/// </summary>
	public class AnsiString
	{
	    /// <summary>
        /// 初始化AnsiString新实例。
		/// </summary>
		/// <param name="str">字符串</param>
		public AnsiString(string str)
		{
			Value = str;
		}

	    /// <summary>
		/// 字符串值
		/// </summary>
		public string Value 
		{ 
			get; 
			private set; 
		}
	}

}
