using System;
using System.Collections.Generic;
using System.Web.Mvc;
using DotNet.Edu.Entity;
using DotNet.Edu.Service;
using DotNet.Extensions;

namespace DotNet.StudentWeb.Controllers
{
    public class FavoriteQuestionController : StudentWebController
    {
        public ActionResult Index()
        {
            CurrentStudent.FavoriteQuestions = EduService.QuestionFavorite.GetFavoriteQuestions(CurrentStudent.StudentId, 2);
            return View();
        }

        public ActionResult _Question(int index = 0)
        {
            int count = CurrentStudent.FavoriteQuestions.Count;
            ViewBag.studentSession = CurrentStudent;
            ViewBag.index = index;
            ViewBag.favoriteQuestions = CurrentStudent.FavoriteQuestions;
            ExcerciseQuestion entity = null;
            if (index >= 0 && index < count)
            {
                entity = CurrentStudent.FavoriteQuestions[index];
            }
            return View("_Question", entity);
        }

        public ActionResult DeleteQuestion(string questionId)
        {
            var result = EduService.QuestionFavorite.Delete(CurrentStudent.StudentId, questionId, 2);
            CurrentStudent.FavoriteQuestions.Delete(p => p.Id == questionId);
            return Json(result);
        }

        public ActionResult ClearQuestion(string questionId)
        {
            var result = EduService.QuestionFavorite.Clear(CurrentStudent.StudentId, 2);
            CurrentStudent.FavoriteQuestions = new List<ExcerciseQuestion>();
            return Json(result);
        }
    }
}