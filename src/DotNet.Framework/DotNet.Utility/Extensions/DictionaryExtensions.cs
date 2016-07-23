// ===============================================================================
// DotNet.Platform 开发框架 2016 版权所有
// ===============================================================================
using System;
using System.Collections.Generic;

namespace DotNet.Extensions
{
    /// <summary>
    /// Dictionary扩展操作
    /// </summary>
    public static class DictionaryExtensions
    {
        /// <summary>
        /// 连续添加键值
        /// </summary>
        /// <typeparam name="TKey">键类型</typeparam>
        /// <typeparam name="TValue">值类型</typeparam>
        /// <param name="dic">字典对象</param>
        /// <param name="key">键对象</param>
        /// <param name="value">值对象</param>
        /// <returns>返回字典本身</returns>
        public static IDictionary<TKey, TValue> Append<TKey, TValue>(this IDictionary<TKey, TValue> dic, TKey key, TValue value)
        {
            dic.Add(key, value);
            return dic;
        }

        /// <summary>
        /// 获取指定键对应值,如果不存在指定键,返回指定的默认值.
        /// </summary>
        /// <param name="dic">字典对象</param>
        /// <param name="key">键对象</param>
        public static string GetString<TKey, TValue>(this IDictionary<TKey, TValue> dic, TKey key)
        {
            TValue result;
            if (dic.TryGetValue(key, out result))
            {
                return result.ToStringOrEmpty();
            }
            return string.Empty;
        }

        /// <summary>
        /// 获取指定键对应值,如果不存在指定键,返回指定的默认值.
        /// </summary>
        /// <param name="dic">字典对象</param>
        /// <param name="key">键对象</param>
        /// <param name="defaultValue">默认值</param>
        public static TValue Get<TKey, TValue>(this IDictionary<TKey, TValue> dic, TKey key, TValue defaultValue = default(TValue))
        {
            TValue result;
            return dic.TryGetValue(key, out result) ? result : defaultValue;
        }

        /// <summary>
        /// 设置键值
        /// </summary>
        /// <param name="dic">字典对象</param>
        /// <param name="key">键对象</param>
        /// <param name="value">值对象</param>
        public static TValue Set<TKey, TValue>(this IDictionary<TKey, TValue> dic, TKey key, TValue value)
        {
            return dic[key] = value;
        }

        /// <summary>
        /// 获取指定键对应值,如果不存在指定键,添加指定的键和值.
        /// </summary>
        /// <param name="dic">字典对象</param>
        /// <param name="key">键对象</param>
        /// <param name="value">值对象</param>
        public static TValue GetAdd<TKey, TValue>(this IDictionary<TKey, TValue> dic, TKey key, TValue value)
        {
            if (!dic.ContainsKey(key))
            {
                dic.Add(key, value);
                return value;
            }
            return dic[key];
        }
    }
}
