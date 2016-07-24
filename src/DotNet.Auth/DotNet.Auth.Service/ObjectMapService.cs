// ===============================================================================
// DotNet.Platform 开发框架 2016 版权所有
// ===============================================================================

using System.Collections.Generic;
using System.Linq;
using DotNet.Edu.Entity;
using DotNet.Utility;
using DotNet.Auth.Service;
using DotNet.Data;

namespace DotNet.Edu.Service
{
    /// <summary>
    /// 对象资源映射服务
    /// </summary>
    public class ObjectMapService
    {
        /// <summary>
        /// 添加对象
        /// </summary>
        /// <param name="entity">实体</param>
        public BoolMessage Create(List<ObjectMap> list)
        {
            var repos = new AuthRepository<ObjectMap>();
            try
            {
                DbSession.Begin(new AuthDatabase());
                foreach (var entity in list)
                {
                    repos.Insert(entity);
                }
                DbSession.Commit();
                return BoolMessage.True;
            }
            catch
            {
                DbSession.Rollback();
                throw;
            }
        }

        /// <summary>
        /// 删除对象
        /// </summary>
        public BoolMessage Delete(string objectName, string objectId)
        {
            var repos = new AuthRepository<ObjectMap>();
            repos.Delete(p => p.ObjectName == objectName && p.ObjectId == objectId);
            return BoolMessage.True;
        }

        /// <summary>
        /// 获取对象数组
        /// </summary>
        public string[] GetTargetIds(string objectName, string objectId, string targetName)
        {
            var repos = new AuthRepository<ObjectMap>();
            var query = repos.SQL
                .Where(p => p.ObjectName == objectName && p.ObjectId == objectId && p.TargetName == targetName)
                .Select(p => p.TargetId);
            return repos.Query(query).Select(p=>p.TargetId).ToArray();
        }
    }
}
