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
    /// 系统序列服务
    /// </summary>
    public class SeqService
    {
        private static readonly Cache<string, Seq> Cache = new Cache<string, Seq>();
        private static readonly object lockObject = new object();

        /// <summary>
        /// 构造服务
        /// </summary>
        internal SeqService()
        {
            InitCache();
        }

        /// <summary>
        /// 初始化缓存
        /// </summary>
        public void InitCache()
        {
            var repos = new AuthRepository<Seq>();
            Cache.Clear().Set(repos.Query().ToDictionary(p => p.Id, p => p));
        }

        /// <summary>
        /// 获取序列值,完成后更新序列值
        /// </summary>
        /// <param name="name">名称</param>
        public int GetNextValue(string name)
        {
            lock (lockObject)
            {
                var repos = new AuthRepository<Seq>();
                var entity = GetByName(name);
                if (entity == null)
                {
                    entity = new Seq();
                    entity.Id = StringHelper.Guid();
                    entity.Name = name;
                    entity.Value = 2;
                    entity.Step = 1;
                    repos.Insert(entity);
                    Cache.Set(entity.Id, entity);
                    return 1;
                }

                var v = entity.Value;
                entity.Value++; //此处会自动更新缓存对象,所以无需再次Cache.Set
                repos.Update(entity, p => p.Value);
                return v;
            }
        }

        /// <summary>
        /// 获取当前序列值,完成后不更新序列值
        /// </summary>
        /// <param name="name">名称</param>
        public int GetCurrentValue(string name)
        {
            var entity = GetByName(name);
            return entity?.Value ?? 0;
        }

        /// <summary>
        /// 是否存在指定名称的序列
        /// </summary>
        /// <param name="name">名称</param>
        public bool Contain(string name)
        {
            return Cache.ValueList().Contains(p => p.Name.Equals(name));
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
            return has ? new BoolMessage(false, "指定的序列名称已经存在") : BoolMessage.True;
        }

        /// <summary>
        /// 添加对象
        /// </summary>
        /// <param name="entity">实体</param>
        public BoolMessage Create(Seq entity)
        {
            var repos = new AuthRepository<Seq>();
            repos.Insert(entity);
            Cache.Set(entity.Id, entity);
            return BoolMessage.True;
        }

        /// <summary>
        /// 更新对象
        /// </summary>
        /// <param name="entity">实体</param>
        public BoolMessage Update(Seq entity)
        {
            var repos = new AuthRepository<Seq>();
            repos.Update(entity);
            Cache.Set(entity.Id, entity);
            return BoolMessage.True;
        }

        /// <summary>
        /// 保存对象
        /// </summary>
        /// <param name="entity">实体</param>
        /// <param name="isCreate">是否新增</param>
        public BoolMessage Save(Seq entity, bool isCreate)
        {
            return isCreate ? Create(entity) : Update(entity);
        }

        /// <summary>
        /// 删除对象
        /// </summary>
        /// <param name="ids">主键数组</param>
        public BoolMessage Delete(string[] ids)
        {
            var repos = new AuthRepository<Seq>();
            repos.Delete(ids);
            Cache.Remove(ids);
            return BoolMessage.True;
        }

        /// <summary>
        /// 获取对象
        /// </summary>
        /// <param name="id">主键</param>
        public Seq Get(string id)
        {
            return Cache.Get(id);
        }

        /// <summary>
        /// 获取指定名称的对象
        /// </summary>
        /// <param name="name">名称</param>
        public Seq GetByName(string name)
        {
            return Cache.ValueList().FirstOrDefault(p => p.Name.Equals(name));
        }

        /// <summary>
        /// 获取对象集合
        /// </summary>
        public List<Seq> GetList()
        {
            return Cache.ValueList();
        }

        /// <summary>
        /// 获取对象分页集合
        /// </summary>
        /// <param name="pageCondition">分页对象</param>
        /// <param name="name">名称</param>
        public PageList<Seq> GetPageList(PaginationCondition pageCondition, string name)
        {
            pageCondition.SetDefaultOrder(nameof(Seq.Name), false);
            var repos = new AuthRepository<Seq>();
            var query = repos.PageQuery(pageCondition);
            if (name.IsNotEmpty())
            {
                name = name.Trim();
                query.Where(p => p.Name.Contains(name));
            }
            return repos.Page(query);
        }
    }
}