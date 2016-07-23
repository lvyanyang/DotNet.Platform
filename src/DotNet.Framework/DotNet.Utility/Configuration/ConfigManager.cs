// ===============================================================================
// DotNet.Platform 开发框架 2016 版权所有
// ===============================================================================
using System;
using System.Collections.Generic;
using System.Configuration;
using Newtonsoft.Json.Linq;

namespace DotNet.Configuration
{
    /// <summary>
    /// 系统配置管理
    /// </summary>
    public static class ConfigManager
    {
        private const string appSettingsSectionName = "AppSettings";
        private static readonly object lockObject = new object();
        private static JsonConfigFile<Dictionary<string, object>> configFile;

        /// <summary>
        /// 读取配置文件数据
        /// </summary>
        static ConfigManager()
        {
            lock (lockObject)
            {
                string directoryName = SystemDirectory.RootDirectory;
                var v = ConfigurationManager.AppSettings["ConfigFileName"];
                string fileName = string.IsNullOrEmpty(v) ? "app.json" : v;
                configFile = new JsonConfigFile<Dictionary<string, object>>(directoryName, fileName);
                if (configFile.Data == null)
                {
                    configFile.Load();
                }
            }
        }

        /// <summary>
        /// 获取应用程序配置值
        /// </summary>
        /// <param name="key">配置键名(区分大小写)</param>
        /// <param name="defaultValueFn">找不到指定键名时,返回的默认值</param>
        /// <returns>返回指定键名对应的值</returns>
        public static string GetAppValue(string key, Func<string> defaultValueFn = null)
        {
            lock (lockObject)
            {
                var dicData = GetSettingData(appSettingsSectionName, () => new Dictionary<string, string>());
                string value;
                if ( dicData.TryGetValue(key, out value))
                {
                    return value;
                }
                value = defaultValueFn != null ? defaultValueFn() : null;
                dicData[key] = value;
                return value;
            }
        }

        /// <summary>
        /// 设置应用程序配置值
        /// </summary>
        /// <param name="key">配置键名(区分大小写)</param>
        /// <param name="value">键值</param>
        public static void SetAppValue(string key, string value)
        {
            lock (lockObject)
            {
                var dicData = GetSettingData(appSettingsSectionName, () => new Dictionary<string, string>());
                dicData[key] = value;
            }
        }

        /// <summary>
        /// 加载配置数据
        /// </summary>
        /// <typeparam name="T">配置类型</typeparam>
        /// <param name="name">配置名称</param>
        /// <param name="defaultValueFn">无配置时的默认值</param>
        public static T GetSetting<T>(string name, Func<T> defaultValueFn = null) where T : class
        {
            lock (lockObject)
            {
                return GetSettingData(name, defaultValueFn);
            }
        }

        /// <summary>
        /// 设置配置数据
        /// </summary>
        /// <typeparam name="T">配置类型</typeparam>
        /// <param name="name">配置名称</param>
        /// <param name="value">配置值</param>
        public static void SetSetting<T>(string name, T value) where T : class
        {
            lock (lockObject)
            {
                configFile.Data[name] = value;
            }
        }

        private static T GetSettingData<T>(string name, Func<T> defaultValueFn = null) where T : class
        {
            lock (lockObject)
            {
                object value;
                if (configFile.Data.TryGetValue(name, out value))
                {
                    var value1 = value as T;
                    if (value1 != null)
                    {
                        return value1;
                    }
                    var value2 = value as JContainer;
                    if (value2 != null)
                    {
                        var value3 = value2.ToObject<T>();
                        configFile.Data[name] = value3;
                        return value3;
                    }
                }
                var defautlValue = defaultValueFn != null ? defaultValueFn() : null;
                if (defautlValue != null)
                {
                    configFile.Data[name] = defautlValue;
                }
                return defautlValue;
            }
        }

        /// <summary>
        /// 从文件中重新加载配置数据
        /// </summary>
        public static void Load()
        {
            lock (lockObject)
            {
                configFile.Load();
            }
        }

        /// <summary>
        /// 从文件中重新加载配置数据
        /// </summary>
        /// <param name="filePath">文件路径</param>
        public static void Load(string filePath)
        {
            lock (lockObject)
            {
                System.IO.FileInfo info = new System.IO.FileInfo(filePath);
                string directoryName = info.DirectoryName;
                string fileName = info.Name;
                configFile = new JsonConfigFile<Dictionary<string, object>>(directoryName, fileName);
                configFile.Load();
            }
        }

        /// <summary>
        /// 从文件中重新加载配置数据
        /// </summary>
        /// <param name="directoryName">目录路径</param>
        /// <param name="fileName">文件名称</param>
        public static void Load(string directoryName, string fileName)
        {
            lock (lockObject)
            {
                configFile = new JsonConfigFile<Dictionary<string, object>>(directoryName, fileName);
                configFile.Load();
            }
        }

        /// <summary>
        /// 保存配置数据到文件中
        /// </summary>
        public static void Save()
        {
            lock (lockObject)
            {
                configFile.Save();
            }
        }
    }
}