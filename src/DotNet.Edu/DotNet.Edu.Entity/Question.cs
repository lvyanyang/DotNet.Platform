// ===============================================================================
// DotNet.Platform 开发框架 2016 版权所有
// ===============================================================================

using System;
using DotNet.Auth.Service;
using DotNet.Edu.Utility;
using DotNet.Entity;

namespace DotNet.Edu.Entity
{
    /// <summary>
    /// 题库数据
    /// </summary>
    [Table("题库数据")]    
    public class Question
    {
        /// <summary>
        /// 主键
        /// </summary>
        [PrimaryKey]
        [Column("主键",false)]
        public string Id { get; set; }

		/// <summary>
        /// 名称
        /// </summary>
		[Column("题目名称")]
        public string Name { get; set; }

        /// <summary>
        /// 题目类型:1判断题2单选题3多选题
        /// </summary>
        [Column("题目类型")]
        public string QuestType { get; set; }

        /// <summary>
        /// 题目类型名称
        /// </summary>
        [Ignore]
        public string QuestTypeName => AuthService.DicDetail.GetNameByValue(EduDicConst.QuestType, QuestType);

        /// <summary>
        /// 从业类型
        /// </summary>
        [Column("从业类型")]
        public string WorkType { get; set; }

        /// <summary>
        /// 从业类型名称
        /// </summary>
        [Ignore]
        public string WorkTypeName => AuthService.DicDetail.GetNameByValue(EduDicConst.WorkType, WorkType);

        /// <summary>
        /// 题目单元
        /// </summary>
        [Column("题目单元")]
        public string QuestUnit { get; set; }

        /// <summary>
        /// 题目单元名称
        /// </summary>
        [Ignore]
        public string QuestUnitName => AuthService.DicDetail.GetNameByValue(EduDicConst.QuestUnit, QuestUnit);

        /// <summary>
        /// 答案A
        /// </summary>
        [Column("答案A")]
        public string A { get; set; }

		/// <summary>
        /// 答案B
        /// </summary>
		[Column("答案B")]
        public string B { get; set; }

		/// <summary>
        /// 答案C
        /// </summary>
		[Column("答案C")]
        public string C { get; set; }

		/// <summary>
        /// 答案D
        /// </summary>
		[Column("答案D")]
        public string D { get; set; }

		/// <summary>
        /// 正确答案
        /// </summary>
		[Column("正确答案")]
        public string Answer { get; set; }

        /// <summary>
        /// 分数
        /// </summary>
        [Column("分数")]
        public int Score { get; set; } = 1;

        /// <summary>
        /// 创建时间
        /// </summary>
		[Column("创建时间")]
        public DateTime CreateDateTime { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [Column("备注")]
        public string Note { get; set; }

        /// <summary>
        /// 复制对象
        /// </summary>
        public Question Clone()
        {
            return (Question)MemberwiseClone();
        }
    }

    /// <summary>
    /// 练习题库
    /// </summary>
    public class ExcerciseQuestion : Question
    {
        public ExcerciseQuestion(Question q)
        {
            Id = q.Id;
            Name = q.Name;
            QuestType = q.QuestType;
            WorkType = q.WorkType;
            QuestUnit = q.QuestUnit;
            A = q.A;
            B = q.B;
            C = q.C;
            D = q.D;
            Answer = q.Answer;
            Score = q.Score;
            CreateDateTime = q.CreateDateTime;
            Note = q.Note;
        }

        /// <summary>
        /// 是否作答
        /// </summary>
        [Ignore]
        public bool? UserSelected { get; set; }

        /// <summary>
        /// 用户答案结果
        /// </summary>
        [Ignore]
        public bool UserResult { get; set; }

        /// <summary>
        /// 用户答案
        /// </summary>
        [Ignore]
        public string UserAnswer { get; set; }
    }
}