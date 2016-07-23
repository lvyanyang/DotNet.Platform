// ===============================================================================
// DotNet.Platform 开发框架 2016 版权所有
// ===============================================================================
using System;
using DotNet.Auth.Entity;
using DotNet.Collections;
using DotNet.Extensions;
using DotNet.Utility;

namespace DotNet.Auth.Service
{
    /// <summary>
    /// 系统日志服务
    /// </summary>
    public class LogService
    {
        /// <summary>
        /// 构造服务
        /// </summary>
        internal LogService() { }

        /// <summary>
        /// 添加对象
        /// </summary>
        /// <param name="entity">实体</param>
        public BoolMessage Create(Log entity)
        {
            var repos = new AuthRepository<Log>();
            repos.Insert(entity);
            return BoolMessage.True;
        }

        /// <summary>
        /// 删除对象
        /// </summary>
        /// <param name="ids">主键数组</param>
        public BoolMessage Delete(string[] ids)
        {
            var repos = new AuthRepository<Log>();
            repos.Delete(ids);
            return BoolMessage.True;
        }

        /// <summary>
        /// 获取对象
        /// </summary>
        /// <param name="id">主键</param>
        public Log Get(string id)
        {
            var repos = new AuthRepository<Log>();
            return repos.Get(id);
        }

        /// <summary>
        /// 获取对象分页集合
        /// </summary>
        /// <param name="pageCondition">分页对象</param>
        /// <param name="startDate">开始日期</param>
        /// <param name="endDate">结束日期</param>
        /// <param name="ip">IP地址</param>
        /// <param name="title">标题关键字</param>
        public PageList<Log> GetPageList(PaginationCondition pageCondition,
            DateTime? startDate, DateTime? endDate, string ip, string title)
        {
            pageCondition.SetDefaultOrder(nameof(Log.CreateDateTime));
            var repos = new AuthRepository<Log>();
            var query = repos.PageQuery(pageCondition);
            if (startDate.HasValue)
            {
                var startDateDt = startDate.ToDateTime();
                query.Where(p => p.CreateDateTime >= startDateDt);
            }

            if (endDate.HasValue)
            {
                var endDateDt = endDate.ToDateTime().AddDays(1);
                query.Where(p => p.CreateDateTime <= endDateDt);
            }

            if (ip.IsNotEmpty())
            {
                ip = ip.Trim();
                query.Where(p => p.IPAddress.Contains(ip));
            }

            if (title.IsNotEmpty())
            {
                title = title.Trim();
                query.Where(p => p.Title.Contains(title));
            }
            return repos.Page(query);
        }
    }
}