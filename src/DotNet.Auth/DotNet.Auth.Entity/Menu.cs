// ===============================================================================
// DotNet.Platform 开发框架 2016 版权所有
// ===============================================================================
using System;
using DotNet.Entity;
using DotNet.Utility;
using Newtonsoft.Json;

namespace DotNet.Auth.Entity
{
    /// <summary>
    /// 系统菜单
    /// </summary>
    [Table("Menus", "系统菜单")]
    public class Menu
    {
        /// <summary>
        /// 主键
        /// </summary>
        [PrimaryKey]
        [Column("主键", false)]
        public string Id { get; set; }

        /// <summary>
        /// 父主键
        /// </summary>
        [ParentKey]
        [Column("父主键")]
        [JsonProperty("_parentId", NullValueHandling = NullValueHandling.Ignore)]
        public string ParentId { get; set; }

        /// <summary>
        /// 编码
        /// </summary>
        [Column("编码")]
        public string Code { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        [TextKey]
        [Column("名称")]
        public string Name { get; set; }

        /// <summary>
        /// 简拼
        /// </summary>
        [Column("简拼")]
        public string Spell { get; set; }

        /// <summary>
        /// 导航地址
        /// </summary>
        [Column("导航地址")]
        public string Url { get; set; }

        /// <summary>
        /// 图标样式
        /// </summary>
        [Column("图标样式")]
        [JsonProperty("iconCls", NullValueHandling = NullValueHandling.Ignore)]
        public string IconCls { get; set; }

        /// <summary>
        /// 展开
        /// </summary>
        [Column("展开")]
        public bool IsExpand { get; set; }

        /// <summary>
        /// 公开
        /// </summary>
        [Column("公开")]
        public bool IsPublic { get; set; }

        /// <summary>
        /// 启用
        /// </summary>
        [Column("启用")]
        public bool IsEnabled { get; set; } = true;

        /// <summary>
        /// 排序路径
        /// </summary>
        [Column("排序路径")]
        [SortPath]
        public string SortPath { get; set; }

        /// <summary>
        /// 节点状态
        /// </summary>
        [Ignore]
        [JsonProperty("state")]
        public TreeNodeState? State { get; set; }

        /// <summary>
        /// 创建用户Id
        /// </summary>
        [Column("创建用户Id")]
        public string CreateUserId { get; set; }

        /// <summary>
        /// 创建用户姓名
        /// </summary>
        [Column("创建用户姓名")]
        public string CreateUserName { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        [Column("创建时间")]
        public DateTime? CreateDateTime { get; set; }

        /// <summary>
        /// 修改用户Id
        /// </summary>
        [Column("修改用户Id")]
        public string ModifyUserId { get; set; }

        /// <summary>
        /// 修改用户姓名
        /// </summary>
        [Column("修改用户姓名")]
        public string ModifyUserName { get; set; }

        /// <summary>
        /// 修改时间
        /// </summary>
        [Column("修改时间")]
        public DateTime? ModifyDateTime { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [Column("备注")]
        public string Note { get; set; }

        /// <summary>
        /// 复制对象
        /// </summary>
        public Menu Clone()
        {
            return (Menu)MemberwiseClone();
        }
    }
}