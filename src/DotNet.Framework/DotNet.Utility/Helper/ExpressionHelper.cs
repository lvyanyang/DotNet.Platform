// ===============================================================================
// DotNet.Platform 开发框架 2016 版权所有
// ===============================================================================
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;

namespace DotNet.Helper
{
    /// <summary>
    /// Lambda表达式操作类
    /// </summary>
    public class ExpressionHelper
    {
        /// <summary>
        /// 获取表达式对象属性名称
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="exp">表达式</param>
        /// <returns>返回对象属性名称</returns>
        public static string GetPropertyName<T>(Expression<Func<T, object>> exp)
        {
            return GetPropertyName<T, object>(exp);
        }

        /// <summary>
        /// 获取表达式对象属性值
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="obj">对象</param>
        /// <param name="exp">表达式</param>
        /// <returns>返回对象属性值</returns>
        public static object GetPropertyValue<T>(T obj, Expression<Func<T, object>> exp)
        {
            var fun = Expression.Lambda<Func<T, object>>(exp.Body, exp.Parameters).Compile();
            return fun.Invoke(obj);
        }

        /// <summary>
        /// 获取表达式对象属性名称
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <typeparam name="Y">返回值类型</typeparam>
        /// <param name="exp">表达式</param>
        /// <returns>返回对象属性名称</returns>
        public static string GetPropertyName<T, Y>(Expression<Func<T, Y>> exp)
        {
            MemberExpression memberExpression = null;

            // Get memberexpression.
            if (exp.Body is MemberExpression)
            {
                memberExpression = exp.Body as MemberExpression;
            }

            if (exp.Body is UnaryExpression)
            {
                var unaryExpression = exp.Body as UnaryExpression;
                if (unaryExpression != null && unaryExpression.Operand is MemberExpression)
                {
                    memberExpression = unaryExpression.Operand as MemberExpression;
                }
            }

            if (memberExpression == null)
            {
                throw new InvalidOperationException("没有成员访问表达式");
            }

            var info = memberExpression.Member as PropertyInfo;
            if (info != null)
            {
                return info.Name;
            }
            return string.Empty;
        }

        /// <summary>
        /// 获取表达式对象属性名称
        /// </summary>
        /// <param name="exp">表达式</param>
        /// <returns>返回对象属性名称</returns>
        public static string GetPropertyName(Expression<Func<object>> exp)
        {
            var memberExpression = exp.Body as MemberExpression;
            if (memberExpression == null)
            {
                throw new InvalidOperationException("没有成员访问表达式");
            }

            var info = memberExpression.Member as PropertyInfo;
            if (info != null)
            {
                return info.Name;
            }
            return string.Empty;
        }

        /// <summary>
        /// 获取表达式对象属性值和属性名称
        /// </summary>
        /// <param name="exp">表达式</param>
        /// <returns>返回属性名称和属性值</returns>
        public static KeyValuePair<string, object> GetPropertyNameAndValue(Expression<Func<object>> exp)
        {
            string key = string.Empty;

            PropertyInfo propInfo = null;
            var body = exp.Body as MemberExpression;
            if (body != null)
            {
                propInfo = body.Member as PropertyInfo;
            }
            else
            {
                var expression = exp.Body as UnaryExpression;
                if (expression != null)
                {
                    Expression op = expression.Operand;
                    propInfo = ((MemberExpression)op).Member as PropertyInfo;
                }
            }

            object value = exp.Compile().DynamicInvoke();
            if (propInfo != null)
            {
                key = propInfo.Name;
            }
            var pair = new KeyValuePair<string, object>(key, value);
            return pair;
        }

        /// <summary>
        /// 根据表达式数组,获取属性名称数组
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="exps">表达式数组</param>
        /// <returns>返回属性名称数组</returns>
        public static string[] GetPropertyNameArray<T>(params Expression<Func<T, object>>[] exps)
        {
            if (exps == null || exps.Length == 0)
                return new string[0];
            string[] cols = new string[exps.Length];
            for (int ndx = 0; ndx < exps.Length; ndx++)
            {
                cols[ndx] = GetPropertyName(exps[ndx]);
            }
            return cols;
        }

        /// <summary>
        /// 创建属性访问表达式 (p=>p.propertyName)
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="propertyName">属性名称</param>
        /// <returns>返回属性访问表达式</returns>
        public static Expression<Func<T, object>> CreatePropertyExpression<T>(string propertyName)
        {
            var param = Expression.Parameter(typeof(T), "p");
            var pinfo = typeof(T).GetProperty(propertyName);
            var memAccess = Expression.MakeMemberAccess(param, pinfo);
            if (pinfo.PropertyType == typeof(string))
            {
                return Expression.Lambda<Func<T, object>>(memAccess, param);
            }
            var convert = Expression.Convert(memAccess, typeof(object));
            return Expression.Lambda<Func<T, object>>(convert, param);
        }

        /// <summary>
        /// 生成查询语句,成类似与“p.Name.ToString()==常量”的表达式
        /// </summary>
        /// <param name="name">字段名称</param>
        /// <param name="value">字段值</param>
        /// <returns></returns>
        public static Expression<Func<T, bool>> BuildExpression<T>(string name, object value)
        {
            ParameterExpression param = Expression.Parameter(typeof(T), "p");
            var entityType = typeof(T);

            //生成类似与“p.Name.ToString()==常量”的表达式
            var property = entityType.GetProperty(name);
            Expression result = Expression.Equal(Expression.Property(param, property),
                    Expression.Constant(ObjectHelper.ConvertObjectValue(value, property.PropertyType)));
            return Expression.Lambda<Func<T, bool>>(result, new[] { param });
        }

        /// <summary>
        /// 生成查询语句,成类似与 p.Name.ToString().Contains(常量) 的表达式
        /// </summary>
        /// <param name="name">字段名称</param>
        /// <param name="value">字段值</param>
        /// <returns></returns>
        public static Expression<Func<T, bool>> BuildLikeExpression<T>(string name, object value)
        {
            ParameterExpression param = Expression.Parameter(typeof(T), "p");
            var entityType = typeof(T);
            //生成类似与 p.Name.ToString().Contains(常量) 的表达式
            var property = entityType.GetProperty(name);
            MethodInfo info = typeof (string).GetMethod("Contains");
            var conExp = Expression.Constant(ObjectHelper.ConvertObjectValue(value, property.PropertyType));
            Expression result = Expression.Call(Expression.Property(param, property), info, conExp);
            return Expression.Lambda<Func<T, bool>>(result, new[] { param });
        }
    }
}
