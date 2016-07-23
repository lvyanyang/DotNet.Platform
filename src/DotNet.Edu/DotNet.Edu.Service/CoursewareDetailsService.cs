// ===============================================================================
// DotNet.Platform 开发框架 2016 版权所有
// ===============================================================================

using System.Collections.Generic;
using System.Linq;
using DotNet.Edu.Entity;
using DotNet.Helper;
using DotNet.Utility;

namespace DotNet.Edu.Service
{
    /// <summary>
    /// 课件明细服务
    /// </summary>
    public class CoursewareDetailsService
    {
        /// <summary>
        /// 构造服务
        /// </summary>
        internal CoursewareDetailsService()
        {
        }
        
        /// <summary>
        /// 添加对象
        /// </summary>
        /// <param name="entity">实体</param>
        public BoolMessage Create(CoursewareDetails entity)
        {
            var repos = new EduRepository<CoursewareDetails>();
            repos.Insert(entity);
            return BoolMessage.True;
        }

        /// <summary>
        /// 删除对象
        /// </summary>
        /// <param name="id">主键</param>
        public BoolMessage Delete(string id)
        {
            var repos = new EduRepository<CoursewareDetails>();
            var entity = repos.Get(id);
            if (entity != null)
            {
                FileHelper.DeleteFile(WebHelper.MapPath(entity.Url));
                repos.Delete(id);
                return BoolMessage.True;
            }
            return new BoolMessage(false,"此课件已被删除");
        }

        /// <summary>
        /// 获取对象
        /// </summary>
        /// <param name="id">主键</param>
        public CoursewareDetails Get(string id)
        {
            return new EduRepository<CoursewareDetails>().Get(id);
        }

        /// <summary>
        /// 获取启用的对象集合(已排序)
        /// </summary>
        public List<CoursewareDetails> GetList(string coursewareId)
        {
            var repos = new EduRepository<CoursewareDetails>();
            var query = repos.SQL.Where(p=>p.CourseId== coursewareId).OrderByAsc(p => p.RowIndex);
            return repos.Query(query).ToList();
        }

        /// <summary>
        /// 保存对象顺序
        /// </summary>
        /// <param name="sortPaths">更改的数据</param>
        public BoolMessage SaveRowIndex(PrimaryKeyValue[] sortPaths)
        {
            var repos = new EduRepository<CoursewareDetails>();
            repos.BatchUpdate(sortPaths);
            return BoolMessage.True;
        }
    }
}
