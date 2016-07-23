// ===============================================================================
// DotNet.Platform 开发框架 2016 版权所有
// ===============================================================================

using System.Collections.Generic;
using System.Linq;
using DotNet.Auth.Utility;
using DotNet.Edu.Entity;
using DotNet.Collections;
using DotNet.Extensions;
using DotNet.Utility;

namespace DotNet.Edu.Service
{
    /// <summary>
    /// 企业基础信息服务
    /// </summary>
    public class CompanyService
    {
        private static readonly Cache<string, Company> Cache = new Cache<string, Company>();

        /// <summary>
        /// 构造服务
        /// </summary>
        internal CompanyService()
        {
            InitCache();
        }

        /// <summary>
        /// 初始化缓存
        /// </summary>
        public void InitCache()
        {
            Cache.Clear().Set(new EduRepository<Company>().Query().ToDictionary(p => p.Id, p => p));
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
            return has ? new BoolMessage(false, "指定的企业名称已经存在") : BoolMessage.True;
        }

        /// <summary>
        /// 添加对象
        /// </summary>
        /// <param name="entity">实体</param>
        public BoolMessage Create(Company entity)
        {
            var repos = new EduRepository<Company>();
            repos.Insert(entity);
            Cache.Set(entity.Id, entity);
            return BoolMessage.True;
        }

        /// <summary>
        /// 更新对象
        /// </summary>
        /// <param name="entity">实体</param>
        public BoolMessage Update(Company entity)
        {
            var repos = new EduRepository<Company>();
            repos.Update(entity);
            Cache.Set(entity.Id, entity);
            return BoolMessage.True;
        }

        /// <summary>
        /// 保存对象
        /// </summary>
        /// <param name="entity">实体</param>
        /// <param name="isCreate">是否新增</param>
        public BoolMessage Save(Company entity, bool isCreate)
        {
            return isCreate ? Create(entity) : Update(entity);
        }

        /// <summary>
        /// 删除对象
        /// </summary>
        /// <param name="ids">主键数组</param>
        public BoolMessage Delete(string[] ids)
        {
            var repos = new EduRepository<Company>();
            repos.Delete(ids);
            Cache.Remove(ids);
            return BoolMessage.True;
        }

        /// <summary>
        /// 获取对象
        /// </summary>
        /// <param name="id">主键</param>
        public Company Get(string id)
        {
            return Cache.Get(id);
        }

        /// <summary>
        /// 获取主键
        /// </summary>
        /// <param name="name">培训机构名称</param>
        public string GetIdByName(string name)
        {
            return Cache.ValueList().FirstOrDefault(p => p.Name.Equals(name))?.Id;
        }

        /// <summary>
        /// 获取启用的简单对象集合(已排序)
        /// </summary>
        public List<Simple> GetSimpleList()
        {
            var list = Cache.ValueList().Where(p => p.IsEnabled).ToList();
            var user = AuthHelper.GetSessionUser();
            if (user.IsCompany)
            {
                list = Cache.ValueList().Where(p => p.Id.Equals(user.User.CompanyId)).ToList();
            }
            return list.OrderByAsc(p => p.Name).Select(p => new Simple(p.Id, p.Name, p.Spell)).ToList();
        }

        /// <summary>
        /// 获取启用的对象集合(已排序)
        /// </summary>
        public List<Company> GetList()
        {
            return Cache.ValueList()
                .Where(p => p.IsEnabled).ToList()
                .OrderByAsc(p => p.CreateDateTime);
        }
        
        /// <summary>
        /// 获取对象分页集合
        /// </summary>
        /// <param name="pageCondition">分页对象</param>
        /// <param name="name">名称关键字</param>
        /// <param name="enabled">启用</param>
        public PageList<Company> GetPageList(PaginationCondition pageCondition,
            string name,bool? enabled)
        {
            pageCondition.SetDefaultOrder(nameof(Company.CreateDateTime));
            var repos = new EduRepository<Company>();
            var query = repos.PageQuery(pageCondition);
            if (enabled.HasValue)
            {
                query.Where(p => p.IsEnabled == enabled.Value);
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
