// ===============================================================================
// DotNet.Platform 开发框架 2016 版权所有
// ===============================================================================
using System;
using System.Diagnostics;
using System.Reflection;

namespace DotNet.Helper
{
    /// <summary>
    /// 进程操作帮助类
    /// </summary>
    public static class ProcessHelper
    {
        /// <summary>
        /// 激活指定进程主界面
        /// </summary>
        /// <param name="instance">指定进程</param>
        public static void ActivateWindow(Process instance)
        {
            IntPtr ParenthWnd = NativeMethods.FindWindow(null, instance.MainWindowTitle);
            if (ParenthWnd != IntPtr.Zero)
            {
                NativeMethods.ShowWindow(ParenthWnd, 1);//显示
                NativeMethods.SetForegroundWindow(ParenthWnd);//当到最前端
            }
        }

        /// <summary>
        /// 查找当前进程对象
        /// </summary>
        public static Process GetProcess()
        {
            Process currentProcess = Process.GetCurrentProcess();
            Process[] Processes = Process.GetProcessesByName(currentProcess.ProcessName);
            foreach (Process process in Processes)
            {
                if (process.Id != currentProcess.Id)
                {
                    if (Assembly.GetExecutingAssembly().Location.Replace("/", "\\") == currentProcess.MainModule.FileName)
                    {
                        return process;
                    }
                }
            }
            return null;
        }
    }
}