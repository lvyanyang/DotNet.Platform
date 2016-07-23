// ===============================================================================
// DotNet.Platform 开发框架 2016 版权所有
// ===============================================================================
using System;

namespace DotNet.Utility
{
    /// <summary>
    /// 内部操作对象
    /// </summary>
    public class InnerAction
    {
        /// <summary>
        /// 初始化内部操作对象
        /// </summary>
        public InnerAction()
        {
            
        }

        /// <summary>
        /// 初始化内部操作对象,并制定操作状态
        /// </summary>
        /// <param name="isInner"></param>
        public InnerAction(bool isInner)
        {
            this.IsInner = isInner;
        }

        /// <summary>
        /// 是否正在进行内部操作
        /// </summary>
        public bool IsInner { get; private set; }

        /// <summary>
        /// 执行操作
        /// </summary>
        /// <param name="action">执行函数</param>
        public void Execute(Action action)
        {
            if (action!=null)
            {
                IsInner = true;
                action();
                IsInner = false;
            }
        }

        /// <summary>
        /// 开始执行操作,设置内部操作为true
        /// </summary>
        public void BeginExecute()
        {
            IsInner = true;
        }

        /// <summary>
        /// 结束执行操作,设置内部操作为false
        /// </summary>
        public void EndExecute()
        {
            IsInner = false;
        }
    }
}