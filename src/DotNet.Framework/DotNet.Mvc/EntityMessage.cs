// ===============================================================================
// DotNet.Platform 开发框架 2016 版权所有
// ===============================================================================
using Newtonsoft.Json;
using DotNet.Mvc;
using DotNet.Utility;

namespace DotNet.Mvc
{
    /// <summary>
    /// Json实体消息
    /// </summary>
    public class EntityMessage : JsonMessage
    {
        /// <summary>
        /// 构造Json实体消息
        /// </summary>
        public EntityMessage()
        {

        }

        /// <summary>
        /// 初始化Json实体消息
        /// </summary>
        /// <param name="boolMessage">指定的布尔消息</param>
        public EntityMessage(BoolMessage boolMessage)
        {
            this.Success = boolMessage.Success;
            this.Message = boolMessage.Message;
        }

        /// <summary>
        /// 初始化Json实体消息
        /// </summary>
        /// <param name="boolMessage">指定的布尔消息</param>
        /// <param name="entity">指定的实体对象</param>
        public EntityMessage(BoolMessage boolMessage, object entity)
        {
            this.Success = boolMessage.Success;
            this.Message = boolMessage.Message;
            this.Data = entity;
        }

        /// <summary>
        /// 构造Json实体消息
        /// </summary>
        /// <param name="entity">指定的实体对象</param>
        public EntityMessage(object entity)
            : this(true, null, entity)
        {

        }

        /// <summary>
        /// 构造Json实体消息
        /// </summary>
        /// <param name="success">是否获取成功</param>
        /// <param name="message">获取消息</param>
        public EntityMessage(bool success, string message)
            : this(success, message, null)
        {

        }

        /// <summary>
        /// 构造Json实体消息
        /// </summary>
        /// <param name="success">是否获取成功</param>
        /// <param name="message">获取消息</param>
        /// <param name="data">指定的实体对象</param>
        public EntityMessage(bool success, string message, object data)
        {
            this.Success = success;
            this.Message = message;
            this.Data = data;
        }

        /// <summary>
        /// 管理的数据
        /// </summary>
        [JsonProperty("data")]
        public object Data { get; set; }
    }
}