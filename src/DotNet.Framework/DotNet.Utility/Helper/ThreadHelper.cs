// ===============================================================================
// DotNet.Platform 开发框架 2016 版权所有
// ===============================================================================
using System;
using System.Threading;

namespace DotNet.Helper
{
    /// <summary>
    /// 多线程操作类
    /// </summary>
    public static class ThreadHelper
    {
        /// <summary>
        /// 开始一个新线程
        /// </summary>
        /// <param name="action">执行的代码</param>
        /// <param name="isBackground">是否后台进程</param>
        public static Thread StartThread(Action action, bool isBackground)
        {
            return StartThread(action, isBackground, null);
        }

        /// <summary>
        /// 开始一个新线程
        /// </summary>
        /// <param name="action">执行的代码</param>
        /// <param name="isBackground">是否后台进程</param>
        /// <param name="name">线程名称</param>
        public static Thread StartThread(Action action, bool isBackground, string name)
        {
            Thread thread = new Thread(new ThreadStart(action));
            if (!string.IsNullOrEmpty(name))
            {
                thread.Name = name;
            }
            thread.IsBackground = isBackground;
            thread.Start();
            return thread;
        }

        /// <summary>
        /// 开始一个新的后台进程线程
        /// </summary>
        /// <param name="action">执行的代码</param>
        public static Thread StartThread(Action action)
        {
            return StartThread(action, true);
        }
    }
}