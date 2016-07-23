// ===============================================================================
// DotNet.Platform 开发框架 2016 版权所有
// ===============================================================================
using System;
using System.Collections.Generic;
using System.Linq;
using DotNet.Collections;
using DotNet.Extensions;

namespace DotNet.Utility
{
    /// <summary>
    /// 安全的列表操作
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class SafeList<T>
    {
        private readonly List<T> _list = new List<T>();
        private readonly RWLock _rwlock = new RWLock();

        public SafeList()
        {

        }

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="data"></param>
        public void Init(IEnumerable<T> data)
        {
            using (_rwlock.Write())
            {
                _list.Clear();
                _list.AddRange(data);
            }
        }

        /// <summary>
        /// 获取记录总数
        /// </summary>
        public int Count()
        {
            using (_rwlock.Read())
            {
                return _list.Count;
            }
        }

        public int Max(Func<T, int> selector)
        {
            using (_rwlock.Read())
            {
                if (_list.Count == 0)
                {
                    return 1;
                }
                return _list.Max(selector) + 1;
            }
        }

        /// <summary>
        /// 是否存在指定条件的列表项
        /// </summary>
        /// <param name="predicate">查找条件</param>
        /// <returns>存在返回true</returns>
        public bool Contains(Func<T, bool> predicate)
        {
            using (_rwlock.Read())
            {
                return _list.Contains(predicate);
            }
        }

        /// <summary>
        /// 添加列表项
        /// </summary>
        /// <param name="entity">实体对象</param>
        public BoolMessage Add(T entity)
        {
            CheckEntity(entity);
            using (_rwlock.Write())
            {
                _list.Add(entity);
                return BoolMessage.True;
            }
        }

        /// <summary>
        /// 更新列表项
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <param name="predicate">查找条件</param>
        public BoolMessage Update(T entity, Func<T, bool> predicate)
        {
            CheckEntity(entity);
            using (_rwlock.Write())
            {
                var success = _list.Update(entity, predicate);
                return success ? BoolMessage.True : new BoolMessage(false, "没有找到指定条件的实体对象");
            }
        }

        /// <summary>
        /// 删除列表项
        /// </summary>
        /// <param name="predicate">查找条件</param>
        public BoolMessage Delete(Func<T, bool> predicate)
        {
            using (_rwlock.Write())
            {
                var success = _list.Delete(predicate) > 0;
                return success ? BoolMessage.True : new BoolMessage(false, "没有找到指定条件的实体对象");
            }
        }

        /// <summary>
        /// 获取单个实体对象
        /// </summary>
        /// <param name="predicate">查找条件</param>
        /// <returns>返回第一个符合条件的项,没有找到返回空</returns>
        public T Get(Func<T, bool> predicate)
        {
            using (_rwlock.Read())
            {
                return _list.FirstOrDefault(predicate);
            }
        }

        /// <summary>
        /// 获取列表
        /// </summary>
        public List<T> GetList()
        {
            using (_rwlock.Read())
            {
                return _list.ToList();
            }
        }

        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="predicate">查找条件</param>
        public List<T> GetList(Func<T, bool> predicate)
        {
            using (_rwlock.Read())
            {
                return predicate == null ? _list.ToList() : _list.Where(predicate).ToList();
            }
        }

        /// <summary>
        /// 获取原始列表对象
        /// </summary>
        public List<T> GetOriginalList()
        {
            return _list;
        }

        /// <summary>
        /// 获取分页列表
        /// </summary>
        /// <param name="pageCondition">分页条件</param>
        /// <param name="predicate">查找条件</param>
        public PageList<T> GetPageList(PaginationCondition pageCondition, Func<T, bool> predicate)
        {
            using (_rwlock.Read())
            {
                return _list.Page(pageCondition, predicate);
            }
        }

        /// <summary>
        /// 读锁操作
        /// </summary>
        /// <param name="action">操作</param>
        public void ReadAction(Action action)
        {
            using (_rwlock.Read())
            {
                action?.Invoke();
            }
        }

        /// <summary>
        /// 写锁操作
        /// </summary>
        /// <param name="action">操作</param>
        public void WriteAction(Action action)
        {
            using (_rwlock.Write())
            {
                action?.Invoke();
            }
        }

        private void CheckEntity(T entity)
        {
            if (entity == null)
            {
                throw new System.ArgumentNullException(nameof(entity), "实体对象不能为空。");
            }
        }
    }
}