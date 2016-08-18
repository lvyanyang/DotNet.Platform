using DotNet.Collections;
using DotNet.Edu.Entity;
using DotNet.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using DotNet.Extensions;
using DotNet.Helper;

namespace DotNet.Edu.Service
{
    public class PeriodDetailsService
    {
        private readonly EduRepository<PeriodDetails> repos = new EduRepository<PeriodDetails>();

        /// <summary>
        /// 添加对象
        /// </summary>
        /// <param name="entity">实体</param>
        public BoolMessage Create(PeriodDetails entity)
        {
            if (string.IsNullOrEmpty(entity.Id))
            {
                entity.Id = StringHelper.Guid();
            }
            repos.Insert(entity);
            return BoolMessage.True;
        }

        /// <summary>
        /// 获取对象
        /// </summary>
        /// <param name="id">主键</param>
        public PeriodDetails Get(string id)
        {
            return repos.Get(id);
        }

        /// <summary>
        /// 获取对象
        /// </summary>
        public PeriodDetails GetStudentLast(string studentId)
        {
            var query = repos.SQL.Take(1)
                .Where(p => p.StudentId == studentId)
                .OrderByDesc(p => p.CreateDateTime);
            return repos.Query(query).FirstOrDefault();
        }

        /// <summary>
        /// 获取对象集合(已排序)
        /// </summary>
        public List<PeriodDetails> GetStudentList(string studentId)
        {
            var query = repos.SQL.Where(p => p.StudentId == studentId).OrderByDesc(p => p.CreateDateTime);
            return repos.Query(query).ToList();
        }

        /// <summary>
        /// 获取对象分页集合
        /// </summary>
        public PageList<PeriodDetails> GetStudentPageList(PaginationCondition pageCondition,string studentId,
            DateTime? startDate = null, DateTime? endDate = null)
        {
            pageCondition.SetDefaultOrder(nameof(PeriodDetails.CreateDateTime));
            var query = repos.PageQuery(pageCondition).Where(p=>p.StudentId==studentId);
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

        public PageList<PeriodDetails> GetPageList(PaginationCondition pageCondition, string studentName,
            DateTime? startDate = null, DateTime? endDate = null)
        {
            pageCondition.SetDefaultOrder(nameof(PeriodDetails.CreateDateTime));
            var query = repos.PageQuery(pageCondition);
            if (studentName.IsNotEmpty())
            {
                query.Where(p => p.StudentName == studentName);
            }
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
