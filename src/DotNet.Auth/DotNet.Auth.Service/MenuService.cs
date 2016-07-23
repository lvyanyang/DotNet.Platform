// ===============================================================================
// DotNet.Platform 开发框架 2016 版权所有
// ===============================================================================
using System;
using System.Collections.Generic;
using System.Linq;
using DotNet.Auth.Entity;
using DotNet.Collections;
using DotNet.Data;
using DotNet.Data.Extensions;
using DotNet.Extensions;
using DotNet.Helper;
using DotNet.Utility;

namespace DotNet.Auth.Service
{
    /// <summary>
    /// 系统菜单服务
    /// </summary>
    public class MenuService
    {
        private static readonly Cache<string, Menu> Cache = new Cache<string, Menu>();

        /// <summary>
        /// 构造服务
        /// </summary>
        internal MenuService()
        {
            InitCache();
        }

        /// <summary>
        /// 初始化缓存
        /// </summary>
        public void InitCache()
        {
            var repos = new AuthRepository<Menu>();
            Cache.Clear().Set(repos.Query().ToDictionary(p => p.Id, p => p));
        }

        /// <summary>
        /// 是否存在指定编码的对象
        /// </summary>
        /// <param name="id">主键</param>
        /// <param name="code">编码</param>
        /// <returns>存在返回false</returns>
        public BoolMessage ExistsByCode(string id, string code)
        {
            var has = Cache.ValueList().Contains(p => p.Code.Equals(code) && !p.Id.Equals(id));
            return has ? new BoolMessage(false, "指定的菜单编码已经存在") : BoolMessage.True;
        }

        /// <summary>
        /// 添加对象
        /// </summary>
        /// <param name="entity">实体</param>
        public BoolMessage Create(Menu entity)
        {
            var repos = new AuthRepository<Menu>();
            repos.Insert(entity);
            Cache.Set(entity.Id, entity);
            return BoolMessage.True;
        }

        /// <summary>
        /// 更新对象
        /// </summary>
        /// <param name="entity">实体</param>
        public BoolMessage Update(Menu entity)
        {
            var repos = new AuthRepository<Menu>();
            repos.Update(entity);
            Cache.Set(entity.Id, entity);
            return BoolMessage.True;
        }

        /// <summary>
        /// 保存对象
        /// </summary>
        /// <param name="entity">实体</param>
        /// <param name="isCreate">是否新增</param>
        public BoolMessage Save(Menu entity, bool isCreate)
        {
            return isCreate ? Create(entity) : Update(entity);
        }

        /// <summary>
        /// 删除对象(删除包括所有子节点以及所有子节点)
        /// </summary>
        /// <param name="id">主键</param>
        public BoolMessage Delete(string id)
        {
            if (!Cache.Contains(id)) return new BoolMessage(false, "找不到指定主键的数据");
            var childs = GetChildNodeList(id, true);
            var ids = childs.Select(p => p.Id).ToArray();
            var repos = new AuthRepository<Menu>();
            repos.Delete(ids);
            Cache.Remove(ids);
            return BoolMessage.True;
        }

        /// <summary>
        /// 保存指定对象父节点
        /// </summary>
        /// <param name="id">主键</param>
        /// <param name="newParentId">新父节点主键</param>
        public BoolMessage SaveParent(string id, string newParentId)
        {
            var repos = new AuthRepository<Menu>();
            repos.Update(new Menu { Id = id, ParentId = newParentId }, p => p.ParentId);
            var entity = Cache.Get(id);
            if (entity != null) entity.ParentId = newParentId;
            return BoolMessage.True;
        }

        /// <summary>
        /// 保存对象节点排序路径
        /// </summary>
        /// <param name="sortPaths">更改的数据</param>
        public BoolMessage SaveSortPath(PrimaryKeyValue[] sortPaths)
        {
            var repos = new AuthRepository<Menu>();
            repos.BatchUpdate(sortPaths);
            sortPaths.ForEach(p =>
            {
                var entity = Cache.Get(p.Id);
                if (entity != null) entity.SortPath = p.Value;
            });
            return BoolMessage.True;
        }

        /// <summary>
        /// 获取对象
        /// </summary>
        /// <param name="id">主键</param>
        public Menu Get(string id)
        {
            return Cache.Get(id);
        }

        /// <summary>
        /// 获取对象
        /// </summary>
        /// <param name="code">字典编码</param>
        public Menu GetByCode(string code)
        {
            return Cache.ValueList().FirstOrDefault(p => p.Code == code);
        }

        /// <summary>
        /// 获取新建节点排序路径
        /// </summary>
        /// <param name="parendId">新建节点的父主键</param>
        public string GetNewSortPath(string parendId)
        {
            var entity = Cache.Get(parendId);
            return TreeHelper.GetNewNodeSortPath(Cache.ValueList(), entity);
        }

        /// <summary>
        /// 获取启用的对象集合
        /// </summary>
        public List<Menu> GetList(string name = null)
        {
            if (string.IsNullOrEmpty(name))
            {
                return Cache.ValueList().OrderByAsc(p => p.SortPath);
            }
            return Cache.ValueList().Where(p => p.Name.Contains(name) || (!string.IsNullOrEmpty(p.Spell)&& p.Spell.Contains(name))).ToList().OrderByAsc(p => p.SortPath);
        }

        /// <summary>
        /// 获取指定节点主键的所有子节点集合
        /// </summary>
        /// <param name="id">节点主键</param>
        /// <param name="containSelf">是否包含指定节点对象</param>
        public List<Menu> GetChildNodeList(string id, bool containSelf)
        {
            var entity = Cache.Get(id);
            CheckEntityNull(entity, paramName: nameof(id));
            return TreeHelper.GetAllChilds(Cache.ValueList(), entity, containSelf);
        }

        private void CheckEntityNull(Menu entity,
            string message = "指定的节点主键有误,找不到对应的数据记录",
            string paramName = null)
        {
            if (entity == null)
            {
                throw new ArgumentException(message, paramName);
            }
        }
    }
}