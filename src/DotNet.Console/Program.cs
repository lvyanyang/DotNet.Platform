// ===============================================================================
// DotNet.Platform 开发框架 2016 版权所有
// ===============================================================================
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using DotNet.Data;

namespace DotNet
{
    class Program
    {
        static void Main(string[] args)
        {
            //var instance = new DbSessionTest();
            //instance.Test();

            var repos = new Repository<WaitSmsRecord>(new Database("station"));
            Console.WriteLine(repos.Count());
            Console.WriteLine(repos.Count(p => p.SendClientNum == "15802963862"));
        }
    }
}
