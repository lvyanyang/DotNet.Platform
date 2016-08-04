// ===============================================================================
// DotNet.Platform 开发框架 2016 版权所有
// ===============================================================================
using System;
using System.Collections.Generic;
using System.Text;

namespace DotNet.Helper
{
    /// <summary>
    /// 随机字符操作类
    /// </summary>
    /// <example>
    /// <code>
    /// Win32Helper.AllocConsole();//打开cmd窗口
    /// Console.Title = "随机字符串测试";
    /// Console.ForegroundColor = ConsoleColor.Yellow;
    /// Console.WriteLine("生成9位随机数字:\t{0}", RandomHelper.GenerateRandomNumber(9));
    /// Console.WriteLine("生成9位含特殊字符:\t{0}", RandomHelper.GenerateRandomPassword(9));
    /// Console.WriteLine("生成9位含数字字母:\t{0}", RandomHelper.GenerateRandomString(9));
    /// Console.WriteLine("生成9位小写随机字母:\t{0}", RandomHelper.GenerateRandomString(9, false));
    /// Console.WriteLine("生成9位大写随机字母:\t{0}", RandomHelper.GenerateRandomString(9, true));
    /// Console.WriteLine("-------------------------------------------------");
    /// </code>
    /// <img src="images/RandomHelper.jpg" />
    /// </example>
    public static class RandomHelper
    {
        /// <summary>
        /// 生成随机字符串,字符范围为(0-9 a-b A-B)
        /// </summary>
        /// <param name="randomStringLength">生成字符串长度</param>
        /// <returns>返回字符范围为(0-9 a-b A-B)的字符串</returns>
        public static string GenerateRandomString(int randomStringLength)
        {
            char[] constant =
                {
                    '0', '1', '2', '3', '4', '5', '6', '7', '8', '9',
                    'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't',
                    'u', 'v', 'w', 'x', 'y', 'z',
                    'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T',
                    'U', 'V', 'W', 'X', 'Y', 'Z'
                };
            return GetRandomString(constant, randomStringLength);
        }

        /// <summary>
        /// 生成随机字符串,字符范围为(a-b)或者(A-B),参数<paramref name="isUpper" />控制大小写
        /// </summary>
        /// <param name="randomStringLength">生成字符串长度</param>
        /// <param name="isUpper">是否大写</param>
        /// <returns>返回字符范围为(a-b)或者(A-B)的字符串</returns>
        public static string GenerateRandomString(int randomStringLength, bool isUpper)
        {
            char[] constant =
                {
                    'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r',
                    's', 't', 'u', 'v', 'w', 'x', 'y', 'z'
                };
            string result = GetRandomString(constant, randomStringLength);
            return isUpper ? result.ToUpper() : result;
        }

        /// <summary>
        /// 生成随机密码串,字符范围为(0-9 a-b A-B 特殊字符)
        /// </summary>
        /// <param name="randomStringLength">生成密码串长度</param>
        /// <returns>返回字符范围为(0-9 a-b A-B)的密码串</returns>
        public static string GenerateRandomPassword(int randomStringLength)
        {
            char[] constant = new char[94];
            int j = 0;
            for (int i = 33; i <= 126; i++)//32-126是可见字符,其中32是空格,不使用空格
            {
                constant[j] = Convert.ToChar(i);
                j++;
            }
            return GetRandomString(constant, randomStringLength);
        }

        /// <summary>
        /// 生成随机数字,取值范围为(0-9)
        /// </summary>
        /// <param name="randomStringLength">生成数字长度</param>
        /// <returns>返回取值范围为(0-9)的数字</returns>
        public static long GenerateRandomNumber(int randomStringLength)
        {
            char[] constant = { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' };
            return Convert.ToInt64(GetRandomString(constant, randomStringLength));
        }

        /// <summary>
        /// 获取随机字符串
        /// </summary>
        /// <param name="constant">字符取值数组</param>
        /// <returns></returns>
        public static string GenerateRandomString(string[] constant)
        {
            Random rd = new Random((int)DateTime.Now.Ticks);
            return constant[rd.Next(constant.Length)];
        }

        /// <summary>
        /// 获取一个打乱的整数数组
        /// </summary>
        /// <param name="len">数组长度</param>
        /// <returns></returns>
        public static int[] GenerateRandomArray(int len)
        {
            Random rd = new Random((int)DateTime.Now.Ticks);
            var list = new List<int>(len);
            var newList = new List<int>(len);
            for (int i = 0; i < len; i++)
            {
                list.Add(i);
            }
            while (list.Count > 0)
            {
                var sj = rd.Next(list.Count);
                newList.Add(list[sj]);
                list.RemoveAt(sj);
            }
            
            return newList.ToArray();
        }

        /// <summary>
        /// 打乱数组
        /// </summary>
        /// <param name="source">原始数组</param>
        /// <returns></returns>
        public static string[] RandomArray(string[] source)
        {
            Random rd = new Random((int)DateTime.Now.Ticks);
            var list = new List<string>();
            var newList = new List<string>();
            foreach (var item in source)
            {
                list.Add(item);
            }
            while (list.Count > 0)
            {
                var sj = rd.Next(list.Count);
                newList.Add(list[sj]);
                list.RemoveAt(sj);
            }

            return newList.ToArray();
        }


        /// <summary>
        /// 获取随机字符串
        /// </summary>
        /// <param name="constant">字符取值数组</param>
        /// <param name="randomStringLength">生成随机字符串长度</param>
        /// <returns>返回指定长度的随机字符串</returns>
        private static string GetRandomString(char[] constant, int randomStringLength)
        {
            StringBuilder sb = new StringBuilder(randomStringLength);
            Random rd = new Random((int)DateTime.Now.Ticks);
            for (int i = 0; i < randomStringLength; i++)
            {
                sb.Append(constant[rd.Next(constant.Length)]);
            }
            return sb.ToString();
        }
    }
}