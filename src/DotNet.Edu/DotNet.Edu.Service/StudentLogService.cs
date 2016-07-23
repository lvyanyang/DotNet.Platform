// ===============================================================================
// DotNet.Platform 开发框架 2016 版权所有
// ===============================================================================

using System;
using System.Collections.Generic;
using System.Linq;
using DotNet.Edu.Entity;
using DotNet.Collections;
using DotNet.Extensions;
using DotNet.Helper;
using DotNet.Utility;

namespace DotNet.Edu.Service
{
    /// <summary>
    /// 学员日志服务
    /// </summary>
    public class StudentLogService
    {
        /// <summary>
        /// 构造服务
        /// </summary>
        internal StudentLogService()
        {
        }

        /// <summary>
        /// 添加对象
        /// </summary>
        /// <param name="studentId">学员主键</param>
        /// <param name="studentName">学员信息</param>
        /// <param name="message">消息</param>
        public BoolMessage Create(string studentId,string studentName,string message)
        {
            var entity = new StudentLog();
            entity.Id = StringHelper.Guid();
            entity.CreateDateTime = DateTime.Now;
            entity.StudentId = studentId;
            entity.StudentName = studentName;
            entity.Message = message;
            var repos = new EduRepository<StudentLog>();
            repos.Insert(entity);
            return BoolMessage.True;
        }

        /// <summary>
        /// 获取对象
        /// </summary>
        /// <param name="id">主键</param>
        public StudentLog Get(string id)
        {
            return new EduRepository<StudentLog>().Get(id);
        }

        /// <summary>
        /// 获取对象分页集合
        /// </summary>
        /// <param name="pageCondition">分页对象</param>
        /// <param name="studentId">学员主键</param>
        public PageList<StudentLog> GetPageList(PaginationCondition pageCondition,string studentId)
        {
            pageCondition.SetDefaultOrder(nameof(StudentLog.CreateDateTime));
            var repos = new EduRepository<StudentLog>();
            var query = repos.PageQuery(pageCondition).Where(p=>p.StudentId==studentId);
            return repos.Page(query);
        }
    }
}
