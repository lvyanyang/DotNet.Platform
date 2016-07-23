// ===============================================================================
// DotNet.Platform 开发框架 2016 版权所有
// ===============================================================================
using Microsoft.Win32;

namespace DotNet.Helper
{
    /// <summary>
    /// 注册表操作类
    /// </summary>
    /// <example>
    /// <code>
    /// using XCI.Core;
    /// using XCI.Helper;
    /// 
    /// RegeditHelper.CreateSubKey("Software\\XCISoft", RegDomain.LocalMachine); //创建新子项。字符串 subkey 不区分大小写。
    /// RegeditHelper.CreateSubKey("Software\\XCISoft\\Config", RegDomain.LocalMachine); //创建新子项。字符串 subkey 不区分大小写。
    /// RegeditHelper.DeleteSubKey("Software\\XCISoft\\Config", RegDomain.LocalMachine); //删除指定的子项。字符串 subkey 不区分大小写。如果有下级请使用DeleteSubKeyTree  
    /// RegeditHelper.DeleteSubKeyTree("Software\\XCISoft", RegDomain.LocalMachine); //递归删除子项和任何子级子项。字符串 subkey 不区分大小写。  
    /// 
    /// RegeditHelper.SetValue("Home", 997, "Software\\XCISoft", RegDomain.LocalMachine, RegValueKind.String); //使用指定的注册表数据类型设置注册表项中的名称/值对的值。
    /// Console.WriteLine(RegeditHelper.GetValue("Home", "Software\\XCISoft", RegDomain.LocalMachine)); //检索与指定名称关联的值。如果注册表中不存在名称/值对，则返回 null。
    /// RegeditHelper.DeleteValue("Home", "Software\\XCISoft", RegDomain.LocalMachine);//从此项中删除指定值。
    /// </code>
    /// </example>
    public static class RegistryHelper
    {
        /// <summary>
        /// 创建新子项。字符串 subkey 不区分大小写。
        /// </summary>
        /// <param name="subkey">子项的名称或路径</param>
        /// <param name="regDomain">注册表基项</param>
        /// <exception cref="System.ArgumentNullException">subkey 为 null。</exception>
        /// <remarks>
        /// 例如:
        /// subkey="Software\XCISoft",regDomain="RegDomain.LocalMachine"
        /// 将创建HKEY_LOCAL_MACHINE\Software\XCISoft注册表项
        /// </remarks>
        public static void CreateSubKey(string subkey, RegDomain regDomain)
        {
            if (string.IsNullOrEmpty(subkey)) throw new System.ArgumentNullException("subkey");

            var rootReg = GetRegDomain(regDomain);
            var reg = rootReg.CreateSubKey(subkey);
            if (reg != null)
            {
                reg.Close();
            }
            rootReg.Close();
        }

        /// <summary>
        /// 删除指定的子项。字符串 subkey 不区分大小写。如果有下级请使用DeleteSubKeyTree
        /// </summary>
        /// <param name="subkey">子项的名称</param>
        /// <param name="regDomain">注册表基项</param>
        /// <exception cref="System.ArgumentNullException">subkey 为 null。</exception>
        public static void DeleteSubKey(string subkey, RegDomain regDomain)
        {
            if (string.IsNullOrEmpty(subkey)) throw new System.ArgumentNullException("subkey");

            var rootReg = GetRegDomain(regDomain);
            rootReg.DeleteSubKey(subkey, false);
            rootReg.Close();
        }

        /// <summary>
        /// 递归删除子项和任何子级子项。字符串 subkey 不区分大小写。
        /// </summary>
        /// <param name="subkey">子项的名称或路径</param>
        /// <param name="regDomain">注册表基项</param>
        /// <exception cref="System.ArgumentNullException">subkey 为 null。</exception>
        public static void DeleteSubKeyTree(string subkey, RegDomain regDomain)
        {
            if (string.IsNullOrEmpty(subkey)) throw new System.ArgumentNullException("subkey");

            var rootReg = GetRegDomain(regDomain);
            rootReg.DeleteSubKeyTree(subkey);
            rootReg.Close();
        }

        /// <summary>
        /// 检索与指定名称关联的值。如果注册表中不存在名称/值对，则返回 null。
        /// </summary>
        /// <param name="name">要检索的值的名称。</param>
        /// <param name="subkey">子项的名称或路径。</param>
        /// <param name="regDomain">注册表基项</param>
        /// <returns>与 name 关联的值；如果未找到 name，则为 null。</returns>
        public static object GetValue(string name, string subkey, RegDomain regDomain)
        {
            object value = null;
            var sub = GetSubKey(subkey, false, regDomain);
            if (sub != null)
            {
                value = sub.GetValue(name);
                sub.Close();
            }
            return value;
        }

        /// <summary>
        /// 使用指定的注册表数据类型设置注册表项中的名称/值对的值。
        /// </summary>
        /// <param name="name">要存储的值的名称。</param>
        /// <param name="value">要存储的数据。</param>
        /// <param name="subkey">子项的名称或路径。</param>
        /// <param name="regDomain">注册表基项</param>
        /// <param name="regValueKind">在存储数据时使用的注册表数据类型。</param>
        public static void SetValue(string name, object value, string subkey, RegDomain regDomain, RegValueKind regValueKind)
        {
            var sub = GetSubKey(subkey, true, regDomain);
            if (sub != null)
            {
                sub.SetValue(name, value, GetRegValueKind(regValueKind));
                sub.Close();
            }
        }

        /// <summary>
        /// 从此项中删除指定值。
        /// </summary>
        /// <param name="name">要删除的值的名称。</param>
        /// <param name="subkey">子项的名称或路径。</param>
        /// <param name="regDomain">注册表基项</param>
        public static void DeleteValue(string name, string subkey, RegDomain regDomain)
        {
            var sub = GetSubKey(subkey, true, regDomain);
            if (sub != null)
            {
                sub.DeleteValue(name,false);
                sub.Close();
            }
        }

        /// <summary>
        /// 检索指定的子项。
        /// </summary>
        /// <param name="subkey">要打开的子项的名称或路径。</param>
        /// <param name="writable">如果需要项的写访问权限，则设置为 true。</param>
        /// <param name="regDomain">注册表基项</param>
        /// <returns>请求的子项；如果操作失败，则为 null。</returns>
        private static RegistryKey GetSubKey(string subkey, bool writable, RegDomain regDomain)
        {
            var rootReg = GetRegDomain(regDomain);
            return rootReg.OpenSubKey(subkey, writable);
        }

        /// <summary>
        /// 获取注册表基项域对应顶级节点
        /// </summary>
        /// <param name="regDomain">注册表基项域</param>
        /// <returns>注册表基项域对应顶级节点</returns>
        static RegistryKey GetRegDomain(RegDomain regDomain)
        {
            switch (regDomain)
            {
                case RegDomain.ClassesRoot:
                    return Registry.ClassesRoot;
                case RegDomain.CurrentUser:
                    return Registry.CurrentUser;
                case RegDomain.LocalMachine:
                    return Registry.LocalMachine;
                case RegDomain.User:
                    return Registry.Users;
                case RegDomain.CurrentConfig:
                    return Registry.CurrentConfig;
                case RegDomain.DynDa:
                    return Registry.PerformanceData;
                case RegDomain.PerformanceData:
                    return Registry.PerformanceData;
                default:
                    return Registry.LocalMachine;
            }
        }

        /// <summary>
        /// 获取在注册表中对应的值数据类型
        /// </summary>
        /// <param name="regValueKind">注册表数据类型</param>
        /// <returns>注册表中对应的数据类型</returns>
        static RegistryValueKind GetRegValueKind(RegValueKind regValueKind)
        {
            switch (regValueKind)
            {
                case RegValueKind.Unknown:
                    return RegistryValueKind.Unknown;
                case RegValueKind.String:
                    return RegistryValueKind.String;
                case RegValueKind.ExpandString:
                    return RegistryValueKind.ExpandString;
                case RegValueKind.Binary:
                    return RegistryValueKind.Binary;
                case RegValueKind.DWord:
                    return RegistryValueKind.DWord;
                case RegValueKind.MultiString:
                    return RegistryValueKind.MultiString;
                case RegValueKind.QWord:
                    return RegistryValueKind.QWord;
                default:
                    return RegistryValueKind.String;
            }
        }
    }

    /// <summary>
    /// 注册表基项静态域
    /// </summary>
    public enum RegDomain
    {
        /// <summary>
        /// 对应于HKEY_CLASSES_ROOT主键
        /// </summary>
        ClassesRoot = 0,
        /// <summary>
        /// 对应于HKEY_CURRENT_USER主键
        /// </summary>
        CurrentUser = 1,
        /// <summary>
        /// 对应于HKEY_LOCAL_MACHINE主键
        /// </summary>
        LocalMachine = 2,
        /// <summary>
        /// 对应于HKEY_USER主键
        /// </summary>
        User = 3,
        /// <summary>
        /// 对应于HEKY_CURRENT_CONFIG主键
        /// </summary>
        CurrentConfig = 4,
        /// <summary>
        /// 对应于HKEY_DYN_DATA主键
        /// </summary>
        DynDa = 5,
        /// <summary>
        /// 对应于HKEY_PERFORMANCE_DATA主键
        /// </summary>
        PerformanceData = 6,
    }

    /// <summary>
    /// 指定在注册表中存储值时所用的数据类型，或标识注册表中某个值的数据类型
    /// </summary>
    public enum RegValueKind
    {
        /// <summary>
        /// 指示一个不受支持的注册表数据类型。例如，不支持 Microsoft Win32 API 注册表数据类型 REG_RESOURCE_LIST。使用此值指定
        /// </summary>
        Unknown = 0,
        /// <summary>
        /// 指定一个以 Null 结尾的字符串。此值与 Win32 API 注册表数据类型 REG_SZ 等效。
        /// </summary>
        String = 1,
        /// <summary>
        /// 指定一个以 NULL 结尾的字符串，该字符串中包含对环境变量（如 %PATH%，当值被检索时，就会展开）的未展开的引用。
        /// 此值与 Win32 API注册表数据类型 REG_EXPAND_SZ 等效。
        /// </summary>
        ExpandString = 2,
        /// <summary>
        /// 指定任意格式的二进制数据。此值与 Win32 API 注册表数据类型 REG_BINARY 等效。
        /// </summary>
        Binary = 3,
        /// <summary>
        /// 指定一个 32 位二进制数。此值与 Win32 API 注册表数据类型 REG_DWORD 等效。
        /// </summary>
        DWord = 4,
        /// <summary>
        /// 指定一个以 NULL 结尾的字符串数组，以两个空字符结束。此值与 Win32 API 注册表数据类型 REG_MULTI_SZ 等效。
        /// </summary>
        MultiString = 5,
        /// <summary>
        /// 指定一个 64 位二进制数。此值与 Win32 API 注册表数据类型 REG_QWORD 等效。
        /// </summary>
        QWord = 6,
    }
}