// ===============================================================================
// DotNet.Platform 开发框架 2016 版权所有
// ===============================================================================
using System;
using System.ComponentModel;
using System.Windows.Forms;
using DotNet.Extensions;

namespace DotNet.Helper
{
    /// <summary>
    /// 绑定数据源操作类
    /// </summary>
    public static class CurrencyManagerHelper
    {
        /// <summary>
        /// 获取数据项
        /// </summary>
        /// <param name="dataManager">数据管理对象</param>
        /// <param name="index">索引</param>
        /// <returns>返回指定位置的数据项</returns>
        public static object GetItem(CurrencyManager dataManager, int index)
        {
            if (index > -1 && dataManager != null && index < dataManager.List.Count)
            {
                return dataManager.List[index];
            }
            return null;
        }

        /// <summary>
        /// 获取数据项
        /// </summary>
        /// <param name="dataManager">数据管理对象</param>
        /// <param name="propertyName">属性名称</param>
        /// <param name="propertyValue">属性值</param>
        /// <returns>返回指定属性名称的项的索引</returns>
        public static object GetItem(CurrencyManager dataManager, string propertyName, object propertyValue)
        {
            if (string.IsNullOrEmpty(propertyName) || propertyValue == null) return null;
            var index = GetIndex(dataManager, propertyName, propertyValue);
            return GetItem(dataManager, index);
        }

        /// <summary>
        /// 获取数据项的属性值
        /// </summary>
        /// <param name="dataManager">数据管理对象</param>
        /// <param name="item">数据项</param>
        /// <param name="propertyName">属性名称</param>
        /// <returns>返回指定属性的值</returns>
        public static object GetValue(CurrencyManager dataManager, object item, string propertyName)
        {
            if (item == null || propertyName == null || propertyName.Length == 0) return null;
            try
            {
                PropertyDescriptor descriptor = dataManager != null ?
                    dataManager.GetItemProperties().Find(propertyName, true) :
                    TypeDescriptor.GetProperties(item).Find(propertyName, true);
                if (descriptor != null)
                {
                    item = descriptor.GetValue(item);
                }
            }
            catch
            {
                return null;
            }
            return item;
        }

        /// <summary>
        /// 获取数据项索引
        /// </summary>
        /// <param name="dataManager">数据管理对象</param>
        /// <param name="propertyName">属性名称</param>
        /// <param name="propertyValue">属性值</param>
        /// <returns>返回指定属性名称的项的索引</returns>
        public static int GetIndex(CurrencyManager dataManager, string propertyName, object propertyValue)
        {
            if (propertyValue == null) return -1;
            PropertyDescriptorCollection props = dataManager.GetItemProperties();
            PropertyDescriptor property = props.Find(propertyName, true);
            if (property == null) throw new ArgumentNullException(propertyName);
            var list = dataManager.List;
            if ((list is IBindingList) && ((IBindingList)list).SupportsSearching)
            {
                return ((IBindingList)list).Find(property, propertyValue);
            }
            for (int i = 0; i < list.Count; i++)
            {
                object obj2 = property.GetValue(list[i]);
                if (propertyValue.ToStringOrEmpty().Equals(obj2.ToStringOrEmpty()))
                {
                    return i;
                }
            }
            return -1;
        }
    }
}