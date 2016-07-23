// ===============================================================================
// DotNet.Platform 开发框架 2016 版权所有
// ===============================================================================
using System;
using System.Collections.Generic;
using System.Linq;
using DotNet.Auth.Entity;
using DotNet.Data;
using DotNet.Extensions;
using DotNet.Helper;
using DotNet.Utility;

namespace DotNet.Auth.Service
{
    /// <summary>
    /// 系统字典服务
    /// </summary>
    public class DicService
    {
        private static readonly Cache<string, Dic> Cache = new Cache<string, Dic>();

        /// <summary>
        /// 构造服务
        /// </summary>
        internal DicService()
        {
            InitCache();
        }

        /// <summary>
        /// 初始化缓存
        /// </summary>
        public void InitCache()
        {
            var repos = new AuthRepository<Dic>();
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
            return has ? new BoolMessage(false, "指定的字典编码已经存在") : BoolMessage.True;
        }

        /// <summary>
        /// 添加对象
        /// </summary>
        /// <param name="entity">实体</param>
        public BoolMessage Create(Dic entity)
        {
            var repos = new AuthRepository<Dic>();
            repos.Insert(entity);
            Cache.Set(entity.Id, entity);
            return BoolMessage.True;
        }

        /// <summary>
        /// 更新对象
        /// </summary>
        /// <param name="entity">实体</param>
        public BoolMessage Update(Dic entity)
        {
            var repos = new AuthRepository<Dic>();
            repos.Update(entity);
            Cache.Set(entity.Id, entity);
            return BoolMessage.True;
        }

        /// <summary>
        /// 保存对象
        /// </summary>
        /// <param name="entity">实体</param>
        /// <param name="isCreate">是否新增</param>
        public BoolMessage Save(Dic entity, bool isCreate)
        {
            return isCreate ? Create(entity) : Update(entity);
        }

        /// <summary>
        /// 删除对象(删除包括所有子节点以及所有子节点对应的字典明细)
        /// </summary>
        /// <param name="id">主键</param>
        public BoolMessage Delete(string id)
        {
            if (!Cache.Contains(id)) return new BoolMessage(false, "找不到指定主键的数据");

            var childs = GetChildNodeList(id, true);
            var ids = childs.Select(p => p.Id).ToArray();
            var details = AuthService.DicDetail.GetList(ids, null, false);
            var detailIds = details.Select(p => p.Id).ToArray();
            Action action = () =>
            {
                var repos = new AuthRepository<Dic>();
                repos.Delete(ids);
                Cache.Remove(ids);
            };
            if (detailIds.Length == 0) //如果没有明细项,不启用事务
            {
                action();
                return BoolMessage.True;
            }
            try
            {
                DbSession.Begin(new AuthDatabase());
                AuthService.DicDetail.Delete(detailIds);
                action();
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
        /// 保存指定对象父节点
        /// </summary>
        /// <param name="id">主键</param>
        /// <param name="newParentId">新父节点主键</param>
        public BoolMessage SaveParent(string id, string newParentId)
        {
            var repos = new AuthRepository<Dic>();
            repos.Update(new Dic { Id = id, ParentId = newParentId }, p => p.ParentId);
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
            var repos = new AuthRepository<Dic>();
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
        public Dic Get(string id)
        {
            return Cache.Get(id);
        }

        /// <summary>
        /// 获取对象
        /// </summary>
        /// <param name="code">字典编码</param>
        public Dic GetByCode(string code)
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
        /// 获取对象集合(已排序)
        /// </summary>
        public List<Dic> GetList()
        {
            return Cache.ValueList().OrderByAsc(p => p.SortPath);
        }

        /// <summary>
        /// 获取对象节点集合
        /// </summary>
        public List<TreeNode> GetNodeList()
        {
            var dataList = GetList();
            var nodeList = TreeHelper.CreateNodeList(dataList, "font-icon icon-settings");
            nodeList.Insert(0, TreeHelper.CreateRootNodet("数据字典", "font-icon icon-globe"));
            return nodeList;
        }

        /// <summary>
        /// 获取指定节点主键的所有子节点集合
        /// </summary>
        /// <param name="id">节点主键</param>
        /// <param name="containSelf">是否包含指定节点对象</param>
        public List<Dic> GetChildNodeList(string id, bool containSelf)
        {
            var entity = Cache.Get(id);
            CheckEntityNull(entity, paramName: nameof(id));
            return TreeHelper.GetAllChilds(Cache.ValueList(), entity, containSelf);
        }

        private void CheckEntityNull(Dic entity,
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