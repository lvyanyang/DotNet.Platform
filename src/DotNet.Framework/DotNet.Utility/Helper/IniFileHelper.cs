// ===============================================================================
// DotNet.Platform 开发框架 2016 版权所有
// ===============================================================================
using System;
using System.IO;
using System.Text;

namespace DotNet.Helper
{
    /// <summary>
    /// Ini文件读写操作类
    /// </summary>
    /// <example>
    /// <code>
    /// using XCI.Helper;
    /// 
    /// const string path = @"c:\test.ini";
    /// const string section = "App";
    /// const string section1 = "XCI";
    /// const string key = "UserName";
    /// const string value = "lyy";
    /// Console.WriteLine("读取key({0})={1}", key, IniFileHelper.Read(path, section, key));
    /// Console.WriteLine("写入key({0}) value({1})", key, value);
    /// IniFileHelper.Write(path, section, key, value);
    /// IniFileHelper.Write(path, section1, key, value);
    /// Console.WriteLine("读取key({0})={1}", key, IniFileHelper.Read(path, section, key));
    /// //Console.WriteLine("清空段落{0}", section);
    /// //IniFileHelper.Clear(path, section);
    /// Console.WriteLine("读取key({0})={1}", key, IniFileHelper.Read(path, section, key));
    /// Console.ReadLine();
    /// 
    /// /* 结果
    /// [XCI]
    /// UserName=lyy
    /// [App]
    /// UserName=lyy
    /// */
    /// </code>
    /// </example>
    public static class IniFileHelper
    {
        /// <summary>
        /// 写Ini文件,如果文件没有找到，则函数会创建它
        /// </summary>
        /// <param name="path">文件路径</param>
        /// <param name="section">段落</param>
        /// <param name="key">关键字</param>
        /// <param name="value">值</param>
        public static void Write(string path, string section, string key, string value)
        {
            if (path == null) throw new ArgumentNullException("path");
            NativeMethods.WritePrivateProfileString(section, key, value, path);
        }

        /// <summary>
        /// 读取Ini文件
        /// </summary>
        /// <param name="path">文件路径</param>
        /// <param name="section">段落</param>
        /// <param name="key">关键字</param>
        /// <returns>值</returns>
        public static string Read(string path, string section, string key)
        {
            if (path == null)
            {
                throw new ArgumentNullException("path");
            }
            if (!File.Exists(path))
            {
                return string.Empty;
            }
            StringBuilder temp = new StringBuilder(255);
            NativeMethods.GetPrivateProfileString(section, key, string.Empty, temp, 255, path);
            return temp.ToString();
        }

        /// <summary>
        /// 删除Ini文件下指定段落下的所有键
        /// </summary>
        /// <param name="path">文件路径</param>
        /// <param name="section">段落</param>
        public static void Clear(string path, string section)
        {
            if (path == null)
            {
                throw new ArgumentNullException("path");
            }
            Write(path, section, null, null);
        }
    }
}