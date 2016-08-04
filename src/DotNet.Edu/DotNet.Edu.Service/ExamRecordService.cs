// ===============================================================================
// DotNet.Platform 开发框架 2016 版权所有
// ===============================================================================

using System.Collections.Generic;
using System.Linq;
using DotNet.Edu.Entity;
using DotNet.Collections;
using DotNet.Extensions;
using DotNet.Utility;

namespace DotNet.Edu.Service
{
    /// <summary>
    /// 考试记录服务
    /// </summary>
    public class ExamRecordService
    {
        /// <summary>
        /// 获取对象集合(已排序)
        /// </summary>
        public List<ExamRecord> GetList(string studentIdNumber)
        {
            var repos = new ExamRepository<ExamRecord>();
            var query = repos.SQL.Where(p => p.IdNumber == studentIdNumber && p.UserIsCommit == 1).OrderByAsc(p => p.UserStartDateTime);
            return repos.Query(query).ToList();
        }
    }
}
