// ===============================================================================
// DotNet.Platform 开发框架 2016 版权所有
// ===============================================================================
using System;
using System.Diagnostics;
using System.Reflection;

namespace DotNet.Helper
{
    /// <summary>
    /// 调试操作类
    /// </summary>
    public static class DebugHelper
    {
        /// <summary>
        /// 输出调试信息到控制台
        /// </summary>
        /// <param name="message">调试消息</param>
        public static void Debug(string message)
        {
#if DEBUG
            string className = string.Empty;
            string methodName = string.Empty;
            var trace = new StackTrace();
            var sf = trace.GetFrame(0);
            var method1 = (sf.GetMethod());

            if (method1.DeclaringType != null)
            {
                className = method1.DeclaringType.FullName;
                methodName = method1.Name;
            }

            string msg = string.Format("{0} Debug {1}.{2} {3}",
                DateTimeHelper.FormatDateHasSecond(DateTime.Now),
                className, methodName, message);
            System.Diagnostics.Debug.WriteLine(msg);
#endif
        }

        /// <summary>
        /// 输出调试信息到控制台
        /// </summary>
        /// <param name="message">调试消息</param>
        /// <param name="startMilliSecond">代码执行的开始毫秒数</param>
        public static void Debug(string message, int startMilliSecond)
        {
#if DEBUG
            int tickCount = Environment.TickCount;
            string className = string.Empty;
            string methodName = string.Empty;
            var trace = new StackTrace();
            var sf = trace.GetFrame(0);
            var method1 = (sf.GetMethod());

            if (method1.DeclaringType != null)
            {
                className = method1.DeclaringType.FullName;
                methodName = method1.Name;
            }
            TimeSpan ts = TimeSpan.FromMilliseconds((tickCount - startMilliSecond));
            string timeString = DateTimeHelper.GetTimeString(ts);
            if (string.IsNullOrEmpty(timeString))
            {
                timeString = string.Concat(ts.TotalMilliseconds, "毫秒");
            }
            string msg = string.Format("{0} Debug {1}.{2} {3} 执行耗时：{4}",
                DateTimeHelper.FormatDateHasSecond(DateTime.Now),
                className, methodName, message, timeString);
            System.Diagnostics.Debug.WriteLine(msg);
#endif
        }

        /// <summary>
        /// 开始计算程序执行时间(默认返回:查询耗时：xx毫秒)
        /// </summary>
        /// <param name="action">执行内容</param>
        /// <returns>返回程序执行的时间</returns>
        public static string StartWatch(Action action)
        {
            if (action == null) return string.Empty;
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            action();
            stopwatch.Stop(); //  停止监视
            TimeSpan ts = stopwatch.Elapsed;
            string timeString = DateTimeHelper.GetTimeString(ts);
            if (string.IsNullOrEmpty(timeString))
            {
                timeString = string.Concat(ts.TotalMilliseconds, "毫秒");
            }
            return string.Concat("执行耗时：", timeString);
        }

        ///// </summary>
        ///// 添加调试信息

        ///// <summary>
        ///// <paramColl name="title">标题</paramColl>
        ///// <paramColl name="sql">sql语句</paramColl>
        ///// <paramColl name="parameters">db参数</paramColl>
        //public static void WriteDebug(string title, string sql, DbParameterCollection parameters)
        //{
        //    StringBuilder sb = new StringBuilder();
        //    //sb.Append("------ ChinaSoft 项目调试信息 ------\n");
        //    if (title != null) sb.Append(title);
        //    //sb.Append("\n");
        //    if (sql != null) sb.Append("\nsql命令->" + sql);
        //    sb.Append("\n");
        //    sb.Append(GetParameterString(parameters));
        //    Debug.WriteLine(sb.ToString());
        //}

        ///// <summary>
        ///// 添加调试异常信息
        ///// </summary>
        ///// <paramColl name="title">标题</paramColl>
        ///// <paramColl name="e">异常对象</paramColl>
        //public static void WriteDebug(string title, Exception e)
        //{
        //    StringBuilder sb = new StringBuilder();
        //    if (e != null)
        //    {
        //        if (title != null) sb.Append(string.Format("\n------ {0}时发生异常  ------\n", title));
        //        sb.Append(GetExceptionString(e));
        //        if (title != null) sb.Append(string.Format("\n------ {0}时发生异常  ------\n", title));
        //        Debug.WriteLine(sb.ToString());
        //    }
        //}

        ///// <summary>
        ///// 开始调试
        ///// </summary>
        ///// <paramColl name="methodBase">方法信息</paramColl>
        ///// <returns>获取系统启动后经过的毫秒数</returns>
        //public static int StartDebug(MethodBase methodBase)
        //{
        //    Debug.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " 开始: " + methodBase.ReflectedType.Name + "." + methodBase.Name);
        //    //Trace.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " :Begin: " + methodBase.ReflectedType.Name + "." + methodBase.Name);
        //    return Environment.TickCount;
        //}

        ///// <summary>
        ///// 结束调试
        ///// </summary>
        ///// <paramColl name="methodBase">方法信息</paramColl>
        ///// <paramColl name="milliStart">开始毫秒</paramColl>
        ///// <returns>执行时间</returns>
        //public static int EndDebug(MethodBase methodBase, int milliStart)
        //{
        //    int tickCount = Environment.TickCount;
        //    string[] strArray1 = new string[7];
        //    string[] strArray2 = strArray1;
        //    DateTime now = DateTime.Now;
        //    string str1 = now.ToString("yyyy-MM-dd HH:mm:ss");
        //    strArray2[0] = str1;
        //    strArray1[1] = " Ticks: ";
        //    strArray1[2] = TimeSpan.FromMilliseconds((tickCount - milliStart)).ToString();
        //    strArray1[3] = " :End: ";
        //    strArray1[4] = methodBase.ReflectedType.Name;
        //    strArray1[5] = ".";
        //    strArray1[6] = methodBase.Name;
        //    Debug.WriteLine(string.Concat(strArray1));
        //    string[] strArray3 = new string[7];
        //    string[] strArray4 = strArray3;
        //    now = DateTime.Now;
        //    string str2 = now.ToString("yyyy-MM-dd HH:mm:ss");
        //    strArray4[0] = str2;
        //    strArray3[1] = " Ticks: ";
        //    strArray3[2] = TimeSpan.FromMilliseconds((tickCount - milliStart)).ToString();
        //    strArray3[3] = " :End: ";
        //    strArray3[4] = methodBase.ReflectedType.Name;
        //    strArray3[5] = ".";
        //    strArray3[6] = methodBase.Name;
        //    Debug.WriteLine(string.Concat(strArray3));
        //    return tickCount - milliStart;
        //}

        ///// <summary>
        ///// 获取异常信息
        ///// </summary>
        ///// <paramColl name="e">异常对象</paramColl>
        ///// <returns>返回异常信息字符串</returns>
        //private static string GetExceptionString(Exception e)
        //{
        //    StringBuilder sb = new StringBuilder();
        //    sb.Append("--- 错误信息 ---\n");
        //    sb.Append(e.Message);
        //    sb.Append("\n--- 导致错误的应用程序或对象的名称 ---\n");
        //    sb.Append(e.Source);
        //    sb.Append("\n--- 引发当前异常的方法 ---\n");
        //    sb.Append(e.TargetSite.Name);
        //    sb.Append("\n--- 当前异常发生时调用堆栈上的帧的字符串表示形式 ---\n");
        //    sb.Append(e.StackTrace);
        //    return sb.ToString();
        //}

        ///// <summary>
        ///// 把参数集合转换为字符串
        ///// </summary>
        ///// <paramColl name="paramColl">参数集合</paramColl>
        ///// <returns>返回参数信息字符串</returns>
        //private static string GetParameterString(DbParameterCollection paramColl)
        //{
        //    if (paramColl == null || paramColl.Count == 0)
        //        return string.Empty;
        //    StringBuilder sb = new StringBuilder();
        //    //sb.Append("\n");
        //    foreach (DbParameter item in paramColl)
        //    {
        //        sb.Append(item.ParameterName);
        //        sb.Append(" ->");
        //        sb.Append(" 值:");
        //        sb.Append(item.Value);
        //        sb.Append(" 类型:");
        //        sb.Append(item.DbType.ToString());
        //        sb.Append(" 大小:");
        //        sb.Append(item.Size);
        //        sb.Append(" 输入输出:");
        //        sb.Append(item.Direction.ToString());
        //        sb.Append("\n");
        //    }
        //    sb.Append("\n");
        //    return sb.ToString();
        //}
    }
}
