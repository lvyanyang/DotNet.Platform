// ===============================================================================
// DotNet.Platform 开发框架 2016 版权所有
// ===============================================================================

using System.Collections.Generic;
using System.Linq;
using DotNet.Edu.Entity;
using DotNet.Collections;
using DotNet.Extensions;
using DotNet.Utility;
using System;

namespace DotNet.Edu.Service
{
    /// <summary>
    /// 学员登录日志服务
    /// </summary>
    public class StudentAuditsService
    { 
        /// <summary>
        /// 添加对象
        /// </summary>
        /// <param name="entity">实体</param>
        public BoolMessage Create(StudentAudits entity)
        {
            var repos = new EduRepository<StudentAudits>();
            repos.Insert(entity);
            return BoolMessage.True;
        }

        /// <summary>
        /// 获取对象
        /// </summary>
        /// <param name="id">主键</param>
        public StudentAudits Get(string id)
        {
            return new EduRepository<StudentAudits>().Get(id);
        }

        /// <summary>
        /// 获取对象分页集合
        /// </summary>
        public PageList<StudentAudits> GetPageList(PaginationCondition pageCondition,
            string studentId, DateTime? startDate = null, DateTime? endDate = null)
        {
            pageCondition.SetDefaultOrder(nameof(StudentAudits.LoginDateTime));
            var repos = new EduRepository<StudentAudits>();
            var query = repos.PageQuery(pageCondition).Where(p => p.StudentId == studentId);

            if (startDate.HasValue)
            {
                var startDateDt = startDate.ToDateTime();
                query.Where(p => p.LoginDateTime >= startDateDt);
            }

            if (endDate.HasValue)
            {
                var endDateDt = endDate.ToDateTime().AddDays(1);
                query.Where(p => p.LoginDateTime <= endDateDt);
            }
            return repos.Page(query);
        }
    }
}
