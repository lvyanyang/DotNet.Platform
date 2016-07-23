// ===============================================================================
// DotNet.Platform 开发框架 2016 版权所有
// ===============================================================================

using System;
using DotNet.Data;
using DotNet.Entity;

namespace DotNet
{
    public class DbSessionTest
    {
        public void Test()
        {
            try
            {
                DbSession.Begin(new StationDatabase());

                new SessionTestDal1().Write();

                new SessionTestDal2().Write();

                DbSession.Commit();
            }
            catch (Exception e)
            {
                DbSession.Rollback();
                Console.WriteLine(e.Message);
            }
        }

        public class SessionTestDal1
        {
            public void Write()
            {
                var repos = new Repository<SessionTest>();
                repos.Insert(new SessionTest { Name = "SessionTestDal1" });
            }
        }

        public class SessionTestDal2
        {
            public void Write()
            {
                var repos = new Repository<SessionTest>();
                repos.Insert(new SessionTest { Name = null });
            }
        }

        public class StationDatabase : Database
        {
            public StationDatabase() : base("station") { }
        }

        public class SessionTest
        {
            [PrimaryKey(true)]
            public int Id { get; set; }

            public string Name { get; set; }
        }
    }
}