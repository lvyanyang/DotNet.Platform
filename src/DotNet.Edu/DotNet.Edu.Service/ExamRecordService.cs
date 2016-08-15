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

        /// <summary>
        /// 获取对象
        /// </summary>
        /// <param name="id">主键</param>
        public ExamRecord Get(string id)
        {
            return new ExamRepository<ExamRecord>().Get(id);
        }

        /// <summary>
        /// 获取对象分页集合
        /// </summary>
        /// <param name="pageCondition">分页对象</param>
        /// <param name="name">姓名</param>
        public PageList<ExamRecord> GetPageList(PaginationCondition pageCondition,string name)
        {
            pageCondition.SetDefaultOrder(nameof(ExamRecord.UserStartDateTime));
            var repos = new ExamRepository<ExamRecord>();
            var query = repos.PageQuery(pageCondition);
           
            if (name.IsNotEmpty())
            {
                name = name.Trim();
                query.Where(p => p.UserName.Contains(name) || p.UserNameSpell.Contains(name) || p.IdNumber.Contains(name));
            }
            return repos.Page(query);
        }
    }
}
