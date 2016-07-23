// ===============================================================================
// DotNet.Platform 开发框架 2016 版权所有
// ===============================================================================

using System;
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
    /// 考试用户服务
    /// </summary>
    public class ExamUserService
    {
        /// <summary>
        /// 构造服务
        /// </summary>
        internal ExamUserService()
        {
        }

        /// <summary>
        /// 添加对象
        /// </summary>
        private BoolMessage Create(Student student)
        {
            var idNumber = student.IDCardNo;
            if (idNumber.IsEmpty())
            {
                return new BoolMessage(false, "请指定学员身份证号码");
            }
            var entity = new ExamUser();
            entity.LoginName = idNumber;
            entity.Password = StringHelper.EncryptString(idNumber.Substring(idNumber.Length - 6));
            entity.UserName = student.Name;
            entity.UserNameSpell = student.Name.Spell();
            entity.IdNumber = idNumber;
            entity.IsAdmin = false;
            entity.IsPrep = false;
            entity.ExamType = null;
            var repos = new ExamRepository<ExamUser>();
            repos.Insert(entity);
            return BoolMessage.True;
        }

        /// <summary>
        /// 考试预约
        /// </summary>
        /// <param name="studentIds"></param>
        /// <returns></returns>
        public List<BoolMessage> Prep(string[] studentIds)
        {
            if (studentIds == null || studentIds.Length == 0) return new List<BoolMessage>();
            var results = new List<BoolMessage>();

            var examRepos = new ExamRepository<ExamUser>();
            var stuRepos = new EduRepository<Student>();
            foreach (var id in studentIds)
            {
                var student = stuRepos.Get(p => p.Id == id, p => p.Id, p => p.Name, p => p.IDCardNo,p => p.Status);
                if (student == null)
                {
                    results.Add(new BoolMessage(false, "学员主键无效"));
                    continue;
                }
                if (student.Status != 3)
                {
                    results.Add(new BoolMessage(false, $"学员 {student.Name} {student.StatusName} 的状态不是准备考试,无法预约考试"));
                    continue;
                }
                if (!examRepos.Exists(p => p.IdNumber == student.IDCardNo))
                {
                    var result = Create(student);
                    if (result.Failure)
                    {
                        results.Add(result);
                        continue;
                    }
                }

                var examUser = new ExamUser();
                examUser.IdNumber = student.IDCardNo;
                examUser.IsPrep = true;
                examUser.ExamType = "初考";
                examRepos.Update(examUser, p => p.IdNumber == student.IDCardNo, new[] { nameof(examUser.IsPrep), nameof(examUser.ExamType) });

                student.Status = 4;
                stuRepos.Update(student, new[] { nameof(student.Status) });
                EduService.StudentLog.Create(student.Id, student.Name, "学员预约考试");
                results.Add(BoolMessage.True);
            }
            return results;
        }
    }
}
