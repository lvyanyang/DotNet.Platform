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
    public class SimulatorExerciseController : StudentWebController
    {
        public ActionResult Index()
        {
            SeqQuest();
            return View();
        }

        public ActionResult Seq()
        {
            SeqQuest();
            return Json(BoolMessage.True);
        }

        public ActionResult Random()
        {
            RandomQuest();
            return Json(BoolMessage.True);
        }

        public ActionResult _Question(int index = 0)
        {
            int count = CurrentStudent.ExerciseQuestions.Count;
            ViewBag.studentSession = CurrentStudent;
            ViewBag.index = index;
            ViewBag.exerciseQuestions = CurrentStudent.ExerciseQuestions;
            ExcerciseQuestion entity = null;
            if (index >= 0 && index < count)
            {
                entity = CurrentStudent.ExerciseQuestions[index];
            }
            return View("_Question", entity);
        }

        public ActionResult FavoriteQuestion(string questionId, int favoriteType, bool isFavorite)
        {
            var result = isFavorite ? 
                EduService.QuestionFavorite.Create(CurrentStudent.StudentId, questionId, favoriteType) : 
                EduService.QuestionFavorite.Delete(CurrentStudent.StudentId, questionId, favoriteType);
            return Json(result);
        }

        public ActionResult CheckQuestion(string questionId,bool userResult,string userAnswer)
        {
            var q = CurrentStudent.ExerciseQuestions.FirstOrDefault(p => p.Id == questionId);
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


        private void SeqQuest()
        {
            CurrentStudent.ExerciseType = 1;
            CurrentStudent.ExerciseQuestions = EduService.Question.GetSeqQuestions(CurrentStudent.Student.WorkCategoryId);
        }

        private void RandomQuest()
        {
            CurrentStudent.ExerciseType = 2;
            CurrentStudent.ExerciseQuestions = EduService.Question.GetRandomQuestions(CurrentStudent.Student.WorkCategoryId);
        }
    }
}