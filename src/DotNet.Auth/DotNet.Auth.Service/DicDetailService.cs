// ===============================================================================
// DotNet.Platform 开发框架 2016 版权所有
// ===============================================================================
using System.Collections.Generic;
using System.Linq;
using DotNet.Auth.Entity;
using DotNet.Collections;
using DotNet.Extensions;
using DotNet.Utility;

namespace DotNet.Auth.Service
{
    /// <summary>
    /// 系统字典明细服务
    /// </summary>
    public class DicDetailService
    {
        private static readonly Cache<string, DicDetail> Cache = new Cache<string, DicDetail>();

        /// <summary>
        /// 构造服务
        /// </summary>
        internal DicDetailService()
        {
            InitCache();
        }

        /// <summary>
        /// 初始化缓存
        /// </summary>
        public void InitCache()
        {
            var repos = new AuthRepository<DicDetail>();
            Cache.Clear().Set(repos.Query().ToDictionary(p => p.Id, p => p));
        }

        /// <summary>
        /// 是否存在指定名称的对象
        /// </summary>
        /// <param name="dicId">字典主键</param>
        /// <param name="id">明细主键</param>
        /// <param name="name">明细名称</param>
        /// <returns>存在返回false</returns>
        public BoolMessage ExistsByName(string dicId, string id, string name)
        {
            var has = Cache.ValueList().Contains(
                p => p.DicId.Equals(dicId) && p.Name.Equals(name) && !p.Id.Equals(id));
            return has ? new BoolMessage(false, "指定的项名称已经存在") : BoolMessage.True;
        }

        /// <summary>
        /// 是否存在指定字典主键数组的明细项
        /// </summary>
        /// <param name="dicIds">字典主键数组</param>
        /// <returns>存在返回true</returns>
        public bool ExistsByDicIds(string[] dicIds)
        {
            return Cache.ValueList().Contains(p => p.DicId.InArray(dicIds));
        }

        /// <summary>
        /// 添加对象
        /// </summary>
        /// <param name="entity">实体</param>
        public BoolMessage Create(DicDetail entity)
        {
            var repos = new AuthRepository<DicDetail>();
            repos.Insert(entity);
            Cache.Set(entity.Id, entity);
            return BoolMessage.True;
        }

        /// <summary>
        /// 更新对象
        /// </summary>
        /// <param name="entity">实体</param>
        public BoolMessage Update(DicDetail entity)
        {
            var repos = new AuthRepository<DicDetail>();
            repos.Update(entity);
            Cache.Set(entity.Id, entity);
            return BoolMessage.True;
        }

        /// <summary>
        /// 保存对象
        /// </summary>
        /// <param name="entity">实体</param>
        /// <param name="isCreate">是否新增</param>
        public BoolMessage Save(DicDetail entity, bool isCreate)
        {
            return isCreate ? Create(entity) : Update(entity);
        }

        /// <summary>
        /// 删除对象
        /// </summary>
        /// <param name="ids">主键数组</param>
        public BoolMessage Delete(string[] ids)
        {
            if (ids.Length == 0) return BoolMessage.True;
            var repos = new AuthRepository<DicDetail>();
            repos.Delete(ids);
            Cache.Remove(ids);
            return BoolMessage.True;
        }

        /// <summary>
        /// 获取对象
        /// </summary>
        /// <param name="id">主键</param>
        public DicDetail Get(string id)
        {
            return Cache.Get(id);
        }

        /// <summary>
        /// 根据数据字典明细名称,获取字典明细名称
        /// </summary>
        /// <param name="dicCode">字典编码</param>
        /// <param name="value">明细值</param>
        /// <param name="defaultName">找不到时的默认值</param>
        public string GetNameByValue(string dicCode, string value, string defaultName = null)
        {
            var list = GetListByDicCode(dicCode);
            return list.FirstOrDefault(p => p.Value.Equals(value))?.Name ?? defaultName;
        }

        /// <summary>
        /// 根据数据字典明细名称,获取字典明细名称
        /// </summary>
        /// <param name="dicCode">字典编码</param>
        /// <param name="value">明细值</param>
        public string GetNameByValue(string dicCode, int value)
        {
            var list = GetListByDicCode(dicCode);
            return list.FirstOrDefault(p => p.Value.Equals(value.ToString()))?.Name ?? string.Empty;
        }

        /// <summary>
        /// 根据数据字典明细值,获取字典明细名称
        /// </summary>
        /// <param name="dicCode">字典编码</param>
        /// <param name="name">明细名称</param>
        /// <param name="defaultValue">找不到时的默认值</param>
        public string GetValueByName(string dicCode, string name, string defaultValue = null)
        {
            var list = GetListByDicCode(dicCode);
            return list.FirstOrDefault(p => p.Name.Equals(name))?.Value ?? defaultValue;
        }

        /// <summary>
        /// 获取新建序号
        /// </summary>
        public int GetNewRowIndex(string dicId)
        {
            return Cache.ValueList().Where(p => p.DicId.Equals(dicId)).ToList().Max(p => p.RowIndex, 0) + 1;
        }

        /// <summary>
        /// 获取启用的简单对象集合(已排序)
        /// </summary>
        /// <param name="dicId">字典主键</param>
        public List<Simple> GetSimpleList(string dicId)
        {
            return Cache.ValueList()
                .Where(p => p.DicId.Equals(dicId)).ToList()
                .OrderByAsc(p => p.RowIndex)
                .Select(p => new Simple(p.Value, p.Name, p.Spell)).ToList();
        }

        /// <summary>
        /// 获取对象集合(已排序)
        /// </summary>
        /// <param name="dicId">字典主键</param>
        /// <param name="isEnabled">启用状态(null为全部,true为启用,false为禁用)</param>
        /// <param name="isOrderBy">是否排序</param>
        public List<DicDetail> GetList(string dicId, bool? isEnabled = true, bool isOrderBy = true)
        {
            var list = Cache.ValueList()
                .Where(p => p.DicId.Equals(dicId));
            if (isEnabled.HasValue)
            {
                list = list.Where(p => p.IsEnabled == isEnabled);
            }
            return isOrderBy ? list.ToList().OrderByAsc(p => p.RowIndex) : list.ToList();
        }

        /// <summary>
        /// 获取数组字典明细根据字典编码
        /// </summary>
        /// <param name="dicCode">字典编码</param>
        public List<DicDetail> GetListByDicCode(string dicCode)
        {
            var dic = AuthService.Dic.GetByCode(dicCode);
            return Cache.ValueList()
                .Where(p => p.IsEnabled && p.DicId.Equals(dic.Id)).ToList()
                .OrderByAsc(p => p.RowIndex);
        }

        /// <summary>
        /// 获取对象集合
        /// </summary>
        /// <param name="dicIds">字典主键数组</param>
        /// <param name="isEnabled">启用状态(null为全部,true为启用,false为禁用)</param>
        /// <param name="isOrderBy">是否排序</param>
        public List<DicDetail> GetList(string[] dicIds, bool? isEnabled = true, bool isOrderBy = true)
        {
            var list = Cache.ValueList()
                .Where(p => p.DicId.InArray(dicIds));
            if (isEnabled.HasValue)
            {
                list = list.Where(p => p.IsEnabled == isEnabled);
            }
            return isOrderBy ? list.ToList().OrderByAsc(p => p.RowIndex) : list.ToList();
        }

        /// <summary>
        /// 获取对象分页集合
        /// </summary>
        /// <param name="pageCondition">分页对象</param>
        /// <param name="dicId">字典主键</param>
        /// <param name="name">项名称关键字</param>
        /// <param name="isEnabled">启用</param>
        public PageList<DicDetail> GetPageList(PaginationCondition pageCondition,
            string dicId, string name, bool? isEnabled)
        {
            pageCondition.SetDefaultOrder(nameof(DicDetail.RowIndex));
            var repos = new AuthRepository<DicDetail>();
            var query = repos.PageQuery(pageCondition).Where(p => p.DicId.Equals(dicId));
            if (isEnabled.HasValue)
            {
                query.Where(p => p.IsEnabled == isEnabled.Value);
            }
            if (name.IsNotEmpty())
            {
                name = name.Trim();
                query.Where(p => p.Name.Contains(name) || p.Spell.Contains(name));
            }
            return repos.Page(query);
        }
    }
}