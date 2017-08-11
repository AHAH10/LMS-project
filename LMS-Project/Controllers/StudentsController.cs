using LMS_Project.Models.LMS;
using LMS_Project.Repositories;
using LMS_Project.ViewModels;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using System;

namespace LMS_Project.Controllers
{
    public class StudentsController : Controller
    {
        private SchedulesRepository schedRepo = new SchedulesRepository();
        private UsersRepository usersRepo = new UsersRepository();

        [Authorize(Roles = "Student,Admin")]
        public ActionResult Planning(string studentId)
        {
            if (studentId == null || studentId.Length == 0)
            {
                studentId = User.Identity.GetUserId();
            }

            if (usersRepo.GetUserRole(studentId).Name == RoleConstants.Student)
            {
                User user = usersRepo.UserById(studentId);

                List<Schedule> schedules = schedRepo.StudentSchedules(studentId).ToList();

                return View(new UsersScheduleVM
                {
                    UserFullName = user.ToString(),
                    Schedules = schedules,
                    ShowDocumentsLink = user.Id == User.Identity.GetUserId(),
                    ShowSchedulesLink = User.IsInRole(RoleConstants.Admin)
                });
            }
            else
                return RedirectToAction("Index", "Home");
        }

        [Authorize(Roles = "Student")]
        public ActionResult Notifications()
        {
            return View();
        }

        public ActionResult LastLesson(string studentName)
        {
            ViewBag.UsersName = studentName;
            return View();
        }

        public ActionResult DetailedSchedule(string studentId)
        {
            if (studentId == null)
                return RedirectToAction("Index", "Home");

            User student = usersRepo.UserById(studentId);
            if (student == null)
                return RedirectToAction("Index", "Home");

            WeekDays weekDay = schedRepo.GetCurrentDay();
            ViewBag.StudentsName = student.ToString();

            return View(schedRepo.StudentSchedules(studentId)
                                 .Where(s => s.WeekDay == weekDay)
                                 .Select(s => new PartialScheduleVM
                                 {
                                     SubjectName = s.Course.Subject.Name,
                                     TeacherName = s.Course.Teacher.ToString(),
                                     Classroom = s.Classroom.Name + " (" + s.Classroom.Location + ")",
                                     WeekDay = weekDay.ToString() + "s",
                                     BeginningTime = s.BeginningTime,
                                     EndingTime = s.EndingTime
                                 })
                                 .ToList());
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