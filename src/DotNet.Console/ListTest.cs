// ===============================================================================
// DotNet.Platform 开发框架 2016 版权所有
// ===============================================================================
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using DotNet.Data;
using DotNet.Extensions;
using DotNet.Helper;
using DotNet.Utility;

namespace DotNet
{
    public class ListTest
    {
        //private static readonly object lockObject = new object();
        //private readonly RWLock rwlock = new RWLock();
        private List<WaitSmsRecord> smslist;

        private void InitSmsList()
        {
            if (smslist == null)
            {
                var repos = new Repository<WaitSmsRecord>(new Database("station"));
                smslist = new List<WaitSmsRecord>(repos.Query());
            }
        }

        public void Test()
        {
            InitSmsList();

            Thread thForeach = new Thread(() =>
            {
                while (true)
                {
                    if (smslist.Count>0)
                    {
                        Console.WriteLine($"准备Query...smslist.count={smslist.Count}");
                        var qList = smslist.ToList();
                        foreach (var item in qList)
                        {
                            Console.WriteLine($"threadId={Thread.CurrentThread.ManagedThreadId},RecordID={item.RecordID},qList.count={qList.Count}");
                            Thread.Sleep(200);
                        }
                    }
                }
            });

            Thread thForeach2 = new Thread(() =>
            {
                while (true)
                {
                    if (smslist.Count > 0)
                    {
                        Console.WriteLine($"准备Query2...smslist.count={smslist.Count}");
                        var qList = smslist.ToList();
                        foreach (var item in qList)
                        {
                            Console.WriteLine($"threadId2={Thread.CurrentThread.ManagedThreadId},RecordID={item.RecordID},qList.count={qList.Count}");
                            //Thread.Sleep(200);
                        }
                    }
                }
            });

            Thread thFor = new Thread(() =>
            {
                var count = smslist.Count;
                for (int index = 0; index < count; index++)
                {
                    var item = smslist[index];
                    Console.WriteLine($"threadId={Thread.CurrentThread.ManagedThreadId},RecordID={item.RecordID},index={index},count={smslist.Count}");
                    Thread.Sleep(500);
                }
            });

            Thread thRemove = new Thread(() =>
            {
                int index = smslist.Count;
                while (true)
                {
                    //Thread.Sleep(400);
                    var count = smslist.Count;
                    if (count > 0)
                    {
                        index++;
                        Console.WriteLine($"threadId={Thread.CurrentThread.ManagedThreadId},正在移除...smslist.count={count}");
                        var index1 = index;
                        var counts = smslist.Delete(p => p.RecordID == index1);
                        if (counts>0)
                        {
                            Console.WriteLine("删除成功");
                        }
                    }
                }
            });

            Thread thAdd = new Thread(() =>
            {
                while (true)
                {
                    //Thread.Sleep(300);
                    var count = smslist.Count + 1;
                    smslist.Add(new WaitSmsRecord()
                    {
                        RecordID = count,
                        SendClientNum = "139888822333" + count,
                        SendContent = "短信内容" + count,
                        AdderID = 998,
                        AdderName = "张三" + count,
                        AddTime = DateTime.Now
                    });
                    Console.WriteLine($"threadId={Thread.CurrentThread.ManagedThreadId},添加项,smslist.Count={smslist.Count}");
                }
            });

            Thread thSort = new Thread(() =>
            {
                bool isdesc = true;
                while (true)
                {
                    Thread.Sleep(1000);
                    //isdesc = !isdesc;
                    var sortDir = isdesc ? "倒序" : "正序";
                    Console.WriteLine($"threadId={Thread.CurrentThread.ManagedThreadId},正在排序...{sortDir}");
                    var list = smslist.ToList();
                    list.OrderBy(p => p.RecordID, isdesc);
                }
            });

            Thread thFind = new Thread(() =>
            {
                int index = 0;
                while (true)
                {
                    //Thread.Sleep(100);
                    index++;
                    var index1 = index;
                    var entity = smslist.FirstOrDefault(p => p.RecordID == index1);
                    Console.WriteLine($"threadId={Thread.CurrentThread.ManagedThreadId},正在搜索...找到Id={entity?.RecordID}");
                }
            });

            Thread thUpdate = new Thread(() =>
            {
                int index = 0;
                while (true)
                {
                    //Thread.Sleep(100);
                    index++;
                    var index1 = index;
                    var entity = smslist.FirstOrDefault(p => p.RecordID == index1);
                    if (entity != null)
                    {
                        entity.SendClientNum += 9999;
                    }
                    Console.WriteLine($"threadId={Thread.CurrentThread.ManagedThreadId},修改Id={entity?.RecordID}");
                }
            });

            //thForeach.Start();
            //thRemove.Start();
            //thAdd.Start();

            ////thForeach2.Start();
            ////thSort.Start();
            //thFind.Start();
            //thUpdate.Start();
        }

        public void FastTest()
        {

            var nums = new[] {"15802963862", "15091626189", "15029188830"};

            var initTime = DebugHelper.StartWatch(() =>
            {
                InitSmsList();
            });
            Console.WriteLine(initTime);
            Console.WriteLine($"记录总数={smslist.Count}");
            Console.WriteLine(smslist[smslist.Count/2]);

            Console.WriteLine("-------------------------");
            Console.WriteLine("-------------------------");
            List<WaitSmsRecord> newList = null;
            var newTime = DebugHelper.StartWatch(() =>
            {
                newList = new List<WaitSmsRecord>(
                    smslist.Where(p => Array.IndexOf(nums, p.SendClientNum) > -1));
            });
            Console.WriteLine(newTime);
            Console.WriteLine($"newList记录总数={newList.Count}");
            Console.WriteLine(newList[newList.Count / 2]);

            Console.WriteLine("-------------------------");
            Console.WriteLine("-------------------------");
            List<WaitSmsRecord> toList = null;
            var toTime = DebugHelper.StartWatch(() =>
            {
                toList = smslist.ToList();
            });
            Console.WriteLine(toTime);
            Console.WriteLine($"toList={toList.Count}");
            Console.WriteLine(toList[toList.Count / 2]);
        }
    }
}