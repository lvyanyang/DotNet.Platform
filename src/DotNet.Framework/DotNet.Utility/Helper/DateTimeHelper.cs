// ===============================================================================
// DotNet.Platform 开发框架 2016 版权所有
// ===============================================================================
using System;
using System.Globalization;
using System.Text;

namespace DotNet.Helper
{
    /// <summary>
    /// 日期操作类
    /// </summary>
    /// <example>
    /// <code>
    /// using XCI.Helper;
    /// 
    /// DateTime date1 = new DateTime(2012, 11, 6);
    /// DateTime date2 = new DateTime(2012, 2, 4);
    /// DateTime dateNow = new DateTime(2012, 11, 6, 16, 31, 20, 456);
    /// 
    /// Console.WriteLine("date1 = {0}", date1);//date1 = 2012/11/6 0:00:00
    /// Console.WriteLine("date2 = {0}", date2);//date2 = 2012/2/4 0:00:00
    /// Console.WriteLine("dateNow = {0}", dateNow);//dateNow = 2012/11/6 16:31:20
    /// Console.WriteLine();
    /// 
    /// Console.WriteLine(DateTimeHelper.FormatDate(date1)); //2012-11-06 //格式化日期(格式 yyyy-MM-dd)  
    /// Console.WriteLine(DateTimeHelper.FormatDate(date1, "yyyy年MM月dd日")); //2012年11月06日 //格式化日期  
    /// Console.WriteLine(DateTimeHelper.FormatDateHasMilliSecond(dateNow)); //2012-11-06 16:31:20.456 //格式化日期(格式 yyyy-MM-dd HH:mm:ss.FFF)  
    /// Console.WriteLine(DateTimeHelper.FormatDateHasSecond(dateNow)); //2012-11-06 16:31:20 //获取格式化的日期字符串(如 yyyy-MM-dd HH:mm:ss)  
    /// Console.WriteLine(DateTimeHelper.GetMonthLastDay(date1)); //30 //获取月份最后一天  
    /// Console.WriteLine(DateTimeHelper.GetMonthLastDay(date2)); //29 //获取月份最后一天  
    /// Console.WriteLine(DateTimeHelper.GetTimeString(new TimeSpan(1, 2, 3, 4)));//1天2小时3分钟 //获取时间的字符串描述(xx天xx小时xx分钟)  
    /// Console.WriteLine(DateTimeHelper.GetDateDiffString(new DateTime(2012, 11, 6, 16, 17, 5), dateNow)); //14分钟前 //获取时间差的字符串描述(xx小时前或者xx分钟前)  
    /// Console.WriteLine(DateTimeHelper.GetWeekDay());//星期二 //获取当前是星期数  
    /// Console.WriteLine(DateTimeHelper.GetWeekDay(date2)); //星期六 //获取日期对应的星期数  
    /// Console.WriteLine(DateTimeHelper.IsLeapYear(date1.Year)); //True //判断指定年份是否是闰年 
    /// Console.WriteLine(DateTimeHelper.IsLeapYear(2009)); //False //判断指定年份是否是闰年 
    /// Console.WriteLine(DateTimeHelper.IsWeekend(date1)); //False //判断指定日期是否是周末(周六或者周日)  
    /// Console.WriteLine(DateTimeHelper.IsWeekend(date2)); //True //判断指定日期是否是周末(周六或者周日)
    /// </code>
    /// </example>
    public static class DateTimeHelper
    {
        /// <summary>
        /// 判断指定年份是否是闰年
        /// </summary>
        /// <param name="year">年份</param>
        /// <returns>如果指定日期是闰年返回true</returns>
        public static bool IsLeapYear(int year)
        {
            return year % 4 == 0 && (year % 100 != 0 || year % 400 == 0);
        }

        /// <summary>
        /// 判断指定日期是否是周末(周六或者周日)
        /// </summary>
        /// <param name="date">日期</param>
        /// <returns>如果指定日期是周六或者周日返回true</returns>
        public static bool IsWeekEnd(DateTime date)
        {
            return date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday;
        }

        /// <summary>
        /// 获取当前月第一天
        /// </summary>
        public static DateTime GetMonthFirstDate()
        {
            return GetMonthFirstDate(DateTime.Now);
        }

        /// <summary>
        /// 获取指定日期所在月的第一天
        /// </summary>
        /// <param name="date">指定日期</param>
        public static DateTime GetMonthFirstDate(DateTime date)
        {
            return new DateTime(date.Year, date.Month, 1);
        }

        /// <summary>
        /// 获取当前年第一天
        /// </summary>
        public static DateTime GetYearFirstDate()
        {
            return GetYearFirstDate(DateTime.Now);
        }

        /// <summary>
        /// 获取当前年第一天
        /// </summary>
        /// <param name="year">年份</param>
        public static DateTime GetYearFirstDate(int year)
        {
            return new DateTime(year, 1, 1);
        }

        /// <summary>
        /// 获取指定日期所在年的第一天
        /// </summary>
        public static DateTime GetYearFirstDate(DateTime date)
        {
            return new DateTime(date.Year, 1, 1);
        }

        /// <summary>
        /// 获取当前年最后一天
        /// </summary>
        public static DateTime GetYearLastDate()
        {
            return GetYearLastDate(DateTime.Now);
        }

        /// <summary>
        /// 获取指定年份最后一天
        /// </summary>
        /// <param name="year">指定年份</param>
        public static DateTime GetYearLastDate(int year)
        {
            return new DateTime(year, 12, GetYearLastDay(year));
        }

        /// <summary>
        /// 获取指定日期所在年的最后一天
        /// </summary>
        /// <param name="date">指定日期</param>
        public static DateTime GetYearLastDate(DateTime date)
        {
            return new DateTime(date.Year, 12, GetYearLastDay(date.Year));
        }

        /// <summary>
        /// 获取月份最后一天
        /// </summary>
        /// <param name="year">年份</param>
        /// <returns>返回月份最后一天</returns>
        public static int GetYearLastDay(int year)
        {
            int day = new GregorianCalendar().GetDaysInMonth(year, 12);
            return day;
        }

        /// <summary>
        /// 获取当前月最后一天
        /// </summary>
        public static DateTime GetMonthLastDate()
        {
            return GetMonthLastDate(DateTime.Now);
        }

        /// <summary>
        /// 获取指定日期所在月的最后一天
        /// </summary>
        public static DateTime GetMonthLastDate(DateTime date)
        {
            return new DateTime(date.Year, date.Month, GetMonthLastDay(date));
        }

        /// <summary>
        /// 获取月份最后一天
        /// </summary>
        /// <param name="date">指定日期</param>
        /// <returns>返回月份最后一天</returns>
        public static int GetMonthLastDay(DateTime date)
        {
            int year = date.Year;
            int month = date.Month;
            int day = new GregorianCalendar().GetDaysInMonth(year, month);//获取指定月的天数
            DateTime lastDay = new DateTime(year, month, day);
            return lastDay.Day;
        }

        /// <summary>
        /// 获取时间间隔的字符串描述(xx天xx小时xx分钟)
        /// </summary>
        /// <param name="ts">时间间隔</param>
        /// <param name="hasMilliseconds">包含毫秒</param>
        /// <returns>返回时间间隔的字符串描述(xx天xx小时xx分钟)</returns>
        public static string GetTimeString(TimeSpan ts, bool hasMilliseconds = true)
        {
            StringBuilder sb = new StringBuilder();
            var newts = ts;
            if (newts.Days >= 1)
            {
                sb.AppendFormat("{0}天", ts.Days);
                newts -= new TimeSpan(ts.Days, 0, 0, 0);
            }
            if (newts.Hours >= 1)
            {
                sb.AppendFormat("{0}小时", ts.Hours);
                newts -= new TimeSpan(ts.Hours, 0, 0);
            }
            if (newts.Minutes >= 1)
            {
                sb.AppendFormat("{0}分钟", ts.Minutes);
                newts -= new TimeSpan(0, ts.Minutes, 0);
            }
            if (newts.Seconds >= 1)
            {
                sb.AppendFormat("{0}秒", ts.Seconds);
                newts -= new TimeSpan(0, 0, ts.Seconds);
            }
            if (hasMilliseconds && newts.Milliseconds >= 1)
            {
                sb.AppendFormat("{0}毫秒", ts.Milliseconds);
            }
            return sb.ToString();
        }

        /// <summary>
        /// 获取时间描述(时分秒)
        /// </summary>
        /// <param name="seconds"></param>
        public static string GetTimeStringHMS(int seconds)
        {
            StringBuilder sb = new StringBuilder();
            var ones = new TimeSpan(0, 0, 1).Ticks;
            var ticks = ones*seconds;
            TimeSpan ts = new TimeSpan(ticks);
            var newts = ts;
            if (newts.Hours >= 1)
            {
                sb.AppendFormat("{0}小时", ts.Hours);
                newts -= new TimeSpan(ts.Hours, 0, 0);
            }
            if (newts.Minutes >= 1)
            {
                sb.AppendFormat("{0}分钟", ts.Minutes);
                newts -= new TimeSpan(0, ts.Minutes, 0);
            }
            if (newts.Seconds >= 1)
            {
                sb.AppendFormat("{0}秒", ts.Seconds);
            }
            return sb.ToString();
        }

        /// <summary>
        /// 获取时间差的字符串描述(xx小时前或者xx分钟前),如果超过一天则显示xx月xx日
        /// </summary>
        /// <param name="dateTime">日期1</param>
        /// <param name="nowDateTime">日期2</param>
        /// <returns>xx小时前,xx分钟前</returns>
        /// <remarks>如果超过一天则显示xx月xx日</remarks>
        public static string GetDateDiffString(DateTime dateTime, DateTime nowDateTime)
        {
            string dateDiff;
            TimeSpan ts = nowDateTime - dateTime;
            if (ts.Days >= 1)
            {
                dateDiff = dateTime.ToString("MM月dd日 HH:mm"); //dateTime.Month + "月" + dateTime.Day + "日";
            }
            else
            {
                if (ts.Hours > 1)
                {
                    dateDiff = ts.Hours + "小时前";
                }
                else
                {
                    dateDiff = ts.Minutes + "分钟前";
                }
            }
            return dateDiff;
        }

        /// <summary>
        /// 获取当前是星期几
        /// </summary>
        /// <returns>返回当前是星期几</returns>
        public static string GetWeekDay()
        {
            return GetWeekDay(DateTime.Now);
        }

        /// <summary>
        /// 获取日期对应的星期数(中文)
        /// </summary>
        /// <param name="date">日期</param>
        /// <returns>返回日期对应的星期数(星期日,星期一...)</returns>
        public static string GetWeekDay(DateTime date)
        {
            string[] weekDay = { "星期日", "星期一", "星期二", "星期三", "星期四", "星期五", "星期六" };
            return weekDay[(int)(date.DayOfWeek)];
        }

        /// <summary>
        /// 获取日期对应的星期数(英文)
        /// </summary>
        /// <param name="date">日期</param>
        /// <returns>返回日期对应的星期数(星期日,星期一...)</returns>
        public static string GetWeekDayEnglish(DateTime date)
        {
            string[] weekDay = { "Sunday", "Monday", "Tuesday", "Wednesday", "Thurday", "Friday", "Saturday" };
            return weekDay[(int)(date.DayOfWeek)];
        }

        /// <summary>
        /// 获取月份对应的英文月份
        /// </summary>
        /// <param name="date">日期</param>
        public static string GetMonthEnglish(DateTime date)
        {
            string[] monthDay = { "January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December" };
            return monthDay[date.Month];
        }


        /// <summary>
        /// 格式化日期
        /// </summary>
        /// <param name="datetime">日期</param>
        /// <param name="format">格式字符串</param>
        /// <returns>返回日期格式化后的字符串</returns>
        /// <remarks>格式字符串:
        ///     <list type="table">
        ///         <item>
        ///             <term>d</term>
        ///             <description>将月中日期表示为从 1 至 31 的数字。一位数字的日期设置为不带前导零的格式。有关使用单个格式说明符的更多信息，请参见使用单个自定义格式说明符。</description>
        ///         </item>
        ///         <item>
        ///             <term>dd</term>
        ///             <description>将月中日期表示为从 01 至 31 的数字。一位数字的日期设置为带前导零的格式。</description>
        ///         </item>
        ///         <item>
        ///             <term>h</term>
        ///             <description>将小时表示为从 1 至 12 的数字，即通过 12 小时制表示小时，自午夜或中午开始对整小时计数。因此，午夜后经过的某特定小时数与中午过后的相同小时数无法加以区分。小时数不进行舍入，一位数字的小时数设置为不带前导零的格式。例如，给定时间为 5:43，则此格式说明符显示“5”。</description>
        ///         </item>
        ///         <item>
        ///             <term>hh, hh（另加任意数量的“h”说明符）</term>
        ///             <description>将小时表示为从 01 至 12 的数字，即通过 12 小时制表示小时，自午夜或中午开始对整小时计数。因此，午夜后经过的某特定小时数与中午过后的相同小时数无法加以区分。小时数不进行舍入，一位数字的小时数设置为带前导零的格式。例如，给定时间为 5:43，则此格式说明符显示“05”。</description>
        ///         </item>
        ///         <item>
        ///             <term>H</term>
        ///             <description>将小时表示为从 0 至 23 的数字，即通过从零开始的 24 小时制表示小时，自午夜开始对小时计数。一位数字的小时数设置为不带前导零的格式。</description>
        ///         </item>
        ///         <item>
        ///             <term>HH, HH（另加任意数量的“H”说明符）</term>
        ///             <description>将小时表示为从 00 至 23 的数字，即通过从零开始的 24 小时制表示小时，自午夜开始对小时计数。一位数字的小时数设置为带前导零的格式。</description>
        ///         </item>
        ///         <item>
        ///             <term>K</term>
        ///             <description>表示 DateTime.Kind 属性的不同值，即“Local”、“Utc”或“Unspecified”。此说明符以文本形式循环设置 Kind 值并保留时区。如果 Kind 值为“Local”，则此说明符等效于“zzz”说明符，用于显示本地时间偏移量，例如“-07:00”。对于“Utc”类型值，该说明符显示字符“Z”以表示 UTC 日期。对于“Unspecified”类型值，该说明符等效于“”（无任何内容）。</description>
        ///         </item>
        ///         <item>
        ///             <term>m</term>
        ///             <description>将分钟表示为从 0 至 59 的数字。分钟表示自前一小时后经过的整分钟数。一位数字的分钟数设置为不带前导零的格式。</description>
        ///         </item>
        ///         <item>
        ///             <term>mm, mm（另加任意数量的“m”说明符）</term>
        ///             <description>将分钟表示为从 00 至 59 的数字。分钟表示自前一小时后经过的整分钟数。一位数字的分钟数设置为带前导零的格式。</description>
        ///         </item>
        ///         <item>
        ///             <term>M</term>
        ///             <description>将月份表示为从 1 至 12 的数字。一位数字的月份设置为不带前导零的格式。</description>
        ///         </item>
        ///         <item>
        ///             <term>MM</term>
        ///             <description>将月份表示为从 01 至 12 的数字。一位数字的月份设置为带前导零的格式。</description>
        ///         </item>
        ///         <item>
        ///             <term>s</term>
        ///             <description>将秒表示为从 0 至 59 的数字。秒表示自前一分钟后经过的整秒数。一位数字的秒数设置为不带前导零的格式。</description>
        ///         </item>
        ///         <item>
        ///             <term>ss, ss（另加任意数量的“s”说明符）</term>
        ///             <description>将秒表示为从 00 至 59 的数字。秒表示自前一分钟后经过的整秒数。一位数字的秒数设置为带前导零的格式。</description>
        ///         </item>
        ///         <item>
        ///             <term>t</term>
        ///             <description>
        ///                 表示当前 <see cref="System.Globalization.DateTimeFormatInfo.AMDesignator" /> 或
        ///                 <see
        ///                     cref="System.Globalization.DateTimeFormatInfo.PMDesignator" />
        ///                 属性中定义的 A.M./P.M. 指示符的第一个字符。如果正在格式化的时间中的小时数小于 12，则使用 A.M. 指示符；否则使用 P.M. 指示符。
        ///             </description>
        ///         </item>
        ///         <item>
        ///             <term>tt, tt（另加任意数量的“t”说明符）</term>
        ///             <description>将 A.M./P.M. 指示符表示为当前 System.Globalization.DateTimeFormatInfo.AMDesignator 或 System.Globalization.DateTimeFormatInfo.PMDesignator 属性中定义的内容。如果正在格式化的时间中的小时数小于 12，则使用 A.M. 指示符；否则使用 P.M. 指示符。</description>
        ///         </item>
        ///         <item>
        ///             <term>y</term>
        ///             <description>将年份表示为最多两位数字。如果年份多于两位数，则结果中仅显示两位低位数。如果年份少于两位数，则该数字设置为不带前导零的格式。</description>
        ///         </item>
        ///         <item>
        ///             <term>yy</term>
        ///             <description>将年份表示为两位数字。如果年份多于两位数，则结果中仅显示两位低位数。如果年份少于两位数，则用前导零填充该数字使之达到两位数。</description>
        ///         </item>
        ///         <item>
        ///             <term>yyy</term>
        ///             <description>
        ///                 将年份表示为三位数字。如果年份多于三位数，则结果中仅显示三位低位数。如果年份少于三位数，则用前导零填充该数字使之达到三位数。
        ///                 <para>请注意，对于年份可以为五位数的泰国佛历，此格式说明符将显示全部五位数。</para>
        ///             </description>
        ///         </item>
        ///         <item>
        ///             <term>yyyy</term>
        ///             <description>
        ///                 将年份表示为四位数字。如果年份多于四位数，则结果中仅显示四位低位数。如果年份少于四位数，则用前导零填充该数字使之达到四位数。
        ///                 <para>请注意，对于年份可以为五位数的泰国佛历，此格式说明符将呈现全部五位数。</para>
        ///             </description>
        ///         </item>
        ///         <item>
        ///             <term>yyyyy（另加任意数量的“y”说明符）</term>
        ///             <description>
        ///                 将年份表示为五位数字。如果年份多于五位数，则结果中仅显示五位低位数。如果年份少于五位数，则用前导零填充该数字使之达到五位数。
        ///                 <para>如果存在额外的“y”说明符，则用所需个数的前导零填充该数字使之达到“y”说明符的数目。</para>
        ///             </description>
        ///         </item>
        ///         <item>
        ///             <term>z</term>
        ///             <description>
        ///                 表示系统时间距格林威治时间 (GMT) 以小时为单位测量的带符号时区偏移量。例如，位于太平洋标准时区中的计算机的偏移量为“-8”。
        ///                 <para>偏移量始终显示为带有前导符号。加号 (+) 指示小时数早于 GMT，减号 (-) 指示小时数迟于 GMT。偏移量范围为 –12 至 +13。一位数字的偏移量设置为不带前导零的格式。偏移量受夏时制影响。</para>
        ///             </description>
        ///         </item>
        ///         <item>
        ///             <term>zz</term>
        ///             <description>
        ///                 表示系统时间距格林威治时间 (GMT) 以小时为单位测量的带符号时区偏移量。例如，位于太平洋标准时区中的计算机的偏移量为“-08”。
        ///                 <para>偏移量始终显示为带有前导符号。加号 (+) 指示小时数早于 GMT，减号 (-) 指示小时数迟于 GMT。偏移量范围为 –12 至 +13。一位数字的偏移量设置为带前导零的格式。偏移量受夏时制影响。</para>
        ///             </description>
        ///         </item>
        ///         <item>
        ///             <term>zzz, zzz（另加任意数量的“z”说明符）</term>
        ///             <description>
        ///                 表示系统时间距格林威治时间 (GMT) 以小时和分钟为单位测量的带符号时区偏移量。例如，位于太平洋标准时区中的计算机的偏移量为“-08:00”。
        ///                 <para>偏移量始终显示为带有前导符号。加号 (+) 指示小时数早于 GMT，减号 (-) 指示小时数迟于 GMT。偏移量范围为 –12 至 +13。一位数字的偏移量设置为带前导零的格式。偏移量受夏时制影响。</para>
        ///             </description>
        ///         </item>
        ///         <item>
        ///             <term>任何其他字符</term>
        ///             <description>所有其他字符被复制到结果字符串中，而且不影响格式化。</description>
        ///         </item>
        ///         <item>
        ///             <term>f</term>
        ///             <description>
        ///                 表示秒部分的最高有效位。
        ///                 <para>请注意，如果“f”格式说明符单独使用，没有其他格式说明符，则该说明符被看作是“f”标准 DateTime 格式说明符（完整日期/时间模式）。有关使用单个格式说明符的更多信息，请参见使用单个自定义格式说明符。将此格式说明符与 ParseExact 或 TryParseExact 方法一起使用时，所用“f”格式说明符的数目指示要分析的秒部分的最高有效位位数。</para>
        ///             </description>
        ///         </item>
        ///         <item>
        ///             <term>ff</term>
        ///             <description>表示秒部分的两个最高有效位。</description>
        ///         </item>
        ///         <item>
        ///             <term>fff</term>
        ///             <description>表示秒部分的三个最高有效位。</description>
        ///         </item>
        ///         <item>
        ///             <term>ffff</term>
        ///             <description>表示秒部分的四个最高有效位。</description>
        ///         </item>
        ///         <item>
        ///             <term>fffff</term>
        ///             <description>表示秒部分的五个最高有效位。</description>
        ///         </item>
        ///         <item>
        ///             <term>ffffff</term>
        ///             <description>表示秒部分的六个最高有效位。</description>
        ///         </item>
        ///         <item>
        ///             <term>fffffff</term>
        ///             <description>表示秒部分的七个最高有效位。</description>
        ///         </item>
        ///         <item>
        ///             <term>F</term>
        ///             <description>
        ///                 表示秒部分的最高有效位。如果该位为零，则不显示任何信息。有关使用单个格式说明符的更多信息，请参见使用单个自定义格式说明符。
        ///                 <para>将此格式说明符与 ParseExact 或 TryParseExact 方法一起使用时，所用“F”格式说明符的数目指示要分析的秒部分的最高有效位最大位数。</para>
        ///             </description>
        ///         </item>
        ///         <item>
        ///             <term>FF</term>
        ///             <description>表示秒部分的两个最高有效位。但不显示尾随零（或两个零位）。</description>
        ///         </item>
        ///         <item>
        ///             <term>FFF</term>
        ///             <description>表示秒部分的三个最高有效位。但不显示尾随零（或三个零位）。</description>
        ///         </item>
        ///         <item>
        ///             <term>FFFF</term>
        ///             <description>表示秒部分的四个最高有效位。但不显示尾随零（或四个零位）。</description>
        ///         </item>
        ///         <item>
        ///             <term>FFFFF</term>
        ///             <description>表示秒部分的五个最高有效位。但不显示尾随零（或五个零位）。</description>
        ///         </item>
        ///         <item>
        ///             <term>FFFFFF</term>
        ///             <description>表示秒部分的六个最高有效位。但不显示尾随零（或六个零位）。</description>
        ///         </item>
        ///         <item>
        ///             <term>FFFFFFF</term>
        ///             <description>表示秒部分的七个最高有效位。但不显示尾随零（或七个零位）。</description>
        ///         </item>
        ///     </list>
        /// </remarks>
        public static string FormatDate(DateTime datetime, string format)
        {
            if (datetime == DateTime.MinValue || datetime == DateTime.MaxValue)
            {
                return string.Empty;
            }
            return datetime.ToString(format);
        }

        /// <summary>
        /// 格式化日期
        /// </summary>
        /// <param name="datetime">日期</param>
        /// <param name="format">格式字符串</param>
        /// <returns>返回日期格式化后的字符串</returns>
        public static string FormatDate(DateTime? datetime, string format)
        {
            if (datetime.HasValue)
            {
                if (datetime.Value == DateTime.MinValue || datetime.Value == DateTime.MaxValue)
                {
                    return string.Empty;
                }
                return datetime.Value.ToString(format);
            }
            return string.Empty;
        }

        /// <summary>
        /// 格式化日期(默认格式yyyy-MM-dd)
        /// </summary>
        /// <param name="datetime">日期</param>
        /// <returns>返回日期格式化后的字符串</returns>
        public static string FormatDate(DateTime datetime)
        {
            return datetime.ToString("yyyy-MM-dd");
        }

        /// <summary>
        /// 格式化日期(默认格式yyyy-MM-dd)
        /// </summary>
        /// <param name="datetime">日期</param>
        /// <returns>返回日期格式化后的字符串</returns>
        public static string FormatDate(DateTime? datetime)
        {
            if (datetime.HasValue)
            {
                return FormatDate(datetime.Value);
            }
            return string.Empty;
        }

        /// <summary>
        /// 格式化日期(默认格式 yyyy-MM-dd HH:mm:ss.FFF)
        /// </summary>
        /// <param name="datetime">日期</param>
        /// <returns>返回日期格式化后的字符串</returns>
        public static string FormatDateHasMilliSecond(DateTime datetime)
        {
            return FormatDate(datetime, "yyyy-MM-dd HH:mm:ss.FFF");
        }

        /// <summary>
        /// 获取格式化的日期字符串(默认格式 yyyy-MM-dd HH:mm:ss)
        /// </summary>
        /// <param name="datetime">指定的日期</param>
        /// <returns>返回日期格式化后的字符串</returns>
        public static string FormatDateHasSecond(DateTime datetime)
        {
            return FormatDate(datetime, "yyyy-MM-dd HH:mm:ss");
        }

        /// <summary>
        /// 获取格式化的日期字符串(默认格式 yyyy-MM-dd HH:mm:ss)
        /// </summary>
        /// <param name="datetime">指定的日期</param>
        /// <returns>返回日期格式化后的字符串</returns>
        public static string FormatDateHasSecond(DateTime? datetime)
        {
            if (datetime.HasValue)
            {
                return FormatDate(datetime.Value, "yyyy-MM-dd HH:mm:ss");
            }
            return string.Empty;
        }

        /// <summary>
        ///  获取格式化的日期字符串(默认格式 yyyy-MM-dd)
        /// </summary>
        /// <param name="datetime"></param>
        /// <returns></returns>
        public static string FormatDateHasThird(DateTime? datetime)
        {
            if (datetime.HasValue)
            {
                return FormatDate(datetime.Value, "yyyy-MM-dd");
            }
            return string.Empty;
        }


        /// <summary>
        /// 获取格式化的日期字符串(默认格式 yyyy-MM-dd HH:mm)
        /// </summary>
        /// <param name="datetime">指定的日期</param>
        /// <returns>返回日期格式化后的字符串</returns>
        public static string FormatDateHasMinute(DateTime datetime)
        {
            return FormatDate(datetime, "yyyy-MM-dd HH:mm");
        }

        /// <summary>
        /// 获取格式化的日期字符串(默认格式 yyyy-MM-dd HH:mm)
        /// </summary>
        /// <param name="datetime">指定的日期</param>
        /// <param name="format">格式字符串</param>
        /// <returns>返回日期格式化后的字符串</returns>
        public static string FormatDateHasMinute(DateTime datetime, string format)
        {
            return FormatDate(datetime, format);
        }

        /// <summary>
        /// 获取格式化的日期字符串(默认格式 yyyy-MM-dd HH:mm)
        /// </summary>
        /// <param name="datetime">指定的日期</param>
        /// <returns>返回日期格式化后的字符串</returns>
        public static string FormatDateHasMinute(DateTime? datetime)
        {
            if (datetime.HasValue)
            {
                return FormatDate(datetime.Value, "yyyy-MM-dd HH:mm");
            }
            return string.Empty;
        }

        /// <summary>
        /// 获取格式化的日期字符串(默认格式 yyyy-MM-dd HH:mm)
        /// </summary>
        /// <param name="datetime">指定的日期</param>
        /// <param name="format">格式字符串</param>
        /// <returns>返回日期格式化后的字符串</returns>
        public static string FormatDateHasMinute(DateTime? datetime, string format)
        {
            if (datetime.HasValue)
            {
                return FormatDate(datetime.Value, format);
            }
            return string.Empty;
        }

        /// <summary>
        /// 获取格式化的时间字符串(默认格式 HH:mm:ss)
        /// </summary>
        /// <param name="datetime">指定的时间</param>
        /// <param name="format">格式字符串</param>
        /// <returns>返回日期格式化后的字符串</returns>
        public static string FormatTime(DateTime datetime,string  format="HH:mm:ss")
        {
            return FormatDate(datetime, format);
        }

        /// <summary>
        /// 获取格式化的时间字符串(默认格式 HH:mm:ss)
        /// </summary>
        /// <param name="datetime">指定的时间</param>
        /// <param name="format">格式字符串</param>
        /// <returns>返回日期格式化后的字符串</returns>
        public static string FormatTime(DateTime? datetime, string format = "HH:mm:ss")
        {
            if (datetime.HasValue)
            {
                return FormatDate(datetime, format);
            }
            return string.Empty;
        }
    }
}