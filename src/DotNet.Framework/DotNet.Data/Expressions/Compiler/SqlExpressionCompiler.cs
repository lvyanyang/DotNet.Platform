﻿// ===============================================================================
// DotNet.Platform 开发框架 2016 版权所有
// ===============================================================================
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using DotNet.Data.Extensions;
using DotNet.Helper;

namespace DotNet.Data.Expressions.Compiler
{
    /// <summary>
    /// 
    /// </summary>
    public class SqlExpressionCompiler
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="existingParameterCount"></param>
        public SqlExpressionCompiler(int existingParameterCount = 0)
        {
            this.ExistingParameterCount = existingParameterCount;
        }

        /// <summary>
        /// 
        /// </summary>
        public int ExistingParameterCount { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="existingParameterCount"></param>
        /// <param name="expressions"></param>
        /// <returns></returns>
        public static SqlExpressionCompilerResult Compile(int existingParameterCount, IEnumerable<Expression> expressions)
        {
            return new SqlExpressionCompiler(existingParameterCount).Compile(expressions);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="existingParameterCount"></param>
        /// <param name="expression"></param>
        /// <returns></returns>
        public static SqlExpressionCompilerSelectorResult CompileSelector(int existingParameterCount, Expression expression)
        {
            return new SqlExpressionCompiler(existingParameterCount).CompileSelector(expression);
        }

        //private void CheckRequiredProperties()
        //{

        //}

        #region Private Methods

        private string GetParameterName()
        {
            this.ExistingParameterCount++;
            return "@p_" + this.ExistingParameterCount;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        public SqlExpressionCompilerResult Compile(Expression expression)
        {
            return this.Compile(new Expression[] { expression });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="expressions"></param>
        /// <returns></returns>
        public SqlExpressionCompilerResult Compile(IEnumerable<Expression> expressions)
        {
            //this.CheckRequiredProperties();

            var result = new SqlExpressionCompilerResult();

            var sb = new StringBuilder();
            var isFirstWhere = true;
            foreach (var e in expressions)
            {
                if (!isFirstWhere)
                {
                    sb.Append(" AND ");
                }
                isFirstWhere = false;

                sb.Append(ProcessExpression(e, e, result.Parameters, this.GetParameterName));
            }
            result.SQL = sb.ToString();

            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        public SqlExpressionCompilerSelectorResult CompileSelector(Expression expression)
        {
            return this.CompileSelector(new Expression[] { expression });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="expressions"></param>
        /// <returns></returns>
        public SqlExpressionCompilerSelectorResult CompileSelector(IEnumerable<Expression> expressions)
        {
            //this.CheckRequiredProperties();

            var result = new SqlExpressionCompilerSelectorResult();

            foreach (var e in expressions)
            {
                ProcessSelector(e, e, result.Select, result.Parameters, this.GetParameterName);
            }

            return result;
        }

        #endregion

        #region Private Static Methods

        static void ProcessSelector(Expression rootExpression, Expression e, IList<string> select, IDictionary<string, object> parameters, Func<string> getParameterName)
        {
            if (e == null)
            {
                return;
            }

            if (e.NodeType == ExpressionType.Lambda)
            {
                ProcessSelector(rootExpression, ((LambdaExpression)e).Body, select, parameters, getParameterName);
            }
            else if (e.NodeType == ExpressionType.New)
            {
                var n = (NewExpression)e;
                var len = n.Members.Count();
                for (var i = 0; i < len; i++)
                {
                    var arg = n.Arguments[i];
                    var field = ProcessExpression(rootExpression, arg, parameters, getParameterName);
                    select.Add(field);
                }
            }
            else if (e.NodeType == ExpressionType.Convert)
            {
                var u = (UnaryExpression)e;
                select.Add(ProcessExpression(rootExpression, u.Operand, parameters, getParameterName));
            }
            else if (e.NodeType == ExpressionType.MemberAccess)
            {
                var s = ProcessSingleSideExpression(rootExpression, e, parameters, getParameterName);
                select.Add(s);
            }
            else if (e.NodeType == ExpressionType.Call)
            {
                var s = ProcessCallExpression(rootExpression, (MethodCallExpression)e, parameters, getParameterName);
                select.Add(s);
            }
        }

        //http://weblogs.asp.net/mehfuzh/archive/2007/10/04/writing-custom-linq-provider.aspx
        static string ProcessExpression(Expression rootExpression, Expression e, IDictionary<string, object> parameters, Func<string> getParameterName)
        {
            switch (e.NodeType)
            {
                case ExpressionType.Equal:
                    return ProcessEqualExpression(rootExpression, (BinaryExpression)e, parameters, getParameterName);

                case ExpressionType.GreaterThan:
                    return ProcessGreaterThanExpression(rootExpression, (BinaryExpression)e, parameters, getParameterName);

                case ExpressionType.GreaterThanOrEqual:
                    return ProcessGreaterThanOrEqualExpression(rootExpression, (BinaryExpression)e, parameters, getParameterName);

                case ExpressionType.LessThan:
                    return ProcessLessThanExpression(rootExpression, (BinaryExpression)e, parameters, getParameterName);

                case ExpressionType.LessThanOrEqual:
                    return ProcessLessThanOrEqualExpression(rootExpression, (BinaryExpression)e, parameters, getParameterName);

                case ExpressionType.NotEqual:
                    //var nee = (BinaryExpression)e;
                    //return string.Format("({0} <> {1})", ProcessExpression(rootExpression, nee.Left, parameters, getParameterName), ProcessExpression(rootExpression, nee.Right, parameters, getParameterName));
                    return ProcessNotEqualExpression(rootExpression, (BinaryExpression)e, parameters, getParameterName);

                case ExpressionType.And:
                case ExpressionType.AndAlso:
                    var aae = (BinaryExpression)e;
                    return string.Format("({0} AND {1})", ProcessExpression(rootExpression, aae.Left, parameters, getParameterName), ProcessExpression(rootExpression, aae.Right, parameters, getParameterName));

                case ExpressionType.Or:
                case ExpressionType.OrElse:
                    var oee = (BinaryExpression)e;
                    return string.Format("({0} OR {1})", ProcessExpression(rootExpression, oee.Left, parameters, getParameterName), ProcessExpression(rootExpression, oee.Right, parameters, getParameterName));

                case ExpressionType.Add:
                    var ae = (BinaryExpression)e;
                    return string.Format("({0} + {1})", ProcessExpression(rootExpression, ae.Left, parameters, getParameterName), ProcessExpression(rootExpression, ae.Right, parameters, getParameterName));

                case ExpressionType.Subtract:
                    var se = (BinaryExpression)e;
                    return string.Format("({0} - {1})", ProcessExpression(rootExpression, se.Left, parameters, getParameterName), ProcessExpression(rootExpression, se.Right, parameters, getParameterName));

                case ExpressionType.Multiply:
                    var me = (BinaryExpression)e;
                    return string.Format("({0} * {1})", ProcessExpression(rootExpression, me.Left, parameters, getParameterName), ProcessExpression(rootExpression, me.Right, parameters, getParameterName));

                case ExpressionType.Divide:
                    var de = (BinaryExpression)e;
                    return string.Format("({0} / {1})", ProcessExpression(rootExpression, de.Left, parameters, getParameterName), ProcessExpression(rootExpression, de.Right, parameters, getParameterName));

                case ExpressionType.Modulo:
                    var moduloe = (BinaryExpression)e;
                    return string.Format("({0} % {1})", ProcessExpression(rootExpression, moduloe.Left, parameters, getParameterName), ProcessExpression(rootExpression, moduloe.Right, parameters, getParameterName));

                case ExpressionType.Lambda:
                    return ProcessExpression(rootExpression, ((LambdaExpression)e).Body, parameters, getParameterName);

                case ExpressionType.Call:
                    return ProcessCallExpression(rootExpression, (MethodCallExpression)e, parameters, getParameterName);

                case ExpressionType.Convert:
                    var u = (UnaryExpression)e;
                    return ProcessExpression(rootExpression, u.Operand, parameters, getParameterName);

                default:
                    //if (e.NodeType == ExpressionType.MemberAccess)
                    //{
                    return ProcessSingleSideExpression(rootExpression, e, parameters, getParameterName);
                    //}

                    //throw new Exception("Unrecognized NodeType (" + e.NodeType.ToString() + ")");
            }
        }

        static string ProcessCallExpression(Expression rootExpression, MethodCallExpression e, IDictionary<string, object> parameters, Func<string> getParameterName)
        {
            var method = e.Method;
            string memberName = null;

            if (e.Object is ParameterExpression)
            {
                // This is used by DynamicSQLinqLambdaExpression
                memberName = "{FieldName}";
            }
            else if (e.Object is ConstantExpression)
            {
                throw new Exception("SqlExpressionCompiler.ProcessCallExpresion: ConstantExpression Unsupported");
            }
            else if (e.Object != null)
            {
                // Get Column Name to Use
                var member = (MemberInfo)((dynamic)e.Object).Member;
                memberName = GetMemberColumnName(member);
            }


            if (method.DeclaringType == typeof(Guid))
            {
                var parameterName = getParameterName();

                switch (method.Name.ToLower())
                {
                    case "newguid":
                        SetParamValue(parameters, parameterName, Guid.NewGuid());
                        //parameters.Add(parameterName,Guid.NewGuid() );
                        return string.Format("{0}{1}", memberName, parameterName);

                    default:
                        throw new Exception("Unsupported Method Name (" + method.Name + ") on Guid object");
                }
            }
            else if (method.DeclaringType == typeof(string))
            {
                string parameterName = null;
                string secondParameterName = null;
                if (e.Arguments.Count > 0)
                {
                    parameterName = GetExpressionValue(rootExpression, e.Arguments[0], parameters, getParameterName);

                    if (e.Arguments.Count > 1)
                    {
                        secondParameterName = GetExpressionValue(rootExpression, e.Arguments[1], parameters, getParameterName);
                    }
                }
                string _name = null;
                switch (method.Name.ToLower())
                {
                    case "startswith":
                        _name = parameterName.Substring(1);
                        SetParamValue(parameters, parameterName, parameters[_name].ToString() + "%");
                        //parameters[parameterName] = _value
                        return string.Format("{0} LIKE {1}", memberName, parameterName);

                    case "endswith":
                        _name = parameterName.Substring(1);
                        SetParamValue(parameters, parameterName, "%" + parameters[_name].ToString());
                        //parameters[parameterName] = "%" + parameters[parameterName].ToString();
                        return string.Format("{0} LIKE {1}", memberName, parameterName);

                    case "contains":
                        _name = parameterName.Substring(1);
                        SetParamValue(parameters, parameterName, "%" + parameters[_name].ToString() + "%");
                        //parameters[parameterName] = "%" + parameters[parameterName].ToString() + "%";
                        return string.Format("{0} LIKE {1}", memberName, parameterName);

                    case "between":
                        return string.Format("{0} BETWEEN {1} AND {2}", memberName, parameterName, secondParameterName);

                    case "in":
                        return string.Format("{0} in ({1})", memberName, parameterName);

                    case "notin":
                        return string.Format("{0} not in ({1})", memberName, parameterName);

                    case "equals":
                        return string.Format("{0} = {1}", memberName, parameterName);

                    case "toupper":
                        return string.Format("UCASE({0})", memberName);

                    case "tolower":
                        return string.Format("LCASE({0})", memberName);

                    case "replace":
                        return string.Format("REPLACE({0}, {1}, {2})", memberName, parameterName, secondParameterName);

                    case "substring":
                        if (secondParameterName != null)
                        {
                            return string.Format("SUBSTR({0}, {1}, {2})", memberName, parameterName, secondParameterName);
                        }
                        else
                        {
                            return string.Format("SUBSTR({0}, {1})", memberName, parameterName);
                        }

                    case "indexof":
                        return string.Format("CHARINDEX({0}, {1})", parameterName, memberName);
                    case "trim":
                        return string.Format("LTRIM(RTRIM({0}))", memberName);

                    default:
                        throw new Exception("Unsupported Method Name (" + method.Name + ") on String object");
                }
            }
            else if (method.DeclaringType == typeof(SQLinqExtensions))
            {
                MemberExpression memberExpression;
                var args0 = e.Arguments[0];

                if (args0 is UnaryExpression)
                {
                    memberExpression = (args0 as UnaryExpression).Operand as MemberExpression;
                }
                else
                {
                    memberExpression = args0 as MemberExpression;
                }
                memberName = memberExpression.Member.Name;
                string parameterName = null;
                string secondParameterName = null;
                if (e.Arguments.Count > 1)
                {
                    parameterName = GetExpressionValue(rootExpression, e.Arguments[1], parameters, getParameterName);

                    if (e.Arguments.Count > 2)
                    {
                        secondParameterName = GetExpressionValue(rootExpression, e.Arguments[2], parameters,
                            getParameterName);
                    }
                }

                switch (method.Name.ToLower())
                {
                    case "between":
                        return string.Format("{0} BETWEEN {1} AND {2}", memberName, parameterName, secondParameterName);

                    case "orarraystring":
                        var __name = parameterName.Substring(1);
                        var __value = parameters[__name];
                        if (__value == null) return string.Empty;
                        Array array = (Array)__value;
                        var sb = new StringBuilder();
                        sb.Append("(");
                        for (int i = 0; i < array.Length; i++)
                        {
                            if (i != 0)
                            {
                                sb.Append(" OR ");
                            }
                            var _v = array.GetValue(i);
                            sb.Append($"{memberName}='{_v}'");
                        }
                        sb.Append(")");
                        return sb.ToString();

                    case "inarraystring":
                        var _name = parameterName.Substring(1);
                        var _value = parameters[_name];
                        if (_value == null) return string.Empty;
                        return string.Format("{0} in ({1})", memberName, StringHelper.ConvertArrayToString((IEnumerable)_value, pix: "'"));
                    case "in":
                        return string.Format("{0} in ({1})", memberName, parameterName);

                    case "notin":
                        return string.Format("{0} not in ({1})", memberName, parameterName);

                    default:
                        throw new Exception("不支持方法 (" + method.Name + ") ");
                }
            }
            else
            {
                throw new Exception("Unsupported Method Declaring Type (" + method.DeclaringType.Name + ")");
            }
        }

        static string ProcessBinaryExpression(string sqlOperator, Expression rootExpression, BinaryExpression e, IDictionary<string, object> parameters, Func<string> getParameterName)
        {
            var left = ProcessSingleSideExpression(rootExpression, e.Left, parameters, getParameterName);
            var right = ProcessRightSingleSideExpression(rootExpression, e.Right, parameters, getParameterName);

            var op = sqlOperator;
            if (right == "NULL")
            {
                if (sqlOperator == "=")
                {
                    op = "IS";
                }
                else if (sqlOperator == "<>")
                {
                    op = "IS NOT";
                }
            }

            return string.Format("{1} {0} {2}", op, left, right);
        }

        static string ProcessRightSingleSideExpression(Expression rootExpression, Expression e, IDictionary<string, object> parameters, Func<string> getParameterName)
        {
            var value = Expression.Lambda(e).Compile().DynamicInvoke();
            if (value == null)
            {
                return "NULL";
            }
            else
            {
                var name = getParameterName();
                SetParamValue(parameters, name, value);
                return name;
            }
        }


        static string ProcessLessThanOrEqualExpression(Expression rootExpression, BinaryExpression e, IDictionary<string, object> parameters, Func<string> getParameterName)
        {
            return ProcessBinaryExpression("<=", rootExpression, e, parameters, getParameterName);
        }

        static string ProcessLessThanExpression(Expression rootExpression, BinaryExpression e, IDictionary<string, object> parameters, Func<string> getParameterName)
        {
            return ProcessBinaryExpression("<", rootExpression, e, parameters, getParameterName);
        }

        static string ProcessGreaterThanOrEqualExpression(Expression rootExpression, BinaryExpression e, IDictionary<string, object> parameters, Func<string> getParameterName)
        {
            return ProcessBinaryExpression(">=", rootExpression, e, parameters, getParameterName);
        }

        static string ProcessGreaterThanExpression(Expression rootExpression, BinaryExpression e, IDictionary<string, object> parameters, Func<string> getParameterName)
        {
            return ProcessBinaryExpression(">", rootExpression, e, parameters, getParameterName);
        }

        static string ProcessEqualExpression(Expression rootExpression, BinaryExpression e, IDictionary<string, object> parameters, Func<string> getParameterName)
        {
            return ProcessBinaryExpression("=", rootExpression, e, parameters, getParameterName);
        }

        static string ProcessNotEqualExpression(Expression rootExpression, BinaryExpression e, IDictionary<string, object> parameters, Func<string> getParameterName)
        {
            return ProcessBinaryExpression("<>", rootExpression, e, parameters, getParameterName);
        }

        internal static string GetMemberColumnName(MemberInfo p)
        {
            return p.Name;
        }

        private static void SetParamValue(IDictionary<string, object> parameters, string paramName, object paramValue)
        {
            var _name = paramName.Replace("@", "");
            parameters[_name] = paramValue;
        }

        /// <summary>
        /// This checks if the specified Expression is (in the end) an operation of a Parameter of the Root Expression.
        /// </summary>
        /// <param name="rootExpression"></param>
        /// <param name="e"></param>
        /// <returns></returns>
        static bool IsPropertyExpressionRootParameter(Expression rootExpression, Expression e)
        {
            var retVal = false;
            if (rootExpression is LambdaExpression)
            {
                var lambda = rootExpression as LambdaExpression;

                var lastExpression = TraverseToLastExpression(e);

                if (lastExpression.NodeType == ExpressionType.Parameter)
                {
                    if (lastExpression is ParameterExpression)
                    {
                        var pe = lastExpression as ParameterExpression;

                        foreach (var p in lambda.Parameters)
                        {
                            if (p.Name == pe.Name && p.Type == pe.Type)
                            {
                                retVal = true;
                                break;
                            }
                        }
                    }
                }
            }
            return retVal;
        }

        /// <summary>
        /// Gets the next sub-Expression from the specified Expression recursively until the last one is retrieved.
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        static Expression TraverseToLastExpression(Expression e)
        {
            if (e is MemberExpression)
            {
                return TraverseToLastExpression((e as MemberExpression).Expression);
            }
            return e;
        }

        static string ProcessSingleSideExpression(Expression rootExpression, Expression e, IDictionary<string, object> parameters, Func<string> getParameterName)
        {
            switch (e.NodeType)
            {
                case ExpressionType.Parameter:
                    // This is used by DynamicSQLinqLambdaExpression
                    return "{FieldName}";

                case ExpressionType.Constant:
                    var val = GetExpressionValue(rootExpression, e, parameters, getParameterName);
                    return val.ToString();

                case ExpressionType.MemberAccess:
                    var dynExpr = ((dynamic)e).Expression;
                    if (dynExpr is ConstantExpression)
                    {
                        return GetExpressionValue(rootExpression, e, parameters, getParameterName);
                    }
                    if (dynExpr is MethodCallExpression)
                    {
                        throw new Exception("SqlExpression.ProcessSingleSideExpression: MethodCallExpression Unsupported");
                        /* **** TODO ****
                        var mce = (MethodCallExpression)dynExpr;
                        if (mce.Object is ConstantExpression)
                        {
                            var paramName = ProcessExpression(rootExpression, mce.Object, parameters, getParameterName);
                            return paramName; // BROKEN - Hit when calling a method within lambda expression
                        }
                        return ProcessCallExpression(rootExpression, mce, parameters, getParameterName);
                        */
                    }
                    else
                    {
                        var d = (dynamic)e;

                        if (
                            (d.NodeType == ExpressionType.MemberAccess && d.Expression == null) ||
                            (!IsPropertyExpressionRootParameter(rootExpression, e) && d.Expression.NodeType != ExpressionType.Parameter)
                        )
                        {
                            // A property of an object is being used as a query parameter
                            // This isn't the object that represents a column in the database
                            return GetExpressionValue(rootExpression, e, parameters, getParameterName);
                        }


                        // //////// Get Column Name to Use
                        var memberName = GetMemberColumnName(d.Member);
                        string methodName = null;

                        PropertyInfo pi = null;
                        try
                        {
                            if (d is MemberExpression && d.Expression is MemberExpression)
                            {
                                pi = d.Expression.Member as PropertyInfo;
                            }
                        }
                        catch
                        {
                            pi = null;
                        }
                        if (pi != null)
                        {
                            if (pi.PropertyType == typeof(string))
                            {
                                if (memberName.ToLower() == "[length]")
                                {
                                    methodName = "LEN";
                                    memberName = GetMemberColumnName(pi);
                                }
                            }
                        }
                        else
                        {
                            memberName = string.Format("{0}", memberName);
                        }

                        // ////////////////////////////////////////////////////////////////

                        //var addTableAlias = false; // true;
                        //if (this.JoinExpressions.Count == 0)
                        //{
                        //    if (this.Parent == null)
                        //    {
                        //        addTableAlias = false;
                        //    }
                        //}


                        // //////  Build full SQL to get data for expression
                        string fullMemberName = memberName;
                        //if (addTableAlias)
                        //{
                        //    // Get Table / View Name to Use
                        //    var tableName = d.Expression.Name as String;

                        //    fullMemberName = string.Format("[{0}].{1}", tableName, memberName);
                        //}
                        //else
                        //{
                        //    fullMemberName = string.Format("{0}", memberName);
                        //}

                        // ////// Build full SQL statement for this expression
                        if (methodName != null)
                        {
                            return string.Format("{0}({1})", methodName, fullMemberName);
                        }
                        else
                        {
                            return fullMemberName;
                        }
                    }

                case ExpressionType.Multiply:
                case ExpressionType.Add:
                case ExpressionType.Subtract:
                case ExpressionType.Divide:
                case ExpressionType.Modulo:
                    return ProcessExpression(rootExpression, e, parameters, getParameterName);

                case ExpressionType.And:
                case ExpressionType.Or:
                    return ProcessExpression(rootExpression, e, parameters, getParameterName);

                case ExpressionType.Call:
                    return ProcessCallExpression(rootExpression, (MethodCallExpression)e, parameters, getParameterName);

                case ExpressionType.Convert:
                    return ProcessExpression(rootExpression, (UnaryExpression)e, parameters, getParameterName);

                default:
                    throw new Exception("Unrecognized NodeType (" + e.NodeType.ToString() + ")");
            }
        }

        static string GetExpressionValue(Expression rootExpression, Expression e, IDictionary<string, object> parameters, Func<string> getParameterName)
        {
            var de = (dynamic)e;
            if (de.NodeType == ExpressionType.Convert)
            {
                return GetExpressionValue(rootExpression, de.Operand, parameters, getParameterName);
            }
            if (de.NodeType == ExpressionType.Call || de.NodeType == ExpressionType.NewArrayInit)
            {
                object val = Expression.Lambda(de).Compile().DynamicInvoke();
                var id = getParameterName();
                SetParamValue(parameters, id, val);
                return id;
            }
            if (de.NodeType == ExpressionType.Constant)
            {
                var val = de.Value;
                if (!(e is ConstantExpression))
                {
                    var t = (Type)de.Type;
                    var fieldName = de.Member.Name;

                    var fieldInfo = t.GetField(fieldName) ?? t.GetField(fieldName, BindingFlags.Instance | BindingFlags.NonPublic);
                    if (fieldInfo != null)
                    {
                        val = fieldInfo.GetValue(val);
                    }
                    else
                    {
                        PropertyInfo propInfo = t.GetProperty(fieldName) ?? t.GetProperty(fieldName, BindingFlags.Instance | BindingFlags.NonPublic);
                        if (propInfo != null)
                        {
                            val = propInfo.GetValue(val, null);
                        }
                    }
                }
                else if (e.Type == typeof(SQLStatement))
                {
                    return ((SQLStatement)((ConstantExpression)e).Value).Statement;
                }

                if (val == null)
                {
                    return "NULL";
                }
                if ((val as string) == " ")
                {
                    return "' '";
                }
                var id = getParameterName();
                SetParamValue(parameters, id, val);
                return id;
            }
            if (de.NodeType == ExpressionType.MemberAccess)
            {
                var val = GetMemberAccessValue(rootExpression, e);

                if (val == null)
                {
                    return "NULL";
                }
                if ((val as string) == " ")
                {
                    return "' '";
                }
                var id = getParameterName();
                SetParamValue(parameters, id, val);
                return id;
            }

            return null;
        }

        static object GetMemberAccessValue(Expression rootExpression, Expression e)
        {
            var de = (dynamic)e;

            if (e.NodeType == ExpressionType.Parameter)
            {
                return null;
            }
            else if (e.NodeType == ExpressionType.Constant)
            {
                var ce = e as ConstantExpression;
                return ce.Value;
            }
            else if (e.NodeType == ExpressionType.MemberAccess)
            {
                object val = null;
                var t = (Type)de.Type;
                var fieldName = de.Member.Name;
                if (t == typeof(DateTime))
                {
                    if (fieldName == "Now")
                    {
                        return DateTime.Now;
                    }
                    if (fieldName == "UtcNow")
                    {
                        return DateTime.UtcNow;
                    }
                }
                if (de.Expression != null)
                {
                    val = GetMemberAccessValue(rootExpression, de.Expression);
                    if (val == null) return null;
                }
                if (de.Member is PropertyInfo)
                {
                    var memberPropInfo = de.Member as PropertyInfo;
                    return memberPropInfo.GetValue(val, null);
                }
                else if (de.Member is FieldInfo)
                {
                    var memberFieldInfo = de.Member as FieldInfo;
                    return memberFieldInfo.GetValue(val);
                }
                else
                {
                    throw new NotSupportedException("SQExpressionCompiler.GetMemberAccessValue: Member Type is not supported");
                }
            }

            throw new NotSupportedException("SQExpressionCompiler.GetMemberAccessValue: Expression Not Supported");
        }

        #endregion
    }
}
