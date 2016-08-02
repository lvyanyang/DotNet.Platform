// ===============================================================================
// DotNet.Platform 开发框架 2016 版权所有
// ===============================================================================

using System.Collections.Generic;
using System.Linq;
using DotNet.Edu.Entity;
using DotNet.Collections;
using DotNet.Extensions;
using DotNet.Utility;

namespace DotNet.Edu.Service
{
    /// <summary>
    /// 学员课件学时服务
    /// </summary>
    public class StudentCoursewarePeriodService
    {
        public BoolMessage Save(string studentId, string coursewareId, int period)
        {
            var repos = new EduRepository<StudentCoursewarePeriod>();
            var entity = repos.Get(p => p.StudentId == studentId && p.CoursewareId == coursewareId);
            if (entity == null)
            {
                //insert
                entity = new StudentCoursewarePeriod();
                entity.StudentId = studentId;
                entity.CoursewareId = coursewareId;
                entity.Period = period;
                repos.Insert(entity);
            }
            else
            {
                //update
                string sql = "UPDATE StudentCoursewarePeriod SET Period=Period+@Period WHERE StudentId=@StudentId";
                repos.Database.Execute(sql, new object[] { period, studentId });
            }
            return BoolMessage.True;
        }

        public int GetStudentPeriod(string studentId)
        {
            var repos = new EduRepository<StudentCoursewarePeriod>();
            return repos.Sum(p => p.Period, p => p.StudentId == studentId);
        }

        public List<StudentCoursewarePeriodView> GetList(string studentId,string workType)
        {
            string sql = @"
SELECT c.Id CoursewareId,c.Name CoursewareName,c.Period CoursewarePeriod,WorkType,CourseType,ISNULL(v.Period,0) LearnPeriod,RowIndex FROM (
SELECT StudentId, CoursewareId, Period FROM StudentCoursewarePeriod WHERE StudentId=@StudentId
) v RIGHT JOIN (SELECT Id,Name,Period,WorkType,CourseType,RowIndex FROM Courseware WHERE WorkType=@WorkType) c ON v.CoursewareId = c.Id
ORDER BY RowIndex asc
";
            var repos = new EduRepository<StudentCoursewarePeriod>();
            return repos.Database.Query<StudentCoursewarePeriodView>(sql,new object[] { studentId, workType }).ToList();
        }
    }
}
