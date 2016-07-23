// ===============================================================================
// DotNet.Platform 开发框架 2016 版权所有
// ===============================================================================
namespace DotNet.Configuration
{
    /// <summary>
    /// 组件配置
    /// </summary>
    public class ComponentSetting
    {
        /// <summary>
        /// 初始化组件配置
        /// </summary>
        public ComponentSetting()
        {
        }

        /// <summary>
        /// 初始化组件配置
        /// </summary>
        /// <param name="name">组件名称</param>
        /// <param name="provider">实现类</param>
        public ComponentSetting(string name, string provider)
        {
            Name = name;
            Provider = provider;
        }

        /// <summary>
        /// 初始化组件配置
        /// </summary>
        /// <param name="name">组件名称</param>
        /// <param name="caption">组件标题</param>
        /// <param name="provider">实现类</param>
        public ComponentSetting(string name, string caption, string provider)
        {
            Provider = provider;
            Caption = caption;
            Name = name;
        }

        /// <summary>
        /// 组件名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 组件标题
        /// </summary>
        public string Caption { get; set; }

        /// <summary>
        /// 实现类
        /// </summary>
        public string Provider { get; set; }
    }
}