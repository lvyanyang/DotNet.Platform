// ===============================================================================
// DotNet.Platform 开发框架 2016 版权所有
// ===============================================================================
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace DotNet.Helper
{
    /// <summary>
    /// 验证操作类
    /// </summary>
    public static class ValidateHelper
    {
        /// <summary>
        /// 验证字符串长度
        /// </summary>
        /// <param name="text">检测的文本</param>
        /// <param name="allowNull">是否允许空</param>
        /// <param name="checkMinLength">是否检测最小长度</param>
        /// <param name="checkMaxLength">是否检测最大长度</param>
        /// <param name="minLength">指定最小长度</param>
        /// <param name="maxLength">指定最大长度</param>
        /// <returns>验证成功返回True</returns>
        public static bool IsStringLengthMatch(string text, bool allowNull, bool checkMaxLength, bool checkMinLength, int minLength, int maxLength)
        {
            if (string.IsNullOrEmpty(text))
                return allowNull;

            if (checkMinLength && text.Length < minLength)
                return false;
            if (checkMaxLength && text.Length > maxLength)
                return false;

            return true;
        }

        /// <summary>
        /// 检测整数是否在指定的数字之间
        /// </summary>
        /// <param name="number">检测的数字</param>
        /// <param name="checkMin">是否检测最小值</param>
        /// <param name="checkMax">是否检测最大值</param>
        /// <param name="min">指定最小值</param>
        /// <param name="max">指定最大值</param>
        /// <returns>验证成功返回True</returns>
        public static bool IsBetween(int number, bool checkMin, bool checkMax, int min, int max)
        {
            if (checkMin && number < min)
                return false;
            if (checkMax && number > max)
                return false;

            return true;
        }

        /// <summary>
        /// 检测浮点数是否在指定的浮点数之间
        /// </summary>
        /// <param name="num">检测的数字</param>
        /// <param name="checkMin">是否检测最小值</param>
        /// <param name="checkMax">是否检测最大值</param>
        /// <param name="min">指定最小值</param>
        /// <param name="max">指定最大值</param>
        /// <returns>验证成功返回True</returns>
        public static bool IsBetween(double num, bool checkMin, bool checkMax, double min, double max)
        {
            if (checkMin && num < min)
                return false;

            if (checkMax && num > max)
                return false;

            return true;
        }

        /// <summary>
        /// 字符串匹配正则表达式
        /// </summary>
        /// <param name="text">检测的文本</param>
        /// <param name="allowNull">是否允许为空</param>
        /// <param name="regEx">正则表达式</param>
        /// <returns>验证成功返回True</returns>
        public static bool IsStringRegExMatch(string text, bool allowNull, string regEx)
        {
            if (allowNull && string.IsNullOrEmpty(text))
                return true;

            return Regex.IsMatch(text, regEx);
        }

        /// <summary>
        /// 检测文本是否包含在指定的值的字符串
        /// </summary>
        /// <param name="text">检测的文本</param>
        /// <param name="allowNull">是否允许为空</param>
        /// <param name="compareCase">是否忽略大小写</param>
        /// <param name="allowedValues">允许的值 逗号分隔</param>
        /// <returns>验证成功返回True</returns>
        public static bool IsStringIn(string text, bool allowNull, bool compareCase, IEnumerable<string> allowedValues)
        {
            if (string.IsNullOrEmpty(text)) 
                return allowNull;
            foreach (string val in allowedValues)
            {
                if (string.Compare(text, val, compareCase) == 0)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 是否整数
        /// </summary>
        /// <param name="text">输入字符串</param>
        /// <returns>如果是整数返回true</returns>
        public static bool IsInteger(string text)
        {
            return Regex.IsMatch(text, RegexConstant.Integer);
        }

        /// <summary>
        /// 是否是布尔字符串
        /// </summary>
        /// <param name="text">输入字符串</param>
        /// <returns></returns>
        public static bool IsBoolean(string text)
        {
            if (text.ToLower().Equals("true", StringComparison.Ordinal) || text.ToLower().Equals("false", StringComparison.Ordinal))
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// 指定文本是否是数字
        /// </summary>
        /// <param name="text">检测的文本</param>
        /// <returns>验证成功返回True</returns>
        public static bool IsNumeric(string text)
        {
            return Regex.IsMatch(text, RegexConstant.Numeric);
        }

        /// <summary>
        /// 判断文本是否小写/大写字母
        /// </summary>
        /// <param name="text">检测的文本</param>
        /// <param name="allowNull">是否允许为空</param>
        /// <returns>验证成功返回True</returns>
        public static bool IsAlpha(string text, bool allowNull)
        {
            return IsMatchReg(text, allowNull, RegexConstant.Alpha);
        }

        /// <summary>
        /// 检测是否有中文字符
        /// </summary>
        /// <param name="text">输入字符串</param>
        /// <returns>成功返回true</returns>
        public static bool IsChinaChar(string text)
        {
            return IsMatchReg(text, false, RegexConstant.ChinaChar);
        }

        /// <summary>
        /// 判断文本是否小写/大写字母或者数字
        /// </summary>
        /// <param name="text">检测的文本</param>
        /// <param name="allowNull">是否允许为空</param>
        /// <returns>验证成功返回True</returns>
        public static bool IsAlphaNumeric(string text, bool allowNull)
        {
            return IsMatchReg(text, allowNull, RegexConstant.AlphaNumeric);
        }

        /// <summary>
        /// 判断文本是否是日期
        /// </summary>
        /// <param name="text">检测的文本</param>
        /// <returns>验证成功返回True</returns>
        public static bool IsDate(string text)
        {
            DateTime result;
            return DateTime.TryParse(text, out result);
        }

        /// <summary>
        /// 检测日期是否在指定的日期之间
        /// </summary>
        /// <param name="date">检测的日期</param>
        /// <param name="checkMin">是否检测最小日期</param>
        /// <param name="checkMax">是否检测最大日期</param>
        /// <param name="minDate">指定最小日期</param>
        /// <param name="maxDate">指定最大日期</param>
        /// <returns>验证成功返回True</returns>
        public static bool IsBetween(DateTime date, bool checkMin, bool checkMax, DateTime minDate, DateTime maxDate)
        {
            if (checkMin && date.Date < minDate.Date) return false;
            if (checkMax && date.Date > maxDate.Date) return false;

            return true;
        }

        /// <summary>
        /// 检测文本是否是一个时间格式 是否在指定的时间之间
        /// </summary>
        /// <param name="time">检测的文本</param>
        /// <returns>验证成功返回True</returns>
        public static bool IsTimeSpan(string time)
        {
            TimeSpan span;
            return TimeSpan.TryParse(time, out span);
        }

        /// <summary>
        /// 检测是否在指定的时间之间
        /// </summary>
        /// <param name="time">检测的文本</param>
        /// <param name="checkMin">是否检测最小日期</param>
        /// <param name="checkMax">是否检测最大日期</param>
        /// <param name="min">指定最小时间</param>
        /// <param name="max">指定最大时间</param>
        /// <returns>验证成功返回True</returns>
        public static bool IsBetween(TimeSpan time, bool checkMin, bool checkMax, TimeSpan min, TimeSpan max)
        {
            if (checkMin && time.Ticks < min.Ticks) return false;
            if (checkMax && time.Ticks > max.Ticks) return false;

            return true;
        }

        /// <summary>
        /// 是否是手机号码
        /// </summary>
        /// <param name="text">检测的文本</param>
        /// <param name="allowNull">是否允许为空</param>
        /// <returns>验证成功返回True</returns>
        public static bool IsMobilePhone(string text, bool allowNull)
        {
            return IsMatchReg(text, allowNull, RegexConstant.MobilePhone);
        }

        /// <summary>
        /// 是否是身份证号码
        /// </summary>
        /// <param name="text">检测的文本</param>
        /// <param name="allowNull">是否允许为空</param>
        /// <returns>验证成功返回True</returns>
        public static bool IsSsn(string text, bool allowNull)
        {
            return IsMatchReg(text, allowNull, RegexConstant.SocialSecurity);
        }

        /// <summary>
        /// 是否是Email
        /// </summary>
        /// <param name="text">检测的文本</param>
        /// <param name="allowNull">是否允许为空</param>
        /// <returns>验证成功返回True</returns>
        public static bool IsEmail(string text, bool allowNull)
        {
            return IsMatchReg(text, allowNull, RegexConstant.Email);
        }

        /// <summary>
        /// 是否是Url
        /// </summary>
        /// <param name="text">检测的文本</param>
        /// <param name="allowNull">是否允许为空</param>
        /// <returns>验证成功返回True</returns>
        public static bool IsUrl(string text, bool allowNull)
        {
            return IsMatchReg(text, allowNull, RegexConstant.Url);
        }

        /// <summary>
        /// 是否是邮政编码
        /// </summary>
        /// <param name="text">检测的文本</param>
        /// <param name="allowNull">是否允许为空</param>
        /// <returns>验证成功返回True</returns>
        public static bool IsZipCode(string text, bool allowNull)
        {
            return IsMatchReg(text, allowNull, RegexConstant.ZipCode);
        }

        /// <summary>
        /// 是否是IP
        /// </summary>
        /// <param name="text">检测的文本</param>
        /// <param name="allowNull">是否允许为空</param>
        /// <returns>验证成功返回True</returns>
        public static bool IsIP(string text, bool allowNull)
        {
            return IsMatchReg(text, allowNull, RegexConstant.IP);
        }

        /// <summary>
        /// 是否是QQ
        /// </summary>
        /// <param name="text">检测的文本</param>
        /// <param name="allowNull">是否允许为空</param>
        /// <returns>验证成功返回True</returns>
        public static bool IsQQ(string text, bool allowNull)
        {
            return IsMatchReg(text, allowNull, RegexConstant.QQ);
        }

        /// <summary>
        /// 检查文本是否匹配一个正则表达式
        /// </summary>
        /// <param name="text">检测的文本</param>
        /// <param name="allowNull">是否允许为空</param>
        /// <param name="regExPattern">正则表达式</param>
        /// <returns>验证成功返回True</returns>
        public static bool IsMatchReg(string text, bool allowNull, string regExPattern)
        {
            bool isEmpty = string.IsNullOrEmpty(text);
            if (isEmpty && allowNull) return true;
            if (isEmpty) return false;

            return Regex.IsMatch(text, regExPattern);
        }
    }

    /// <summary>
    /// 正则表达式常量
    /// </summary>
    public static class RegexConstant
    {
        /// <summary>
        /// 大小写字母正则
        /// </summary>
        public const string Alpha = @"^[a-zA-Z]*$";

        /// <summary>
        /// 中文字符
        /// </summary>
        public const string ChinaChar = @"[\u4e00-\u9fa5]";

        /// <summary>
        /// 大小字母正则
        /// </summary>
        public const string AlphaUpperCase = @"^[A-Z]*$";

        /// <summary>
        /// 小写字母正则
        /// </summary>
        public const string AlphaLowerCase = @"^[a-z]*$";

        /// <summary>
        /// 大小写字母数字正则
        /// </summary>
        public const string AlphaNumeric = @"^[a-zA-Z0-9]*$";

        /// <summary>
        /// 大小写字母数字空格正则
        /// </summary>
        public const string AlphaNumericSpace = @"^[a-zA-Z0-9 ]*$";

        /// <summary>
        /// 大小写字母数字空格和破折号正则
        /// </summary>
        public const string AlphaNumericSpaceDash = @"^[a-zA-Z0-9 \-]*$";

        /// <summary>
        /// 大小写字母数字空格和破折号和下划线正则
        /// </summary>
        public const string AlphaNumericSpaceDashUnderscore = @"^[a-zA-Z0-9 \-_]*$";

        /// <summary>
        /// 大小写字母数字空格和破折号和点和下划线正则
        /// </summary>
        public const string AlphaNumericSpaceDashUnderscorePeriod = @"^[a-zA-Z0-9\. \-_]*$";

        /// <summary>
        /// 数字正则
        /// </summary>
        public const string Numeric = @"^\-?[0-9]*\.?[0-9]*$";

        /// <summary>
        /// 整数正则
        /// </summary>
        public const string Integer = @"^\-?[0-9]*$";

        /// <summary>
        /// 身份证
        /// </summary>
        public const string SocialSecurity = @"^\d{15}$|^\d{17}(?:\d|x|X)$";

        /// <summary>
        /// E-mail正则
        /// </summary>
        public const string Email = @"^([0-9a-zA-Z]+[-._+&])*[0-9a-zA-Z]+@([-0-9a-zA-Z]+[.])+[a-zA-Z]{2,6}$";

        /// <summary>
        /// Url正则
        /// </summary>
        public const string Url = @"^^(ht|f)tp(s?)\:\/\/[0-9a-zA-Z]([-.\w]*[0-9a-zA-Z])*(:(0-9)*)*(\/?)([a-zA-Z0-9\-\.\?\,\'\/\\\+&%\$#_=]*)?$";

        /// <summary>
        /// 邮政编码正则
        /// </summary>
        public const string ZipCode = @"[1-9]\d{5}(?!\d)";

        /// <summary>
        /// 移动电话正则
        /// </summary>
        public const string MobilePhone = @"^(13[0-9]|15[0|3|6|7|8|9]|18[0|8|9])\d{8}$";

        /// <summary>
        /// IP地址正则
        /// </summary>
        public const string IP = @"\d+\.\d+\.\d+\.\d+";

        /// <summary>
        /// QQ号码
        /// </summary>
        public const string QQ = @"[1-9][0-9]{4,}";

        /// <summary>
        /// 空行
        /// </summary>
        public const string EmptyLine = @"\n[\s| ]*\r";

        /// <summary>
        /// HTML标记
        /// </summary>
        public const string HtmlMark = @"<[^>]+>|</[^>]+>";

        /// <summary>
        /// Http协议Url
        /// </summary>
        public const string HttpUrl = @"http://[^\s]*";
    }
}
