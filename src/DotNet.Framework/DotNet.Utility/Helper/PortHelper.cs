// ===============================================================================
// DotNet.Platform 开发框架 2016 版权所有
// ===============================================================================
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;

namespace DotNet.Helper
{
    /// <summary>
    /// 系统端口操作类
    /// </summary>
    /// <example>
    /// <code>
    /// using XCI.Helper;
    /// 
    /// #region 获取随机端口
    /// 
    /// Console.WriteLine("获取随机端口:{0}", PortHelper.GetRandomAvailablePort());
    /// Console.WriteLine();
    /// 
    /// #endregion
    /// 
    /// #region 检测指定端口是否被占用
    /// 
    /// try
    /// {
    ///     const int port = 2038;
    ///     Console.WriteLine("正在检测端口{0}...", port);
    ///     bool isUse = PortHelper.CheckPortIsUse(port);
    ///     Console.WriteLine(isUse ? string.Format("端口{0}已经使用", port) : string.Format("端口{0}没有使用", port));
    /// }
    /// catch (ArgumentException ex)
    /// {
    ///     Console.WriteLine(ex.Message);
    /// }
    /// Console.WriteLine();
    /// 
    /// #endregion
    /// 
    /// #region 获取操作系统已占用的端口列表
    /// 
    /// int[] ports = PortHelper.GetAllUsePort();
    /// StringBuilder sb = new StringBuilder();
    /// foreach (int i in ports)
    /// {
    ///     sb.AppendFormat("{0},", i);
    /// }
    /// Console.WriteLine("系统已占用的端口:");
    /// Console.WriteLine(sb.ToString().TrimEnd(','));
    /// 
    /// #endregion
    /// 
    /// Console.ReadLine();
    /// 
    /// /* 输出结果
    ///    获取随机端口:56131
    /// 
    ///    正在检测端口2038...
    ///    端口2038没有使用
    /// 
    ///    系统已占用的端口:
    ///    135,443,445,902,912,1025,1026,1027,1029,1034,1035,1433,1521,3306,4498,5678,8798,
    ///    18386,1030,1434,5354,5939,8307,27015,139,500,4000,4001,4002,4003,4004,4005,4007,
    ///    4008,4500,5355,5678,8976,9139,18386,49154,49160,52355,55662,56034,56210,57052,57
    ///    054,57055,62392,62393,64545,1900,49152,49153,49157,49158,49159,49161,51280,57984
    ///    ,61901,137,138,1900,5353,51279,1028,1032,5354,27015,1267,1509,1596,2954,2955,449
    ///    5,5542,5729,5730,8036,9140,9485,10650,10925,11128,11129,11169,11201,11302,11322,
    ///    11324,11325,11326,11327,11379,11396,11418,11433,11434,11439,11449,11451,11459,11
    ///    461,11462,11463,11464,11465
    ///  */
    /// </code>
    /// </example>
    public static class PortHelper
    {
        /// <summary>
        /// 系统最大端口
        /// </summary>
        private const int MaxPort = 65535;

        /// <summary>
        /// 获取第一个可用端口 (系统最大端口是65535)
        /// </summary>
        /// <param name="beginPort">起始端口(不能超过65535)</param>
        /// <exception cref="System.ArgumentException">起始端口不能超过65535</exception>
        /// <returns>返回从起始端口开始的第一个可用端口</returns>
        public static int GetFirstAvailablePort(int beginPort)
        {
            if (beginPort >= MaxPort)
            {
                throw new ArgumentException("起始端口不能超过65535");
            }
            var portList = GetAllUsePort();
            for (int i = beginPort; i < MaxPort; i++)
            {
                if (portList.Any(p => p != i))
                {
                    return i;
                }
            }
            return -1;
        }

        /// <summary>
        /// 获取一个随机可用端口
        /// </summary>
        /// <returns>返回一个随机可用端口</returns>
        public static int GetRandomAvailablePort()
        {
            var portList = GetAllUsePort();
            Random random = new Random();
            int port = 0;
            bool available = true;
            while (available)
            {
                port = random.Next(10001, 65534);
                available = portList.Any(p => p == port);
            }
            return port;
        }

        /// <summary>
        /// 获取操作系统已占用的端口列表
        /// </summary>
        /// <returns>返回已占用的端口列表</returns>
        public static int[] GetAllUsePort()
        {
            //获取本地计算机的网络连接和通信统计数据的信息 
            IPGlobalProperties ipGlobalProperties = IPGlobalProperties.GetIPGlobalProperties();

            //返回本地计算机上的所有Tcp监听程序 
            IPEndPoint[] ipsTcp = ipGlobalProperties.GetActiveTcpListeners();

            //返回本地计算机上的所有UDP监听程序 
            IPEndPoint[] ipsUdp = ipGlobalProperties.GetActiveUdpListeners();

            //返回本地计算机上的Internet协议版本4(IPV4 传输控制协议(TCP)连接的信息。 
            TcpConnectionInformation[] tcpConnInfoArray = ipGlobalProperties.GetActiveTcpConnections();

            List<int> allPorts = new List<int>();
            foreach (IPEndPoint ep in ipsTcp) allPorts.Add(ep.Port);
            foreach (IPEndPoint ep in ipsUdp) allPorts.Add(ep.Port);
            foreach (TcpConnectionInformation conn in tcpConnInfoArray) allPorts.Add(conn.LocalEndPoint.Port);

            return allPorts.ToArray();
        }

        /// <summary>
        /// 检测指定端口是否被占用
        /// </summary>
        /// <param name="port">测试的端口</param>
        /// <exception cref="System.ArgumentException">起始端口不能超过65535</exception>
        /// <returns>如果端口被占用返回true,否则返回false</returns>
        public static bool CheckPortIsUse(int port)
        {
            if (port >= MaxPort)
            {
                throw new ArgumentException("起始端口不能超过65535");
            }
            var portUsed = GetAllUsePort();
            return portUsed.Any(p => p == port);
        }
    }
}