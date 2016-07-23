// ===============================================================================
// DotNet.Platform ������� 2016 ��Ȩ����
// ===============================================================================
using System;
using System.IO;
using System.Text;

namespace DotNet.Helper
{
    /// <summary>
    /// Ini�ļ���д������
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
    /// Console.WriteLine("��ȡkey({0})={1}", key, IniFileHelper.Read(path, section, key));
    /// Console.WriteLine("д��key({0}) value({1})", key, value);
    /// IniFileHelper.Write(path, section, key, value);
    /// IniFileHelper.Write(path, section1, key, value);
    /// Console.WriteLine("��ȡkey({0})={1}", key, IniFileHelper.Read(path, section, key));
    /// //Console.WriteLine("��ն���{0}", section);
    /// //IniFileHelper.Clear(path, section);
    /// Console.WriteLine("��ȡkey({0})={1}", key, IniFileHelper.Read(path, section, key));
    /// Console.ReadLine();
    /// 
    /// /* ���
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
        /// дIni�ļ�,����ļ�û���ҵ��������ᴴ����
        /// </summary>
        /// <param name="path">�ļ�·��</param>
        /// <param name="section">����</param>
        /// <param name="key">�ؼ���</param>
        /// <param name="value">ֵ</param>
        public static void Write(string path, string section, string key, string value)
        {
            if (path == null) throw new ArgumentNullException("path");
            NativeMethods.WritePrivateProfileString(section, key, value, path);
        }

        /// <summary>
        /// ��ȡIni�ļ�
        /// </summary>
        /// <param name="path">�ļ�·��</param>
        /// <param name="section">����</param>
        /// <param name="key">�ؼ���</param>
        /// <returns>ֵ</returns>
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
        /// ɾ��Ini�ļ���ָ�������µ����м�
        /// </summary>
        /// <param name="path">�ļ�·��</param>
        /// <param name="section">����</param>
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