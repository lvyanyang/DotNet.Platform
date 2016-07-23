// ===============================================================================
// DotNet.Platform 开发框架 2016 版权所有
// ===============================================================================
using System;
using System.Threading;
using DotNet.Helper;
using DotNet.Utility;

namespace DotNet
{
    public class CacheTest
    {
        public void Test()
        {
            Cache<string, User> cache = new Cache<string, User>();

            cache.Set("1", new User() { Id = 1, Name = "张三" });
            cache.Set("2", new User() { Id = 2, Name = "李四" });
            cache.Set("3", new User() { Id = 3, Name = "王五" });

            Thread t1 = new Thread(() =>
            { 
                Thread.Sleep(2000);
                Console.WriteLine("加写锁");

                Thread.Sleep(8000);
                Console.WriteLine("释放写锁");
            });
            Thread t2 = new Thread(() =>
            {
                Thread.Sleep(4000);
                Console.WriteLine("读取数据...");
                Console.WriteLine(cache.Get("2"));
                Console.WriteLine("读取数据完成");
            });


            //t1.Start();
            //t2.Start();

            Thread t3 = new Thread(() =>
            {
                Thread.Sleep(1000);
                Console.WriteLine("加读锁");


                Thread.Sleep(20000);
                Console.WriteLine("释放读锁");
            });
            Thread t4 = new Thread(() =>
            {
                while (true)
                {
                    Console.WriteLine(cache.Get("2"));
                    Thread.Sleep(1000);
                }
            });

            Thread t5 = new Thread(() =>
            {
                while (true)
                {
                    Thread.Sleep(3000);
                    Console.WriteLine("写入数据");

                    Thread.Sleep(2000);
                    Console.WriteLine("写入完成");
                }
            });


            t3.Start();
            t4.Start();
            t5.Start();

            //cache.TestLockWrite();
            //Console.WriteLine($"读取缓存 2 =>{cache.Get("2")}");


            //Console.WriteLine($"读取缓存 2 =>{cache.Get("2")}");
            //Console.WriteLine("开始遍历缓存");
            //var list = cache.QueryValue();
            //foreach (var item in list)
            //{
            //    if (item.Id == 2)
            //    {
            //        item.Name += "xxxx";
            //    }
            //    Console.WriteLine(item);
            //}
            //Console.WriteLine("结束遍历缓存");
            //Console.WriteLine($"读取缓存 2 =>{cache.Get("2")}");
        }

        public class User
        {
            public int Id { get; set; }

            public string Name { get; set; }

            public override string ToString()
            {
                return $"Name: {Name}";
            }
        }
    }
}