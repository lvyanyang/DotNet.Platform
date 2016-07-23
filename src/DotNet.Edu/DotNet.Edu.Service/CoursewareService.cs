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
    /// 课件服务
    /// </summary>
    public class CoursewareService
    {
        private static readonly Cache<string, Courseware> Cache = new Cache<string, Courseware>();

        /// <summary>
        /// 构造服务
        /// </summary>
        internal CoursewareService()
        {
            InitCache();
        }

        /// <summary>
        /// 初始化缓存
        /// </summary>
        public void InitCache()
        {
            Cache.Clear().Set(new EduRepository<Courseware>().Query().ToDictionary(p => p.Id, p => p));
        }

        /// <summary>
        /// 是否存在指定名称的对象
        /// </summary>
        /// <param name="id">主键</param>
        /// <param name="name">名称</param>
        /// <returns>如果存在返回false</returns>
        public BoolMessage ExistsByName(string id, string name)
        {
            var has = Cache.ValueList().Contains(p => p.Name.Equals(name) && !p.Id.Equals(id));
            return has ? new BoolMessage(false, "指定的课件名称已经存在") : BoolMessage.True;
        }

        /// <summary>
        /// 添加对象
        /// </summary>
        /// <param name="entity">实体</param>
        public BoolMessage Create(Courseware entity)
        {
            var repos = new EduRepository<Courseware>();
            repos.Insert(entity);
            Cache.Set(entity.Id, entity);
            return BoolMessage.True;
        }

        /// <summary>
        /// 更新对象
        /// </summary>
        /// <param name="entity">实体</param>
        public BoolMessage Update(Courseware entity)
        {
            var repos = new EduRepository<Courseware>();
            repos.Update(entity);
            Cache.Set(entity.Id, entity);
            return BoolMessage.True;
        }

        /// <summary>
        /// 保存对象
        /// </summary>
        /// <param name="entity">实体</param>
        /// <param name="isCreate">是否新增</param>
        public BoolMessage Save(Courseware entity, bool isCreate)
        {
            return isCreate ? Create(entity) : Update(entity);
        }

        /// <summary>
        /// 删除对象
        /// </summary>
        /// <param name="ids">主键数组</param>
        public BoolMessage Delete(string[] ids)
        {
            var repos = new EduRepository<Courseware>();
            foreach (var id in ids)
            {
                var details = EduService.CoursewareDetails.GetList(id);
                foreach (var item in details)
                {
                    EduService.CoursewareDetails.Delete(item.Id);
                }
            }
            repos.Delete(ids);
            Cache.Remove(ids);
            return BoolMessage.True;
        }

        /// <summary>
        /// 获取对象
        /// </summary>
        /// <param name="id">主键</param>
        public Courseware Get(string id)
        {
            return Cache.Get(id);
        }
        
        /// <summary>
        /// 获取新建序号
        /// </summary>
        public int GetNewRowIndex()
        {
            return Cache.ValueList().Max(p => p.RowIndex, 1) + 1;
        }

        /// <summary>
        /// 获取启用的简单对象集合(已排序)
        /// </summary>
        public List<Simple> GetSimpleList()
        {
            return Cache.ValueList()
                .OrderByAsc(p => p.RowIndex)
                .Select(p => new Simple(p.Id, p.Name, p.Spell))
                .ToList();
        }
       

        /// <summary>
        /// 获取启用的对象集合(已排序)
        /// </summary>
        public List<Courseware> GetList()
        {
            return Cache.ValueList().OrderByAsc(p => p.RowIndex);
        }

        /// <summary>
        /// 获取对象分页集合
        /// </summary>
        /// <param name="pageCondition">分页对象</param>
        /// <param name="name">名称关键字</param>
        /// <param name="workType">从业类型</param>
        /// <param name="courseType">课件类型</param>
        public PageList<Courseware> GetPageList(PaginationCondition pageCondition,
            string name,int? workType, int? courseType)
        {
            pageCondition.SetDefaultOrder(nameof(Courseware.RowIndex));
            var repos = new EduRepository<Courseware>();
            var query = repos.PageQuery(pageCondition);
            if (workType.HasValue)
            {
                query.Where(p => p.WorkType == workType.Value);
            }
            if (courseType.HasValue)
            {
                query.Where(p => p.CourseType == courseType.Value);
            }
            if (name.IsNotEmpty())
            {
                name = name.Trim();
                query.Where(p => p.Name.Contains(name) || p.Spell.Contains(name));
            }
            return repos.Page(query);
        }
    }
}
