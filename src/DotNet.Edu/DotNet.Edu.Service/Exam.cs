// ===============================================================================
// DotNet.Platform 开发框架 2016 版权所有
// ===============================================================================

using DotNet.Data;

namespace DotNet.Edu.Service
{
    public class ExamDatabase : Database
    {
        public ExamDatabase() : base("dotnet.exam") { }
    }

    public class ExamRepository<T> : Repository<T> where T : class, new()
    {
        public ExamRepository() : base("dotnet.exam") { }
    }
}