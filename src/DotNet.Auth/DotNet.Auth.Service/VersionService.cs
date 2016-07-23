// ===============================================================================
// DotNet.Platform 开发框架 2016 版权所有
// ===============================================================================

using DotNet.Auth.Entity;
using DotNet.Collections;
using DotNet.Extensions;
using DotNet.Utility;

namespace DotNet.Auth.Service
{
    /// <summary>
    /// 系统数据版本服务
    /// </summary>
    public class VersionService
    {
        /// <summary>
        /// 构造服务
        /// </summary>
        internal VersionService()
        {
        }

        /// <summary>
        /// 添加对象
        /// </summary>
        /// <param name="entity">实体</param>
        public BoolMessage Create(Version entity)
        {
            var repos = new AuthRepository<Role>();
            repos.Insert(entity);
            return BoolMessage.True;
        }

        /// <summary>
        /// 获取对象
        /// </summary>
        /// <param name="id">主键</param>
        public Version Get(string id)
        {
            return new AuthRepository<Version>().Get(id);
        }

        /// <summary>
        /// 获取对象分页集合
        /// </summary>
        /// <param name="pageCondition">分页对象</param>
        /// <param name="tableName">表名</param>
        /// <param name="pkValue">主键值</param>
        public PageList<Version> GetPageList(PaginationCondition pageCondition,
            string tableName, string pkValue)
        {
            pageCondition.SetDefaultOrder(nameof(Version.CreateDateTime));
            var repos = new AuthRepository<Version>();
            var query = repos.PageQuery(pageCondition);

            if (tableName.IsNotEmpty())
            {
                tableName = tableName.Trim();
                query.Where(p => p.TableName == tableName);
            }
            if (pkValue.IsNotEmpty())
            {
                pkValue = pkValue.Trim();
                query.Where(p => p.KeyValue == pkValue);
            }
            return repos.Page(query);
        }
    }
}
