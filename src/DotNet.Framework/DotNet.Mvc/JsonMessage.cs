// ===============================================================================
// DotNet.Platform 开发框架 2016 版权所有
// ===============================================================================
using Newtonsoft.Json;
using DotNet.Utility;

namespace DotNet.Mvc
{
    /// <summary>
    /// Json消息
    /// </summary>
    public class JsonMessage
    {
        /// <summary>
        /// 初始化Json消息
        /// </summary>
        public JsonMessage()
        {
            
        }

        /// <summary>
        /// 初始化Json消息
        /// </summary>
        /// <param name="success">是否成功</param>
        public JsonMessage(bool success)
        {
            this.Success = success;
        }

        /// <summary>
        /// 初始化Json消息
        /// </summary>
        /// <param name="success">是否成功</param>
        /// <param name="message">消息</param>
        public JsonMessage(bool success, string message)
        {
            this.Success = success;
            this.Message = message;
        }

        /// <summary>
        /// 初始化Json消息
        /// </summary>
        /// <param name="boolMessage">指定的布尔消息</param>
        public JsonMessage(BoolMessage boolMessage)
        {
            this.Success = boolMessage.Success;
            this.Message = boolMessage.Message;
        }

        /// <summary>
        /// 是否成功
        /// </summary>
        [JsonProperty("success")]
        public bool Success { get; set; }

        /// <summary>
        /// 消息
        /// </summary>
        [JsonProperty("message")]
        public string Message { get; set; }
    }
}