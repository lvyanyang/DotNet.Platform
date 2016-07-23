// ===============================================================================
// DotNet.Platform 开发框架 2016 版权所有
// ===============================================================================
using System.Collections.Generic;
using System.Linq;
using DotNet.Auth.Entity;
using DotNet.Collections;
using DotNet.Extensions;
using DotNet.Utility;

namespace DotNet.Auth.Service
{
    /// <summary>
    /// 系统角色服务
    /// </summary>
    public class RoleService
    {
        private static readonly Cache<string, Role> Cache = new Cache<string, Role>();

        /// <summary>
        /// 构造服务
        /// </summary>
        internal RoleService()
        {
            InitCache();
        }

        /// <summary>
        /// 初始化缓存
        /// </summary>
        public void InitCache()
        {
            Cache.Clear().Set(new AuthRepository<Role>().Query().ToDictionary(p => p.Id, p => p));
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
            return has ? new BoolMessage(false, "指定的角色名称已经存在") : BoolMessage.True;
        }

        /// <summary>
        /// 添加对象
        /// </summary>
        /// <param name="entity">实体</param>
        public BoolMessage Create(Role entity)
        {
            var repos = new AuthRepository<Role>();
            repos.Insert(entity);
            Cache.Set(entity.Id, entity);
            return BoolMessage.True;
        }

        /// <summary>
        /// 更新对象
        /// </summary>
        /// <param name="entity">实体</param>
        public BoolMessage Update(Role entity)
        {
            var repos = new AuthRepository<Role>();
            repos.Update(entity);
            Cache.Set(entity.Id, entity);
            return BoolMessage.True;
        }

        /// <summary>
        /// 保存对象
        /// </summary>
        /// <param name="entity">实体</param>
        /// <param name="isCreate">是否新增</param>
        public BoolMessage Save(Role entity, bool isCreate)
        {
            return isCreate ? Create(entity) : Update(entity);
        }

        /// <summary>
        /// 删除对象
        /// </summary>
        /// <param name="ids">主键数组</param>
        public BoolMessage Delete(string[] ids)
        {
            var repos = new AuthRepository<Role>();
            repos.Delete(ids);
            Cache.Remove(ids);
            return BoolMessage.True;
        }

        /// <summary>
        /// 获取对象
        /// </summary>
        /// <param name="id">主键</param>
        public Role Get(string id)
        {
            return Cache.Get(id);
        }

        /// <summary>
        /// 获取新建序号
        /// </summary>
        public int GetNewRowIndex()
        {
            return Cache.ValueList().Max(p => p.RowIndex, 0) + 1;
        }

        /// <summary>
        /// 获取启用的简单对象集合(已排序)
        /// </summary>
        public List<Simple> GetSimpleList()
        {
            return Cache.ValueList()
                .Where(p => p.IsEnabled).ToList()
                .OrderByAsc(p => p.RowIndex)
                .Select(p => new Simple(p.Id, p.Name, p.Spell))
                .ToList();
        }

        /// <summary>
        /// 获取启用的对象集合(已排序)
        /// </summary>
        public List<Role> GetList()
        {
            return Cache.ValueList()
                .Where(p => p.IsEnabled).ToList()
                .OrderByAsc(p => p.RowIndex);
        }

        /// <summary>
        /// 获取对象分页集合
        /// </summary>
        /// <param name="pageCondition">分页对象</param>
        /// <param name="name">名称关键字</param>
        /// <param name="categoryId">分类主键</param>
        /// <param name="isEnabled">启用</param>
        public PageList<Role> GetPageList(PaginationCondition pageCondition,
            string name, string categoryId, bool? isEnabled)
        {
            pageCondition.SetDefaultOrder(nameof(Role.RowIndex));
            var repos = new AuthRepository<Role>();
            var query = repos.PageQuery(pageCondition);
            if (isEnabled.HasValue)
            {
                query.Where(p => p.IsEnabled == isEnabled.Value);
            }
            if (name.IsNotEmpty())
            {
                name = name.Trim();
                query.Where(p => p.Name.Contains(name) || p.Spell.Contains(name));
            }
            if (categoryId.IsNotEmpty())
            {
                categoryId = categoryId.Trim();
                query.Where(p => p.CategoryId == categoryId);
            }
            return repos.Page(query);
        }
    }
}