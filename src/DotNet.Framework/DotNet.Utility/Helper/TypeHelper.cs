// ===============================================================================
// DotNet.Platform 开发框架 2016 版权所有
// ===============================================================================
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using DotNet.Utility;

namespace DotNet.Helper
{
    /// <summary>
    /// 应用程序中的所有类操作
    /// </summary>
    public static class TypeHelper
    {
        private static readonly List<Type> Types = new List<Type>();
        private static readonly object typeLockObject = new object();

        /// <summary>
        /// 数字类型
        /// </summary>
        private static readonly IDictionary<string, bool> NumericTypes;

        /// <summary>
        /// 基础类型
        /// </summary>
        private static readonly IDictionary<string, bool> BasicTypes;

        /// <summary>
        /// 静态构造
        /// </summary>
        static TypeHelper()
        {
            NumericTypes = new Dictionary<string, bool>();
            NumericTypes[typeof(int).Name] = true;
            NumericTypes[typeof(long).Name] = true;
            NumericTypes[typeof(float).Name] = true;
            NumericTypes[typeof(double).Name] = true;
            NumericTypes[typeof(decimal).Name] = true;
            NumericTypes[typeof(sbyte).Name] = true;
            NumericTypes[typeof(Int16).Name] = true;
            NumericTypes[typeof(Int32).Name] = true;
            NumericTypes[typeof(Int64).Name] = true;
            NumericTypes[typeof(Double).Name] = true;
            NumericTypes[typeof(Decimal).Name] = true;

            BasicTypes = new Dictionary<string, bool>();
            BasicTypes[typeof(int).Name] = true;
            BasicTypes[typeof(long).Name] = true;
            BasicTypes[typeof(float).Name] = true;
            BasicTypes[typeof(double).Name] = true;
            BasicTypes[typeof(decimal).Name] = true;
            BasicTypes[typeof(sbyte).Name] = true;
            BasicTypes[typeof(Int16).Name] = true;
            BasicTypes[typeof(Int32).Name] = true;
            BasicTypes[typeof(Int64).Name] = true;
            BasicTypes[typeof(Double).Name] = true;
            BasicTypes[typeof(Decimal).Name] = true;
            BasicTypes[typeof(bool).Name] = true;
            BasicTypes[typeof(DateTime).Name] = true;
            BasicTypes[typeof(string).Name] = true;
        }

        /// <summary>
        /// 测试对象是否是数字类型
        /// </summary>
        /// <param name="val">测试对象</param>
        public static bool IsNumeric(object val)
        {
            return NumericTypes.ContainsKey(val.GetType().Name);
        }

        /// <summary>
        /// 测试类型是否是数字类型
        /// </summary>
        /// <param name="type">测试类型</param>
        public static bool IsNumeric(Type type)
        {
            return NumericTypes.ContainsKey(type.Name);
        }

        /// <summary>
        /// 测试类型是否是布尔类型
        /// </summary>
        /// <param name="type">测试类型</param>
        public static bool IsBoolean(Type type)
        {
            return type == typeof(bool) || type == typeof(bool?);
        }

        /// <summary>
        /// 测试类型是否是日期类型
        /// </summary>
        /// <param name="type">测试类型</param>
        public static bool IsDateTime(Type type)
        {
            return type == typeof(DateTime);
        }

        /// <summary>
        /// 测试类型是否是字符串数组
        /// </summary>
        /// <param name="type">测试类型</param>
        /// <returns></returns>
        public static bool IsArray(Type type)
        {
            return type == typeof(Object[]);
        }

        /// <summary>
        /// 测试类型是否是基础类型
        /// </summary>
        /// <param name="type">测试类型</param>
        public static bool IsBasicType(Type type)
        {
            return BasicTypes.ContainsKey(type.Name);
        }

        /// <summary>
        /// 获取当前应用程序中加载的所有类型
        /// </summary>
        public static IEnumerable<Type> GetApplicateTypes()
        {
            lock (typeLockObject)
            {
                if (Types.Count == 0)
                {
                    var files = FileHelper.GetFiles(DirHelper.OutDirectory);
                    var names = new List<string>();
                    foreach (string item in files)
                    {
                        var _name = Path.GetFileName(item);
                        if (names.IndexOf(_name) > -1)
                        {
                            continue;
                        }
                        if (!ValidateFileExtension(item)) continue;
                        string assemblyName = Path.GetFileNameWithoutExtension(item);
                        if (string.IsNullOrEmpty(assemblyName)) continue;
                        try
                        {
                            Types.AddRange(Assembly.Load(assemblyName).GetTypes());
                            names.Add(_name);
                        }
                        catch (Exception e)
                        {
                            DebugHelper.Debug(e.Message);
                        }
                    }
                }
                return Types;
            }
        }

        /// <summary>
        /// 从当前应用程序中的所有类型中检索继承自指定类型或者实现了指定类型的类型列表。
        /// </summary>
        /// <param name="loadedTypes">已经加载的类型</param>
        /// <param name="type">指定的接口或者父类</param>
        /// <returns>返回当前应用程序中的所有类型中检索继承自指定类型或者实现了指定类型的类型列表。</returns>
        public static IEnumerable<Type> GetSubClassList(IEnumerable<Type> loadedTypes, Type type)
        {
            if (type == null) return null;
            List<Type> list = new List<Type>();
            foreach (var t in loadedTypes)
            {
                if (AssemblyHelper.ValidInterfaceClass(t, false, type))
                {
                    list.Add(t);
                }
                if (CheckSubClass(t, type))
                {
                    list.Add(t);
                }
            }
            return list;
        }

        private static bool CheckSubClass(Type type, Type checkType)
        {
            if (type.BaseType != null)
            {
                if (type.BaseType == checkType)
                {
                    return true;
                }
                return CheckSubClass(type.BaseType, checkType);
            }
            return false;
        }


        /// <summary>
        /// 验证文件扩展名
        /// </summary>
        /// <param name="filePath">文件路径</param>
        /// <returns>如果文件扩展是exe或者dll返回True</returns>
        private static bool ValidateFileExtension(string filePath)
        {
            string fileExtension = Path.GetExtension(filePath);
            if (string.IsNullOrEmpty(fileExtension)) return false;

            bool result = (fileExtension.Equals(".dll") || fileExtension.Equals(".exe"))
                          && filePath.IndexOf("vshost", StringComparison.Ordinal) == -1;
            return result;
        }
    }
}