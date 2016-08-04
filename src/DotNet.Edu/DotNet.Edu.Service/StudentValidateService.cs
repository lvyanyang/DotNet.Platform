// ===============================================================================
// DotNet.Platform 开发框架 2016 版权所有
// ===============================================================================

using System.Collections.Generic;
using System.Linq;
using DotNet.Edu.Entity;
using DotNet.Collections;
using DotNet.Extensions;
using DotNet.Helper;
using DotNet.Utility;

namespace DotNet.Edu.Service
{
    /// <summary>
    /// 学习验证服务
    /// </summary>
    public class StudentValidateService
    {
        private static readonly Cache<string, StudentValidate> Cache = new Cache<string, StudentValidate>();

        /// <summary>
        /// 构造服务
        /// </summary>
        internal StudentValidateService()
        {
            InitCache();
        }

        /// <summary>
        /// 初始化缓存
        /// </summary>
        public void InitCache()
        {
            Cache.Clear().Set(new EduRepository<StudentValidate>().Query().ToDictionary(p => p.Id, p => p));
        }

        /// <summary>
        /// 是否存在指定名称的对象
        /// </summary>
        /// <param name="id">主键</param>
        /// <param name="name">名称</param>
        /// <returns>如果存在返回false</returns>
        public BoolMessage ExistsByName(string id, string name)
        {
            var has = Cache.ValueList().Contains(p => p.Name.Equals(name) && !p.Id.Equals(id));
            return has ? new BoolMessage(false, "指定的名称已经存在") : BoolMessage.True;
        }

        /// <summary>
        /// 添加对象
        /// </summary>
        /// <param name="entity">实体</param>
        public BoolMessage Create(StudentValidate entity)
        {
            var repos = new EduRepository<StudentValidate>();
            repos.Insert(entity);
            Cache.Set(entity.Id, entity);
            return BoolMessage.True;
        }

        /// <summary>
        /// 更新对象
        /// </summary>
        /// <param name="entity">实体</param>
        public BoolMessage Update(StudentValidate entity)
        {
            var repos = new EduRepository<StudentValidate>();
            repos.Update(entity);
            Cache.Set(entity.Id, entity);
            return BoolMessage.True;
        }

        /// <summary>
        /// 保存对象
        /// </summary>
        /// <param name="entity">实体</param>
        /// <param name="isCreate">是否新增</param>
        public BoolMessage Save(StudentValidate entity, bool isCreate)
        {
            return isCreate ? Create(entity) : Update(entity);
        }

        /// <summary>
        /// 删除对象
        /// </summary>
        /// <param name="ids">主键数组</param>
        public BoolMessage Delete(string[] ids)
        {
            var repos = new EduRepository<StudentValidate>();
            repos.Delete(ids);
            Cache.Remove(ids);
            return BoolMessage.True;
        }

        /// <summary>
        /// 获取对象
        /// </summary>
        /// <param name="id">主键</param>
        public StudentValidate Get(string id)
        {
            return Cache.Get(id);
        }

        /// <summary>
        /// 获取随机对象
        /// </summary>
        public StudentValidate GetRandom()
        {
            var list = Cache.ValueList();
            #region Init
            if (list.Count == 0)
            {
                list.Add(new StudentValidate
                {
                    Id = StringHelper.Guid(),
                    Name = "2+3",
                    A = "4",
                    B = "5",
                    C = "6",
                    D = "7",
                    Answer = "5"
                });
                list.Add(new StudentValidate
                {
                    Id = StringHelper.Guid(),
                    Name = "7+8",
                    A = "12",
                    B = "13",
                    C = "14",
                    D = "15",
                    Answer = "15"
                });
            }
            #endregion
            var randomNumber = RandomHelper.GenerateRandomString(StringHelper.GetArrayByRange(0, list.Count - 1));
            return list[randomNumber.ToInt()];
        }

        /// <summary>
        /// 获取对象集合
        /// </summary>
        public List<StudentValidate> GetList()
        {
            return Cache.ValueList();
        }

        /// <summary>
        /// 获取对象分页集合
        /// </summary>
        /// <param name="pageCondition">分页对象</param>
        /// <param name="name">名称关键字</param>
        public PageList<StudentValidate> GetPageList(PaginationCondition pageCondition, string name)
        {
            pageCondition.SetDefaultOrder(nameof(StudentValidate.Name));
            var repos = new EduRepository<StudentValidate>();
            var query = repos.PageQuery(pageCondition);
            if (name.IsNotEmpty())
            {
                name = name.Trim();
                query.Where(p => p.Name.Contains(name));
            }
            return repos.Page(query);
        }
    }
}
