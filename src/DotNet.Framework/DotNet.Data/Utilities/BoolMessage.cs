// ===============================================================================
// DotNet.Platform 开发框架 2016 版权所有
// ===============================================================================
using System;

namespace DotNet.Data.Utilities
{
    /// <summary>
    /// 状态消息
    /// </summary>
    public class StatusMessage
    {
        private readonly Exception _exptionObject;
        private readonly string _message;
        private readonly bool _success;

        /// <summary>
        /// 初始化状态消息
        /// </summary>
        /// <param name="success">布尔状态</param>
        public StatusMessage(bool success)
        {
            _success = success;
        }

        /// <summary>
        /// 初始化状态消息
        /// </summary>
        /// <param name="success">布尔状态</param>
        /// <param name="message">状态消息</param>
        public StatusMessage(bool success, string message)
        {
            _success = success;
            _message = message;
        }

        /// <summary>
        /// 初始化状态消息
        /// </summary>
        /// <param name="success">布尔状态</param>
        /// <param name="message">状态消息</param>
        /// <param name="exptionObject">异常对象</param>
        public StatusMessage(bool success, string message, Exception exptionObject)
        {
            _exptionObject = exptionObject;
            _message = message;
            _success = success;
        }

        /// <summary>
        /// 成功状态
        /// </summary>
        public bool Success
        {
            get { return _success; }
        }

        /// <summary>
        /// 状态消息
        /// </summary>
        public string Message
        {
            get { return _message; }
        }

        /// <summary>
        /// 异常对象
        /// </summary>
        public Exception ExptionObject
        {
            get { return _exptionObject; }
        }
    }
}