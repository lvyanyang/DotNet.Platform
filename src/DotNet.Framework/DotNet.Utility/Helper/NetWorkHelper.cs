// ===============================================================================
// DotNet.Platform ������� 2016 ��Ȩ����
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
    /// ���������
    /// </summary>
    /// <example>
    /// <code>
    /// using XCI.Helper;
    /// 
    /// #region �����������
    /// 
    /// bool isConn = NetWorkHelper.IsConnected();
    /// if (isConn)
    /// {
    ///     Console.ForegroundColor = ConsoleColor.Green;
    ///     Console.WriteLine("����������");
    /// }
    /// else
    /// {
    ///     Console.ForegroundColor = ConsoleColor.Red;
    ///     Console.WriteLine("û�л����");
    /// }
    /// 
    /// #endregion
    /// 
    /// Console.ForegroundColor = ConsoleColor.Gray;
    /// 
    /// #region Ping
    /// 
    /// const string serverip = "192.168.0.1";
    /// Console.WriteLine("����Ping������{0}...", serverip);
    /// bool isPing = NetWorkHelper.Ping(serverip);
    /// if (isPing)
    /// {
    ///     Console.ForegroundColor = ConsoleColor.Green;
    ///     Console.WriteLine("�������{0}ͨѶ����", serverip);
    /// }
    /// else
    /// {
    ///     Console.ForegroundColor = ConsoleColor.Red;
    ///     Console.WriteLine("�������{0}�޷�ͨѶ", serverip);
    /// }
    /// 
    /// #endregion
    /// 
    /// Console.ForegroundColor = ConsoleColor.Gray;
    /// 
    /// #region ���������� ������Ϣ
    /// 
    /// Console.WriteLine("��ȡ������Ϣ...");
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
    /// #region ��ȡIP
    /// 
    /// Console.WriteLine("��ȡ����IP...");
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
        /// ����Ƿ��л����
        /// </summary>
        /// <returns>����л���ӷ���true</returns>
        public static bool IsConnected()
        {
            int i;
            return NativeMethods.InternetGetConnectedState(out i, 0);
        }

        /// <summary>
        /// Pingָ��������
        /// </summary>
        /// <param name="ip">ip��ַ��������������</param>
        /// <returns>�������Pingͨ����true</returns>
        public static bool Ping(string ip)
        {
            try
            {
                using (Ping p = new Ping())
                {
                    PingOptions options = new PingOptions { DontFragment = true };
                    const string data = "Test";
                    byte[] buffer = Encoding.ASCII.GetBytes(data);
                    const int timeout = 3000; // Timeout ʱ�䣬��λ������
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
        /// ��ȡ����������(����)
        /// </summary>
        /// <param name="isShowAll">�Ƿ���ʾȫ�� ���Ϊfalse,��ֻ��ʾ�����ӵ�����</param>
        /// <returns>���������������б�</returns>
        public static string[] GetNetworkAdapter(bool isShowAll)
        {
            List<string> allNetworkAdapte = new List<string>();
            ManagementClass mc = new ManagementClass("Win32_NetworkAdapterConfiguration");
            var conns = mc.GetInstances();
            foreach (var o in conns)
            {
                var mo = (ManagementObject)o;
                //foreach (PropertyData property in mo.Properties) //��ʾ:����-ֵ
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
        /// ��ȡ��������������(����)
        /// </summary>
        /// <returns>���������������б�</returns>
        public static string[] GetAllNetworkAdapter()
        {
            return GetNetworkAdapter(true);
        }

        /// <summary>
        /// �������������ƻ�ȡ����������
        /// </summary>
        /// <param name="adapterName">����������</param>
        /// <returns>��������������</returns>
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
        /// �����Զ���ȡDns������
        /// </summary>
        /// <param name="adapterName">����������</param>
        public static void SetAutoGetDnsServer(string adapterName)
        {
            ManagementObject networkAdapter = GetNetworkAdapterByName(adapterName);
            ManagementBaseObject parDNSRegistration = networkAdapter.GetMethodParameters("SetDynamicDNSRegistration");
            //�Զ���ȡDNS

            parDNSRegistration["FullDNSRegistrationEnabled"] = false;
            parDNSRegistration["DomainDNSRegistrationEnabled"] = false;

            networkAdapter.InvokeMethod("SetDynamicDNSRegistration", parDNSRegistration, null);
        }

        /// <summary>
        /// �����Զ���ȡIP��ַ
        /// </summary>
        /// <param name="adapterName">����������</param>
        public static void SetAutoGetIPAdress(string adapterName)
        {
            ManagementObject networkAdapter = GetNetworkAdapterByName(adapterName);
            networkAdapter.InvokeMethod("EnableDHCP", null, null);//�Զ���ȡIP
        }

        /// <summary>
        /// ����IP��ַ(�������ö��)
        /// </summary>
        /// <param name="adapterName">����������</param>
        /// <param name="ipAddress">IP��ַ����</param>
        /// <param name="subNetMask">������������</param>
        /// <param name="defaultIPGateway">Ĭ����������</param>
        public static void SetIPAdress(string adapterName, string[] ipAddress, string[] subNetMask,
                                       string[] defaultIPGateway)
        {
            ManagementObject networkAdapter = GetNetworkAdapterByName(adapterName);
            ManagementBaseObject parIPSetting = networkAdapter.GetMethodParameters("EnableStatic");//��̬IP
            parIPSetting["IPAddress"] = ipAddress;
            parIPSetting["SubnetMask"] = subNetMask;
            networkAdapter.InvokeMethod("EnableStatic", parIPSetting, null);

            ManagementBaseObject parSetGateways = networkAdapter.GetMethodParameters("SetGateways");//Ĭ������
            parSetGateways["DefaultIPGateway"] = defaultIPGateway;
            networkAdapter.InvokeMethod("SetGateways", parSetGateways, null);
        }

        /// <summary>
        /// ����Dns������(�������ö��)
        /// </summary>
        /// <param name="adapterName">����������</param>
        /// <param name="dsnServer">Dns����������</param>
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
        /// ����IE���������
        /// </summary>
        /// <param name="isProxyEnable">�Ƿ����ô���</param>
        /// <param name="proxyServer">���������</param>
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
                //�����������
                NativeMethods.InternetSetOption(0, 39, IntPtr.Zero, 0);
                NativeMethods.InternetSetOption(0, 37, IntPtr.Zero, 0);
            }
        }

        /// <summary>
        /// ��ȡ����IP��ַ����
        /// </summary>
        /// <rereturns>���ر���IP��ַ����</rereturns>
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
        /// ��ȡ����IP��ַ
        /// </summary>
        /// <returns>���ر���IP��ַ</returns>
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
        /// ��ȡ����Mac
        /// </summary>
        /// <returns>���ػ�ȡ����Mac</returns>
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