// ===============================================================================
// DotNet.Platform ������� 2016 ��Ȩ����
// ===============================================================================
using System;
using System.Collections.Generic;
using System.Linq;
using DotNet.Utility;

namespace DotNet.Helper
{
    /// <summary>
    /// ö�ٲ�����
    /// </summary>
    public static class EnumHelper
    {
        private static Cache<Type, EnumCaptionAttribute[]> EnumCaches = new Cache<Type, EnumCaptionAttribute[]>();

        /// <summary>
        /// �ַ���תΪö��
        /// </summary>
        /// <typeparam name="T">ö������</typeparam>
        /// <param name="enumString">Ҫת�����ַ���</param>
        /// <returns>����ַ���Ϊ�� ����ö��Ĭ��ֵ</returns>
        public static T ToEnum<T>(string enumString)
        {
            if (string.IsNullOrEmpty(enumString))
            {
                return default(T);
            }
            return (T)Enum.Parse(typeof(T), enumString,true);
        }

        /// <summary>
        /// �ַ���תΪö��
        /// </summary>
        /// <param name="typeString">�����ַ���</param>
        /// <param name="enumString">Ҫת�����ַ���</param>
        /// <returns>����ַ���Ϊ�� ����ö��Ĭ��ֵ</returns>
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
        /// ��ȡö��Ԫ����
        /// </summary>
        /// <param name="enumItem">ö����</param>
        /// <param name="enumType">ö������</param>
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
        /// ��ȡö��Ԫ����
        /// </summary>
        /// <param name="enumType">ö������</param>
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