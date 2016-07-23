using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Web;
using Newtonsoft.Json;
using DotNet.Configuration;
using DotNet.Utility;

namespace DotNet.Edu.WebUtility
{
    /// <summary>
    /// 区域信息
    /// </summary>
    public class Region
    {
        /// <summary>
        /// 编码
        /// </summary>
        [JsonProperty("code")]
        public string Code { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; set; }
    }

    public static class RegionHelper
    {
        private static readonly JsonDataFile<List<Region>> Province = new JsonDataFile<List<Region>>("Province");
        private static readonly JsonDataFile<List<Region>> City = new JsonDataFile<List<Region>>("City");
        private static readonly JsonDataFile<List<Region>> District = new JsonDataFile<List<Region>>("District");

        /// <summary>
        /// 获取省份信息
        /// </summary>
        public static ReadOnlyCollection<Region> GetProvince()
        {
            return Province.Data.OrderBy(p => p.Code).ToList().AsReadOnly();
        }

        /// <summary>
        /// 获取城市信息
        /// </summary>
        /// <param name="provinceCode">省份编码</param>
        public static ReadOnlyCollection<Region> GetCity(string provinceCode)
        {
            if (string.IsNullOrEmpty(provinceCode)) return null;
            var pro = provinceCode.Substring(0, 2);
            return City.Data.Where(p => p.Code.Substring(0, 2).Equals(pro)).OrderBy(p => p.Code).ToList().AsReadOnly();
        }

        /// <summary>
        /// 获取区县信息
        /// </summary>
        /// <param name="cityCode">城市编码</param>
        public static ReadOnlyCollection<Region> GetDistrict(string cityCode)
        {
            if (string.IsNullOrEmpty(cityCode)) return null;
            var ci = cityCode.Substring(0, 4);
            return District.Data.Where(p => p.Code.Substring(0, 4).Equals(ci)).OrderBy(p => p.Code).ToList().AsReadOnly();
        }

        /// <summary>
        /// 获取区域编码根据区域名称(层级使用/隔开(省/市/区(县)))
        /// </summary>
        /// <param name="name">区域名称</param>
        public static string GetRegionCodeByName(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                return string.Empty;
            }
            var names = name.Split(new[] { '/' }, StringSplitOptions.RemoveEmptyEntries);
            var code = string.Empty;
            var count = names.Count();
            Region province = null;
            Region city = null;
            if (count>0)
            {
                province = GetProvince().FirstOrDefault(p => p.Name.Equals(names[0]));
                if (province!=null)
                {
                    code = province.Code;
                }
            }
            if (province != null && count > 1)
            {
                city = GetCity(province.Code).FirstOrDefault(p => p.Name.Equals(names[1]));
                if (city != null)
                {
                    code = city.Code;
                }
            }
            if (city != null && count > 2)
            {
                var district = GetDistrict(city.Code).FirstOrDefault(p => p.Name.Equals(names[2]));
                if (district != null)
                {
                    code = district.Code;
                }
            }

            return code;
        }

        /// <summary>
        /// 获取地区列表
        /// </summary>
        /// <returns></returns>
        public static List<KeyValue> GetRegionList()
        {
            var data = new List<KeyValue>();
            var provinces = GetProvince();
            foreach (var province in provinces)
            {
                var provinceName = province.Name;
                //data.Add(new KeyValue(provinceName, province.Code));
                var citys = GetCity(province.Code);
                foreach (var city in citys)
                {
                    var cityName = city.Name;
                    //data.Add(new KeyValue($"{provinceName}/{cityName}", city.Code));
                    var districts = GetDistrict(city.Code);
                    foreach (var district in districts)
                    {
                        var districtName = district.Name;
                        data.Add(new KeyValue($"{provinceName}/{cityName}/{districtName}", district.Code));
                    }
                }
            }

            return data;
        }
    }
}