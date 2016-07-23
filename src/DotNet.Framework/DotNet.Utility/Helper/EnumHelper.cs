// ===============================================================================
// DotNet.Platform 开发框架 2016 版权所有
// ===============================================================================
using System;
using System.Collections.Generic;
using System.Linq;
using DotNet.Utility;

namespace DotNet.Helper
{
    /// <summary>
    /// 枚举操作类
    /// </summary>
    public static class EnumHelper
    {
        private static Cache<Type, EnumCaptionAttribute[]> EnumCaches = new Cache<Type, EnumCaptionAttribute[]>();

        /// <summary>
        /// 字符串转为枚举
        /// </summary>
        /// <typeparam name="T">枚举类型</typeparam>
        /// <param name="enumString">要转换的字符串</param>
        /// <returns>如果字符串为空 返回枚举默认值</returns>
        public static T ToEnum<T>(string enumString)
        {
            if (string.IsNullOrEmpty(enumString))
            {
                return default(T);
            }
            return (T)Enum.Parse(typeof(T), enumString,true);
        }

        /// <summary>
        /// 字符串转为枚举
        /// </summary>
        /// <param name="typeString">类型字符串</param>
        /// <param name="enumString">要转换的字符串</param>
        /// <returns>如果字符串为空 返回枚举默认值</returns>
        public static object ToEnum(string typeString, string enumString)
        {
            if (string.IsNullOrEmpty(enumString))
            {
                return null;
            }
            var enumType = Type.GetType(typeString, false, true);
            if (enumType == null)
            {
                return null;
            }
            return Enum.Parse(enumType, enumString);
        }

        /// <summary>
        /// 获取枚举元数据
        /// </summary>
        /// <param name="enumItem">枚举项</param>
        /// <param name="enumType">枚举类型</param>
        public static string GetEnumItemCaption(string enumItem, Type enumType)
        {
            var attrs = EnumCaches.Get(enumType, () => GetEnumItems(enumType));
            var item = attrs.FirstOrDefault(p=>p.Name==enumItem);
            if (item!=null)
            {
                return item.Caption;
            }
            return enumItem;
        }

        /// <summary>
        /// 获取枚举元数据
        /// </summary>
        /// <param name="enumType">枚举类型</param>
        public static EnumCaptionAttribute[] GetEnumItems(Type enumType)
        {
            var list = new List<EnumCaptionAttribute>();
            var pros = enumType.GetFields();
            foreach (var info in pros)
            {
                var einfo = AssemblyHelper.GetAttribute<EnumCaptionAttribute>(info);
                if (einfo == null) continue;
                einfo.Name = info.Name;
                list.Add(einfo);
            }
            return list.ToArray();
        }
    }
}