// ===============================================================================
// DotNet.Platform 开发框架 2016 版权所有
// ===============================================================================
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using DotNet.Collections;
using DotNet.Helper;
using DotNet.Utility;

namespace DotNet.Extensions
{
    /// <summary>
    /// Enumerable扩展操作
    /// </summary>
    public static class EnumerableExtensions
    {
        /// <summary>
        /// 复制实体列表(创建新列表,并且新创建每个元素)
        /// </summary>
        /// <param name="list">列表对象</param>
        /// <param name="filterPredicate">过滤的操作,如果为空则复制全部</param>
        /// <exception cref="System.ArgumentNullException">参数action 为null</exception>
        public static List<TSource> Copy<TSource>(this IEnumerable<TSource> list, Func<TSource, bool> filterPredicate) where TSource : class, new()
        {
            List<TSource> newList = new List<TSource>();
            foreach (TSource item in list)
            {
                if (filterPredicate != null && !filterPredicate(item))
                {
                    continue;
                }
                var newItem = new TSource();
                ObjectHelper.CopyProperty(item, newItem);
                newList.Add(newItem);
            }
            return newList;
        }

        /// <summary>
        /// 对列表的每个元素执行指定操作
        /// </summary>
        /// <param name="list">列表对象</param>
        /// <param name="action">执行的操作</param>
        /// <exception cref="System.ArgumentNullException">参数action 为null</exception>
        public static void ForEach<TSource>(this IEnumerable<TSource> list, Action<TSource> action)
        {
            if (action == null)
            {
                throw new System.ArgumentNullException("action", "参数action不能为null");
            }
            foreach (TSource item in list)
            {
                action(item);
            }
        }

        #region 私有方法

        /// <summary>
        /// 检查查找条件，为null抛出异常。
        /// </summary>
        /// <param name="predicate">查找条件。</param>
        private static void CheckPredicate<TSource>(Func<TSource, bool> predicate)
        {
            if (predicate == null)
            {
                throw new System.ArgumentNullException("predicate", "参数predicate不能为null。");
            }
        }

        #endregion

        #region 公共方法

        /// <summary>
        /// 确定某元素是否在列表中。
        /// </summary>
        /// <param name="list">列表对象</param>
        /// <param name="predicate">查找条件。</param>
        /// <exception cref="System.ArgumentNullException">predicate 为null。</exception>
        /// <returns>如果在列表中找到项，则为true，否则为false。</returns>
        public static bool Contains<TSource>(this IEnumerable<TSource> list, Func<TSource, bool> predicate)
        {
            CheckPredicate(predicate);

            foreach (var item in list)
            {
                if (predicate(item))
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 用指定条件搜索整个列表，并返回整个列表中第一个匹配项的从零开始的索引。
        /// </summary>
        /// <param name="list">列表对象</param>
        /// <param name="predicate">查找条件。</param>
        /// <exception cref="System.ArgumentNullException">predicate 为null。</exception>
        /// <returns>如果在整个列表中找到元素的第一个匹配项，则为该项的从零开始的索引；否则为-1。</returns>
        public static int IndexOf<TSource>(this IEnumerable<TSource> list, Func<TSource, bool> predicate)
        {
            CheckPredicate(predicate);

            int i = 0;
            foreach (TSource item in list)
            {
                if (predicate(item))
                {
                    return i;
                }
                i++;
            }
            return -1;
        }

        /// <summary>
        /// 从列表中删除指定查询条件的匹配项。
        /// </summary>
        /// <param name="list">列表对象</param>
        /// <param name="predicate">查找条件。</param>
        /// <exception cref="System.ArgumentNullException">predicate 为null。</exception>
        /// <returns>返回移除的数量。</returns>
        public static int Delete<TSource>(this IList<TSource> list, Func<TSource, bool> predicate)
        {
            CheckPredicate(predicate);

            int removeCount = 0;
            var newlist = new List<TSource>();
            foreach (var item in list)
            {
                if (predicate(item))
                {
                    newlist.Add(item);
                }
            }

            foreach (TSource item in newlist)
            {
                list.Remove(item);
                removeCount++;
            }
            return removeCount;
        }

        /// <summary>
        /// 更新列表项
        /// </summary>
        /// <typeparam name="TSource">实体类型</typeparam>
        /// <param name="list">列表对象</param>
        /// <param name="predicate">查找条件</param>
        /// <param name="newEntity">新对象</param>
        /// <returns>更新成功返回true</returns>
        public static bool Update<TSource>(this IList<TSource> list, TSource newEntity, Func<TSource, bool> predicate)
        {
            var index = IndexOf(list, predicate);
            if (index > -1)
            {
                list[index] = newEntity;
                return true;
            }
            return false;
        }

        /// <summary>
        /// 获取列表指定字段最大值
        /// </summary>
        /// <typeparam name="TSource">实体类型</typeparam>
        /// <param name="source">列表对象</param>
        /// <param name="selector">指定字段</param>
        /// <param name="defaultValue">如果列表没有元素时指定默认值</param>
        public static int Max<TSource>(this List<TSource> source, Func<TSource, int> selector, int defaultValue)
        {
            if (!source.Any())
            {
                return defaultValue;
            }
            return source.Max(selector);
        }

        /// <summary>
        /// 列表单个字段排序
        /// </summary>
        /// <typeparam name="TSource">实体类型</typeparam>
        /// <param name="list">列表对象</param>
        /// <param name="name">排序字段名称</param>
        /// <param name="dir">排序方式</param>
        public static List<TSource> OrderBy<TSource>(this List<TSource> list, string name, string dir = "desc")
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new System.ArgumentNullException(nameof(name), "请指定排序字段名称。");
            }
            if (string.IsNullOrEmpty(dir))
            {
                throw new System.ArgumentNullException(nameof(dir), "请指定排序方式。");
            }

            var isAsc = dir.ToLower().Equals("asc");
            Type t = typeof(TSource);
            PropertyInfo pro = t.GetProperty(name);
            var comparer = new CaseInsensitiveComparer();
            list.Sort((x, y) => isAsc ?
                comparer.Compare(pro.GetValue(x, null), pro.GetValue(y, null)) :
                comparer.Compare(pro.GetValue(y, null), pro.GetValue(x, null)));
            return list;
        }

        /// <summary>
        /// 列表单个字段排序
        /// </summary>
        /// <typeparam name="TSource">实体类型</typeparam>
        /// <param name="list">列表对象</param>
        /// <param name="name">排序字段名称</param>
        /// <param name="isDesc">是否倒序</param>
        public static List<TSource> OrderBy<TSource>(this List<TSource> list, string name, bool isDesc = true)
        {
            return OrderBy(list, name, isDesc ? "desc" : "asc");
        }

        /// <summary>
        /// 列表单个字段排序(正序)
        /// </summary>
        /// <typeparam name="TSource">实体类型</typeparam>
        /// <param name="list">列表对象</param>
        /// <param name="name">排序字段名称</param>
        public static List<TSource> OrderByAsc<TSource>(this List<TSource> list, string name)
        {
            return OrderBy(list, name, "asc");
        }

        /// <summary>
        /// 列表单个字段排序(倒序)
        /// </summary>
        /// <typeparam name="TSource">实体类型</typeparam>
        /// <param name="list">列表对象</param>
        /// <param name="name">排序字段名称</param>
        public static List<TSource> OrderByDesc<TSource>(this List<TSource> list, string name)
        {
            return OrderBy(list, name, "desc");
        }

        /// <summary>
        /// 列表单个字段排序
        /// </summary>
        /// <typeparam name="TSource">实体类型</typeparam>
        /// <param name="list">列表对象</param>
        /// <param name="name">排序字段名称</param>
        /// <param name="isDesc">是否倒序</param>
        public static List<TSource> OrderBy<TSource>(this List<TSource> list, Expression<Func<TSource, object>> name, bool isDesc = true)
        {
            return OrderBy(list, ExpressionHelper.GetPropertyName(name), isDesc ? "desc" : "asc");
        }

        /// <summary>
        /// 列表单个字段排序(正序)
        /// </summary>
        /// <typeparam name="TSource">实体类型</typeparam>
        /// <param name="list">列表对象</param>
        /// <param name="name">排序字段名称</param>
        public static List<TSource> OrderByAsc<TSource>(this List<TSource> list, Expression<Func<TSource, object>> name)
        {
            return OrderBy(list, ExpressionHelper.GetPropertyName(name), "asc");
        }

        /// <summary>
        /// 列表单个字段排序(倒序)
        /// </summary>
        /// <typeparam name="TSource">实体类型</typeparam>
        /// <param name="list">列表对象</param>
        /// <param name="name">排序字段名称</param>
        public static List<TSource> OrderByDesc<TSource>(this List<TSource> list, Expression<Func<TSource, object>> name)
        {
            return OrderBy(list, ExpressionHelper.GetPropertyName(name), "desc");
        }

        /// <summary>
        /// 列表多字段排序
        /// </summary>
        /// <typeparam name="TSource">实体类型</typeparam>
        /// <param name="list">列表对象</param>
        /// <param name="orders">排序条件</param>
        public static List<TSource> OrderBy<TSource>(this List<TSource> list,
            params KeyValuePair<string, string>[] orders)
        {
            if (orders.Length == 0)
            {
                throw new System.ArgumentNullException(nameof(orders), "请指定排序条件。");
            }
            Type t = typeof(TSource);
            var comparer = new CaseInsensitiveComparer();
            Func<TSource, TSource, PropertyInfo, string, int> func = (x, y, p, d) =>
            {
                return d.ToLower().Equals("asc")
                    ? comparer.Compare(p.GetValue(x, null), p.GetValue(y, null))
                    : comparer.Compare(p.GetValue(y, null), p.GetValue(x, null));
            };
            int compare = 0;
            list.Sort((x, y) =>
            {
                for (var i = 0; i < orders.Length; ++i)
                {
                    var order = orders[i];
                    var pro = t.GetProperty(order.Key);
                    compare = func(x, y, pro, order.Value);
                    if (compare != 0) return compare;
                }

                return compare;
            });
            return list;
        }

        /// <summary>
        /// 返回分页列表
        /// </summary>
        /// <typeparam name="TSource">实体类型</typeparam>
        /// <param name="list">列表对象</param>
        /// <param name="pageCondition">分页条件</param>
        /// <param name="predicate">查找条件</param>
        public static PageList<TSource> Page<TSource>(this List<TSource> list, PaginationCondition pageCondition,
            Func<TSource, bool> predicate)
        {
            var filterData = predicate == null ? list.ToList() : list.Where(predicate).ToList();
            if (!string.IsNullOrEmpty(pageCondition.OrderName))
            {
                filterData.OrderBy(pageCondition.OrderName, pageCondition.OrderDir);
            }
            var pageIndex = pageCondition.PageIndex;
            var pageSize = pageCondition.PageSize;
            var pageData = filterData.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
            return new PageList<TSource>(pageIndex, pageSize, filterData.Count, pageData);
        }

        /// <summary>
        /// 转为DataTable
        /// </summary>
        /// <typeparam name="TSource">实体类型</typeparam>
        /// <param name="list">实体列表</param>
        public static DataTable ToDataTable<TSource>(this List<TSource> list) where TSource : class, new()
        {
            return DataTableHelper.ConvertToDataTable(list);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="strList"></param>
        /// <param name="split"></param>
        /// <returns></returns>
        public static string ToSplitString(this IList<string> strList, string split = ",")
        {
            StringBuilder sb = new StringBuilder(strList.Count * 10);
            for (int i = 0; i < strList.Count; i++)
            {
                sb.Append(strList[i]);
                if (i < strList.Count - 1)
                {
                    sb.Append(split);
                }
            }
            return sb.ToString();
        }

        #endregion
    }
}