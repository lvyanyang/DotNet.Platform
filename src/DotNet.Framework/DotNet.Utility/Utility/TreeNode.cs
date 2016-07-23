// ===============================================================================
// DotNet.Platform 开发框架 2016 版权所有
// ===============================================================================
using System.Collections.Generic;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace DotNet.Utility
{
    /// <summary>
    /// 数树节点
    /// </summary>
    public class TreeNode
    {
        /// <summary>
        /// 节点Id
        /// </summary>
        [JsonProperty("id")]
        public string Id { get; set; }

        /// <summary>
        /// 父节点Id
        /// </summary>
        [JsonProperty("parentid")]
        public string ParentId { get; set; }

        /// <summary>
        /// 节点文本
        /// </summary>
        [JsonProperty("text")]
        public string Text { get; set; }

        /// <summary>
        /// 节点简拼
        /// </summary>
        [JsonProperty("spell")]
        public string Spell { get; set; }

        /// <summary>
        /// 节点Css类
        /// </summary>
        [JsonProperty("iconCls")]
        public string IconCls { get; set; }

        /// <summary>
        /// 该节点是否被选中
        /// </summary>
        [JsonProperty("checked")]
        public bool? Checked { get; set; }

        /// <summary>
        /// 节点状态，'open' 或 'closed'
        /// </summary>
        [JsonProperty("state")]
        public TreeNodeState? State { get; set; }

        /// <summary>
        /// 链接地址
        /// </summary>
        [JsonProperty("url")]
        public string Url { get; set; }

        /// <summary>
        /// 子节点
        /// </summary>
        [JsonProperty("children")]
        public List<TreeNode> Children { get; set; }

        /// <summary>
        /// 绑定该节点的自定义属性
        /// </summary>
        [JsonProperty("attributes")]
        public Dictionary<string, string> Attributes { get; set; }
    }

    /// <summary>
    /// 节点展开状态
    /// </summary>
    public enum TreeNodeState
    {
        /// <summary>
        /// 展开
        /// </summary>
        Open,

        /// <summary>
        /// 不展开
        /// </summary>
        Closed
    }
}