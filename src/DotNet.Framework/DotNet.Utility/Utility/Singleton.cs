// ===============================================================================
// DotNet.Platform 开发框架 2016 版权所有
// ===============================================================================
namespace DotNet.Utility
{
    /// <summary>
    /// 单例对象管理
    /// </summary>
    /// <typeparam name="T">对象类型</typeparam>
    public static class Singleton<T> where T : new()
    {
        private static readonly T _instance = new T();

        /// <summary>
        /// 单例对象
        /// </summary>
        public static T Instance
        {
            get { return _instance; }
        }
    }
}
