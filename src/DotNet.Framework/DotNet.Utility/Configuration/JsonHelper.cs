// ===============================================================================
// DotNet.Platform 开发框架 2016 版权所有
// ===============================================================================
using System;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace DotNet.Configuration
{
    /// <summary>
    /// Json序列化操作类
    /// </summary>
    public static class JsonHelper
    {
        /// <summary>
        /// 反序列化Json文件
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="path">文件路径</param>
        /// <param name="defaultValue">无数据时的默认值</param>
        public static T Deserialize<T>(string path, T defaultValue)
        {
            T data = default(T);
            if (File.Exists(path))
            {
                var json = File.ReadAllText(path, System.Text.Encoding.UTF8);
                data = JsonConvert.DeserializeObject<T>(json);
            }
            if (data != null)
            {
                return data;
            }
            return defaultValue;
        }

        /// <summary>
        /// 反序列化Json字符串
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="json">json字符串</param>
        public static T Deserialize<T>(string json)
        {
            if (string.IsNullOrEmpty(json)) return default(T);
            return JsonConvert.DeserializeObject<T>(json);
        }

        /// <summary>
        /// 把数据对象Json序列化到文件
        /// </summary>
        /// <param name="path">文件路径</param>
        /// <param name="data">数据对象</param>
        public static void Serialize(string path, object data)
        {
            var json = JsonConvert.SerializeObject(data, Formatting.Indented, GetSetting());

            FileInfo file = new FileInfo(path);
            DirectoryInfo dir = file.Directory;
            if (dir != null)
            {
                string dirFullName = dir.FullName;
                if (!Directory.Exists(dirFullName))
                {
                    Directory.CreateDirectory(dirFullName);
                }
            }
            File.WriteAllText(path, json, System.Text.Encoding.UTF8);
        }

        /// <summary>
        /// 把数据对象Json序列化为字符串
        /// </summary>
        /// <param name="data">数据对象</param>
        /// <param name="formatting">格式化方式</param>
        public static string Serialize(object data, Formatting formatting = Formatting.None)
        {
            if (data == null) return string.Empty;
            return JsonConvert.SerializeObject(data, formatting, GetSetting());
        }

        private static JsonSerializerSettings GetSetting()
        {
            var settings = new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Error,
                NullValueHandling = NullValueHandling.Ignore,
                DateFormatString = "yyyy-MM-dd HH:mm:ss"
            };
            settings.Converters.Add(new StringEnumConverter { CamelCaseText = true });
            return settings;
        }
    }
}
