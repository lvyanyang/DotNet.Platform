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
    /// 学习日志服务
    /// </summary>
    public class LessonLogService
    {
        /// <summary>
        /// 添加对象
        /// </summary>
        /// <param name="entity">实体</param>
        public BoolMessage Create(LessonLog entity)
        {
            var repos = new EduRepository<LessonLog>();
            repos.Insert(entity);
            return BoolMessage.True;
        }
 
        /// <summary>
        /// 获取对象
        /// </summary>
        /// <param name="id">主键</param>
        public LessonLog Get(string id)
        {
            return new EduRepository<LessonLog>().Get(id);
        }

        /// <summary>
        /// 获取对象分页集合
        /// </summary>
        /// <param name="pageCondition">分页对象</param>
        /// <param name="studentId">学员主键</param>
        public PageList<LessonLog> GetPageList(PaginationCondition pageCondition,string studentId)
        {
            pageCondition.SetDefaultOrder(nameof(LessonLog.CreateDateTime));
            var repos = new EduRepository<LessonLog>();
            var query = repos.PageQuery(pageCondition).Where(p => p.StudentId == studentId);
            return repos.Page(query);
        }
    }
}
