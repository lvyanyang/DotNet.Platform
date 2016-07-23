// ===============================================================================
// DotNet.Platform 开发框架 2016 版权所有
// ===============================================================================
using System;
using DotNet.Utility;

namespace DotNet
{
    public class SingletonTest
    {
        public void Test()
        {
            Console.WriteLine(Singleton<User>.Instance.Id);
            Console.WriteLine(Singleton<User>.Instance.Id);
            Console.WriteLine(Singleton<User>.Instance.Id);
            Console.WriteLine(Singleton<User>.Instance.Id);
        }

        public class User
        {
            public int Id = 0;

            public User()
            {
                Id++;
                Console.WriteLine("构造User对象,id="+Id);
            }
        }
    }
}