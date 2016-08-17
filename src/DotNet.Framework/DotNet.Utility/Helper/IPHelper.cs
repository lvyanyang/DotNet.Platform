using System;
using System.Net.Http;
using System.Web;
using DotNet.Configuration;
using DotNet.Utility;
using Newtonsoft.Json;

namespace DotNet.Helper
{
    /// <summary>
    /// IP地址操作类
    /// </summary>
    public static class IPHelper
    {
        /// <summary>
        /// 获取外网IP信息
        /// </summary>
        /// <param name="request">请求对象</param>
        public static IPData GetInternetIP(HttpRequest request)
        {
            string ip = request.ServerVariables["REMOTE_ADDR"];
            if (ip.Equals("127.0.0.1") || ip.Equals("::1"))
            {
                return new IPData();
            }
            var setting = ConfigManager.GetSetting("IPSetting", () => new IPSetting());
            var api = string.Format(setting.Api, ip);
            HttpClient client = new HttpClient();
            var json = client.GetStringAsync(api).Result;
            var info = JsonHelper.Deserialize<IPInfo>(json);
            client.Dispose();
            return info.Data;
        }
    }

    public class IPSetting
    {
        public string Api => "http://ip.taobao.com/service/getIpInfo.php?ip={0}";
    }

    public class IPInfo
    {
        [JsonProperty("code")]
        public string Code { get; set; }

        public bool Success => Code.Equals("0");

        [JsonProperty("data")]
        public IPData Data { get; set; }
    }

    public class IPData
    {
        [JsonProperty("country")]
        public string Country { get; set; }

        [JsonProperty("country_id")]
        public string CountryId { get; set; }

        [JsonProperty("area")]
        public string Area { get; set; }

        [JsonProperty("area_id")]
        public string AreaId { get; set; }

        [JsonProperty("region")]
        public string Region { get; set; }

        [JsonProperty("region_id")]
        public string RegionId { get; set; }

        [JsonProperty("city")]
        public string City { get; set; }

        [JsonProperty("city_id")]
        public string CityId { get; set; }

        [JsonProperty("county")]
        public string County { get; set; }

        [JsonProperty("county_id")]
        public string CountyId { get; set; }

        [JsonProperty("isp")]
        public string Isp { get; set; }

        [JsonProperty("isp_id")]
        public string IspId { get; set; }

        [JsonProperty("ip")]
        public string Ip { get; set; }

        public override string ToString()
        {
            return $"Country: {Country}, Area: {Area}, Region: {Region}, City: {City}, County: {County}, Isp: {Isp}, Ip: {Ip}";
        }
    }
}