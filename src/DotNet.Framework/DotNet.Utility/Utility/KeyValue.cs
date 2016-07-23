// ===============================================================================
// DotNet.Platform 开发框架 2016 版权所有
// ===============================================================================
namespace DotNet.Utility
{
    /// <summary>
    /// 表示一个键值对
    /// </summary>
    public class KeyValue
    {
        /// <summary>
        /// 初始化键值对
        /// </summary>
        public KeyValue()
        {
        }

        /// <summary>
        /// 指定键初始化键值对，键和值相同
        /// </summary>
        /// <param name="key">指定的键，键和值相同</param>
        public KeyValue(string key)
            : this(key, key)
        {
        }

        /// <summary>
        /// 指定键值初始化键值对
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        public KeyValue(string key, string value)
        {
            this.Key = key;
            this.Value = value;
        }

        /// <summary>
        /// 键
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        /// 值
        /// </summary>
        public string Value { get; set; }
    }

    /// <summary>
    /// 表示一个键值简单对象
    /// </summary>
    public class Simple
    {
        public Simple()
        {
        }

        public Simple(string id, string name)
        {
            Id = id;
            Name = name;
        }

        public Simple(string id, string name, string spell)
        {
            Id = id;
            Name = name;
            Spell = spell;
        }

        /// <summary>
        /// 值
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 简拼
        /// </summary>
        public string Spell { get; set; }
    }



    /// <summary>
    /// 表示一个键值对以及描述字段
    /// </summary>
    public class KeyValueCaption : KeyValue
    {
        /// <summary>
        /// 初始化键值对
        /// </summary>
        public KeyValueCaption()
        {
        }

        /// <summary>
        /// 指定键值初始化键值对
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        public KeyValueCaption(string key, string value)
            : base(key, value)
        {
        }

        /// <summary>
        /// 指定键值初始化键值对以及描述字段
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        /// <param name="caption">描述</param>
        public KeyValueCaption(string key, string value, string caption)
            : base(key, value)
        {
            this.Caption = caption;
        }

        /// <summary>
        /// 描述
        /// </summary>
        public string Caption { get; set; }
    }

    /// <summary>
    /// 表示一个主标识键值对，除了包含键和值之外，还包含一个唯一标识
    /// </summary>
    public class PrimaryKeyValue : KeyValue
    {
        /// <summary>
        /// 初始化主标识键值对
        /// </summary>
        public PrimaryKeyValue()
        {
        }

        /// <summary>
        /// 指定唯一标识和键值初始化主标识键值对
        /// </summary>
        /// <param name="id">唯一标识</param>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        public PrimaryKeyValue(string id, string key, string value)
            : base(key, value)
        {
            Id = id;
        }

        /// <summary>
        /// 唯一标识
        /// </summary>
        public string Id { get; set; }
    }

    /// <summary>
    /// 表示一个上下级键值对，除了包含键和值之外，还包含一个唯一标识
    /// </summary>
    public class ParentKeyValue : PrimaryKeyValue
    {
        /// <summary>
        /// 初始化上下级键值对
        /// </summary>
        public ParentKeyValue()
        {
        }

        /// <summary>
        /// 指定上级唯一标识、唯一标识、键值初始化上下级键值对
        /// </summary>
        /// <param name="id">唯一标识</param>
        /// <param name="parentId">上级唯一标识</param>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        public ParentKeyValue(string id, string parentId, string key, string value)
            : base(id, key, value)
        {
            ParentId = parentId;
        }

        /// <summary>
        /// 上级唯一标识
        /// </summary>
        public string ParentId { get; set; }
    }
}