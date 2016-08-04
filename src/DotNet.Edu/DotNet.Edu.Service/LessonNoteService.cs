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
    /// 课堂笔记服务
    /// </summary>
    public class LessonNoteService
    {
        /// <summary>
        /// 添加对象
        /// </summary>
        /// <param name="entity">实体</param>
        public BoolMessage Create(LessonNote entity)
        {
            var repos = new EduRepository<LessonNote>();
            repos.Insert(entity);
            return BoolMessage.True;
        }

        /// <summary>
        /// 更新对象
        /// </summary>
        public BoolMessage Update(string id, string messaage)
        {
            var repos = new EduRepository<LessonNote>();
            repos.UpdateInclude(new LessonNote { Message = messaage }, p => p.Id == id, p => p.Message);
            return BoolMessage.True;
        }

        /// <summary>
        /// 删除对象
        /// </summary>
        /// <param name="id">主键</param>
        public BoolMessage Delete(string id)
        {
            var repos = new EduRepository<LessonNote>();
            repos.Delete(id);
            return BoolMessage.True;
        }

        /// <summary>
        /// 获取对象集合
        /// </summary>
        /// <param name="studentId">学员主键</param>
        /// <param name="taskNumber">提取数量</param>
        public List<LessonNote> GetTopList(string studentId,int taskNumber)
        {
            var repos = new EduRepository<LessonNote>();
            var query = repos.SQL.Take(taskNumber).Where(p => p.StudentId == studentId).OrderByDesc(p=>p.CreateDateTime);
            return repos.Fetch(query);
        }

        /// <summary>
        /// 获取对象分页集合
        /// </summary>
        /// <param name="pageCondition">分页对象</param>
        /// <param name="studentId">学员主键</param>
        public PageList<LessonNote> GetPageList(PaginationCondition pageCondition, string studentId)
        {
            pageCondition.SetDefaultOrder(nameof(LessonNote.CreateDateTime));
            var repos = new EduRepository<LessonNote>();
            var query = repos.PageQuery(pageCondition).Where(p => p.StudentId == studentId);
            return repos.Page(query);
        }
    }
}
