// ===============================================================================
// DotNet.Platform 开发框架 2016 版权所有
// ===============================================================================

using System;
using System.Collections.Generic;
using System.Linq;
using DotNet.Edu.Entity;
using DotNet.Collections;
using DotNet.Extensions;
using DotNet.Utility;

namespace DotNet.Edu.Service
{
    /// <summary>
    /// 通知公告服务
    /// </summary>
    public class NoticeService
    {
        /// <summary>
        /// 是否存在指定名称的对象
        /// </summary>
        /// <param name="id">主键</param>
        /// <param name="title">名称</param>
        /// <returns>如果存在返回false</returns>
        public BoolMessage ExistsByName(string id, string title)
        {
            var repos = new EduRepository<Notice>();

            var has = repos.Exists(p => p.Title == title && p.Id != id);
            return has ? new BoolMessage(false, "输入公告标题已经存在") : BoolMessage.True;
        }

        /// <summary>
        /// 添加对象
        /// </summary>
        /// <param name="entity">实体</param>
        public BoolMessage Create(Notice entity)
        {
            var repos = new EduRepository<Notice>();
            repos.Insert(entity);
            return BoolMessage.True;
        }

        /// <summary>
        /// 更新对象
        /// </summary>
        /// <param name="entity">实体</param>
        public BoolMessage Update(Notice entity)
        {
            var repos = new EduRepository<Notice>();
            repos.Update(entity);
            return BoolMessage.True;
        }

        /// <summary>
        /// 保存对象
        /// </summary>
        /// <param name="entity">实体</param>
        /// <param name="isCreate">是否新增</param>
        public BoolMessage Save(Notice entity, bool isCreate)
        {
            return isCreate ? Create(entity) : Update(entity);
        }

        /// <summary>
        /// 删除对象
        /// </summary>
        /// <param name="ids">主键数组</param>
        public BoolMessage Delete(string[] ids)
        {
            var repos = new EduRepository<Notice>();
            repos.Delete(ids);
            return BoolMessage.True;
        }

        /// <summary>
        /// 获取对象
        /// </summary>
        /// <param name="id">主键</param>
        public Notice Get(string id)
        {
            return new EduRepository<Notice>().Get(id);
        }

        public List<Notice> GetList(string title, DateTime? startDate, DateTime? endDate)
        {
            var repos = new EduRepository<Notice>();
            var query = repos.SQL.OrderByAsc(p => p.CreateDateTime);
            if (title.IsNotEmpty())
            {
                title = title.Trim();
                query.Where(p => p.Title.Contains(title));
            }
            if (startDate.HasValue)//开始日期
            {
                query.Where(p => p.CreateDateTime >= startDate.Value);
            }
            if (endDate.HasValue)//结束日期
            {
                query.Where(p => p.CreateDateTime <= endDate.Value.AddDays(1));
            }
            return repos.Query(query).ToList();
        }

        /// <summary>
        /// 获取对象分页集合
        /// </summary>
        /// <param name="pageCondition">分页对象</param>
        /// <param name="title">名称关键字</param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        public PageList<Notice> GetPageList(PaginationCondition pageCondition,
            string title,DateTime? startDate, DateTime? endDate)
        {
            pageCondition.SetDefaultOrder(nameof(Notice.CreateDateTime));
            var repos = new EduRepository<Notice>();
            var query = repos.PageQuery(pageCondition);
             
            if (title.IsNotEmpty())
            {
                title = title.Trim();
                query.Where(p => p.Title.Contains(title));
            }
            if (startDate.HasValue)//开始日期
            {
                query.Where(p => p.CreateDateTime >= startDate.Value);
            }
            if (endDate.HasValue)//结束日期
            {
                query.Where(p => p.CreateDateTime <= endDate.Value.AddDays(1));
            }
            return repos.Page(query);
        }
    }
}
