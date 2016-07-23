// ===============================================================================
// DotNet.Platform 开发框架 2016 版权所有
// ===============================================================================
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;

namespace DotNet.Data.Utilities
{
    /// <summary>
    /// 参数解析帮助类
    /// </summary>
    public static class ParameterHelper
    {
        /// <summary>
        /// SQL参数匹配正则表达式
        /// </summary>
        public static readonly Regex RXParamsPrefix = new Regex(@"(?<!@)@\w+", RegexOptions.Compiled);

        /// <summary>
        /// 解析SQL语句参数
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <param name="args_src">用户传入的参数对象</param>
        /// <param name="args_dest">接收的参数列表</param>
        /// <returns></returns>
        public static string ParserParameter(string sql, object args_src, List<KeyValuePair<string, object>> args_dest)
        {
            if (args_src == null) return sql;

            object[] arrayArgs = args_src as object[];
            if (arrayArgs != null)
            {
                int paramIndex = -1;

                return RXParamsPrefix.Replace(sql, m =>
                {
                    paramIndex++;
                    string name = m.Value.Substring(1);//取参数名字,不带特殊标记@:
                    if (paramIndex < 0 || paramIndex >= arrayArgs.Length)
                        throw new ArgumentOutOfRangeException("args_src", "解析参数错误,参数数量与给定的参数值数量不符");
                    var value = arrayArgs[paramIndex];
                    return HandleParameter(name, value, args_dest);
                });
            }

            IDictionary<string, object> dicArgs = args_src as IDictionary<string, object>;
            if (dicArgs != null)
            {
                return RXParamsPrefix.Replace(sql, m =>
                {
                    object value;
                    string name = m.Value.Substring(1);//取参数名字,不带特殊标记@:
                    dicArgs.TryGetValue(name, out value);
                    return HandleParameter(name, value, args_dest);
                });
            }

            Type argsType = args_src.GetType();
            return RXParamsPrefix.Replace(sql, m =>
            {
                string name = m.Value.Substring(1);//取参数名字,不带特殊标记@:
                object value = null;
                PropertyInfo pinfo = argsType.GetProperty(name);
                if (pinfo != null)
                {
                    value = pinfo.GetValue(args_src, null);
                }
                return HandleParameter(name, value, args_dest);
            });
        }

        /// <summary>
        /// 添加参数到接收的参数列表
        /// </summary>
        /// <param name="name">参数名称</param>
        /// <param name="value">参数值</param>
        /// <param name="args_dest">接收的参数列表</param>
        /// <returns>返回替换的参数名称</returns>
        private static string HandleParameter(string name, object value, List<KeyValuePair<string, object>> args_dest)
        {
            if ((value as IEnumerable) != null &&
                    (value as string) == null &&
                    (value as byte[]) == null)
            {
                var sb = new StringBuilder();
                int index = 0;
                foreach (var i in (IEnumerable)value)
                {
                    string partName = name + index;
                    sb.Append((index == 0 ? "@" : ",@") + partName);
                    if (!args_dest.Exists(p => p.Key.Equals(partName)))
                    {
                        args_dest.Add(new KeyValuePair<string, object>(partName, i));
                    }
                    index++;
                }
                return sb.ToString();
            }

            if (!args_dest.Exists(p => p.Key.Equals(name)))
            {
                args_dest.Add(new KeyValuePair<string, object>(name, value));
            }
            return "@" + name;
        }

        /// <summary>
        /// 解析存储过程参数
        /// </summary>
        /// <param name="args_src">用户传入的参数对象</param>
        /// <param name="args_dest">接收的参数列表</param>
        public static void ParserStoreProcParameter(object args_src, List<KeyValuePair<string, object>> args_dest)
        {
            if (args_src == null) return;
            object[] arrayArgs = args_src as object[];
            if (arrayArgs != null)
            {
                throw new Exception("此方法不支持数组值类型参数");
            }

            IDictionary<string, object> dicArgs = args_src as IDictionary<string, object>;
            if (dicArgs != null)
            {
                foreach (KeyValuePair<string, object> pair in dicArgs)
                {
                    string name = pair.Key;
                    if (!args_dest.Exists(p => p.Key.Equals(name)))
                    {
                        args_dest.Add(new KeyValuePair<string, object>(name, pair.Value));
                    }
                }
            }
            else
            {
                foreach (PropertyDescriptor propertyDescriptor in TypeDescriptor.GetProperties(args_src))
                {
                    string name = propertyDescriptor.Name;
                    if (!args_dest.Exists(p => p.Key.Equals(name)))
                    {
                        args_dest.Add(new KeyValuePair<string, object>(name, propertyDescriptor.GetValue(args_src)));
                    }
                }
            }
            
        }
    }
}