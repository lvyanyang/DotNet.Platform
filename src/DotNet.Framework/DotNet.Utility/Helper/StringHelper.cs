// ===============================================================================
// DotNet.Platform 开发框架 2016 版权所有
// ===============================================================================
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using DotNet.Extensions;

namespace DotNet.Helper
{
    /// <summary>
    /// 字符串操作帮助类
    /// </summary>
    /// <example>
    /// <code>
    /// using XCI.Helper;
    /// 
    /// Console.WriteLine(StringHelper.Base64Encrypt("China"));//Q2hpbmE= //对文本进行Base64编码
    /// Console.WriteLine(StringHelper.Base64Decrypt("Q2hpbmE="));//China //对文本进行Base64解码
    /// 
    /// Console.WriteLine(StringHelper.BuildCompleteString(30, 100, "已完成{0}%"));//已完成30% //生成一个完成度百分比字符串  
    /// Console.WriteLine(StringHelper.BuildCompleteString(44, 80, "已完成{0}%"));//已完成55% //生成一个完成度百分比字符串  
    /// 
    /// Console.WriteLine(StringHelper.ConvertToFirstCharUpper("china", '-'));//China //转换字符串首字母为大写 
    /// Console.WriteLine(StringHelper.ConvertToFirstCharUpper("ch-ina", '-'));//Ch-Ina //转换字符串首字母为大写 
    /// 
    /// Console.WriteLine(StringHelper.Md5Encrypt("lvyanyang"));//765517F5D319F96B18CF78A4716BFE13 //MD5加密字符串
    /// Console.WriteLine(StringHelper.GetGuid("N"));//433376296e71459ca5f714d54eacb340 //获取Guid 
    /// Console.WriteLine(StringHelper.GetGuid("D"));//111bd33d-10c1-49a9-9682-524d654d8bd9 //获取Guid 
    /// Console.WriteLine(StringHelper.GetGuid("B"));//{cbe61f68-92f3-4f5a-86dc-9b4aca20813e} //获取Guid 
    /// Console.WriteLine(StringHelper.GetGuid("P"));//(c791a249-9fb3-402d-a034-c965dd2621c0) //获取Guid 
    /// Console.WriteLine(StringHelper.GetDateName(true));//2012-11-22-19-44-17 //获取一个不重复日期格式的名称(2008-11-11-20-20-20)  
    /// Console.WriteLine(StringHelper.GetDateName(false));//20121122194418
    /// 
    /// const string htmlText = "<a href='http://news.baidu.com'>新闻</a><b>网页</b><a href='http://tieba.baidu.com'>贴吧</a>";
    /// Console.WriteLine(StringHelper.ReplaceHtml(htmlText));//新闻网页贴吧 //去除HTML格式  
    /// Console.WriteLine(StringHelper.ReplaceHtml(htmlText, 2));//新闻 //去除HTML格式,多于最大长度则会截断 
    /// 
    /// string[] sz = { "a", "b", "c", "d" };
    /// Console.WriteLine(StringHelper.ArrayToString(sz, "-"));//a-b-c-d //数组转为字符串,用指定符号连接
    /// Console.WriteLine(StringHelper.ArrayToString(sz, ","));//a,b,c,d //数组转为字符串,用指定符号连接
    /// 
    /// Console.WriteLine(StringHelper.GetSpaceSizeString(288749232128));//268.92 GB //获取占用空间大小说明,如(30.1 GB 20.2 MB)  
    /// 
    /// Console.WriteLine(StringHelper.GetFixedLengthString("我", 5, "x", true)); //xxxx我 //生成指定长度的字符串,长度不够用指定的字符补上
    /// Console.WriteLine(StringHelper.GetFixedLengthString("我", 5, "x", false));//我xxxx //生成指定长度的字符串,长度不够用指定的字符补上
    /// Console.WriteLine(StringHelper.GetFixedLengthString("我", 5, "xy", false));//我xyxyxyxy //生成指定长度的字符串,长度不够用指定的字符补上
    /// Console.WriteLine(StringHelper.GetRepeatString("你", 4));//你你你你 //重复字符串  
    /// Console.WriteLine(StringHelper.GetRepeatString("你我", 4));//你我你我你我你我 //重复字符串  
    /// 
    /// const string title = "热烈庆祝十八大胜利召开!";
    /// Console.WriteLine(StringHelper.SubStringA(title, 8, "\n")); //截取字符串,不限制字符串长度 
    /// //热烈庆祝十八大胜
    /// //利召开!
    /// 
    /// Console.WriteLine(StringHelper.SubStringB(title, 8, "。。。"));//热烈庆祝十八大胜。。。 //截断字符串,多于最大长度则截断字符串并用指定的字符代替  
    /// Console.WriteLine(StringHelper.TruncateStart(title, 3)); //祝十八大胜利召开! //从开始位置开始移除字符串  
    /// Console.WriteLine(StringHelper.TruncateStart(title, "十"));//八大胜利召开!  //从开始位置开始移除字符串 
    /// Console.WriteLine(StringHelper.TruncateEnd(title, 3)); //热烈庆祝十八大胜利 //从最后位置开始移除字符串  
    /// Console.WriteLine(StringHelper.TruncateEnd(title, "胜")); //热烈庆祝十八大 //从最后位置开始移除字符串
    /// </code>
    /// </example>
    public static class StringHelper
    {
        /// <summary>
        /// 对称加密密钥
        /// </summary>
        public const string SecretKey = "!@#$TFFhanjianpdfdjk)(*&88330921NVJ878";

        /// <summary>
        /// 去除HTML格式
        /// </summary>
        /// <param name="htmlText">html字符串</param>
        /// <returns>返回纯文本</returns>
        public static string ReplaceHtml(string htmlText)
        {
            string stroutput = string.Empty;
            if (htmlText != null)
            {
                stroutput = htmlText.Trim();
                Regex regex = new Regex(RegexConstant.HtmlMark);
                stroutput = regex.Replace(stroutput, "");
                stroutput = stroutput.Replace("\n", "");
                stroutput = stroutput.Replace("\r", "");
                stroutput = stroutput.Replace("&nbsp;", "");
                stroutput = stroutput.Replace("<br>", "").Replace("<BR>", "");
                //htmlText = Regex.Replace(htmlText, "[\\s]{2,}", " ");//替换连个以上空格
                //htmlText = Regex.Replace(htmlText, "(<[b|B][r|R]/*>)+|(<[p|P](.|\\n)*?>)", "\n");//替换<br>
                //htmlText = Regex.Replace(htmlText, "(\\s*&[n|N][b|B][s|S][p|P];\\s*)+", " ");//替换&nbsp;
                //htmlText = Regex.Replace(htmlText, "<(.|\\n)*?>", string.Empty);//替换其他html标记
                //htmlText = htmlText.Replace("'", "''");
            }
            return stroutput;
        }

        /// <summary>
        /// 去除HTML格式,多于最大长度则会截断
        /// </summary>
        /// <param name="htmlText">html字符串</param>
        /// <param name="maxLength">最大长度</param>
        /// <returns>返回纯文本</returns>
        public static string ReplaceHtml(string htmlText, int maxLength)
        {
            var txt = ReplaceHtml(htmlText);
            if (maxLength > 0 && txt.Length > maxLength)
            {
                return txt.Substring(0, maxLength);
            }
            return txt;
        }

        /// <summary>
        /// 获取Guid
        /// </summary>
        /// <param name="format">
        /// 格式参数
        /// <para>
        /// <list type="table">
        /// <item>
        /// <term>N</term>
        /// <description>32 位：<br />00000000000000000000000000000000</description>
        /// </item>
        /// <item>
        /// <term>D</term>
        /// <description>由连字符分隔的 32 位数字：<br />00000000-0000-0000-0000-000000000000</description>
        /// </item>
        /// <item>
        /// <term>B</term>
        /// <description>括在大括号中、由连字符分隔的 32 位数字：<br />{00000000-0000-0000-0000-000000000000}</description>
        /// </item>
        /// <item>
        /// <term>P</term>
        /// <description>括在圆括号中、由连字符分隔的 32 位数字：<br />(00000000-0000-0000-0000-000000000000)</description>
        /// </item>
        /// </list>
        /// </para>
        /// </param>
        public static string Guid(string format)
        {
            return System.Guid.NewGuid().ToString(format);
        }

        /// <summary>
        /// 获取Guid,默认格式为N
        /// </summary>
        /// <returns>返回Guid字符串</returns>
        public static string Guid()
        {
            return System.Guid.NewGuid().ToString("N");
        }

        /// <summary>
        /// 根据当前文件名称生成一个Guid文件名
        /// </summary>
        /// <param name="fileName">当前文件名称</param>
        public static string BuildGuidFileName(string fileName)
        {
            return Guid() + Path.GetExtension(fileName);
        }

        /// <summary>
        /// 获取一个不重复日期格式的名称(2008-11-11-20-20-20),延时一秒保证不重复
        /// </summary>
        /// <param name="hasLine">是否包含分割线</param>
        /// <returns>返回的日期格式的名称</returns>
        public static string GetDateName(bool hasLine)
        {
            Thread.Sleep(1000);
            string str1 = DateTime.Now.Year + "-";
            str1 += (DateTime.Now.Month).ToString().Length < 2
                        ? "0" + DateTime.Now.Month + "-"
                        : DateTime.Now.Month + "-";
            str1 += DateTime.Now.Day.ToString().Length < 2 ? "0" + DateTime.Now.Day + "-" : DateTime.Now.Day + "-";
            str1 += DateTime.Now.Hour.ToString().Length < 2 ? "0" + DateTime.Now.Hour + "-" : DateTime.Now.Hour + "-";
            str1 += DateTime.Now.Minute.ToString().Length < 2
                        ? "0" + DateTime.Now.Minute + "-"
                        : DateTime.Now.Minute + "-";
            str1 += DateTime.Now.Second.ToString().Length < 2
                        ? "0" + DateTime.Now.Second
                        : DateTime.Now.Second.ToString();
            if (!hasLine)
            {
                str1 = str1.Replace("-", "");
            }
            return str1;
        }

        /// <summary>
        /// MD5加密字符串(32位小写)
        /// </summary>
        /// <param name="text">待加密的字符串</param>
        /// <returns>返回密文</returns>
        public static string Md5Encrypt(string text)
        {
            StringBuilder sBuilder = new StringBuilder();
            using (MD5 md5Hash = MD5.Create())
            {
                byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(text));

                for (int i = 0; i < data.Length; i++)
                {
                    sBuilder.Append(data[i].ToString("x2"));
                }
            }
            return sBuilder.ToString();
            //return FormsAuthentication.HashPasswordForStoringInConfigFile(text, FormsAuthPasswordFormat.MD5.ToString());
        }

        /// <summary>
        /// MD5加密字符串(16位小写)
        /// </summary>
        /// <param name="text">待加密的字符串</param>
        /// <returns>返回密文</returns>
        public static string Md5Encrypt16(string text)
        {
            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
            string t2 = BitConverter.ToString(md5.ComputeHash(Encoding.UTF8.GetBytes(text)), 4, 8);
            t2 = t2.Replace("-", "");
            t2 = t2.ToLower();
            return t2;
        }

        /// <summary>
        /// 对称加密
        /// </summary>
        /// <param name="plainText">明文</param>
        /// <param name="secretKey">密钥</param>
        /// <returns>密文</returns>
        public static string SymmetricEncrypt(string plainText, string secretKey = SecretKey)
        {
            SymmetricAlgorithm algorithm = new TripleDESCryptoServiceProvider();
            MD5CryptoServiceProvider hashMd5 = new MD5CryptoServiceProvider();
            algorithm.Key = hashMd5.ComputeHash(Encoding.UTF8.GetBytes(secretKey));
            algorithm.Mode = CipherMode.ECB;
            ICryptoTransform transformer = algorithm.CreateEncryptor();
            byte[] buffer = Encoding.UTF8.GetBytes(plainText);
            return Convert.ToBase64String(transformer.TransformFinalBlock(buffer, 0, buffer.Length));
        }

        /// <summary>
        /// 对称解密
        /// </summary>
        /// <param name="encryptText">密文</param>
        /// <param name="secretKey">密钥</param>
        /// <returns>明文</returns>
        public static string SymmetricDecrypt(string encryptText, string secretKey = SecretKey)
        {
            if (string.IsNullOrEmpty(encryptText))
            {
                return string.Empty;
            }
            SymmetricAlgorithm algorithm = new TripleDESCryptoServiceProvider();
            MD5CryptoServiceProvider hashMd5 = new MD5CryptoServiceProvider();
            algorithm.Key = hashMd5.ComputeHash(Encoding.UTF8.GetBytes(secretKey));
            algorithm.Mode = CipherMode.ECB;
            ICryptoTransform transformer = algorithm.CreateDecryptor();
            byte[] buffer = Convert.FromBase64String(encryptText);
            return Encoding.UTF8.GetString(transformer.TransformFinalBlock(buffer, 0, buffer.Length));
        }

        /// <summary>
        /// 加密连接字符串
        /// </summary>
        /// <param name="connectionString">连接字符串</param>
        /// <returns>返回加密后的连接字符串</returns>
        public static string EncryptString(string connectionString)
        {
            if (String.IsNullOrEmpty(connectionString)) return connectionString;
            if (StringHelper.IsBase64(connectionString)) return connectionString;
            return StringHelper.SymmetricEncrypt(connectionString);
        }

        /// <summary>
        /// 解密连接字符串
        /// </summary>
        /// <param name="connectionString">连接字符串</param>
        /// <returns>返回解密后的连接字符串</returns>
        public static string DecryptString(string connectionString)
        {
            if (String.IsNullOrEmpty(connectionString)) return connectionString;
            if (StringHelper.IsBase64(connectionString))
            {
                return StringHelper.SymmetricDecrypt(connectionString);
            }
            return connectionString;
        }

        /// <summary>
        /// 获取占用空间大小说明,如(30.1 GB  20.2 MB)
        /// </summary>
        /// <param name="size">字节数</param>
        /// <returns>返回占用空间大小说明</returns>
        public static string GetSpaceSizeString(long size)
        {
            #region

            decimal num;
            string strResult;

            if (size > 1073741824)
            {
                num = (Convert.ToDecimal(size) / Convert.ToDecimal(1073741824));
                strResult = num.ToString("N") + " GB";
            }
            else if (size > 1048576)
            {
                num = (Convert.ToDecimal(size) / Convert.ToDecimal(1048576));
                strResult = num.ToString("N") + " MB";
            }
            else if (size > 1024)
            {
                num = (Convert.ToDecimal(size) / Convert.ToDecimal(1024));
                strResult = num.ToString("N") + " KB";
            }
            else
            {
                strResult = size + " B";
            }

            return strResult;

            #endregion
        }

        /// <summary>
        /// 生成一个完成度百分比字符串
        /// </summary>
        /// <param name="current">当前进度</param>
        /// <param name="sum">总进度</param>
        /// <param name="format">显示格式化,例如:完成度{0}</param>
        /// <returns>返回完成度百分比字符串</returns>
        public static string BuildCompleteString(int current, int sum, string format)
        {
            int progress = Convert.ToInt32(Convert.ToDecimal(current) / Convert.ToDecimal(sum) * 100);
            return string.Format(format, progress);
        }

        /// <summary>
        /// 重复字符串
        /// </summary>
        /// <param name="text">源字符串</param>
        /// <param name="times">重复次数</param>
        /// <returns>返回重复后的字符串</returns>
        public static string GetRepeatString(string text, int times)
        {
            if (string.IsNullOrEmpty(text) || times == 0)
            {
                return string.Empty;
            }

            string strfinal = string.Empty;
            for (int i = 0; i < times; i++)
            {
                strfinal += text;
            }

            return strfinal;
        }

        /// <summary>
        /// 获取连续的数字数组
        /// </summary>
        /// <param name="start">开始数字</param>
        /// <param name="end">结束数字</param>
        /// <returns></returns>
        public static string[] GetSequenceArray(int start, int end)
        {
            List<string> list = new List<string>();
            for (int i = start; i <= end; i++)
            {
                list.Add(i.ToString());
            }
            return list.ToArray();
        }

        /// <summary>
        /// 生成指定长度的字符串,长度不够用指定的字符补上
        /// </summary>
        /// <param name="text">文本</param>
        /// <param name="length">生成的文本总长度</param>
        /// <param name="paddingChar">补充字符串</param>
        /// <param name="isLeft">是否补在左边</param>
        /// <returns>返回指定长度的字符串</returns>
        public static string GetFixedLengthString(string text, int length, string paddingChar, bool isLeft)
        {
            int leftOver = length - text.Length;
            string finalText = text;
            for (int ndx = 0; ndx < leftOver; ndx++)
            {
                finalText = isLeft ? string.Concat(paddingChar, finalText) : string.Concat(finalText, paddingChar);
            }
            return finalText;
        }

        ///// <summary>
        ///// 在字符串后面增加\t字符串,当字符串长度小于指定的长度则增加\t
        ///// </summary>
        ///// <param name="text">操作的字符</param>
        ///// <param name="length">最小长度</param>
        ///// <param name="tabCount">\t个数</param>
        //public static string AutoAddTab(string text, int length, int tabCount)
        //{
        //    if (text.Length < length)
        //    {
        //        StringBuilder sb = new StringBuilder(text);
        //        for (int i = 0; i < tabCount; i++)
        //        {
        //            sb.Append("\t");
        //        }
        //        return sb.ToString();
        //    }
        //    return text;
        //}

        /// <summary>
        /// 转换字符串首字母为大写
        /// </summary>
        /// <param name="text">字符串</param>
        /// <param name="delimiter">检测分隔符</param>
        /// <returns>返回首字母为大写的字符串</returns>
        /// <remarks>
        /// UPPER = Upper
        /// lower = Lower
        /// MiXEd = Mixed
        /// </remarks>
        public static string ConvertToFirstCharUpper(string text, char delimiter)
        {
            if (string.IsNullOrEmpty(text))
                return text;

            text = text.Trim();
            if (string.IsNullOrEmpty(text))
                return text;

            if (text.IndexOf(delimiter) < 0)
            {
                text = text.ToLower();
                text = text[0].ToString().ToUpper() + text.Substring(1);
                return text;
            }

            string[] tokens = text.Split(delimiter);
            StringBuilder buffer = new StringBuilder();

            foreach (string token in tokens)
            {
                string currentToken = token.ToLower();
                currentToken = currentToken[0].ToString().ToUpper() + currentToken.Substring(1);
                buffer.Append(currentToken + delimiter);
            }

            text = buffer.ToString();
            return text.TrimEnd(delimiter);
        }

        /// <summary>
        /// 数组转为字符串,用指定符号连接
        /// </summary>
        /// <param name="items">集合</param>
        /// <param name="delimeter">分割符号</param>
        /// <param name="pix">元素修饰符,例如'1','2','3' 修饰符是'</param>
        /// <returns>返回数组元素用指定符号连接的字符串</returns>
        public static string ConvertArrayToString(IEnumerable items, string delimeter = ",", string pix = "")
        {
            if (items == null)
            {
                return string.Empty;
            }
            List<string> itemList = new List<string>();
            foreach (object item in items)
            {
                itemList.Add(pix + item + pix);
            }
            return string.Join(delimeter, itemList.ToArray());
        }

        /// <summary>
        /// 字符串转为字符数组,用指定符号分割
        /// </summary>
        /// <param name="text">字符串</param>
        /// <param name="delimeter">分割符号</param>
        /// <returns>返回字符数组</returns>
        public static string[] ConvertStringToArray(string text, string delimeter = ",")
        {
            if (string.IsNullOrEmpty(text))
                return new string[0];

            string[] tokens = text.Split(new[] { delimeter }, StringSplitOptions.RemoveEmptyEntries);
            return tokens?? new string[0];
        }

        /// <summary>
        /// 字符串转为整数数组,用指定符号分割
        /// </summary>
        /// <param name="text">字符串</param>
        /// <param name="delimeter">分割符号</param>
        /// <returns>返回整数数组</returns>
        public static int[] ConvertToArrayInt(string text, string delimeter = ",")
        {
            var sz = ConvertStringToArray(text, delimeter);
            if (sz == null) return null;
            int[] newsz = new int[sz.Length];
            for (int index = 0; index < sz.Length; index++)
            {
                newsz[index] = Convert.ToInt32(sz[index]);
            }
            return newsz;
        }

        /// <summary>
        /// 转换整形数组为字符串数组
        /// </summary>
        /// <param name="array">整形数组</param>
        public static string[] ConvertToStringArray(int[] array)
        {
            string[] ar = new string[array.Length];
            for (int i = 0; i < ar.Length; i++)
            {
                ar[i] = array[i].ToString();
            }
            return ar;
        }

        /// <summary>
        /// 转换字符串数组为整形数组
        /// </summary>
        /// <param name="array">字符串数组</param>
        public static int[] ConvertToIntArray(string[] array)
        {
            int[] ar = new int[array.Length];
            for (int i = 0; i < ar.Length; i++)
            {
                ar[i] = array[i].ToInt();
            }
            return ar;
        }

        /// <summary>
        /// 生成异常信息
        /// </summary>
        /// <param name="ex">异常对象</param>
        public static string BuildExceptionDetails(Exception ex)
        {
            string targetSiteFullName = string.Empty;
            if (ex.TargetSite.DeclaringType != null)
            {
                targetSiteFullName = ex.TargetSite.DeclaringType.FullName;
            }
            return string.Format("消息:{0}\r\n异常类:{1}\r\n异常方法:{2}\r\n异常程序集:{3}\r\n堆栈跟踪:\r\n{4}",
                ex.Message, targetSiteFullName, ex.TargetSite.Name, ex.Source, ex.StackTrace);
        }

        /// <summary>
        /// 计算文件的 MD5 值
        /// </summary>
        /// <param name="fileName">要计算 MD5 值的文件名和路径</param>
        /// <returns>MD5 值16进制字符串</returns>
        public static string MD5File(string fileName)
        {
            return HashFile(fileName, "md5");
        }

        /// <summary>
        /// 计算文件的 sha1 值
        /// </summary>
        /// <param name="fileName">要计算 sha1 值的文件名和路径</param>
        /// <returns>sha1 值16进制字符串</returns>
        public static string SHA1File(string fileName)
        {
            return HashFile(fileName, "sha1");
        }

        /// <summary>
        /// 计算文件的哈希值
        /// </summary>
        /// <param name="fileName">要计算哈希值的文件名和路径</param>
        /// <param name="algName">算法:sha1,md5</param>
        /// <returns>哈希值16进制字符串</returns>
        private static string HashFile(string fileName, string algName)
        {
            if (!System.IO.File.Exists(fileName))
                return string.Empty;

            System.IO.FileStream fs = new System.IO.FileStream(fileName, System.IO.FileMode.Open, System.IO.FileAccess.Read);
            byte[] hashBytes = HashData(fs, algName);
            fs.Close();
            return ByteArrayToHexString(hashBytes);
        }

        /// <summary>
        /// 计算哈希值
        /// </summary>
        /// <param name="stream">要计算哈希值的 Stream</param>
        /// <param name="algName">算法:sha1,md5</param>
        /// <returns>哈希值字节数组</returns>
        private static byte[] HashData(System.IO.Stream stream, string algName)
        {
            System.Security.Cryptography.HashAlgorithm algorithm;
            if (algName == null)
            {
                throw new ArgumentNullException("algName", "algName 不能为 null");
            }
            if (String.Compare(algName, "sha1", StringComparison.OrdinalIgnoreCase) == 0)
            {
                algorithm = System.Security.Cryptography.SHA1.Create();
            }
            else
            {
                if (String.Compare(algName, "md5", StringComparison.OrdinalIgnoreCase) != 0)
                {
                    throw new Exception("algName 只能使用 sha1 或 md5");
                }
                algorithm = System.Security.Cryptography.MD5.Create();
            }
            return algorithm.ComputeHash(stream);
        }

        /// <summary>
        /// 字节数组转换为16进制表示的字符串
        /// </summary>
        private static string ByteArrayToHexString(byte[] buf)
        {
            return BitConverter.ToString(buf).Replace("-", "");
        }

        /// <summary>
        /// 获取字符串首字母
        /// </summary>
        /// <param name="str">字符串</param>
        /// <returns>返回字符串首字母</returns>
        public static string GetFirstAlpha(string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                return string.Empty;
            }
            return SpellHelper.GetSpell(str.Substring(0, 1));
        }

        /// <summary>
        /// 生成一个分割字符串,使用一个数字范围
        /// </summary>
        /// <param name="start">开始</param>
        /// <param name="end">结束</param>
        /// <param name="spliter">分割符号</param>
        /// <returns></returns>
        public static string GetSplitStringByRange(int start, int end, string spliter = ",")
        {
            StringBuilder sb = new StringBuilder();
            for (int i = start; i <= end; i++)
            {
                sb.Append(i);
                sb.Append(spliter);
            }
            return sb.ToString().TrimEnd(spliter[0]);
        }

        /// <summary>
        /// 获取字符串数组使用指定的范围
        /// </summary>
        /// <param name="start">开始</param>
        /// <param name="end">结束</param>
        /// <returns></returns>
        public static string[] GetArrayByRange(int start, int end)
        {
            List<string> list = new List<string>();
            for (int i = start; i <= end; i++)
            {
                list.Add(i.ToString());
            }
            return list.ToArray();
        }

        #region Base64 编码

        /// <summary>
        /// 对文本进行Base64编码
        /// </summary>
        /// <param name="text">文本</param>
        /// <returns>返回Base64编码后的文本</returns>
        public static string Base64Encrypt(string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                return string.Empty;
            }
            return Convert.ToBase64String(Encoding.UTF8.GetBytes(text));
        }

        /// <summary>
        /// 对文本进行Base64解码
        /// </summary>
        /// <param name="text">Base64编码文本</param>
        /// <returns>返回Base64解码后的文本</returns>
        public static string Base64Decrypt(string text)
        {
            if (!IsBase64(text))
            {
                return text;
            }
            byte[] buffer = Convert.FromBase64String(text);
            return Encoding.UTF8.GetString(buffer);
        }

        /// <summary>
        /// 文本是否是Base64编码字符串
        /// </summary>
        /// <param name="text">字符串</param>
        /// <returns>如果是Base64编码字符串返回true</returns>
        public static bool IsBase64(string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                return false;
            }
            return (text.Length % 4) == 0 && Regex.IsMatch(text, "^[A-Z0-9/+=]*$", RegexOptions.IgnoreCase);
        }

        #endregion

        #region 截取字符串

        /// <summary>
        /// 截取字符串,不限制字符串长度
        /// </summary>
        /// <param name="text">所要截取的字符串</param>
        /// <param name="maxLen">每行的长度，多于这个长度自动换行</param>
        /// <param name="newLineSymbol">换行符号</param>
        /// <returns>截取之后的字符串</returns>
        public static string SubStringA(string text, int maxLen, string newLineSymbol)
        {
            if (text.Length < maxLen)
                return text;
            StringBuilder sb = new StringBuilder();
            int textLength = text.Length;
            int count = textLength / maxLen;
            for (int i = 0; i < count; i++)
            {
                sb.Append(text.Substring(i * maxLen, maxLen));
                sb.Append(newLineSymbol);
            }
            int ys = textLength % maxLen;
            if (ys > 0)
            {
                sb.Append(text.Substring(textLength - ys, ys));
            }
            return sb.ToString();
        }

        /// <summary>
        /// 截断字符串,多于最大长度则截断字符串并用指定的字符代替
        /// </summary>
        /// <param name="text">源字符串</param>
        /// <param name="maxLen">最大长度</param>
        /// <param name="suffix">指定的字符,例如...</param>
        /// <returns>截取之后的字符串</returns>
        public static string SubStringB(string text, int maxLen, string suffix = "...")
        {
            if (string.IsNullOrEmpty(text))
                return text;

            if (text.Length <= maxLen)
                return text;

            string partial = text.Substring(0, maxLen);
            return partial + suffix;
        }

        /// <summary>
        /// 从最后位置开始移除字符串
        /// </summary>
        /// <param name="sourceString">源字符串</param>
        /// <param name="removeCount">移除个数</param>
        /// <returns>截取之后的字符串</returns>
        public static string TruncateEnd(string sourceString, int removeCount)
        {
            string result = sourceString;
            if ((removeCount > 0) && (sourceString.Length > removeCount - 1))
            {
                result = result.Remove(sourceString.Length - removeCount, removeCount);
            }
            return result;
        }

        /// <summary>
        /// 从最后位置开始移除字符串
        /// </summary>
        /// <param name="sourceString">源字符串</param>
        /// <param name="backDownTo">搜索字符串,计算字符串的位置以确定移除个数</param>
        /// <returns>截取之后的字符串</returns>
        public static string TruncateEnd(string sourceString, string backDownTo)
        {
            int removeDownTo = sourceString.LastIndexOf(backDownTo, StringComparison.Ordinal);
            int removeFromEnd = 0;
            if (removeDownTo > -1)
            {
                removeFromEnd = sourceString.Length - removeDownTo;
            }

            if (sourceString.Length > removeFromEnd - 1)
            {
                return sourceString.Remove(removeDownTo, removeFromEnd);
            }

            return sourceString;
        }

        /// <summary>
        /// 从开始位置开始移除字符串
        /// </summary>
        /// <param name="sourceString">源字符串</param>
        /// <param name="removeCount">移除个数</param>
        /// <returns>截取之后的字符串</returns>
        public static string TruncateStart(string sourceString, int removeCount)
        {
            string result = sourceString;
            if (sourceString.Length > removeCount)
            {
                result = result.Remove(0, removeCount);
            }
            return result;
        }

        /// <summary>
        /// 从开始位置开始移除字符串
        /// </summary>
        /// <param name="sourceString">源字符串</param>
        /// <param name="removeUpTo">搜索字符串,计算字符串的位置以确定移除个数</param>
        /// <returns>截取之后的字符串</returns>
        public static string TruncateStart(string sourceString, string removeUpTo)
        {
            int removeFromBeginning = sourceString.IndexOf(removeUpTo, StringComparison.Ordinal);

            if (removeFromBeginning > -1 && sourceString.Length > removeFromBeginning)
            {
                return sourceString.Remove(0, removeFromBeginning + 1);
            }

            return sourceString;
        }

        #endregion

        #region 编码转换

        /// <summary>
        /// 把gb2312编码字符串转为iso8859-1编码
        /// </summary>
        /// <param name="str">待转字符串</param>
        public static string ConvertGb2312ToIso8859(string str)
        {
            Encoding iso8859 = System.Text.Encoding.GetEncoding("iso8859-1");
            Encoding gb2312 = System.Text.Encoding.GetEncoding("gb2312");
            byte[] gb = gb2312.GetBytes(str);
            return iso8859.GetString(gb);
        }

        /// <summary>
        /// 把iso8859-1编码字符串转为gb2312编码
        /// </summary>
        /// <param name="str">待转字符串</param>
        public static string ConvertIso8859ToGb2312(string str)
        {
            Encoding iso8859 = System.Text.Encoding.GetEncoding("iso8859-1");
            Encoding gb2312 = System.Text.Encoding.GetEncoding("gb2312");
            byte[] iso = iso8859.GetBytes(str);
            return gb2312.GetString(iso);
        }

        /// <summary>
        /// 把gb2312编码字符串转为UTF8编码
        /// </summary>
        /// <param name="str">待转字符串</param>
        public static string ConvertGb2312ToUtf8(string str)
        {
            Encoding uft8 = System.Text.Encoding.UTF8;
            Encoding gb2312 = System.Text.Encoding.GetEncoding("gb2312");
            byte[] gb = gb2312.GetBytes(str);
            return uft8.GetString(gb);
        }

        /// <summary>
        /// 把UTF8编码字符串转为gb2312编码
        /// </summary>
        /// <param name="str">待转字符串</param>
        public static string ConvertUtf8ToGb2312(string str)
        {
            Encoding uft8 = System.Text.Encoding.UTF8;
            Encoding gb2312 = System.Text.Encoding.GetEncoding("gb2312");
            byte[] gb = uft8.GetBytes(str);
            return gb2312.GetString(gb);
        }

        #endregion
    }
}