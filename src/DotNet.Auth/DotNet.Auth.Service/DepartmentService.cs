// ===============================================================================
// DotNet.Platform 开发框架 2016 版权所有
// ===============================================================================
using System;
using System.Collections.Generic;
using System.Linq;
using DotNet.Auth.Entity;
using DotNet.Extensions;
using DotNet.Helper;
using DotNet.Utility;

namespace DotNet.Auth.Service
{
    /// <summary>
    /// 系统部门服务
    /// </summary>
    public class DepartmentService
    {
        private static readonly Cache<string, Department> Cache = new Cache<string, Department>();

        /// <summary>
        /// 构造服务
        /// </summary>
        internal DepartmentService()
        {
            InitCache();
        }

        /// <summary>
        /// 初始化缓存
        /// </summary>
        public void InitCache()
        {
            var repos = new AuthRepository<Department>();
            Cache.Clear().Set(repos.Query().ToDictionary(p => p.Id, p => p));
        }

        /// <summary>
        /// 是否存在指定名称的对象
        /// </summary>
        /// <param name="id">主键</param>
        /// <param name="parentId">上级主键</param>
        /// <param name="name">名称</param>
        /// <returns>存在返回false</returns>
        public BoolMessage ExistsByName(string id, string parentId, string name)
        {
            var has = Cache.ValueList().Contains(p => p.Name.Equals(name) && p.ParentId.Equals(parentId) && !p.Id.Equals(id));
            return has ? new BoolMessage(false, "指定的部门名称已经存在") : BoolMessage.True;
        }

        /// <summary>
        /// 添加对象
        /// </summary>
        /// <param name="entity">实体</param>
        public BoolMessage Create(Department entity)
        {
            var repos = new AuthRepository<Department>();
            repos.Insert(entity);
            Cache.Set(entity.Id, entity);
            return BoolMessage.True;
        }

        /// <summary>
        /// 更新对象
        /// </summary>
        /// <param name="entity">实体</param>
        public BoolMessage Update(Department entity)
        {
            var repos = new AuthRepository<Department>();
            repos.Update(entity);
            Cache.Set(entity.Id, entity);
            return BoolMessage.True;
        }

        /// <summary>
        /// 保存对象
        /// </summary>
        /// <param name="entity">实体</param>
        /// <param name="isCreate">是否新增</param>
        public BoolMessage Save(Department entity, bool isCreate)
        {
            return isCreate ? Create(entity) : Update(entity);
        }

        /// <summary>
        /// 删除对象(删除包括所有子节点)
        /// </summary>
        /// <param name="id">主键</param>
        public BoolMessage Delete(string id)
        {
            if(!Cache.Contains(id)) return new BoolMessage(false,"找不到指定主键的数据");
            var childs = GetChildNodeList(id, true);
            var ids = childs.Select(p => p.Id).ToArray();
            var repos = new AuthRepository<Department>();
            repos.Delete(ids);
            Cache.Remove(ids);

            var userIds = AuthService.User.GetList(ids, null, false)
                .Select(p => p.Id).ToArray();
            if (userIds.Length > 0)
            {
                AuthService.User.UpdateUserDept(userIds, null, null);
            }
            return BoolMessage.True;
        }

        /// <summary>
        /// 保存指定对象父节点
        /// </summary>
        /// <param name="id">主键</param>
        /// <param name="newParentId">新父节点主键</param>
        public BoolMessage SaveParent(string id, string newParentId)
        {
            var repos = new AuthRepository<Department>();
            repos.Update(new Department { Id = id, ParentId = newParentId }, p => p.ParentId);
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
            var repos = new AuthRepository<Department>();
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
        public Department Get(string id)
        {
            return Cache.Get(id);
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
        /// 获取简单对象集合(已排序)
        /// </summary>
        public List<Simple> GetSimpleList()
        {
            return Cache.ValueList()
                .OrderByAsc(p => p.SortPath)
                .Select(p => new Simple(p.Id, p.Name, p.Spell))
                .ToList();
        }

        /// <summary>
        /// 获取对象集合(已排序)
        /// </summary>
        public List<Department> GetList()
        {
            return Cache.ValueList()
                .OrderByAsc(p => p.SortPath);
        }

        /// <summary>
        /// 获取对象节点集合
        /// </summary>
        public List<TreeNode> GetNodeList()
        {
            var dataList = GetList();
            var nodeList = TreeHelper.CreateNodeList(dataList, "font-icon icon-users");
            nodeList.Insert(0, TreeHelper.CreateRootNodet("组织机构", "font-icon icon-globe"));
            return nodeList;
        }

        /// <summary>
        /// 获取指定节点主键的所有子节点集合
        /// </summary>
        /// <param name="id">节点主键</param>
        /// <param name="containSelf">是否包含指定节点对象</param>
        public List<Department> GetChildNodeList(string id, bool containSelf)
        {
            var entity = Cache.Get(id);
            CheckEntityNull(entity, paramName: nameof(id));
            return TreeHelper.GetAllChilds(Cache.ValueList(), entity, containSelf);
        }

        private void CheckEntityNull(Department entity,
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