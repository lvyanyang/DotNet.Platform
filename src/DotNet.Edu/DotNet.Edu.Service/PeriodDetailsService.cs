using DotNet.Collections;
using DotNet.Edu.Entity;
using DotNet.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNet.Edu.Service
{
    public class PeriodDetailsService
    {
        /// <summary>
        /// 添加对象
        /// </summary>
        /// <param name="entity">实体</param>
        public BoolMessage Create(PeriodDetails entity)
        {
            var repos = new EduRepository<PeriodDetails>();
            repos.Insert(entity);
            return BoolMessage.True;
        }

        /// <summary>
        /// 获取对象
        /// </summary>
        /// <param name="id">主键</param>
        public PeriodDetails Get(string id)
        {
            return new EduRepository<PeriodDetails>().Get(id);
        }

        /// <summary>
        /// 获取对象集合(已排序)
        /// </summary>
        public List<PeriodDetails> GetList(string studentId)
        {
            var repos = new EduRepository<PeriodDetails>();
            var query = repos.SQL.Where(p => p.StudentId == studentId).OrderByDesc(p => p.CreateDateTime);
            return repos.Query(query).ToList();
        }

        /// <summary>
        /// 获取对象分页集合
        /// </summary>
        public PageList<PeriodDetails> GetPageList(PaginationCondition pageCondition,
            DateTime? startDate, DateTime? endDate)
        {
            pageCondition.SetDefaultOrder(nameof(PeriodDetails.CreateDateTime));
            var repos = new EduRepository<PeriodDetails>();
            var query = repos.PageQuery(pageCondition);
            if (startDate.HasValue)
            {
                query.Where(p => p.CreateDateTime >= startDate.Value);
            }
            if (endDate.HasValue)
            {
                query.Where(p => p.CreateDateTime <= endDate.Value.AddDays(1));
            }
            
            return repos.Page(query);
        }
    }
}
