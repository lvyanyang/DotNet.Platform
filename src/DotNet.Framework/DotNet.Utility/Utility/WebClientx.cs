// ===============================================================================
// DotNet.Platform 开发框架 2016 版权所有
// ===============================================================================
using System.Net;

namespace DotNet.Utility
{
    /// <summary>
    /// 提供向 URI 标识的资源发送数据和从 URI 标识的资源接收数据的公共方法。(扩展类，支持设置超时时间，默认超时时间为3秒。)
    /// </summary>
    [System.ComponentModel.DesignerCategoryAttribute("Code")]
    [System.ComponentModel.ToolboxItem(false)]
    public class WebClientx : System.Net.WebClient
    {
        private int _timeout = 1000 * 3;

        /// <summary>
        /// 超时时间(毫秒为单位),默认10秒
        /// </summary>
        public int Timeout
        {
            get { return _timeout; }
            set { _timeout = value; }
        }

        /// <summary>
        /// 为指定资源返回一个 System.Net.WebRequest 对象
        /// </summary>
        /// <param name="address">一个 System.Uri，用于标识要请求的资源</param>
        /// <returns>一个新的 System.Net.WebRequest 对象，用于指定的资源</returns>
        protected override WebRequest GetWebRequest(System.Uri address)
        {
            HttpWebRequest request = (HttpWebRequest)base.GetWebRequest(address);
            if (request != null)
            {
                request.Timeout = Timeout;
                request.ReadWriteTimeout = Timeout;
            }
            return request;
        }
    }
}