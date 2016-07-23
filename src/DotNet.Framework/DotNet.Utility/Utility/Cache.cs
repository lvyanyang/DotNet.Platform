// ===============================================================================
// DotNet.Platform 开发框架 2016 版权所有
// ===============================================================================
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace DotNet.Utility
{
    /// <summary>
    /// 静态缓存管理(读写锁)
    /// </summary>
    /// <typeparam name="TKey">键类型</typeparam>
    /// <typeparam name="TValue">值类型</typeparam>
    public class Cache<TKey, TValue>
    {
        private readonly ReaderWriterLockSlim _lock = new ReaderWriterLockSlim();
        private readonly Dictionary<TKey, TValue> _map = new Dictionary<TKey, TValue>();

        /// <summary>
        /// 获取缓存数量
        /// </summary>
        public int Count
        {
            get { return _map.Count; }
        }

        /// <summary>
        /// 获取缓存对象使用指定的键
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <returns>返回缓存值</returns>
        public TValue Get(TKey key)
        {
            if (key == null) return default(TValue);
            _lock.EnterReadLock();
            TValue val;
            try
            {
                if (_map.TryGetValue(key, out val))
                    return val;
            }
            finally
            {
                _lock.ExitReadLock();
            }
            return default(TValue);
        }

        /// <summary>
        /// 获取缓存对象使用指定的键
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <param name="factory">找不到缓存时创建函数</param>
        /// <returns>返回缓存值</returns>
        public TValue Get(TKey key, Func<TValue> factory)
        {
            if (key == null) return default(TValue);
            _lock.EnterReadLock();
            TValue val;
            try
            {
                if (_map.TryGetValue(key, out val))
                    return val;
            }
            finally
            {
                _lock.ExitReadLock();
            }

            _lock.EnterWriteLock();
            try
            {
                if (_map.TryGetValue(key, out val))
                    return val;

                val = factory();
                _map.Add(key, val);
                return val;
            }
            finally
            {
                _lock.ExitWriteLock();
            }
        }

        /// <summary>
        /// 确定缓存中是否包含指定的键值
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <returns>存在返回True</returns>
        public bool Contains(TKey key)
        {
            _lock.EnterReadLock();
            try
            {
                return _map.ContainsKey(key);
            }
            finally
            {
                _lock.ExitReadLock();
            }
        }


        /// <summary>
        /// 设置缓存对象使用指定的键
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <param name="value">缓存值</param>
        /// <returns>返回缓存值</returns>
        public Cache<TKey, TValue> Set(TKey key, TValue value)
        {
            if (key == null) return this;
            _lock.EnterWriteLock();
            try
            {
                _map[key] = value;
            }
            finally
            {
                _lock.ExitWriteLock();
            }
            return this;
        }

        /// <summary>
        /// 批量设置缓存对象
        /// </summary>
        /// <param name="dic">需要设置的字典</param>
        public Cache<TKey, TValue> Set(Dictionary<TKey, TValue> dic)
        {
            if (dic == null || dic.Count == 0) return this;
            _lock.EnterWriteLock();
            try
            {
                foreach (var item in dic)
                {
                    if (item.Key == null) continue;
                    _map[item.Key] = item.Value;
                }
            }
            finally
            {
                _lock.ExitWriteLock();
            }
            return this;
        }

        /// <summary>
        /// 删除缓存
        /// </summary>
        /// <param name="key">缓存键</param>
        public Cache<TKey, TValue> Remove(TKey key)
        {
            if (key == null) return this;
            _lock.EnterWriteLock();
            try
            {
                _map.Remove(key);
            }
            finally
            {
                _lock.ExitWriteLock();
            }
            return this;
        }

        /// <summary>
        /// 删除缓存
        /// </summary>
        /// <param name="keys">缓存键集合</param>
        public Cache<TKey, TValue> Remove(TKey[] keys)
        {
            if (keys == null || keys.Length == 0) return this;
            _lock.EnterWriteLock();
            try
            {
                foreach (var key in keys)
                {
                    if (key == null) continue;
                    _map.Remove(key);
                }
            }
            finally
            {
                _lock.ExitWriteLock();
            }
            return this;
        }


        /// <summary>
        /// 清空缓存
        /// </summary>
        public Cache<TKey, TValue> Clear()
        {
            _lock.EnterWriteLock();
            try
            {
                _map.Clear();
            }
            finally
            {
                _lock.ExitWriteLock();
            }
            return this;
        }

        /// <summary>
        /// 键列表
        /// </summary>
        public List<TKey> KeyList()
        {
            _lock.EnterReadLock();
            try
            {
                return _map.Keys.ToList();
            }
            finally
            {
                _lock.ExitReadLock();
            }
        }

        /// <summary>
        /// 值列表
        /// </summary>
        public List<TValue> ValueList()
        {
            _lock.EnterReadLock();
            try
            {
                return _map.Values.ToList();
            }
            finally
            {
                _lock.ExitReadLock();
            }
        }
    }
}
