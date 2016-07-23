// ===============================================================================
// DotNet.Platform 开发框架 2016 版权所有
// ===============================================================================
using System;
using System.Linq;
using System.Threading;
using DotNet.Data;

namespace DotNet
{
    public class DatabaseTest
    {
        public void Test()
        {

            var rx1 = new Repository<WaitSmsRecord>("station");
            var qx1 = rx1.Query().Take(100);
            var qx2 = rx1.Query().Take(200);

            int i = 0,j=0;
            foreach (var item in qx1)
            {
                i++;
                Console.WriteLine(i);
            }

            Console.WriteLine("============================");

            foreach (var item in qx2)
            {
                j++;
                Console.WriteLine(j);
            }

            //var rx2 = new Repository<WaitSmsRecord>("station");
            //var qx2 = rx2.Query();
            //qx2 = rx2.Query();

            //var rx3 = new Repository<WaitSmsRecord>(new Database("station"));
            //var qx3 = rx3.Query();
            //qx3 = rx3.Query();



            //Thread th1 = new Thread(() =>
            //{
            //    foreach (var item in qx1)
            //    {
            //        Thread.Sleep(1000);
            //        Console.WriteLine(item);
            //    }
            //});
            //Thread th2 = new Thread(() =>
            //{
            //    while (true)
            //    {
            //        var r = new Repository<WaitSmsRecord>(new Database("station"));
            //        var entity = new WaitSmsRecord()
            //        {
            //            SendClientNum = "139888822333",
            //            SendContent = "短信内容",
            //            AdderID = 998,
            //            AdderName = "张三",
            //            AddTime = DateTime.Now
            //        };
            //        r.Insert(entity);
            //        Console.WriteLine("Insert成功");
            //        Thread.Sleep(1000);
            //    }
            //});

            //th1.Start();
            //th2.Start();
        }
    }
}