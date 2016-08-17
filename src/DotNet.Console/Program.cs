// ===============================================================================
// DotNet.Platform 开发框架 2016 版权所有
// ===============================================================================
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using DotNet.Configuration;
using DotNet.Data;
using DotNet.Extensions;
using DotNet.Helper;
using DotNet.Mvc;
using Newtonsoft.Json;

namespace DotNet
{
    class Program
    {
        static void Main(string[] args)
        {
            //var instance = new DbSessionTest();
            //instance.Test();

            //var repos = new Repository<WaitSmsRecord>(new Database("station"));
            //Console.WriteLine(repos.Count());
            //Console.WriteLine(repos.Count(p => p.SendClientNum == "15802963862"));

            //Console.WriteLine(RandomHelper.GenerateRandomArray(50).Join());

            //Console.WriteLine(CdnHelper.Url("~/lib/abc/jquery.js"));
            //Console.WriteLine(CdnHelper.Url("~/lib/jquery.css"));
            using (HttpClient client = new HttpClient())
            {
                var url = "http://ip.taobao.com/service/getIpInfo.php?ip=124.115.168.58";
                 
                var json = client.GetStringAsync(url).Result;
                var obj = JsonHelper.Deserialize<IPInfo>(json);
                Console.WriteLine(obj.Data);
            }

            Console.ReadLine();
        }
    }

}
