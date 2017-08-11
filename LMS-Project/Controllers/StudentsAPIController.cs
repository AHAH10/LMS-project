using LMS_Project.Models.LMS;
using LMS_Project.Repositories;
using LMS_Project.ViewModels;
using System;
using System.Collections.Generic;
using System.Web.Http;

namespace LMS_Project.Controllers
{
    public class StudentsAPIController : ApiController
    {
        private SchedulesRepository schedRepo = new SchedulesRepository();
        private UsersRepository usersRepo = new UsersRepository();

        public List<LastLessonVM> GetLastLesson(string studentName)
        {
            if (studentName == null)
                return new List<LastLessonVM>();

            IEnumerable<User> students = usersRepo.UsersByUsersname(studentName);

            List<LastLessonVM> viewModel = new List<LastLessonVM>();

            WeekDays weekDay = schedRepo.GetCurrentDay();

            foreach (User student in students)
            {
                Schedule lastLesson = schedRepo.LastLesson(student.Id, weekDay);

                if (lastLesson != null)
                    viewModel.Add(new LastLessonVM
                    {
                        Student = new PartialUserVM
                        {
                            Id = student.Id,
                            FirstName = student.FirstName,
                            LastName = student.LastName
                        },
                        SubjectName = lastLesson.Course.Subject.Name,
                        Classroom = lastLesson.Classroom,
                        EndingTime = lastLesson.EndingTime
                    });
            }

            return viewModel;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                schedRepo.Dispose();
                usersRepo.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}