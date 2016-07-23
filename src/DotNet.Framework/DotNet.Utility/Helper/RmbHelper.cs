// ===============================================================================
// DotNet.Platform 开发框架 2016 版权所有
// ===============================================================================
using System;

namespace DotNet.Helper
{
    /// <summary>
    /// 人民币操作类
    /// </summary>
    /// <example>
    /// <code>
    /// using XCI.Helper;
    /// 
    /// Console.WriteLine(RmbHelper.ConvertRmbToUpper(243.34));//贰佰肆拾叁元叁角肆分 //转人民币大写
    /// Console.WriteLine(RmbHelper.ConvertRmbToUpper(223.3m));//贰佰贰拾叁元叁角 //转人民币大写
    /// Console.WriteLine(RmbHelper.ConvertRmbToUpper(253));//贰佰伍拾叁元整 //转人民币大写
    /// Console.WriteLine(RmbHelper.ConvertRmbToUpper(253.346));//贰佰伍拾叁元叁角伍分 //转人民币大写
    /// Console.WriteLine();
    /// Console.WriteLine(RmbHelper.ConvertRmbToUpper("243.34"));//贰佰肆拾叁元叁角肆分 //转人民币大写
    /// Console.WriteLine(RmbHelper.ConvertRmbToUpper("253"));//贰佰伍拾叁元整 //转人民币大写
    /// Console.ReadLine();
    /// </code>
    /// </example>
    public static class RmbHelper
    {
        /// <summary>
        /// 转人民币大写
        /// </summary>
        /// <param name="amount">金额</param>
        /// <returns>返回人民币大写</returns>
        public static string ConvertRmbToUpper(decimal amount)
        {
            const string str1 = "零壹贰叁肆伍陆柒捌玖";
            string str2 = "万仟佰拾亿仟佰拾万仟佰拾元角分";//数字位所对应的汉字 
            //从原num值中取出的值 
            //数字的字符串形式 
            string str5 = "";//人民币大写金额形式 
            //循环变量 
            string ch2 = "";//数字位的汉字读法 
            int nzero = 0;//用来计算连续的零值是几个 
            //int temp;            //从原num值中取出的值 

            amount = Math.Round(Math.Abs(amount), 2);//将num取绝对值并四舍五入取2位小数 
            string str4 = ((long) (amount*100)).ToString();//将num乘100并转换成字符串形式 
            int j = str4.Length;
            if (j > 15)
            {
                return "溢出";
            }
            str2 = str2.Substring(15 - j);//取出对应位数的str2的值。如：200.55,j为5所以str2=佰拾元角分 

            //循环取出每一位需要转换的值 
            int i;
            for (i = 0; i < j; i++)
            {
                string str3 = str4.Substring(i, 1);//取出需转换的某一位的值 
                int temp = Convert.ToInt32(str3);//转换为数字 
                string ch1;//数字的汉语读法 
                if (i != (j - 3) && i != (j - 7) && i != (j - 11) && i != (j - 15))
                {
                    //当所取位数不为元、万、亿、万亿上的数字时 
                    if (str3 == "0")
                    {
                        ch1 = "";
                        ch2 = "";
                        nzero = nzero + 1;
                    }
                    else
                    {
                        if (str3 != "0" && nzero != 0)
                        {
                            ch1 = "零" + str1.Substring(temp*1, 1);
                            ch2 = str2.Substring(i, 1);
                            nzero = 0;
                        }
                        else
                        {
                            ch1 = str1.Substring(temp*1, 1);
                            ch2 = str2.Substring(i, 1);
                            nzero = 0;
                        }
                    }
                }
                else
                {
                    //该位是万亿，亿，万，元位等关键位 
                    if (str3 != "0" && nzero != 0)
                    {
                        ch1 = "零" + str1.Substring(temp*1, 1);
                        ch2 = str2.Substring(i, 1);
                        nzero = 0;
                    }
                    else
                    {
                        if (str3 != "0" && nzero == 0)
                        {
                            ch1 = str1.Substring(temp*1, 1);
                            ch2 = str2.Substring(i, 1);
                            nzero = 0;
                        }
                        else
                        {
                            if (str3 == "0" && nzero >= 3)
                            {
                                ch1 = "";
                                ch2 = "";
                                nzero = nzero + 1;
                            }
                            else
                            {
                                if (j >= 11)
                                {
                                    ch1 = "";
                                    nzero = nzero + 1;
                                }
                                else
                                {
                                    ch1 = "";
                                    ch2 = str2.Substring(i, 1);
                                    nzero = nzero + 1;
                                }
                            }
                        }
                    }
                }
                if (i == (j - 11) || i == (j - 3))
                {
                    //如果该位是亿位或元位，则必须写上 
                    ch2 = str2.Substring(i, 1);
                }
                str5 = str5 + ch1 + ch2;

                if (i == j - 2 && str3 == "0")
                {
                    //最后2位（角分）为0时，加上“整” 
                    str5 = str5 + '整';
                }
            }
            if (amount == 0)
            {
                str5 = "零元整";
            }
            return str5;
        }

        /// <summary>
        /// 转人民币大写
        /// </summary>
        /// <param name="amount">金额</param>
        /// <returns>返回人民币大写</returns>
        public static string ConvertRmbToUpper(double amount)
        {
            return ConvertRmbToUpper(Convert.ToDecimal(amount));
        }

        /// <summary>
        /// 转人民币大写
        /// </summary>
        /// <param name="amount">金额</param>
        /// <returns>返回人民币大写</returns>
        public static string ConvertRmbToUpper(int amount)
        {
            return ConvertRmbToUpper(Convert.ToDecimal(amount));
        }

        /// <summary>
        /// 转人民币大写
        /// </summary>
        /// <param name="amountString">金额</param>
        /// <returns>返回人民币大写</returns>
        public static string ConvertRmbToUpper(string amountString)
        {
            decimal amount;
            return decimal.TryParse(amountString, out amount) ? ConvertRmbToUpper(amount) : string.Empty;
        }
    }
}