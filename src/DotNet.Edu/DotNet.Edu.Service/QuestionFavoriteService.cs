// ===============================================================================
// DotNet.Platform 开发框架 2016 版权所有
// ===============================================================================

using System;
using System.Collections.Generic;
using System.Linq;
using DotNet.Edu.Entity;
using DotNet.Collections;
using DotNet.Data.Extensions;
using DotNet.Extensions;
using DotNet.Helper;
using DotNet.Utility;

namespace DotNet.Edu.Service
{
    /// <summary>
    /// 题库收藏服务
    /// </summary>
    public class QuestionFavoriteService
    {
        /// <summary>
        /// 添加对象
        /// </summary>
        public BoolMessage Create(string studentId, string questionId, int favoriteType)
        {
            var repos = new EduRepository<QuestionFavorite>();
            var entity = new QuestionFavorite();
            entity.Id = StringHelper.Guid();
            entity.CreateDateTime = DateTime.Now;
            entity.StudentId = studentId;
            entity.QuestionId = questionId;
            entity.FavoriteType = favoriteType;
            repos.Insert(entity);
            return BoolMessage.True;
        }

        /// <summary>
        /// 添加对象
        /// </summary>
        public BoolMessage CreateError(string studentId, string questionId)
        {
            var repos = new EduRepository<QuestionFavorite>();
            if (repos.Exists(
                    p => p.StudentId == studentId && p.QuestionId == questionId && p.FavoriteType == 1))
            {
                return BoolMessage.True;
            }
            var entity = new QuestionFavorite();
            entity.Id = StringHelper.Guid();
            entity.CreateDateTime = DateTime.Now;
            entity.StudentId = studentId;
            entity.QuestionId = questionId;
            entity.FavoriteType = 1;
            repos.Insert(entity);
            return BoolMessage.True;
        }

        public bool IsFavorite(string studentId, string questionId, int favoriteType)
        {
            var repos = new EduRepository<QuestionFavorite>();
            if (repos.Exists(
                    p => p.StudentId == studentId && p.QuestionId == questionId && p.FavoriteType == favoriteType))
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// 清空对象
        /// </summary>
        public BoolMessage Clear(string studentId, int favoriteType)
        {
            var repos = new EduRepository<QuestionFavorite>();
            repos.Delete(p => p.StudentId == studentId && p.FavoriteType == favoriteType);
            return BoolMessage.True;
        }

        /// <summary>
        /// 删除对象
        /// </summary>
        public BoolMessage Delete(string studentId, string questionId, int favoriteType)
        {
            var repos = new EduRepository<QuestionFavorite>();
            repos.Delete(p => p.StudentId == studentId && p.QuestionId == questionId && p.FavoriteType == favoriteType);
            return BoolMessage.True;
        }

        /// <summary>
        /// 删除对象
        /// </summary>
        public BoolMessage Delete(string[] questionId)
        {
            var repos = new EduRepository<QuestionFavorite>();
            repos.Delete(p => p.QuestionId.In(questionId));
            return BoolMessage.True;
        }

        /// <summary>
        /// 获取对象
        /// </summary>
        /// <param name="id">主键</param>
        public QuestionFavorite Get(string id)
        {
            return new EduRepository<QuestionFavorite>().Get(id);
        }

        /// <summary>
        /// 获取收藏对象集合
        /// </summary>
        /// <param name="studentId">学员主键</param>
        /// <param name="favoriteType">类型:1 错题收藏 2 题库收藏</param>
        /// <returns></returns>
        public List<ExcerciseQuestion> GetFavoriteQuestions(string studentId, int favoriteType)
        {
            var repos = new EduRepository<QuestionFavorite>();
            var query = repos.SQL
                .Where(p => p.StudentId == studentId && p.FavoriteType == favoriteType)
                .OrderByDesc(p => p.CreateDateTime)
                .Select(p => p.QuestionId);
            var questionIds = repos.Query(query).Select(p => p.QuestionId).ToArray();
            var qList = new List<ExcerciseQuestion>();
            foreach (var id in questionIds)
            {
                var qEntity = EduService.Question.Get(id);
                if (qEntity != null)
                {
                    qList.Add(new ExcerciseQuestion(qEntity));
                }
            }
            return qList;
        }
    }
}
