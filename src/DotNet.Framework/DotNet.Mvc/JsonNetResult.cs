// ===============================================================================
// DotNet.Platform 开发框架 2016 版权所有
// ===============================================================================
using System;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace DotNet.Mvc
{
    /// <summary>
    /// JsonNet视图
    /// </summary>
    public class JsonNetResult : JsonResult
    {
        /// <summary>
        /// 初始化 JsonNetResult 类的新实例。
        /// </summary>
        public JsonNetResult()
        {
            Settings = new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Error,
                NullValueHandling = NullValueHandling.Ignore,
                DateFormatString = "yyyy-MM-dd HH:mm:ss"
            };
            Settings.Converters.Add(new StringEnumConverter { CamelCaseText = true });
        }

        /// <summary>
        /// 初始化 JsonNetResult 类的新实例。
        /// </summary>
        /// <param name="data">数据对象</param>
        /// <param name="contentType">获取或设置内容的类型。</param>
        /// <param name="contentEncoding">获取或设置内容的编码。</param>
        /// <param name="behavior">指定是否允许来自客户端的 HTTP GET 请求。</param>
        public JsonNetResult(object data, string contentType = null, Encoding contentEncoding = null, JsonRequestBehavior behavior = JsonRequestBehavior.AllowGet)
            : this()
        {
            this.Data = data;
            this.JsonRequestBehavior = behavior;
            this.ContentEncoding = contentEncoding;
            this.ContentType = contentType;
        }

        /// <summary>
        /// Json序列化配置
        /// </summary>
        public JsonSerializerSettings Settings { get; private set; }

        /// <summary>
        /// 通过从 <see cref="T:System.Web.Mvc.ActionResult"/> 类继承的自定义类型，启用对操作方法结果的处理。
        /// </summary>
        /// <param name="context">执行结果时所处的上下文。</param><exception cref="T:System.ArgumentNullException"><paramref name="context"/> 参数为 null。</exception>
        public override void ExecuteResult(ControllerContext context)
        {
            if (context == null)
                throw new ArgumentNullException("context");
            if (this.JsonRequestBehavior == JsonRequestBehavior.DenyGet && string.Equals(context.HttpContext.Request.HttpMethod, "GET", StringComparison.OrdinalIgnoreCase))
                throw new InvalidOperationException("此请求已被阻止，因为当用在 GET 请求中时，会将敏感信息透漏给第三方网站。若要允许 GET 请求，请将 JsonRequestBehavior 设置为 AllowGet。");

            HttpResponseBase response = context.HttpContext.Response;
            response.ContentType = string.IsNullOrEmpty(this.ContentType) ? "application/json" : this.ContentType;

            if (this.ContentEncoding != null)
                response.ContentEncoding = this.ContentEncoding;
            if (this.Data == null)
                return;

            var scriptSerializer = JsonSerializer.Create(this.Settings);

            using (var sw = new StringWriter())
            {
                scriptSerializer.Serialize(sw, this.Data);
                response.Write(sw.ToString());
            }
        }
    }
}
