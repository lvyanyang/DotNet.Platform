// ===============================================================================
// DotNet.Platform 开发框架 2016 版权所有
// ===============================================================================
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Xsl;

namespace DotNet.Helper
{
    /// <summary>
    /// Xml操作类
    /// </summary>
    public class XmlHelper
    {
        #region Xml序列化

        /// <summary>
        /// Xml序列化
        /// </summary>
        /// <param name="data">待序列化的对象</param>
        /// <returns>序列化后的字符串</returns>
        public static string XmlSerialize(object data)
        {
            if (data == null)
            {
                throw new ArgumentNullException("data");
            }
            System.Xml.Serialization.XmlSerializer serializer = new System.Xml.Serialization.XmlSerializer(data.GetType());
            MemoryStream stream = new MemoryStream();
            XmlWriterSettings settings = new XmlWriterSettings
            {
                Encoding = Encoding.UTF8,
                OmitXmlDeclaration = false,
                Indent = true,
                IndentChars = "\t",
                NewLineChars = Environment.NewLine,
                ConformanceLevel = ConformanceLevel.Document
            };
            System.Xml.Serialization.XmlSerializerNamespaces xmlns = new System.Xml.Serialization.XmlSerializerNamespaces();
            xmlns.Add(string.Empty, string.Empty);
            using (XmlWriter writer = XmlWriter.Create(stream, settings))
            {
                serializer.Serialize(writer, data, xmlns);
            }
            return Encoding.UTF8.GetString(stream.ToArray());
        }

        /// <summary>
        /// Xml序列化对象
        /// </summary>
        /// <param name="path">文件路径</param>
        /// <param name="data">序列化对象</param>
        public static void XmlSerializeByPath(string path, object data)
        {
            FileHelper.CreateDirectoryByPath(path);
            File.WriteAllText(path, XmlSerialize(data));
        }

        /// <summary>
        /// Xml序列化对象
        /// </summary>
        /// <param name="path">文件路径</param>
        /// <param name="list">序列化对象</param>
        public static void XmlSerializeAttributeByPath<T>(string path, List<T> list)
        {
            var doc = new System.Xml.XmlDocument();
            XmlNode rootNode = null;
            Type dataType = typeof(T);
            string rootName = string.Concat(dataType.Name, "s");
            if (!File.Exists(path))
            {
                var dec = doc.CreateXmlDeclaration("1.0", "utf-8", null);
                doc.AppendChild(dec);
            }
            else
            {
                doc.Load(path);
                rootNode = doc.SelectSingleNode(rootName);
            }
            if (rootNode == null)
            {
                rootNode = doc.CreateElement(rootName);
                doc.AppendChild(rootNode);
            }
            if (list.Count > 0)
            {
                FileHelper.CreateDirectoryByPath(path);
                rootNode.RemoveAll();
            }
            foreach (T item in list)
            {
                var ele = doc.CreateElement(dataType.Name);
                foreach (var proInfo in dataType.GetProperties())
                {
                    object valueObj = proInfo.GetValue(item, null);
                    if (valueObj != null)
                    {
                        ele.SetAttribute(proInfo.Name, valueObj.ToString());
                    }
                }
                rootNode.AppendChild(ele);
            }

            doc.Save(path);
        }

        /// <summary>
        /// Xml反序列化对象
        /// </summary>
        /// <param name="data">序列化内容</param>
        /// <param name="objType">对象类型</param>
        /// <returns>序列化对象</returns>
        public static object XmlDeserialize(string data, Type objType)
        {
            System.Xml.Serialization.XmlSerializer serializer = new System.Xml.Serialization.XmlSerializer(objType);
            using (TextReader reader = new StringReader(data))
            {
                return serializer.Deserialize(reader);
            }
        }

        /// <summary>
        /// Xml反序列化对象
        /// </summary>
        /// <param name="path">文件路径</param>
        /// <param name="objType">对象类型</param>
        /// <returns>序列化对象</returns>
        public static object XmlDeserializeByPath(string path, Type objType)
        {
            if (!File.Exists(path)) return null;

            string data = File.ReadAllText(path, Encoding.UTF8);
            if (!string.IsNullOrEmpty(data))
            {
                return XmlDeserialize(data, objType);
            }

            return null;
        }

        /// <summary>
        /// Xml反序列化对象
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="data">序列化内容</param>
        /// <returns>序列化对象</returns>
        public static T XmlDeserialize<T>(string data)
        {
            return (T)XmlDeserialize(data, typeof(T));
        }

        /// <summary>
        /// Xml反序列化对象
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="path">读取文件路径</param>
        /// <returns>序列化对象</returns>
        public static T XmlDeserializeByPath<T>(string path)
        {
            return (T)XmlDeserializeByPath(path, typeof(T));
        }

        /// <summary>
        /// Xml反序列化对象
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="path">读取文件路径</param>
        /// <returns>序列化对象</returns>
        public static List<T> XmlDeserializeAttributeByPath<T>(string path)
        {
            if (!File.Exists(path)) return null;
            System.Xml.XmlDocument doc = new System.Xml.XmlDocument();
            Type dataType = typeof(T);
            string rootName = string.Concat(dataType.Name, "s");
            doc.Load(path);

            var list = new List<T>();
            var selectSingleNode = doc.SelectSingleNode(rootName);
            if (selectSingleNode == null) return list;
            XmlNodeList nodeList = selectSingleNode.ChildNodes;
            foreach (XmlNode xn in nodeList)
            {
                XmlElement xe = (XmlElement)xn;
                T item = Activator.CreateInstance<T>();
                foreach (var proInfo in dataType.GetProperties())
                {
                    proInfo.SetValue(item, ObjectHelper.ConvertObjectValue(xe.GetAttribute(proInfo.Name),proInfo.PropertyType), null);
                }
                list.Add(item);
            }
            return list;
        }

        #endregion

        #region 其他方法

        /// <summary>
        /// 把Xml文档中的标示符变成转义字符
        /// </summary>
        /// <param name="xmlcontent">Xml内容</param>
        /// <returns>转换后的文档</returns>
        public static string EscapeXml(string xmlcontent)
        {
            if (xmlcontent.IndexOf('&') >= 0)
                xmlcontent = xmlcontent.Replace("&", "&amp;");

            if (xmlcontent.IndexOf('\'') >= 0)
                xmlcontent = xmlcontent.Replace("'", "&apos;");

            if (xmlcontent.IndexOf('\"') >= 0)
                xmlcontent = xmlcontent.Replace("\"", "&quot;");

            if (xmlcontent.IndexOf('<') >= 0)
                xmlcontent = xmlcontent.Replace("<", "&lt;");

            if (xmlcontent.IndexOf('>') >= 0)
                xmlcontent = xmlcontent.Replace(">", "&gt;");

            return xmlcontent;
        }

        /// <summary>
        /// 格式化Xml文档,使之美观
        /// </summary>
        /// <param name="xmlContent">Xml内容</param>
        /// <returns>格式化后的文档</returns>
        public static string FormatXml(string xmlContent)
        {
            if (xmlContent == null || xmlContent.Trim().Length == 0)
                return string.Empty;

            string result;

            using (MemoryStream memStream = new MemoryStream())
            {
                XmlTextWriter xmlWriter = new XmlTextWriter(memStream, Encoding.UTF8);
                XmlDocument xmlDoc = new XmlDocument();

                try
                {
                    // 加载Xml文档
                    xmlDoc.LoadXml(xmlContent);
                    xmlWriter.Formatting = Formatting.Indented;
                    xmlDoc.WriteContentTo(xmlWriter);
                    xmlWriter.Flush();
                    memStream.Flush();
                    memStream.Position = 0;
                    StreamReader streamReader = new StreamReader(memStream);

                    result = streamReader.ReadToEnd();
                    memStream.Close();
                }
                catch (Exception)
                {
                    result = xmlContent;
                }
            }
            return result;
        }

        /// <summary>
        /// 使用样式表转换Xml文档
        /// </summary>
        /// <param name="inXml">Xml内容</param>
        /// <param name="styleSheet">样式表内容</param>
        /// <param name="outXml">输出Xml内容</param>
        /// <returns>如果成功返回Html字符串 如果转换失败抛出异常</returns>
        public static void TransformXml(TextReader inXml, TextReader styleSheet, TextWriter outXml)
        {
            if (null == inXml || null == styleSheet)
                return;
            if (outXml == null)
            {
                throw new ArgumentNullException("outXml", "输出Xml对象不能为空");
            }
            try
            {
                XslCompiledTransform xslt = new XslCompiledTransform();
                XsltSettings settings = new XsltSettings(false, false);
                using (XmlReader sheetReader = XmlReader.Create(styleSheet))
                {
                    xslt.Load(sheetReader, settings, null);
                }

                using (XmlReader inReader = XmlReader.Create(inXml))
                {
                    xslt.Transform(inReader, null, outXml);
                }
            }
            catch (Exception e)
            {
                throw new ApplicationException("转换失败", e);
            }
        }

        /// <summary>
        /// 使用样式表转换Xml文档
        /// </summary>
        /// <param name="xmlContent">Xml内容</param>
        /// <param name="xslPath">样式表路径</param>
        /// <returns>如果成功返回Html字符串 如果转换失败返回空</returns>
        public static string TransformXml(string xmlContent, string xslPath)
        {
            if (string.IsNullOrEmpty(xmlContent) || !File.Exists(xslPath))
                return string.Empty;

            string rc;
            try
            {
                var fileStream = new FileStream(xslPath, FileMode.Open, FileAccess.Read, FileShare.Read);
                using (TextReader styleSheet = new StreamReader(fileStream))
                {
                    TextReader inXml = new StringReader(xmlContent);
                    TextWriter outXml = new StringWriter();

                    TransformXml(inXml, styleSheet, outXml);

                    rc = outXml.ToString();

                    fileStream.Close();
                    outXml.Close();
                }
            }
            catch (Exception)
            {
                rc = string.Empty;
            }
            return rc;
        }

        #endregion

        #region Config

        /// <summary>
        /// 读取AppSettings配置
        /// </summary>
        /// <param name="key">键名</param>
        /// <returns>返回key对应的值</returns>
        public static string GetAppConfig(string key)
        {
            return ConfigurationManager.AppSettings[key];
        }

        /// <summary>
        /// 读取AppSettings配置
        /// </summary>
        /// <param name="key">键名</param>
        /// <param name="defaultValue">如果找不到,返回的默认值</param>
        /// <returns>返回key对应的值,如果找不到key则返回指定的默认值</returns>
        public static string GetAppConfig(string key, string defaultValue)
        {
            var v = GetAppConfig(key);
            return string.IsNullOrEmpty(v) ? defaultValue : v;
        }

        /// <summary>
        /// 读取ConnectionStrings配置
        /// </summary>
        /// <param name="key">键名</param>
        /// <returns>返回key对应的连接字符串</returns>
        public static string GetConnectionStrings(string key)
        {
            return ConfigurationManager.ConnectionStrings[key].ConnectionString;
        }

        /// <summary>
        /// 读取ConnectionStrings配置
        /// </summary>
        /// <param name="key">键名</param>
        /// <param name="defaultValue">如果找不到 返回的默认值</param>
        /// <returns>返回key对应的连接字符串,如果找不到key则返回指定的默认值</returns>
        public static string GetConnectionStrings(string key, string defaultValue)
        {
            var v = GetConnectionStrings(key);
            return string.IsNullOrEmpty(v) ? defaultValue : v;
        }

        #endregion
    }
}