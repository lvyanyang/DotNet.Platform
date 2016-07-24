// ===============================================================================
// DotNet.Platform 开发框架 2016 版权所有
// ===============================================================================
using System;
using System.Collections.Generic;
using System.Linq;
using DotNet.Entity;
using DotNet.Extensions;
using DotNet.Utility;

namespace DotNet.Helper
{
    /// <summary>
    /// 树结构操作帮助类
    /// </summary>
    public class TreeHelper
    {
        public const string RootTreeNodeId = "0";

        public static TreeNode CreateNode<T>(T entity, string iconCls = null, Action<TreeNode, T> each = null)
        {
            var meta = EntityMetadata.ForType(typeof(T));

            if (string.IsNullOrEmpty(meta.TableInfo.PrimaryKey)) throw new ArgumentException("请指定实体主键字段");
            if (string.IsNullOrEmpty(meta.TableInfo.TextKey)) throw new ArgumentException("请指定实体显示字段");

            var node = new TreeNode();
            node.Id = meta.TableInfo.PrimaryKeyProperty.Get(entity).ToStringOrEmpty();
            node.Text = meta.TableInfo.TextKeyProperty.Get(entity).ToStringOrEmpty();
            node.Spell = node.Text.Spell();
            if (meta.TableInfo.ParentKeyProperty != null)
            {
                node.ParentId = meta.TableInfo.ParentKeyProperty.Get(entity).ToStringOrEmpty();
            }
            node.IconCls = iconCls;
            each?.Invoke(node, entity);
            return node;
        }

        public static List<TreeNode> CreateNodeList<T>(List<T> entityList,
            string defaultIconCls = null, Action<TreeNode, T> each = null)
        {
            var list = new List<TreeNode>();
            entityList.ForEach(p => list.Add(CreateNode(p, defaultIconCls, each)));
            return list;
        }

        /// <summary>
        /// 获取所有子节点
        /// </summary>
        /// <param name="entityList"></param>
        /// <param name="entity"></param>
        /// <param name="containSelf"></param>
        public static List<T> GetAllChilds<T>(List<T> entityList, T entity, bool containSelf)
        {
            var meta = EntityMetadata.ForType(typeof(T));
            if (string.IsNullOrEmpty(meta.TableInfo.PrimaryKey)) throw new ArgumentException("请指定实体主键字段");
            if (string.IsNullOrEmpty(meta.TableInfo.ParentKey)) throw new ArgumentException("请指定实体父级字段");
            var storeList = new List<T>();
            if (containSelf)
            {
                storeList.Add(entity);
            }
            GetChilds(entityList, storeList, entity);
            return storeList;
        }

        private static void GetChilds<T>(List<T> entityList, List<T> storeList, T entity)
        {
            var meta = EntityMetadata.ForType(typeof(T));
            string id = meta.TableInfo.PrimaryKeyProperty.Get(entity).ToStringOrEmpty();
            var data = entityList.Where(p => meta.TableInfo.ParentKeyProperty.Get(p).ToStringOrEmpty().Equals(id)).ToList();
            if (data.Count <= 0) return;
            foreach (var item in data)
            {
                storeList.Add(item);
                GetChilds(entityList, storeList, item);
            }
        }

        /// <summary>
        /// 获取排序路径
        /// </summary>
        /// <param name="entityList"></param>
        /// <param name="parentEntity">父实体</param>
        public static string GetNewNodeSortPath<T>(List<T> entityList, T parentEntity)
        {
            if (parentEntity == null) return "0000";
            var meta = EntityMetadata.ForType(typeof(T));
            if (string.IsNullOrEmpty(meta.TableInfo.ParentKey)) throw new ArgumentException("请指定实体父级字段");
            if (string.IsNullOrEmpty(meta.TableInfo.SortPath)) throw new ArgumentException("请指定实体排序路径字段");

            var parentSortPath = meta.TableInfo.SortPathProperty.Get(parentEntity).ToStringOrEmpty();

            var parentId = meta.TableInfo.PrimaryKeyProperty.Get(parentEntity).ToStringOrEmpty();
            var childCount = entityList.Count(p => meta.TableInfo.ParentKeyProperty.Get(p).ToStringOrEmpty().Equals(parentId)) + 1;
            return parentSortPath + StringHelper.GetFixedLengthString(childCount.ToString(), 4, "0", true);
        }

        public static List<TreeNode> BuildTree<T>(List<T> entityList, string parentId,
            string rootIconCls = null, string defaultIconCls = null, Action<TreeNode, T> each = null)
        {
            var meta = EntityMetadata.ForType(typeof(T));
            if (string.IsNullOrEmpty(meta.TableInfo.PrimaryKey)) throw new ArgumentException("请指定实体主键字段");
            if (string.IsNullOrEmpty(meta.TableInfo.ParentKey)) throw new ArgumentException("请指定实体父级字段");
            if (string.IsNullOrEmpty(meta.TableInfo.TextKey)) throw new ArgumentException("请指定实体显示字段");

            var list = new List<TreeNode>();
            var childs = entityList.Where(p => meta.TableInfo.ParentKeyProperty
                            .Get(p).ToStringOrEmpty().Equals(parentId)).ToList();
            foreach (T item in childs)
            {
                var node = CreateNode(item, defaultIconCls, each);
                if (string.IsNullOrEmpty(node.IconCls))
                {
                    node.IconCls = rootIconCls;
                }
                BuildTree(entityList, node, meta.TableInfo.PrimaryKeyProperty.Get(item).ToStringOrEmpty(), defaultIconCls, each);
                list.Add(node);
            }
            return list;

            //递推输出
            //public void Test()
            //{
            //    var list = AuthService.Dic.GetList();
            //    list.Add(new Dic { Id = "0", ParentId = "-1", Name = "数据字典" });
            //    var node = TreeHelper.BuildTree(list, "-1").FirstOrDefault();
            //    Write(node, 0);
            //}

            //public void Write(TreeNode node, int level)
            //{
            //    Console.WriteLine($"{StringHelper.GetRepeatString("-", level)}{node.Text}");
            //    if (node.Children == null) return;
            //    foreach (var n in node.Children)
            //    {
            //        Write(n, level + 1);
            //    }
            //}
        }

        private static void BuildTree<T>(List<T> entityList, TreeNode node, string parentId,
            string defaultIconCls = null, Action<TreeNode, T> each = null)
        {
            var meta = EntityMetadata.ForType(typeof(T));
            var childs = entityList.Where(p => meta.TableInfo.ParentKeyProperty
                                                .Get(p).ToStringOrEmpty().Equals(parentId)).ToList();
            if (childs.Any())
            {
                node.Children = new List<TreeNode>();
                if (!node.State.HasValue)
                {
                    node.State = TreeNodeState.Closed;
                }
                
            }
            foreach (T item in childs)
            {
                var n = CreateNode(item, defaultIconCls, each);
                node.Children.Add(n);
                BuildTree(entityList, n, meta.TableInfo.PrimaryKeyProperty.Get(item).ToStringOrEmpty(), defaultIconCls, each);
            }
        }

        public static int GetTreeLevel<T>(List<T> entityList, T entity)
        {
            var meta = EntityMetadata.ForType(typeof(T));
            if (string.IsNullOrEmpty(meta.TableInfo.PrimaryKey)) throw new ArgumentException("请指定实体主键字段");
            if (string.IsNullOrEmpty(meta.TableInfo.ParentKey)) throw new ArgumentException("请指定实体父级字段");
            var parentId = meta.TableInfo.ParentKeyProperty.Get(entity).ToStringOrEmpty();
            var level = GetTreeLevel(entityList,parentId , 1);
            return level;
        }

        private static int GetTreeLevel<T>(List<T> entityList,string parentId, int level)
        {
            var meta = EntityMetadata.ForType(typeof(T));
            var entity = entityList.FirstOrDefault(p => meta.TableInfo.PrimaryKeyProperty
                                                .Get(p).ToStringOrEmpty().Equals(parentId));
            if (entity!=null)
            {
                level++;
                var pid = meta.TableInfo.ParentKeyProperty.Get(entity).ToStringOrEmpty();
                return GetTreeLevel(entityList,pid , level);
            }
            return level;
        }

        public static TreeNode CreateRootNodet(string text,string iconCls)
        {
            return new TreeNode
            {
                Id = RootTreeNodeId,
                ParentId = "-1",
                Text = text,
                Spell = text.Spell(),
                IconCls = iconCls
            };
        }
    }
}