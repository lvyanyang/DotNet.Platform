// ===============================================================================
// DotNet.Platform 开发框架 2016 版权所有
// ===============================================================================
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using DotNet.Data.Provider;
using DotNet.Data.Utilities;
using DotNet.Helper;

namespace DotNet.Data
{
    /// <summary>
    /// 数据库对象
    /// </summary>
    public class Database : IDisposable
    {
        #region IDisposable

        /// <summary>
        /// 执行与释放或重置非托管资源相关的应用程序定义的任务。
        /// </summary>
        public void Dispose()
        {
            CloseSharedConnection();
        }

        #endregion

        #region 测试连接

        /// <summary>
        /// 测试数据库连接
        /// </summary>
        /// <returns>连接成功 返回true</returns>
        public StatusMessage TestConnection()
        {
            const long timeout = 4000;
            Stopwatch sw = new Stopwatch();
            StatusMessage connectSuccess = new StatusMessage(false);

            DbConnection conn = null;
            Action testAction = () =>
            {
                try
                {
                    conn = CreateConnection();
                    // ReSharper disable once PossibleNullReferenceException
                    conn.ConnectionString += ";Connection Timeout=3";
                    sw.Start();
                    conn.Open();
                    connectSuccess = new StatusMessage(true);
                }
                catch (Exception e)
                {
                    connectSuccess = new StatusMessage(false, e.Message);
                }
                finally
                {
                    if (conn != null && conn.State == ConnectionState.Open)
                    {
                        conn.Close();
                    }
                }
            };
            Thread t = new Thread(new ThreadStart(testAction));
            t.IsBackground = true;
            t.Start();

            // ReSharper disable once LoopVariableIsNeverChangedInsideLoop
            while (timeout > sw.ElapsedMilliseconds)
            {
                if (t.Join(1000))
                {
                    break;
                }
            }

            return connectSuccess;
        }

        #endregion

        #region FormatCommand

        /// <summary>
        /// 格式化显示命令对象
        /// </summary>
        /// <param name="cmd">命令对象</param>
        /// <returns>返回格式化显示命令对象字符串</returns>
        public string FormatCommand(DbCommand cmd)
        {
            var sb = new StringBuilder();
            var args = (from IDbDataParameter parameter in cmd.Parameters select parameter).ToArray();
            if (args.Length > 0)
            {
                sb.Append("\n");
                for (int i = 0; i < args.Length; i++)
                {
                    var param = args[i];
                    sb.AppendFormat("\tdeclare {0}{1} {2} = '{3}'\n", _paramPrefix,
                        param.ParameterName, param.DbType, param.Value);
                }
                sb.Remove(sb.Length - 1, 1);
                sb.Append("\n");
            }
            string sql = cmd.CommandText;
            if (string.IsNullOrEmpty(sql))
                return string.Empty;
            sb.Append("\t");
            sb.Append(sql);
            return sb.ToString();
        }

        #endregion

        #region 私有变量

        private readonly string _connectionString;
        private readonly string _providerName;
        private string _paramPrefix;
        private DbProvider _provider;
        private DbConnection _sharedConnection;
        private DbTransaction _transaction;
        private int _sharedConnectionDepth;
        private int _transactionDepth;
        private bool _transactionCancelled;
        private DbCommand currentCommand;
        private readonly Stopwatch stopwatch = new Stopwatch();

        #endregion

        #region 公共属性

        /// <summary>
        /// 设置查询超时时间
        /// </summary>
        public int CommandTimeout { get; set; }

        /// <summary>
        /// 设置查询超时时间(仅对下面执行的一句语句有效)
        /// </summary>
        public int OneTimeCommandTimeout { get; set; }

        /// <summary>
        /// 数据库引擎对象
        /// </summary>
        public IDbProvider Provider { get { return _provider; } }

        #endregion

        #region 构造函数

        /// <summary>
        /// 初始化数据库对象,默认读取第一个数据库配置
        /// </summary>
        public Database()
            : this(0)
        {
        }

        /// <summary>
        /// 初始化数据库对象,读取指定名称的数据库配置
        /// </summary>
        /// <param name="name">配置名称</param>
        public Database(string name)
        {
            var setting = string.IsNullOrEmpty(name) ?
                DbSettingManager.GetSetting(0) :
                DbSettingManager.GetSetting(name);

            if (setting == null)
            {
                throw new InvalidOperationException("无效的数据库配置名称");
            }
            _connectionString = setting.ConnectionString;
            _providerName = setting.Provider;
            CommonConstruct();
        }

        /// <summary>
        /// 初始化数据库对象,读取指定序号的数据库配置
        /// </summary>
        /// <param name="index">配置序号</param>
        public Database(int index)
        {
            var setting = DbSettingManager.GetSetting(index);
            if (setting == null)
            {
                throw new InvalidOperationException("无效的配置序号");
            }
            _connectionString = setting.ConnectionString;
            _providerName = setting.Provider;
            CommonConstruct();
        }

        /// <summary>
        /// 初始化数据库对象,使用指定的连接字符串和数据库类型
        /// </summary>
        /// <param name="connectionString">连接字符串</param>
        /// <param name="providerName">数据库类型</param>
        public Database(string connectionString, string providerName)
        {
            _connectionString = connectionString;
            _providerName = providerName;
            CommonConstruct();
        }

        /// <summary>
        /// 公共构造函数
        /// </summary>
        private void CommonConstruct()
        {
            _transactionDepth = 0;

            if (string.IsNullOrEmpty(_providerName))
            {
                _provider = SqlServerDbProvider.Instance;
            }
            else if (_providerName.StartsWith("SQLServer", StringComparison.OrdinalIgnoreCase))
            {
                _provider = SqlServerDbProvider.Instance;
            }
            else if (_providerName.StartsWith("Oracle", StringComparison.OrdinalIgnoreCase))
            {
                _provider = OracleDbProvider.Instance;
            }
            else if (_providerName.StartsWith("SQLite", StringComparison.OrdinalIgnoreCase))
            {
                _provider = SqliteDbProvider.Instance;
            }
            else if (_providerName.StartsWith("MySQL", StringComparison.OrdinalIgnoreCase))
            {
                _provider = MySqlDbProvider.Instance;
            }
            else
            {
                _provider = SqlServerDbProvider.Instance;
            }

            _paramPrefix = _provider.ParameterPrefix;
        }

        #endregion

        #region 连接管理

        /// <summary>
        /// 保持连接打开状态
        /// </summary>
        public bool KeepConnectionAlive { get; set; }

        /// <summary>
        /// 打开连接
        /// </summary>
        public void OpenSharedConnection()
        {
            if (_sharedConnectionDepth == 0)
            {
                _sharedConnection = CreateConnection();
                // ReSharper disable once PossibleNullReferenceException
                _sharedConnection.ConnectionString = _connectionString;

                if (_sharedConnection.State == ConnectionState.Broken)
                    _sharedConnection.Close();

                //if (_sharedConnection.State == ConnectionState.Closed)
                //    _sharedConnection.Open();

                _sharedConnection = OnConnectionOpened(_sharedConnection);

                if (KeepConnectionAlive)
                    _sharedConnectionDepth++;
            }
            if (_sharedConnection.State == ConnectionState.Closed)
            {
                _sharedConnection.Open();
            }
            _sharedConnectionDepth++;
        }

        /// <summary>
        /// 关闭连接
        /// </summary>
        public void CloseSharedConnection()
        {
            if (_sharedConnectionDepth > 0)
            {
                _sharedConnectionDepth--;
                if (_sharedConnectionDepth == 0)
                {
                    OnConnectionClosing(_sharedConnection);
                    _sharedConnection.Dispose();
                    _sharedConnection = null;
                }
            }
        }

        /// <summary>
        /// 当前连接对象
        /// </summary>
        public IDbConnection Connection
        {
            get { return _sharedConnection; }
        }

        #endregion

        #region 事务管理

        /// <summary>
        /// 获取一个事务包装对象
        /// </summary>
        /// <returns>新创建一个事务包装对象</returns>
        public Transaction GetTransaction()
        {
            return new Transaction(this);
        }

        /// <summary>
        /// 当事务开始执行时调用
        /// </summary>
        protected virtual void OnBeginTransaction()
        {
        }

        /// <summary>
        /// 当事务结束时调用
        /// </summary>
        protected virtual void OnEndTransaction()
        {
        }

        /// <summary>
        /// 开始事务
        /// </summary>
        public void BeginTransaction()
        {
            _transactionDepth++;

            if (_transactionDepth == 1)
            {
                OpenSharedConnection();
                _transaction = _sharedConnection.BeginTransaction();
                _transactionCancelled = false;
                OnBeginTransaction();
                Trace.WriteLine("=======开启事务========", "Database");
            }

        }

        /// <summary>
        /// 清理事务
        /// </summary>
        private void CleanupTransaction()
        {
            OnEndTransaction();

            if (_transactionCancelled)
            {
                _transaction.Rollback();
                Trace.WriteLine("=======回滚事务========", "Database");
            }
            else
            {
                _transaction.Commit();
                Trace.WriteLine("=======提交事务========", "Database");
            }

            _transaction.Dispose();
            _transaction = null;

            CloseSharedConnection();
        }

        /// <summary>
        /// 回滚事务
        /// </summary>
        public void RollbackTransaction()
        {
            _transactionCancelled = true;
            if ((--_transactionDepth) == 0)
                CleanupTransaction();
        }

        /// <summary>
        /// 提交事务
        /// </summary>
        public void CommitTransaction()
        {
            if ((--_transactionDepth) == 0)
                CleanupTransaction();
        }

        #endregion

        #region 数据库对象

        /// <summary>
        /// 创建命令新连接
        /// </summary>
        /// <returns>返回连接新实例</returns>
        public DbConnection CreateConnection()
        {
            return _provider.ProviderFactory.CreateConnection();
        }

        /// <summary>
        /// 创建命令新实例
        /// </summary>
        /// <returns>返回命令新实例</returns>
        public DbCommand CreateCommand()
        {
            return _provider.ProviderFactory.CreateCommand();
        }

        /// <summary>
        /// 创建参数新实例
        /// </summary>
        /// <returns>返回参数新实例</returns>
        public DbParameter CreateParameter()
        {
            return _provider.ProviderFactory.CreateParameter();
        }

        /// <summary>
        /// 创建适配器新实例
        /// </summary>
        /// <returns>返回适配器新实例</returns>
        public DbDataAdapter CreateDataAdapter()
        {
            return _provider.ProviderFactory.CreateDataAdapter();
        }

        #endregion

        #region 命令对象管理

        /// <summary>
        /// 设置参数属性
        /// </summary>
        /// <param name="p">参数对象</param>
        /// <param name="value">参数值</param>
        private void SetParam(DbParameter p, object value)
        {
            if (value == null)
            {
                p.Value = DBNull.Value;
                //p.DbType = CSharpTypeToDbType();
            }
            else
            {
                value = _provider.ConvertParameterValue(value);

                var t = value.GetType();
                if (t.IsEnum)
                {
                    p.Value = (int)value;
                }
                else if (t == typeof(Guid))
                {
                    p.Value = value.ToString();
                    p.DbType = DbType.String;
                    p.Size = 40;
                }
                else if (t == typeof(string))
                {
                    var v = value as string;
                    p.Value = value;
                    if (!string.IsNullOrEmpty(v))
                    {
                        p.Size = Math.Max(v.Length + 1, 4000);
                    }
                }
                else if (t == typeof(AnsiString))
                {
                    p.Size = Math.Max(((AnsiString)value).Value.Length + 1, 4000);
                    p.Value = ((AnsiString)value).Value;
                    p.DbType = DbType.AnsiString;
                }
                else
                {
                    p.Value = value;
                }
            }
        }

        /// <summary>
        /// 添加参数
        /// </summary>
        /// <param name="cmd">命令对象</param>
        /// <param name="name">参数名称</param>
        /// <param name="value">参数值</param>
        private void AddParam(DbCommand cmd, string name, object value)
        {
            name = ParameterHelper.RXParamsPrefix.Replace(name, m => m.Value.Substring(1));
            var idbParam = value as IDbDataParameter;
            if (idbParam != null)
            {
                idbParam.ParameterName = string.Format("{0}{1}", _paramPrefix, name);
                cmd.Parameters.Add(idbParam);
                return;
            }

            var p = cmd.CreateParameter();
            //p.ParameterName = string.Format("{0}{1}", _paramPrefix, name);
            p.ParameterName = name;
            var outParam = value as OutPutParam;
            if (outParam != null)
            {
                p.Direction = ParameterDirection.Output;
                p.Size = 8000;
                outParam.InnerParam = p;
                if (outParam.IsCursor)
                {
                    _provider.SetCursorParam(p);
                }
            }
            else
            {
                SetParam(p, value);
            }
            cmd.Parameters.Add(p);
        }

        /// <summary>
        /// 创建SQL语句命令对象
        /// </summary>
        /// <param name="connection">连接对象</param>
        /// <param name="sql">查询语句</param>
        /// <param name="args">参数</param>
        /// <returns>命令对象</returns>
        public DbCommand CreateCommand(DbConnection connection, string sql, object args)
        {
            if (sql == null)
            {
                throw new ArgumentNullException("sql", "sql语句不能为null");
            }
            DbCommand cmd = CreateCommand();
            if (args != null)
            {
                // 参数解析
                var args_dest = new List<KeyValuePair<string, object>>();
                sql = ParameterHelper.ParserParameter(sql, args, args_dest);
                foreach (var item in args_dest)
                {
                    AddParam(cmd, item.Key, item.Value);
                }
            }
            else if (_paramPrefix != "@")
            {
                sql = ParameterHelper.RXParamsPrefix.Replace(sql, m => _paramPrefix + m.Value.Substring(1));
            }
            sql = sql.Replace("@@", "@");

            if (connection != null)
            {
                cmd.Connection = connection;
                cmd.Transaction = _transaction;
            }
            cmd.CommandText = sql;
            return cmd;
        }

        /// <summary>
        /// 创建SQL语句命令对象
        /// </summary>
        /// <param name="sql">查询语句</param>
        /// <param name="args">参数</param>
        /// <returns>命令对象</returns>
        public DbCommand CreateCommand(string sql, object args)
        {
            return CreateCommand(null, sql, args);
        }

        /// <summary>
        /// 创建存储过程命令对象
        /// </summary>
        /// <param name="connection">连接对象</param>
        /// <param name="storeProc">存储过程定义</param>
        /// <returns>命令对象</returns>
        public DbCommand CreateCommand(DbConnection connection, StoreProc storeProc)
        {
            DbCommand cmd = CreateCommand();
            if (storeProc.Args != null)
            {
                // 参数解析
                var args_dest = new List<KeyValuePair<string, object>>();
                ParameterHelper.ParserStoreProcParameter(storeProc.Args, args_dest);
                foreach (var item in args_dest)
                {
                    AddParam(cmd, item.Key, item.Value);
                }
            }

            if (connection != null)
            {
                cmd.Connection = connection;
                cmd.Transaction = _transaction;
            }
            cmd.CommandText = storeProc.Name;
            cmd.CommandType = CommandType.StoredProcedure;
            return cmd;
        }

        /// <summary>
        /// 创建存储过程命令对象
        /// </summary>
        /// <param name="storeProc">存储过程定义</param>
        /// <returns>命令对象</returns>
        public DbCommand CreateCommand(StoreProc storeProc)
        {
            return CreateCommand(null, storeProc);
        }

        #endregion

        #region Execute

        /// <summary>
        /// 执行命令并返回影响的行数(参数值数组)
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <param name="arrayArgs">参数值数组</param>
        /// <returns>返回受影响的行数</returns>
        public int Execute(string sql, object[] arrayArgs)
        {
            return Execute(sql, args: arrayArgs);
        }

        /// <summary>
        /// 执行命令并返回影响的行数(参数字典)
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <param name="dicArgs">参数字典</param>
        /// <returns>返回受影响的行数</returns>
        public int Execute(string sql, IDictionary<string, object> dicArgs)
        {
            return Execute(sql, args: dicArgs);
        }

        /// <summary>
        /// 执行命令并返回影响的行数(参数对象)
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <param name="args">参数对象</param>
        /// <returns>返回受影响的行数</returns>
        public int Execute(string sql, object args = null)
        {
            try
            {
                OpenSharedConnection();
                try
                {
                    using (var cmd = CreateCommand(_sharedConnection, sql, args))
                    {
                        _provider.PreExecute(cmd);
                        DoPreExecute(cmd);
                        OnExecutingCommand(cmd);
                        var retv = cmd.ExecuteNonQuery();
                        OnExecutedCommand(cmd);
                        return retv;
                    }
                }
                finally
                {
                    CloseSharedConnection();
                }
            }
            catch (Exception x)
            {
                if (OnException(x))
                    throw;
                return -1;
            }
        }

        /// <summary>
        /// 执行命令并返回影响的行数
        /// </summary>
        /// <param name="storeProc">存储过程</param>
        /// <returns>返回受影响的行数</returns>
        public int Execute(StoreProc storeProc)
        {
            try
            {
                OpenSharedConnection();
                try
                {
                    using (var cmd = CreateCommand(_sharedConnection, storeProc))
                    {
                        _provider.PreExecute(cmd);
                        DoPreExecute(cmd);
                        OnExecutingCommand(cmd);
                        var retv = cmd.ExecuteNonQuery();
                        OnExecutedCommand(cmd);
                        return retv;
                    }
                }
                finally
                {
                    CloseSharedConnection();
                }
            }
            catch (Exception x)
            {
                if (OnException(x))
                    throw;
                return -1;
            }
        }

        /// <summary>
        /// 执行命令并返回影响的行数
        /// </summary>
        /// <param name="cmd">命令对象</param>
        /// <returns>返回受影响的行数</returns>
        public int Execute(DbCommand cmd)
        {
            try
            {
                OpenSharedConnection();
                try
                {
                    cmd.Connection = _sharedConnection;
                    cmd.Transaction = _transaction;
                    _provider.PreExecute(cmd);
                    DoPreExecute(cmd);
                    OnExecutingCommand(cmd);
                    var retv = cmd.ExecuteNonQuery();
                    OnExecutedCommand(cmd);
                    return retv;
                }
                finally
                {
                    CloseSharedConnection();
                }
            }
            catch (Exception x)
            {
                if (OnException(x))
                    throw;
                return -1;
            }
        }

        #endregion

        #region ExecuteScalar

        /// <summary>
        /// 执行命令并返回第一行第一列数据(参数值数组)
        /// </summary>
        /// <typeparam name="T">目标数据类型</typeparam>
        /// <param name="sql">SQL语句</param>
        /// <param name="arrayArgs">参数值数组</param>
        /// <returns>返回第一行第一列数据</returns>
        public T ExecuteScalar<T>(string sql, object[] arrayArgs)
        {
            return ExecuteScalar<T>(sql, args: arrayArgs);
        }

        /// <summary>
        /// 执行命令并返回第一行第一列数据(参数字典)
        /// </summary>
        /// <typeparam name="T">目标数据类型</typeparam>
        /// <param name="sql">SQL语句</param>
        /// <param name="dicArgs">参数字典</param>
        /// <returns>返回第一行第一列数据</returns>
        public T ExecuteScalar<T>(string sql, IDictionary<string, object> dicArgs)
        {
            return ExecuteScalar<T>(sql, args: dicArgs);
        }

        /// <summary>
        /// 执行命令并返回第一行第一列数据(参数对象)
        /// </summary>
        /// <typeparam name="T">目标数据类型</typeparam>
        /// <param name="sql">SQL语句</param>
        /// <param name="args">参数对象</param>
        /// <returns>返回第一行第一列数据</returns>
        public T ExecuteScalar<T>(string sql, object args = null)
        {
            try
            {
                OpenSharedConnection();
                try
                {
                    using (var cmd = CreateCommand(_sharedConnection, sql, args))
                    {
                        _provider.PreExecute(cmd);
                        DoPreExecute(cmd);
                        OnExecutingCommand(cmd);
                        object val = cmd.ExecuteScalar();
                        OnExecutedCommand(cmd);

                        Type u = Nullable.GetUnderlyingType(typeof(T));
                        if (u != null && (val == null || val == DBNull.Value))
                            return default(T);

                        return (T)Convert.ChangeType(val, u ?? typeof(T));
                    }
                }
                finally
                {
                    CloseSharedConnection();
                }
            }
            catch (Exception x)
            {
                if (OnException(x))
                    throw;
                return default(T);
            }
        }

        /// <summary>
        /// 执行命令并返回第一行第一列数据
        /// </summary>
        /// <typeparam name="T">目标数据类型</typeparam>
        /// <param name="storeProc">存储过程</param>
        /// <returns>返回第一行第一列数据</returns>
        public T ExecuteScalar<T>(StoreProc storeProc)
        {
            try
            {
                OpenSharedConnection();
                try
                {
                    using (var cmd = CreateCommand(_sharedConnection, storeProc))
                    {
                        _provider.PreExecute(cmd);
                        DoPreExecute(cmd);
                        OnExecutingCommand(cmd);
                        object val = cmd.ExecuteScalar();
                        OnExecutedCommand(cmd);

                        // Handle nullable types
                        Type u = Nullable.GetUnderlyingType(typeof(T));
                        if (u != null && val == null)
                            return default(T);

                        return (T)Convert.ChangeType(val, u ?? typeof(T));
                    }
                }
                finally
                {
                    CloseSharedConnection();
                }
            }
            catch (Exception x)
            {
                if (OnException(x))
                    throw;
                return default(T);
            }
        }

        /// <summary>
        /// 执行命令并返回第一行第一列数据
        /// </summary>
        /// <typeparam name="T">目标数据类型</typeparam>
        /// <param name="cmd">命令对象</param>
        /// <returns>返回第一行第一列数据</returns>
        public T ExecuteScalar<T>(DbCommand cmd)
        {
            try
            {
                OpenSharedConnection();
                try
                {
                    cmd.Connection = _sharedConnection;
                    cmd.Transaction = _transaction;
                    _provider.PreExecute(cmd);
                    DoPreExecute(cmd);
                    OnExecutingCommand(cmd);
                    object val = cmd.ExecuteScalar();
                    OnExecutedCommand(cmd);

                    Type u = Nullable.GetUnderlyingType(typeof(T));
                    if (u != null && val == null)
                        return default(T);

                    return (T)Convert.ChangeType(val, u ?? typeof(T));
                }
                finally
                {
                    CloseSharedConnection();
                }
            }
            catch (Exception x)
            {
                if (OnException(x))
                    throw;
                return default(T);
            }
        }

        #endregion

        #region ExecuteReader

        /// <summary>
        /// 返回DbDataReader对象,需要调用者关闭DbDataReader对象(参数值数组)
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <param name="arrayArgs">参数值数组</param>
        /// <returns>返回DbDataReader对象</returns>
        public DbDataReader ExecuteReader(string sql, object[] arrayArgs)
        {
            return ExecuteReader(sql, args: arrayArgs);
        }

        /// <summary>
        /// 返回DbDataReader对象,需要调用者关闭DbDataReader对象(参数字典)
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <param name="dicArgs">参数字典</param>
        /// <returns>返回DbDataReader对象</returns>
        public DbDataReader ExecuteReader(string sql, IDictionary<string, object> dicArgs)
        {
            return ExecuteReader(sql, args: dicArgs);
        }

        /// <summary>
        /// 返回DbDataReader对象,需要调用者关闭DbDataReader对象(参数对象)
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <param name="args">参数对象</param>
        /// <returns>返回DbDataReader对象</returns>
        public DbDataReader ExecuteReader(string sql, object args = null)
        {
            try
            {
                bool wasClosed = _sharedConnectionDepth == 0;
                CommandBehavior commandBehavior = CommandBehavior.Default;
                if (wasClosed) commandBehavior |= CommandBehavior.CloseConnection;
                OpenSharedConnection();
                try
                {
                    using (var cmd = CreateCommand(_sharedConnection, sql, args))
                    {
                        _provider.PreExecute(cmd);
                        DoPreExecute(cmd);
                        OnExecutingCommand(cmd);
                        DbDataReader reader = cmd.ExecuteReader(commandBehavior);
                        OnExecutedCommand(cmd);
                        wasClosed = false;
                        return reader;
                    }
                }
                finally
                {
                    if (wasClosed)
                    {
                        CloseSharedConnection();
                    }
                }
            }
            catch (Exception x)
            {
                if (OnException(x))
                    throw;
                return null;
            }
        }

        /// <summary>
        /// 返回DbDataReader对象,需要调用者关闭DbDataReader对象
        /// </summary>
        /// <param name="storeProc">存储过程</param>
        /// <returns>返回DbDataReader对象</returns>
        public DbDataReader ExecuteReader(StoreProc storeProc)
        {
            try
            {
                bool wasClosed = _sharedConnectionDepth == 0;
                CommandBehavior commandBehavior = CommandBehavior.Default;
                if (wasClosed) commandBehavior |= CommandBehavior.CloseConnection;
                OpenSharedConnection();
                try
                {
                    using (var cmd = CreateCommand(_sharedConnection, storeProc))
                    {
                        _provider.PreExecute(cmd);
                        DoPreExecute(cmd);
                        OnExecutingCommand(cmd);
                        DbDataReader reader = cmd.ExecuteReader(commandBehavior);
                        OnExecutedCommand(cmd);
                        wasClosed = false;
                        return reader;
                    }
                }
                finally
                {
                    if (wasClosed)
                    {
                        CloseSharedConnection();
                    }
                }
            }
            catch (Exception x)
            {
                if (OnException(x))
                    throw;
                return null;
            }
        }

        /// <summary>
        /// 返回DbDataReader对象,需要调用者关闭DbDataReader对象
        /// </summary>
        /// <param name="cmd">命令对象</param>
        /// <returns>返回DbDataReader对象</returns>
        public DbDataReader ExecuteReader(DbCommand cmd)
        {
            try
            {
                bool wasClosed = _sharedConnectionDepth == 0;
                CommandBehavior commandBehavior = CommandBehavior.Default;
                if (wasClosed) commandBehavior |= CommandBehavior.CloseConnection;
                OpenSharedConnection();
                try
                {
                    cmd.Connection = _sharedConnection;
                    cmd.Transaction = _transaction;
                    _provider.PreExecute(cmd);
                    DoPreExecute(cmd);
                    OnExecutingCommand(cmd);
                    DbDataReader reader = cmd.ExecuteReader(commandBehavior);
                    OnExecutedCommand(cmd);
                    wasClosed = false;
                    return reader;
                }
                finally
                {
                    if (wasClosed)
                    {
                        CloseSharedConnection();
                    }
                }
            }
            catch (Exception x)
            {
                if (OnException(x))
                    throw;
                return null;
            }
        }

        #endregion

        #region ExecuteDataSet

        /// <summary>
        /// 执行命令并返回数据集(参数值数组)
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <param name="arrayArgs">参数值数组</param>
        /// <returns>返回新创建的数据集</returns>
        public DataSet ExecuteDataSet(string sql, object[] arrayArgs)
        {
            return ExecuteDataSet(sql, args: arrayArgs);
        }

        /// <summary>
        /// 执行命令并返回数据集(参数字典)
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <param name="dicArgs">参数字典</param>
        /// <returns>返回新创建的数据集</returns>
        public DataSet ExecuteDataSet(string sql, IDictionary<string, object> dicArgs)
        {
            return ExecuteDataSet(sql, args: dicArgs);
        }

        /// <summary>
        /// 执行命令并返回数据集(参数对象)
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <param name="args">参数对象</param>
        /// <returns>返回新创建的数据集</returns>
        public DataSet ExecuteDataSet(string sql, object args = null)
        {
            try
            {
                OpenSharedConnection();
                try
                {
                    DataSet ds = new DataSet();
                    using (var cmd = CreateCommand(_sharedConnection, sql, args))
                    {
                        using (DbDataAdapter dbDataAdapter = CreateDataAdapter())
                        {
                            // ReSharper disable once PossibleNullReferenceException
                            dbDataAdapter.SelectCommand = cmd;
                            _provider.PreExecute(cmd);
                            DoPreExecute(cmd);
                            OnExecutingCommand(cmd);
                            dbDataAdapter.Fill(ds);
                            OnExecutedCommand(cmd);
                        }
                    }
                    return ds;
                }
                finally
                {
                    CloseSharedConnection();
                }
            }
            catch (Exception x)
            {
                if (OnException(x))
                    throw;
                return null;
            }
        }

        /// <summary>
        /// 执行命令并返回数据集
        /// </summary>
        /// <param name="storeProc">存储过程</param>
        /// <returns>返回新创建的数据集</returns>
        public DataSet ExecuteDataSet(StoreProc storeProc)
        {
            try
            {
                OpenSharedConnection();
                try
                {
                    DataSet ds = new DataSet();
                    using (var cmd = CreateCommand(_sharedConnection, storeProc))
                    {
                        using (DbDataAdapter dbDataAdapter = CreateDataAdapter())
                        {
                            // ReSharper disable once PossibleNullReferenceException
                            dbDataAdapter.SelectCommand = cmd;
                            _provider.PreExecute(cmd);
                            DoPreExecute(cmd);
                            OnExecutingCommand(cmd);
                            dbDataAdapter.Fill(ds);
                            OnExecutedCommand(cmd);
                        }
                    }
                    return ds;
                }
                finally
                {
                    CloseSharedConnection();
                }
            }
            catch (Exception x)
            {
                if (OnException(x))
                    throw;
                return null;
            }
        }

        /// <summary>
        /// 执行命令并返回数据集
        /// </summary>
        /// <param name="cmd">命令对象</param>
        /// <returns>返回新创建的数据集</returns>
        public DataSet ExecuteDataSet(DbCommand cmd)
        {
            try
            {
                OpenSharedConnection();
                try
                {
                    DataSet ds = new DataSet();
                    using (DbDataAdapter dbDataAdapter = CreateDataAdapter())
                    {
                        cmd.Connection = _sharedConnection;
                        cmd.Transaction = _transaction;
                        // ReSharper disable once PossibleNullReferenceException
                        dbDataAdapter.SelectCommand = cmd;
                        _provider.PreExecute(cmd);
                        DoPreExecute(cmd);
                        OnExecutingCommand(cmd);
                        dbDataAdapter.Fill(ds);
                        OnExecutedCommand(cmd);
                    }
                    return ds;
                }
                finally
                {
                    CloseSharedConnection();
                }
            }
            catch (Exception x)
            {
                if (OnException(x))
                    throw;
                return null;
            }
        }

        /// <summary>
        /// 执行命令并返回数据表(参数值数组)
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <param name="arrayArgs">参数值数组</param>
        /// <returns>返回新创建的数据表</returns>
        public DataTable ExecuteDataTable(string sql, object[] arrayArgs)
        {
            return ExecuteDataTable(sql, args: arrayArgs);
        }

        /// <summary>
        /// 执行命令并返回数据表(参数字典)
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <param name="dicArgs">参数字典</param>
        /// <returns>返回新创建的数据表</returns>
        public DataTable ExecuteDataTable(string sql, IDictionary<string, object> dicArgs)
        {
            return ExecuteDataTable(sql, args: dicArgs);
        }

        /// <summary>
        /// 执行命令并返回数据表(参数对象)
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <param name="args">参数对象</param>
        /// <returns>返回新创建的数据表</returns>
        public DataTable ExecuteDataTable(string sql, object args = null)
        {
            var ds = ExecuteDataSet(sql, args);
            return ds.Tables.Count > 0 ? ds.Tables[0] : null;
        }

        /// <summary>
        /// 执行命令并返回数据表
        /// </summary>
        /// <param name="storeProc">存储过程</param>
        /// <returns>返回新创建的数据表</returns>
        public DataTable ExecuteDataTable(StoreProc storeProc)
        {
            var ds = ExecuteDataSet(storeProc);
            return ds.Tables.Count > 0 ? ds.Tables[0] : null;
        }

        /// <summary>
        /// 执行命令并返回数据表
        /// </summary>
        /// <param name="cmd">命令对象</param>
        /// <returns>返回新创建的数据表</returns>
        public DataTable ExecuteDataTable(DbCommand cmd)
        {
            var ds = ExecuteDataSet(cmd);
            return ds.Tables.Count > 0 ? ds.Tables[0] : null;
        }

        #endregion

        #region Query

        /// <summary>
        /// 查询数据(参数对象)
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="sql">查询语句</param>
        /// <param name="args">参数对象</param>
        /// <returns>返回记录集合</returns>
        public IEnumerable<T> Query<T>(string sql, object args = null)
        {
            OpenSharedConnection();
            try
            {
                using (var cmd = CreateCommand(_sharedConnection, sql, args))
                {
                    IDataReader r;
                    var pd = PocoData.ForType(typeof(T));
                    try
                    {
                        _provider.PreExecute(cmd);
                        DoPreExecute(cmd);
                        OnExecutingCommand(cmd);
                        r = cmd.ExecuteReader();
                        OnExecutedCommand(cmd);
                    }
                    catch (Exception x)
                    {
                        if (OnException(x))
                            throw;
                        yield break;
                    }
                    var factory = pd.GetFactory(cmd.CommandText, _sharedConnection.ConnectionString, 0, r.FieldCount, r) as Func<IDataReader, T>;
                    using (r)
                    {
                        while (true)
                        {
                            T poco;
                            try
                            {
                                if (!r.Read())
                                    yield break;
                                // ReSharper disable once PossibleNullReferenceException
                                poco = factory(r);
                            }
                            catch (Exception x)
                            {
                                if (OnException(x))
                                    throw;
                                yield break;
                            }

                            yield return poco;
                        }
                    }
                }
            }
            finally
            {
                CloseSharedConnection();
            }
        }

        /// <summary>
        /// 查询数据(参数对象)
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="cmd">命令对象</param>
        /// <returns>返回记录集合</returns>
        public IEnumerable<T> Query<T>(DbCommand cmd)
        {
            OpenSharedConnection();
            try
            {
                IDataReader r;
                var pd = PocoData.ForType(typeof(T));
                try
                {
                    _provider.PreExecute(cmd);
                    DoPreExecute(cmd);
                    OnExecutingCommand(cmd);
                    r = cmd.ExecuteReader();
                    OnExecutedCommand(cmd);
                }
                catch (Exception x)
                {
                    if (OnException(x))
                        throw;
                    yield break;
                }
                var factory = pd.GetFactory(cmd.CommandText, _sharedConnection.ConnectionString, 0, r.FieldCount, r) as Func<IDataReader, T>;
                using (r)
                {
                    while (true)
                    {
                        T poco;
                        try
                        {
                            if (!r.Read())
                                yield break;
                            // ReSharper disable once PossibleNullReferenceException
                            poco = factory(r);
                        }
                        catch (Exception x)
                        {
                            if (OnException(x))
                                throw;
                            yield break;
                        }

                        yield return poco;
                    }
                }
            }
            finally
            {
                CloseSharedConnection();
            }
        }

        /// <summary>
        /// 查询数据(参数对象)
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="storeProc">存储过程</param>
        /// <returns>返回记录集合</returns>
        public IEnumerable<T> Query<T>(StoreProc storeProc)
        {
            OpenSharedConnection();
            try
            {
                using (var cmd = CreateCommand(_sharedConnection, storeProc))
                {
                    IDataReader r;
                    var pd = PocoData.ForType(typeof(T));
                    try
                    {
                        _provider.PreExecute(cmd);
                        DoPreExecute(cmd);
                        OnExecutingCommand(cmd);
                        r = cmd.ExecuteReader();
                        OnExecutedCommand(cmd);
                    }
                    catch (Exception x)
                    {
                        if (OnException(x))
                            throw;
                        yield break;
                    }
                    var factory = pd.GetFactory(cmd.CommandText, _sharedConnection.ConnectionString, 0, r.FieldCount, r) as Func<IDataReader, T>;
                    using (r)
                    {
                        while (true)
                        {
                            T poco;
                            try
                            {
                                if (!r.Read())
                                    yield break;
                                // ReSharper disable once PossibleNullReferenceException
                                poco = factory(r);
                            }
                            catch (Exception x)
                            {
                                if (OnException(x))
                                    throw;
                                yield break;
                            }

                            yield return poco;
                        }
                    }
                }
            }
            finally
            {
                CloseSharedConnection();
            }
        }

        #endregion

        #region 异常日志管理

        /// <summary>
        /// 当执行数据库查询发送异常时执行
        /// </summary>
        /// <param name="x">异常对象</param>
        /// <returns>返回true则重新抛出异常,否则返回false.</returns>
        protected virtual bool OnException(Exception x)
        {
            Debug.WriteLine(string.Format("发生异常 {0} {1}",
                DateTimeHelper.FormatDateHasMilliSecond(DateTime.Now), x.Message), "Database");
            return true;
        }

        /// <summary>
        /// 当打开数据库连接时调用
        /// </summary>
        /// <param name="conn">数据库连接对象</param>
        protected virtual DbConnection OnConnectionOpened(DbConnection conn)
        {
            Debug.WriteLine(string.Format("{0} 打开连接", DateTimeHelper.FormatDateHasMilliSecond(DateTime.Now)), "Database");
            return conn;
        }

        /// <summary>
        /// 当关闭数据库连接时调用
        /// </summary>
        /// <param name="conn">数据库连接对象</param>
        protected virtual void OnConnectionClosing(DbConnection conn)
        {
            Debug.WriteLine(string.Format("{0} 关闭连接", DateTimeHelper.FormatDateHasMilliSecond(DateTime.Now)), "Database");
        }

        /// <summary>
        /// 当命令对象执行前调用
        /// </summary>
        /// <param name="cmd">命令对象</param>
        protected virtual void OnExecutingCommand(DbCommand cmd)
        {
            currentCommand = cmd;

            TimeSpan ts = stopwatch.Elapsed;
            string timeString = string.Format("执行耗时：{0:###0.##}毫秒", ts.TotalMilliseconds);
            var msg = string.Format("{0} 执行命令 {1}  {2}",
                DateTimeHelper.FormatDateHasMilliSecond(DateTime.Now), FormatCommand(cmd), timeString);
            //Debug.WriteLine(msg, "Database");
            Trace.WriteLine(msg, "Database");
            stopwatch.Reset();
            stopwatch.Start();

        }

        /// <summary>
        /// 当命令对象执行完成时调用
        /// </summary>
        /// <param name="cmd">命令对象</param>
        protected virtual void OnExecutedCommand(DbCommand cmd)
        {
            stopwatch.Stop();
            //if (currentCommand == cmd)
            //{
            //    TimeSpan ts = stopwatch.Elapsed;
            //    string timeString = string.Format("执行耗时：{0:###0.##}毫秒", ts.TotalMilliseconds);
            //    Debug.WriteLine(string.Format("{0} 执行命令 {1} \n\t{2}",
            //        DateTimeHelper.FormatDateHasMilliSecond(DateTime.Now), FormatCommand(cmd), timeString), "Database");
            //}
        }

        #endregion

        #region 私有方法

        ///// <summary>
        ///// 获取排序子句
        ///// </summary>
        ///// <param name="orderBy">排序子句(不包括ORDER BY字符串)</param>
        ///// <param name="pd">实体元数据</param>
        ///// <returns>返回排序子句</returns>
        //private string GetOrderBys(string orderBy, PocoData pd)
        //{
        //    string orderByString = string.Empty;
        //    if (!string.IsNullOrEmpty(orderBy))
        //    {
        //        orderByString = string.Format("ORDER BY {0}", orderBy);
        //    }
        //    else if (!string.IsNullOrEmpty(pd.TableInfo.OrderBy))
        //    {
        //        orderByString = string.Format("ORDER BY {0}", pd.TableInfo.OrderBy);
        //    }
        //    return orderByString;
        //}

        /// <summary>
        /// 设置命令对象属性
        /// </summary>
        /// <param name="cmd">命令对象</param>
        private void DoPreExecute(DbCommand cmd)
        {
            if (OneTimeCommandTimeout != 0)
            {
                cmd.CommandTimeout = OneTimeCommandTimeout;
                OneTimeCommandTimeout = 0;
            }
            else if (CommandTimeout != 0)
            {
                cmd.CommandTimeout = CommandTimeout;
            }
        }

        //internal void ExecuteNonQueryHelper(DbCommand cmd)
        //{
        //    DoPreExecute(cmd);
        //    cmd.ExecuteNonQuery();
        //    OnExecutedCommand(cmd);
        //}

        //internal object ExecuteScalarHelper(DbCommand cmd)
        //{
        //    DoPreExecute(cmd);
        //    object r = cmd.ExecuteScalar();
        //    OnExecutedCommand(cmd);
        //    return r;
        //}

        /// <summary>
        /// 获取字段值类型对应的SQL类型
        /// </summary>
        /// <param name="type">字段值类型</param>
        protected virtual DbType CSharpTypeToDbType(Type type)
        {
            if (type == typeof(string))
                return DbType.String;
            if (type == typeof(Int32))
                return DbType.Int32;
            if (type == typeof(bool))
                return DbType.Boolean;
            if (type == typeof(DateTime))
                return DbType.DateTime;
            if (type == typeof(Decimal))
                return DbType.Decimal;
            if (type == typeof(Double))
                return DbType.Double;
            if (type == typeof(byte[]))
                return DbType.Binary;
            if (type == typeof(Int16))
                return DbType.Int16;
            if (type == typeof(Int64))
                return DbType.Int64;
            if (type == typeof(byte))
                return DbType.Byte;
            if (type == typeof(Guid))
                return DbType.Guid;
            if (type == typeof(char))
                return DbType.AnsiString;

            return DbType.String;
        }

        #endregion
    }
}