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
    /// 教师服务
    /// </summary>
    public class TeacherService
    {
        private static readonly Cache<string, Teacher> Cache = new Cache<string, Teacher>();

        /// <summary>
        /// 构造服务
        /// </summary>
        internal TeacherService()
        {
            InitCache();
        }

        /// <summary>
        /// 初始化缓存
        /// </summary>
        public void InitCache()
        {
            Cache.Clear().Set(new EduRepository<Teacher>().Query().ToDictionary(p => p.Id, p => p));
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
            return has ? new BoolMessage(false, "指定的教师姓名已经存在") : BoolMessage.True;
        }

        /// <summary>
        /// 添加对象
        /// </summary>
        /// <param name="entity">实体</param>
        public BoolMessage Create(Teacher entity)
        {
            var repos = new EduRepository<Teacher>();
            repos.Insert(entity);
            Cache.Set(entity.Id, entity);
            return BoolMessage.True;
        }

        /// <summary>
        /// 更新对象
        /// </summary>
        /// <param name="entity">实体</param>
        public BoolMessage Update(Teacher entity)
        {
            var repos = new EduRepository<Teacher>();
            repos.Update(entity);
            Cache.Set(entity.Id, entity);
            return BoolMessage.True;
        }

        /// <summary>
        /// 保存对象
        /// </summary>
        /// <param name="entity">实体</param>
        /// <param name="isCreate">是否新增</param>
        public BoolMessage Save(Teacher entity, bool isCreate)
        {
            return isCreate ? Create(entity) : Update(entity);
        }

        /// <summary>
        /// 删除对象
        /// </summary>
        /// <param name="ids">主键数组</param>
        public BoolMessage Delete(string[] ids)
        {
            var repos = new EduRepository<Teacher>();
            repos.Delete(ids);
            Cache.Remove(ids);
            return BoolMessage.True;
        }

        /// <summary>
        /// 获取对象
        /// </summary>
        /// <param name="id">主键</param>
        public Teacher Get(string id)
        {
            return Cache.Get(id);
        }

        /// <summary>
        /// 获取启用的简单对象集合(已排序)
        /// </summary>
        public List<Simple> GetSimpleList(string schoolId)
        {
            return Cache.ValueList()
                .Where(p => p.SchoolId == schoolId).ToList()
                .OrderByAsc(p => p.CreateDateTime)
                .Select(p => new Simple(p.Id, p.Name))
                .ToList();
        }

        /// <summary>
        /// 获取对象集合
        /// </summary>
        public List<Teacher> GetList(string schoolId)
        {
            if (schoolId.IsNotEmpty())
            {
                return Cache.ValueList().Where(p => p.SchoolId == schoolId).ToList();
            }
            return Cache.ValueList();
        }

        /// <summary>
        /// 获取对象分页集合
        /// </summary>
        /// <param name="pageCondition">分页对象</param>
        /// <param name="schoolId"></param>
        /// <param name="name">名称关键字</param>
        public PageList<Teacher> GetPageList(PaginationCondition pageCondition, string schoolId, string name)
        {
            pageCondition.SetDefaultOrder(nameof(Teacher.CreateDateTime));
            var repos = new EduRepository<Teacher>();
            var query = repos.PageQuery(pageCondition);
            if (schoolId.IsNotEmpty())
            {
                query.Where(p => p.SchoolId == schoolId);
            }

            if (name.IsNotEmpty())
            {
                name = name.Trim();
                query.Where(p => p.Name.Contains(name));
            }
            return repos.Page(query);
        }
    }
}
