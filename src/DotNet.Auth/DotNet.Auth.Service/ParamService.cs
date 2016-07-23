// ===============================================================================
// DotNet.Platform 开发框架 2016 版权所有
// ===============================================================================

using System.Collections.Generic;
using System.Linq;
using DotNet.Auth.Entity;
using DotNet.Collections;
using DotNet.Extensions;
using DotNet.Helper;
using DotNet.Utility;

namespace DotNet.Auth.Service
{
    /// <summary>
    /// 系统参数服务
    /// </summary>
    public class ParamService
    {
        private static readonly Cache<string, Param> Cache = new Cache<string, Param>();

        /// <summary>
        /// 构造服务
        /// </summary>
        internal ParamService()
        {
            InitCache();
        }

        /// <summary>
        /// 初始化缓存
        /// </summary>
        public void InitCache()
        {
            Cache.Clear().Set(new AuthRepository<Param>().Query().ToDictionary(p => p.Id, p => p));
        }

        /// <summary>
        /// 获取参数值
        /// </summary>
        /// <param name="code">编码</param>
        /// <param name="defaultValue">找不到时返回的默认值</param>
        public string Get(string code, string defaultValue = null)
        {
            var entity = GetByCode(code);
            return entity == null ? defaultValue : entity.Value;
        }

        /// <summary>
        /// 设置参数值
        /// </summary>
        /// <param name="code">编码</param>
        /// <param name="value">值</param>
        public void Set(string code, string value)
        {
            var entity = GetByCode(code);
            if (entity != null)
            {
                entity.Value = value;
                Update(entity);
            }
            else
            {
                Create(new Param
                {
                    Id = StringHelper.Guid(),
                    Code = code,
                    Name = code,
                    Value = value
                });
            }
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
            return has ? new BoolMessage(false, "指定的参数名称已经存在") : BoolMessage.True;
        }

        /// <summary>
        /// 添加对象
        /// </summary>
        /// <param name="entity">实体</param>
        public BoolMessage Create(Param entity)
        {
            var repos = new AuthRepository<Param>();
            repos.Insert(entity);
            Cache.Set(entity.Id, entity);
            return BoolMessage.True;
        }

        /// <summary>
        /// 更新对象
        /// </summary>
        /// <param name="entity">实体</param>
        public BoolMessage Update(Param entity)
        {
            var repos = new AuthRepository<Param>();
            repos.Update(entity);
            Cache.Set(entity.Id, entity);
            return BoolMessage.True;
        }

        /// <summary>
        /// 保存对象
        /// </summary>
        /// <param name="entity">实体</param>
        /// <param name="isCreate">是否新增</param>
        public BoolMessage Save(Param entity, bool isCreate)
        {
            return isCreate ? Create(entity) : Update(entity);
        }

        /// <summary>
        /// 删除对象
        /// </summary>
        /// <param name="ids">主键数组</param>
        public BoolMessage Delete(string[] ids)
        {
            var repos = new AuthRepository<Param>();
            repos.Delete(ids);
            Cache.Remove(ids);
            return BoolMessage.True;
        }

        /// <summary>
        /// 获取对象
        /// </summary>
        /// <param name="id">主键</param>
        public Param GetById(string id)
        {
            return Cache.Get(id);
        }

        /// <summary>
        /// 获取对象
        /// </summary>
        /// <param name="code">编码</param>
        public Param GetByCode(string code)
        {
            return Cache.ValueList().FirstOrDefault(p => p.Code.Equals(code));
        }

        /// <summary>
        /// 获取启用的对象集合(已排序)
        /// </summary>
        public List<Param> GetList()
        {
            return Cache.ValueList().OrderByAsc(p => p.Name);
        }

        /// <summary>
        /// 获取对象分页集合
        /// </summary>
        /// <param name="pageCondition">分页对象</param>
        /// <param name="name">名称关键字</param>
        /// <param name="code">编码关键字</param>
        /// <param name="categoryId">分类主键</param>
        public PageList<Param> GetPageList(PaginationCondition pageCondition,
            string name, string code, string categoryId)
        {
            pageCondition.SetDefaultOrder(nameof(Param.CreateDateTime));
            var repos = new AuthRepository<Param>();
            var query = repos.PageQuery(pageCondition);
            if (name.IsNotEmpty())
            {
                name = name.Trim();
                query.Where(p => p.Name.Contains(name));
            }
            if (code.IsNotEmpty())
            {
                code = code.Trim();
                query.Where(p => p.Code.Contains(code));
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
