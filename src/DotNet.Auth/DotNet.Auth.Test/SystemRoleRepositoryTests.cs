// ===============================================================================
// DotNet.Platform 开发框架 2016 版权所有
// ===============================================================================
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using DotNet.Auth.Entity;
using DotNet.Collections;
using DotNet.Entity;
using DotNet.Extensions;
using DotNet.Utility;
using DotNet.Data;
using DotNet.Helper;

namespace DotNet.Auth.Test
{
    [TestClass]
    public class SystemRoleRepositoryTests
    {
        private SafeList<WaitSmsRecord> _list;

        [TestMethod]
        public void QueryTest()
        {
            //var list = new List<User>();
            //list.Add(new User { Id = 1, Label = "abc", CreateDate = "2016-5-21 12:30:11".ToDateTime() });
            //list.Add(new User { Id = 2, Label = "fff", CreateDate = "2016-5-21 8:30:12".ToDateTime() });
            //list.Add(new User { Id = 3, Label = "abc", CreateDate = "2016-5-21 23:30:13".ToDateTime() });
            //list.Add(new User { Id = 4, Label = "fff", CreateDate = "2016-5-21 14:30:14".ToDateTime() });
            //list.Add(new User { Id = 4, Label = "def", CreateDate = "2016-5-21 12:30:11".ToDateTime() });
            //list.Sort(
            //    new KeyValuePair<string, string>("Label", "asc"),
            //    new KeyValuePair<string, string>("CreateDate", "desc")
            //    );
            //foreach (var item in list)
            //{
            //    Trace.WriteLine($"id={item.Id} date={item.CreateDate} label={item.Label}");
            //}
        }


        public void PerformanceTest(int pageIndex, int pageSize)
        {
            string orderName = "RecordID";
            string orderDir = "asc";
            string searchName = "15802963862";//
            var repos = new Repository<WaitSmsRecord>(new Database("station"));
            var query = repos.SQL.Take(pageSize).Page(pageIndex).
                OrderBy(orderName, orderDir.IsAsc());
            if (searchName.IsNotEmpty())
            {
                searchName = searchName.Trim();
                query.Where(p => p.SendClientNum.Contains(searchName));
            }
            var pageList = repos.Page(query);
            Trace.WriteLine($"总数:{pageList.TotalCount} 总页数:{pageList.TotalPages} {pageList.RecordStartIndex}-{pageList.RecordEndIndex}");
            foreach (var item in pageList)
            {
                Trace.WriteLine($"RecordID={item.RecordID} SendClientNum={item.SendClientNum} AddTime={item.AddTime}");
            }
        }

        public void InitMemoryData()
        {
            var repos = new Repository<WaitSmsRecord>(new Database("station"));
            _list = new SafeList<WaitSmsRecord>();
            _list.Init(repos.Query());
        }

        public void MemoryTest(int pageIndex, int pageSize)
        {
            string orderName = "RecordID";
            string orderDir = "asc";
            var predicate = PredicateExtensions.True<WaitSmsRecord>();
            string searchName = "15802963862";
            if (searchName.IsEmpty())
            {
                predicate = predicate.And(p => p.SendClientNum.Contains(searchName));
            }
            var pageList = _list.GetPageList(new PaginationCondition(pageIndex, pageSize, orderName, orderDir),
                predicate.Compile());
            Trace.WriteLine($"总数:{pageList.TotalCount} 总页数:{pageList.TotalPages} {pageList.RecordStartIndex}-{pageList.RecordEndIndex}");
            foreach (var item in pageList)
            {
                Trace.WriteLine($"RecordID={item.RecordID} SendClientNum={item.SendClientNum} AddTime={item.AddTime}");
            }
        }

        [TestMethod]
        public void PerformanceTestTwo()
        {
            var str = DebugHelper.StartWatch(() =>
            {
                PerformanceTest(2, 10);
                PerformanceTest(3, 10);
            });
            Trace.WriteLine(str);
        }

        [TestMethod]
        public void MemoryTestTwo()
        {
            InitMemoryData();
            var str = DebugHelper.StartWatch(() =>
            {
                MemoryTest(2, 10);
                MemoryTest(3, 10);
            });
            Trace.WriteLine(str);
        }
    }

    public class User
    {
        public int Id { get; set; }

        public DateTime CreateDate { get; set; }

        public string Label { get; set; }
    }

    [Table(Schema = "Sms", Name = "WaitSmsRecord")]
    public class WaitSmsRecord
    {
        [PrimaryKey]
        public int RecordID { get; set; }

        public string SendClientNum { get; set; }
        public string SendContent { get; set; }
        public int AdderID { get; set; }
        public string AdderName { get; set; }
        public DateTime AddTime { get; set; }
    }
}