using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DotNet.Edu.Entity;
using DotNet.Edu.Service;
using DotNet.Utility;

namespace DotNet.Web.Areas.Students.Controllers
{
    public class SimulatorExamController : StudentWebController
    {
        public ActionResult Index()
        {
            CurrentStudent.ExamStartDateTime = DateTime.Now;
            CurrentStudent.IsExamCommit = false;
            CurrentStudent.ExamQuestions = EduService.Question.GetExamQuestions(CurrentStudent.Student.WorkCategoryId);
            return View();
        }

        public ActionResult _Question(int index = 0)
        {
            int count = CurrentStudent.ExamQuestions.Count;
            ViewBag.studentSession = CurrentStudent;
            ViewBag.index = index;
            ViewBag.examQuestions = CurrentStudent.ExamQuestions;
            ExcerciseQuestion entity = null;
            if (index >= 0 && index < count)
            {
                entity = CurrentStudent.ExamQuestions[index];
            }
            return View("_Question", entity);
        }

        public ActionResult CheckQuestion(string questionId, bool userResult, string userAnswer)
        {
            var q = CurrentStudent.ExamQuestions.FirstOrDefault(p => p.Id == questionId);
            if (q != null)
            {
                q.UserSelected = true;
                q.UserResult = userResult;
                q.UserAnswer = userAnswer;
            }
            if (!userResult)
            {
                EduService.QuestionFavorite.CreateError(CurrentStudent.StudentId, questionId);
            }
            return Json(BoolMessage.True);
        }

        public ActionResult Commit()
        {
            var totalScore = 0;
            var totalCount = 0;
            var correctCount = 0;
            var errorCount = 0;
            var userScore = 0;
            var examList = CurrentStudent.ExamQuestions;
            foreach (var item in examList)
            {
                totalCount++;
                totalScore += item.Score;
                if (item.UserResult)
                {
                    userScore+= item.Score;
                    correctCount++;
                }
                else
                {
                    errorCount++;
                }
            }
            CurrentStudent.IsExamCommit = true;
            return Json(new { success = true, totalScore, totalCount, correctCount, errorCount, userScore });
        }
    }
}