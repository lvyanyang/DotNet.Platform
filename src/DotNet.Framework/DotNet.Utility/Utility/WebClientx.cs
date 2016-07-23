// ===============================================================================
// DotNet.Platform ������� 2016 ��Ȩ����
// ===============================================================================
using System.Net;

namespace DotNet.Utility
{
    /// <summary>
    /// �ṩ�� URI ��ʶ����Դ�������ݺʹ� URI ��ʶ����Դ�������ݵĹ���������(��չ�֧࣬�����ó�ʱʱ�䣬Ĭ�ϳ�ʱʱ��Ϊ3�롣)
    /// </summary>
    [System.ComponentModel.DesignerCategoryAttribute("Code")]
    [System.ComponentModel.ToolboxItem(false)]
    public class WebClientx : System.Net.WebClient
    {
        private int _timeout = 1000 * 3;

        /// <summary>
        /// ��ʱʱ��(����Ϊ��λ),Ĭ��10��
        /// </summary>
        public int Timeout
        {
            get { return _timeout; }
            set { _timeout = value; }
        }

        /// <summary>
        /// Ϊָ����Դ����һ�� System.Net.WebRequest ����
        /// </summary>
        /// <param name="address">һ�� System.Uri�����ڱ�ʶҪ�������Դ</param>
        /// <returns>һ���µ� System.Net.WebRequest ��������ָ������Դ</returns>
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