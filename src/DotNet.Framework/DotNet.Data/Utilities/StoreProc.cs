// ===============================================================================
// DotNet.Platform 开发框架 2016 版权所有
// ===============================================================================
using System.Collections.Generic;

namespace DotNet.Data.Utilities
{
    /// <summary>
    /// 存储过程定义
    /// </summary>
    public class StoreProc
    {
        /// <summary>
        /// 初始化存储过程定义的新实例。
        /// </summary>
        /// <param name="name">存储过程名称</param>
        public StoreProc(string name)
        {
            Name = name;
        }

        /// <summary>
        /// 初始化存储过程定义的新实例(参数对象)。
        /// </summary>
        /// <param name="name">存储过程名称</param>
        /// <param name="args">存储过程参数(参数对象)</param>
        public StoreProc(string name, object args)
        {
            Name = name;
            Args = args;
        }

        /// <summary>
        /// 初始化存储过程定义的新实例(参数字典)。
        /// </summary>
        /// <param name="name">存储过程名称</param>
        /// <param name="args">存储过程参数(参数字典)</param>
        public StoreProc(string name, IDictionary<string,object> args)
        {
            Name = name;
            Args = args;
        }

        /// <summary>
        /// 存储过程名称
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// 存储过程参数
        /// </summary>
        public object Args { get; set; }
    }
}