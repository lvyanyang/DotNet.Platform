// ===============================================================================
// DotNet.Platform 开发框架 2016 版权所有
// ===============================================================================

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using DotNet.Auth.Service;
using DotNet.Auth.Utility;
using DotNet.Edu.Entity;
using DotNet.Collections;
using DotNet.Edu.Utility;
using DotNet.Extensions;
using DotNet.Helper;
using DotNet.Utility;

namespace DotNet.Edu.Service
{
    /// <summary>
    /// 题库服务
    /// </summary>
    public class QuestionService
    {
        private static readonly Cache<string, Question> Cache = new Cache<string, Question>();

        /// <summary>
        /// 构造服务
        /// </summary>
        internal QuestionService()
        {
            InitCache();
        }

        /// <summary>
        /// 初始化缓存
        /// </summary>
        public void InitCache()
        {
            Cache.Clear().Set(new EduRepository<Question>().Query().ToDictionary(p => p.Id, p => p));
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
            return has ? new BoolMessage(false, "指定的题目名称已经存在") : BoolMessage.True;
        }

        /// <summary>
        /// 添加对象
        /// </summary>
        /// <param name="entity">实体</param>
        public BoolMessage Create(Question entity)
        {
            var repos = new EduRepository<Question>();
            repos.Insert(entity);
            Cache.Set(entity.Id, entity);
            return BoolMessage.True;
        }

        /// <summary>
        /// 更新对象
        /// </summary>
        /// <param name="entity">实体</param>
        public BoolMessage Update(Question entity)
        {
            var repos = new EduRepository<Question>();
            repos.Update(entity);
            Cache.Set(entity.Id, entity);
            return BoolMessage.True;
        }

        /// <summary>
        /// 保存对象
        /// </summary>
        /// <param name="entity">实体</param>
        /// <param name="isCreate">是否新增</param>
        public BoolMessage Save(Question entity, bool isCreate)
        {
            return isCreate ? Create(entity) : Update(entity);
        }

        /// <summary>
        /// 删除对象
        /// </summary>
        /// <param name="ids">主键数组</param>
        public BoolMessage Delete(string[] ids)
        {
            var repos = new EduRepository<Question>();
            repos.Delete(ids);
            Cache.Remove(ids);
            return BoolMessage.True;
        }

        /// <summary>
        /// 获取对象
        /// </summary>
        /// <param name="id">主键</param>
        public Question Get(string id)
        {
            return Cache.Get(id);
        }

        /// <summary>
        /// 获取对象集合
        /// </summary>
        public List<Question> GetList(string name, string questType, string workType, string questUnit)
        {
            var query = Cache.ValueList().AsEnumerable();
            if (name.IsNotEmpty())
            {
                name = name.Trim();
                query = query.Where(p => p.Name.Contains(name));
            }
            if (questType.IsNotEmpty())
            {
                query = query.Where(p => p.QuestType == questType);
            }
            if (workType.IsNotEmpty())
            {
                query = query.Where(p => p.WorkType == workType);
            }
            if (questUnit.IsNotEmpty())
            {
                query = query.Where(p => p.QuestUnit == questUnit);
            }
            return query.ToList();
        }

        /// <summary>
        /// 获取对象分页集合
        /// </summary>
        /// <param name="pageCondition">分页对象</param>
        /// <param name="name">名称关键字</param>
        /// <param name="questType"></param>
        /// <param name="workType"></param>
        /// <param name="questUnit"></param>
        public PageList<Question> GetPageList(PaginationCondition pageCondition,
            string name, string questType, string workType, string questUnit)
        {
            pageCondition.SetDefaultOrder(nameof(Question.CreateDateTime));
            var repos = new EduRepository<Question>();
            var query = repos.PageQuery(pageCondition);
            if (questType.IsNotEmpty())
            {
                query.Where(p => p.QuestType == questType);
            }
            if (workType.IsNotEmpty())
            {
                query.Where(p => p.WorkType == workType);
            }
            if (questUnit.IsNotEmpty())
            {
                query.Where(p => p.QuestUnit == questUnit);
            }

            if (name.IsNotEmpty())
            {
                name = name.Trim();
                query.Where(p => p.Name.Contains(name));
            }
            return repos.Page(query);
        }

        /// <summary>
        /// 题库导入
        /// </summary>
        /// <param name="questions">题目列表</param>
        public List<BoolMessage> Import(List<Question> questions)
        {
            if (questions == null) return new List<BoolMessage>();
            var results = new List<BoolMessage>();

            var repos = new EduRepository<Question>();
            foreach (var entity in questions)
            {
                #region check

                if (entity.Name.IsEmpty())
                {
                    results.Add(new BoolMessage(false, "请指定题目名称"));
                    continue;
                }
                if (entity.A.IsEmpty())
                {
                    results.Add(new BoolMessage(false, "请指定答案A"));
                    continue;
                }
                if (entity.Answer.IsEmpty())
                {
                    results.Add(new BoolMessage(false, "请指定正确答案"));
                }

                if (entity.QuestType.IsEmpty())
                {
                    results.Add(new BoolMessage(false, "请指定题目类型"));
                }
                if (entity.WorkType.IsEmpty())
                {
                    results.Add(new BoolMessage(false, "请指定从业类型"));
                }
                if (entity.QuestUnit.IsEmpty())
                {
                    results.Add(new BoolMessage(false, "请指定题目单元"));
                }

                #endregion

                #region check 下拉
                entity.WorkType = AuthService.DicDetail.GetValueByName(EduDicConst.WorkType, entity.WorkType);
                if (entity.WorkType.IsEmpty())
                {
                    results.Add(new BoolMessage(false, $"题目 {entity.Name} 从业类型输入错误"));
                    continue;
                }

                entity.QuestType = AuthService.DicDetail.GetValueByName(EduDicConst.QuestType, entity.QuestType);
                if (entity.QuestType.IsEmpty())
                {
                    results.Add(new BoolMessage(false, $"题目 {entity.Name} 题目类型输入错误"));
                    continue;
                }

                entity.QuestUnit = AuthService.DicDetail.GetValueByName(EduDicConst.QuestUnit, entity.QuestUnit);
                if (entity.QuestUnit.IsEmpty())
                {
                    results.Add(new BoolMessage(false, $"题目 {entity.Name} 题目单元输入错误"));
                    continue;
                }
                #endregion

                #region exists

                var has = Cache.ValueList().Contains(p => p.Name.Equals(entity.Name));
                if (has)
                {
                    results.Add(new BoolMessage(false, $"题目 {entity.Name} 已经导入"));
                    continue;
                }

                #endregion

                entity.Id = StringHelper.Guid();
                entity.CreateDateTime = DateTime.Now;
                repos.Insert(entity);
                Cache.Set(entity.Id, entity);
                results.Add(BoolMessage.True);
            }
            return results;
        }

        /// <summary>
        /// 顺序获取题目
        /// </summary>
        /// <param name="workType">从业类型</param>
        /// <returns></returns>
        public List<ExcerciseQuestion> GetSeqQuestions(string workType)
        {
            var qList = Cache.ValueList().Where(p => p.WorkType == workType).ToList().OrderByAsc(p => p.CreateDateTime);
            return qList.Select(q => new ExcerciseQuestion(q)).ToList();
        }

        /// <summary>
        /// 随机获取题目
        /// </summary>
        /// <param name="workType">从业类型</param>
        /// <returns></returns>
        public List<ExcerciseQuestion> GetRandomQuestions(string workType)
        {
            var sourceList = Cache.ValueList().Where(p => p.WorkType == workType).ToList();
            var count = sourceList.Count;
            var sz = RandomHelper.GenerateRandomArray(count);
            var qList = sz.Select(index => sourceList[index]).ToList();
            return qList.Select(q => new ExcerciseQuestion(q)).ToList();
        }

        /// <summary>
        /// 获取模拟考试题目
        /// </summary>
        /// <param name="workType">从业类型</param>
        /// <returns></returns>
        public List<ExcerciseQuestion> GetExamQuestions(string workType)
        {
            var sourceList = GetRandomQuestions(workType);
            var newList = new List<ExcerciseQuestion>();
            var sum = 0;
            foreach (var item in sourceList)
            {
                sum += item.Score;
                newList.Add(item);
                if (sum >= 100)
                {
                    break;
                }
            }
            return newList;
        }
    }
}
