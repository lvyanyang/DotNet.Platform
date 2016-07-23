// ===============================================================================
// DotNet.Platform 开发框架 2016 版权所有
// ===============================================================================
namespace DotNet.Configuration
{
    /// <summary>
    /// 表示一个本地Json数据文件
    /// </summary>
    public class JsonDataFile<T> : JsonConfigFile<T> where T : class,new()
    {
        /// <summary>
        /// 使用指定文件名初始化本地数据文件实例,并自动加载文件数据
        /// </summary>
        /// <param name="fileName">数据文件名称(不含路径)</param>
        public JsonDataFile(string fileName)
            : base(SystemDirectory.DataDirectory,fileName)
        {
        }

        /// <summary>
        /// 使用指定文件名初始化本地数据文件实例,并自动加载文件数据
        /// </summary>
        /// <param name="directoryName">目录名称</param>
        /// <param name="fileName">数据文件名称(不含路径)</param>
        public JsonDataFile(string directoryName, string fileName)
            : base(directoryName, fileName)
        {
        }
    }
}