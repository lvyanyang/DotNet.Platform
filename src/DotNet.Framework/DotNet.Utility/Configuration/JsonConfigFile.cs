// ===============================================================================
// DotNet.Platform 开发框架 2016 版权所有
// ===============================================================================
namespace DotNet.Configuration
{
    /// <summary>
    /// 表示一个本地Json配置文件
    /// </summary>
    public class JsonConfigFile<T> where T : class,new()
    {
        private readonly string _directoryName;
        private readonly string _fileName;
        private string _configPath;
        private T _data;

        /// <summary>
        /// 使用指定文件名初始化本地配置文件实例,并自动加载配置文件数据
        /// </summary>
        /// <param name="fileName">配置文件名称(不含路径)</param>
        public JsonConfigFile(string fileName)
            :this(SystemDirectory.ConfigDirectory,fileName)
        {
        }

        /// <summary>
        /// 使用指定文件名初始化本地配置文件实例,并自动加载配置文件数据
        /// </summary>
        /// <param name="directoryName">目录名称</param>
        /// <param name="fileName">配置文件名称(不含路径)</param>
        public JsonConfigFile(string directoryName, string fileName)
        {
            this._directoryName = directoryName;
            this._fileName = fileName;
            this.Load();
        }

        /// <summary>
        /// 配置文件路径
        /// </summary>
        public string Path
        {
            get
            {
                if (string.IsNullOrEmpty(_configPath))
                {
                    string name = _fileName;
                    if (string.IsNullOrEmpty(System.IO.Path.GetExtension(_fileName)))
                    {
                        name = _fileName + ".json";
                    }
                    _configPath = System.IO.Path.Combine(_directoryName, name);
                }
                return _configPath;
            }
        }

        /// <summary>
        /// 配置数据
        /// </summary>
        public T Data
        {
            get { return _data; }
            set { _data = value; }
        }


        /// <summary>
        /// 从指定文件中重新加载配置数据
        /// </summary>
        public void Load()
        {
            this._data = JsonHelper.Deserialize(Path, new T());
        }

        /// <summary>
        /// 保存配置数据到指定文件中
        /// </summary>
        public void Save()
        {
            JsonHelper.Serialize(_configPath, _data);
        }
    }
}