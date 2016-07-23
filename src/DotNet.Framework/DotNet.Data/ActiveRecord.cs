//// ===============================================================================
//// DotNet.Platform 开发框架 2016 版权所有
//// ===============================================================================
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using DotNet.Entity;
//using DotNet.Extensions;
//using DotNet.Utility;

//namespace DotNet.Data
//{
//    /// <summary>
//    /// 单条记录操作
//    /// </summary>
//    /// <typeparam name="T">实体类型</typeparam>
//    public abstract class ActiveRecord<T> where T : class, new()
//    {
//        protected static readonly Cache<string, T> _cache = new Cache<string, T>();
//        private readonly Database _database;
//        private EntityMetadata _meta;
//        private Repository<T> _repository;

//        public ActiveRecord()
//        {

//        }

//        public ActiveRecord(Database database)
//        {
//            _database = database;
//        }

//        /// <summary>
//        /// 存储器
//        /// </summary>
//        protected Repository<T> Repository => _repository ?? (_repository = new Repository<T>(_database));

//        /// <summary>
//        /// 实体元数据
//        /// </summary>
//        protected EntityMetadata Metadata => _meta ?? (_meta = EntityMetadata.ForType(typeof(T)));

//        /// <summary>
//        /// 缓存对象
//        /// </summary>
//        protected Cache<string, T> Cache => _cache;

//        /// <summary>
//        /// 是否启用缓存
//        /// </summary>
//        protected virtual bool EnableCache { get; }

//        /// <summary>
//        /// 添加对象
//        /// </summary>
//        /// <param name="entity">实体</param>
//        protected BoolMessage Create(T entity)
//        {
//            Repository.Insert(entity);
//            if (EnableCache)
//            {
//                _cache.Set(GetEntityId(entity), entity);
//            }
//            return BoolMessage.True;
//        }

//        /// <summary>
//        /// 更新对象
//        /// </summary>
//        /// <param name="entity">实体</param>
//        protected BoolMessage Update(T entity)
//        {
//            Repository.Update(entity);
//            if (EnableCache)
//            {
//                _cache.Set(GetEntityId(entity), entity);
//            }
//            return BoolMessage.True;
//        }

//        /// <summary>
//        /// 保存对象
//        /// </summary>
//        /// <param name="entity">实体</param>
//        /// <param name="isCreate">是否新增</param>
//        protected BoolMessage Save(T entity, bool isCreate)
//        {
//            return isCreate ? Create(entity) : Update(entity);
//        }

//        /// <summary>
//        /// 删除对象
//        /// </summary>
//        /// <param name="id">主键</param>
//        protected BoolMessage Delete(string id)
//        {
//            Repository.Delete(id);
//            if (EnableCache)
//            {
//                _cache.Remove(id);
//            }
//            return BoolMessage.True;
//        }

//        /// <summary>
//        /// 获取对象
//        /// </summary>
//        /// <param name="id">主键</param>
//        protected T Get(string id)
//        {
//            return EnableCache ? _cache.Get(id) : Repository.Get(id);
//        }

//        /// <summary>
//        /// 获取对象集合
//        /// </summary>
//        protected List<T> GetList()
//        {
//            return EnableCache ? _cache.ValueList(): Repository.Fetch();
//        }

//        /// <summary>
//        /// 获取实体主键
//        /// </summary>
//        /// <param name="entity">实体类型</param>
//        /// <returns>返回实体主键</returns>
//        protected string GetEntityId(T entity)
//        {
//            return Metadata.GetPrimaryKeyValue(entity).ToStringOrEmpty();
//        }
//    }
//}