// ===============================================================================
// DotNet.Platform 开发框架 2016 版权所有
// ===============================================================================
using System;
using System.Collections.Generic;
using System.Management;
using System.Net;
using System.Net.NetworkInformation;
using System.Text;
using Microsoft.Win32;

namespace DotNet.Helper
{
    /// <summary>
    /// 网络操作类
    /// </summary>
    /// <example>
    /// <code>
    /// using XCI.Helper;
    /// 
    /// #region 检测网络活动连接
    /// 
    /// bool isConn = NetWorkHelper.IsConnected();
    /// if (isConn)
    /// {
    ///     Console.ForegroundColor = ConsoleColor.Green;
    ///     Console.WriteLine("网络已连接");
    /// }
    /// else
    /// {
    ///     Console.ForegroundColor = ConsoleColor.Red;
    ///     Console.WriteLine("没有活动连接");
    /// }
    /// 
    /// #endregion
    /// 
    /// Console.ForegroundColor = ConsoleColor.Gray;
    /// 
    /// #region Ping
    /// 
    /// const string serverip = "192.168.0.1";
    /// Console.WriteLine("正在Ping服务器{0}...", serverip);
    /// bool isPing = NetWorkHelper.Ping(serverip);
    /// if (isPing)
    /// {
    ///     Console.ForegroundColor = ConsoleColor.Green;
    ///     Console.WriteLine("与服务器{0}通讯正常", serverip);
    /// }
    /// else
    /// {
    ///     Console.ForegroundColor = ConsoleColor.Red;
    ///     Console.WriteLine("与服务器{0}无法通讯", serverip);
    /// }
    /// 
    /// #endregion
    /// 
    /// Console.ForegroundColor = ConsoleColor.Gray;
    /// 
    /// #region 网络适配器 网卡信息
    /// 
    /// Console.WriteLine("读取网卡信息...");
    /// Console.ForegroundColor = ConsoleColor.Green;
    /// var adapters = NetWorkHelper.GetNetworkAdapter(false);
    /// foreach (string adapter in adapters)
    /// {
    ///     Console.WriteLine(adapter);
    /// }
    /// 
    /// #endregion
    /// 
    /// Console.ForegroundColor = ConsoleColor.Gray;
    /// 
    /// #region 获取IP
    /// 
    /// Console.WriteLine("读取本机IP...");
    /// Console.ForegroundColor = ConsoleColor.Green;
    /// var ips = NetWorkHelper.GetLocalIPs();
    /// foreach (string ip in ips)
    /// {
    ///     Console.WriteLine(ip);
    /// }
    /// 
    /// #endregion
    /// 
    /// Console.ReadLine();
    /// </code>
    /// <img src="images/NetWorkHelper.jpg" />
    /// </example>
    public static class NetWorkHelper
    {
        /// <summary>
        /// 检测是否有活动连接
        /// </summary>
        /// <returns>如果有活动连接返回true</returns>
        public static bool IsConnected()
        {
            int i;
            return NativeMethods.InternetGetConnectedState(out i, 0);
        }

        /// <summary>
        /// Ping指定的主机
        /// </summary>
        /// <param name="ip">ip地址或主机名或域名</param>
        /// <returns>如果网络Ping通返回true</returns>
        public static bool Ping(string ip)
        {
            try
            {
                using (Ping p = new Ping())
                {
                    PingOptions options = new PingOptions { DontFragment = true };
                    const string data = "Test";
                    byte[] buffer = Encoding.ASCII.GetBytes(data);
                    const int timeout = 3000; // Timeout 时间，单位：毫秒
                    PingReply reply = p.Send(ip, timeout, buffer, options);
                    if (reply != null && reply.Status == IPStatus.Success)
                        return true;
                    return false;
                }
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 获取网络适配器(网卡)
        /// </summary>
        /// <param name="isShowAll">是否显示全部 如果为false,则只显示已连接的网卡</param>
        /// <returns>返回网络适配器列表</returns>
        public static string[] GetNetworkAdapter(bool isShowAll)
        {
            List<string> allNetworkAdapte = new List<string>();
            ManagementClass mc = new ManagementClass("Win32_NetworkAdapterConfiguration");
            var conns = mc.GetInstances();
            foreach (var o in conns)
            {
                var mo = (ManagementObject)o;
                //foreach (PropertyData property in mo.Properties) //显示:属性-值
                //{
                //    Debug.WriteLine(property.Name+":"+property.Value);
                //}
                if (isShowAll)
                {
                    allNetworkAdapte.Add(mo["Caption"].ToString());
                }
                else if ((bool)mo["IPEnabled"])
                {
                    allNetworkAdapte.Add(mo["Caption"].ToString());
                }
            }
            mc.Dispose();
            return allNetworkAdapte.ToArray();
        }

        /// <summary>
        /// 获取所有网络适配器(网卡)
        /// </summary>
        /// <returns>返回网络适配器列表</returns>
        public static string[] GetAllNetworkAdapter()
        {
            return GetNetworkAdapter(true);
        }

        /// <summary>
        /// 根据适配器名称获取适配器对象
        /// </summary>
        /// <param name="adapterName">适配器名称</param>
        /// <returns>返回适配器对象</returns>
        public static ManagementObject GetNetworkAdapterByName(string adapterName)
        {
            ManagementClass mc = new ManagementClass("Win32_NetworkAdapterConfiguration");
            var conns = mc.GetInstances();
            foreach (var o in conns)
            {
                var mo = (ManagementObject)o;
                if (mo["Caption"].ToString().Equals(adapterName))
                {
                    return mo;
                }
            }
            mc.Dispose();
            return null;
        }

        /// <summary>
        /// 设置自动获取Dns服务器
        /// </summary>
        /// <param name="adapterName">适配器名称</param>
        public static void SetAutoGetDnsServer(string adapterName)
        {
            ManagementObject networkAdapter = GetNetworkAdapterByName(adapterName);
            ManagementBaseObject parDNSRegistration = networkAdapter.GetMethodParameters("SetDynamicDNSRegistration");
            //自动获取DNS

            parDNSRegistration["FullDNSRegistrationEnabled"] = false;
            parDNSRegistration["DomainDNSRegistrationEnabled"] = false;

            networkAdapter.InvokeMethod("SetDynamicDNSRegistration", parDNSRegistration, null);
        }

        /// <summary>
        /// 设置自动获取IP地址
        /// </summary>
        /// <param name="adapterName">适配器名称</param>
        public static void SetAutoGetIPAdress(string adapterName)
        {
            ManagementObject networkAdapter = GetNetworkAdapterByName(adapterName);
            networkAdapter.InvokeMethod("EnableDHCP", null, null);//自动获取IP
        }

        /// <summary>
        /// 设置IP地址(可以设置多个)
        /// </summary>
        /// <param name="adapterName">适配器名称</param>
        /// <param name="ipAddress">IP地址数组</param>
        /// <param name="subNetMask">子网掩码数组</param>
        /// <param name="defaultIPGateway">默认网关数组</param>
        public static void SetIPAdress(string adapterName, string[] ipAddress, string[] subNetMask,
                                       string[] defaultIPGateway)
        {
            ManagementObject networkAdapter = GetNetworkAdapterByName(adapterName);
            ManagementBaseObject parIPSetting = networkAdapter.GetMethodParameters("EnableStatic");//静态IP
            parIPSetting["IPAddress"] = ipAddress;
            parIPSetting["SubnetMask"] = subNetMask;
            networkAdapter.InvokeMethod("EnableStatic", parIPSetting, null);

            ManagementBaseObject parSetGateways = networkAdapter.GetMethodParameters("SetGateways");//默认网关
            parSetGateways["DefaultIPGateway"] = defaultIPGateway;
            networkAdapter.InvokeMethod("SetGateways", parSetGateways, null);
        }

        /// <summary>
        /// 设置Dns服务器(可以设置多个)
        /// </summary>
        /// <param name="adapterName">适配器名称</param>
        /// <param name="dsnServer">Dns服务器数组</param>
        public static void SetDsnServer(string adapterName, string[] dsnServer)
        {
            ManagementObject networkAdapter = GetNetworkAdapterByName(adapterName);
            ManagementBaseObject parDNSRegistration = networkAdapter.GetMethodParameters("SetDynamicDNSRegistration");

            parDNSRegistration["FullDNSRegistrationEnabled"] = true;
            parDNSRegistration["DomainDNSRegistrationEnabled"] = false;

            networkAdapter.InvokeMethod("SetDynamicDNSRegistration", parDNSRegistration, null);

            ManagementBaseObject parsDsnServer = networkAdapter.GetMethodParameters("SetDNSServerSearchOrder");
            parsDsnServer["DNSServerSearchOrder"] = dsnServer;
            networkAdapter.InvokeMethod("SetDNSServerSearchOrder", parsDsnServer, null);
        }

        /// <summary>
        /// 设置IE代理服务器
        /// </summary>
        /// <param name="isProxyEnable">是否启用代理</param>
        /// <param name="proxyServer">代理服务器</param>
        public static void SetIEProxy(bool isProxyEnable, string proxyServer)
        {
            RegistryKey regKey = Registry.CurrentUser;
            const string subKeyPath = @"Software\Microsoft\Windows\CurrentVersion\Internet Settings";
            RegistryKey optionKey = regKey.OpenSubKey(subKeyPath, true);
            if (optionKey != null)
            {
                optionKey.SetValue("ProxyEnable", (isProxyEnable ? 1 : 0));
                if (isProxyEnable)
                    optionKey.SetValue("ProxyServer", proxyServer);
                regKey.Close();
                //激活代理设置
                NativeMethods.InternetSetOption(0, 39, IntPtr.Zero, 0);
                NativeMethods.InternetSetOption(0, 37, IntPtr.Zero, 0);
            }
        }

        /// <summary>
        /// 获取本机IP地址数组
        /// </summary>
        /// <rereturns>返回本机IP地址数组</rereturns>
        public static string[] GetLocalIPs()
        {
            IPHostEntry ips = Dns.GetHostEntry(Dns.GetHostName());
            List<string> list = new List<string>();
            if (ips.AddressList.Length > 0)
            {
                foreach (IPAddress address in ips.AddressList)
                {
                    if (address.AddressFamily.ToString().Equals("InterNetwork"))
                    {
                        list.Add(address.ToString());
                    }
                }
            }
            return list.ToArray();
        }

        /// <summary>
        /// 获取本机IP地址
        /// </summary>
        /// <returns>返回本机IP地址</returns>
        public static string GetLocalIP()
        {
            var sz = GetLocalIPs();
            if (sz.Length > 0)
            {
                return sz[0];
            }
            return "localhost";
        }

        /// <summary>
        /// 获取本机Mac
        /// </summary>
        /// <returns>返回获取本机Mac</returns>
        public static string GetLocalMac()
        {
            ManagementClass mc = new ManagementClass("Win32_NetworkAdapterConfiguration");
            var conns = mc.GetInstances();
            foreach (var o in conns)
            {
                var mo = (ManagementObject) o;
                if (Convert.ToBoolean(mo["IPEnabled"]))
                {
                    string mac = mo["MacAddress"].ToString();
                    mo.Dispose();
                    return mac;
                }
            }
            mc.Dispose();
            return string.Empty;
        }
    }
}