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
    /// 系统审计日志服务
    /// </summary>
    public class AuditService
    {
        /// <summary>
        /// 构造服务
        /// </summary>
        internal AuditService()
        {
        }

        /// <summary>
        /// 添加对象
        /// </summary>
        /// <param name="entity">实体</param>
        public BoolMessage Create(Audit entity)
        {
            var repos = new AuthRepository<Audit>();
            repos.Insert(entity);
            return BoolMessage.True;
        }

        /// <summary>
        /// 删除对象
        /// </summary>
        /// <param name="ids">主键数组</param>
        public BoolMessage Delete(string[] ids)
        {
            var repos = new AuthRepository<Audit>();
            repos.Delete(ids);
            return BoolMessage.True;
        }

        /// <summary>
        /// 获取对象
        /// </summary>
        /// <param name="id">主键</param>
        public Audit Get(string id)
        {
            return new AuthRepository<Audit>().Get(id);
        }

        /// <summary>
        /// 获取对象分页集合
        /// </summary>
        /// <param name="pageCondition">分页对象</param>
        /// <param name="startDate">开始日期</param>
        /// <param name="endDate">结束日期</param>
        /// <param name="ip">IP地址</param>
        /// <param name="userId">用户主键</param>
        public PageList<Audit> GetPageList(PaginationCondition pageCondition,
            DateTime? startDate, DateTime? endDate, string ip, string userId)
        {
            pageCondition.SetDefaultOrder(nameof(Audit.CreateDateTime));
            var repos = new AuthRepository<Audit>();
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

            if (userId.IsNotEmpty())
            {
                userId = userId.Trim();
                query.Where(p => p.UserId.Contains(userId));
            }
            return repos.Page(query);
        }
    }
}
    