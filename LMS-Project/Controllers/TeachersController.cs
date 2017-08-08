using LMS_Project.Models.LMS;
using LMS_Project.Repositories;
using LMS_Project.ViewModels;
using Microsoft.AspNet.Identity;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace LMS_Project.Controllers
{
    [Authorize(Roles = "Teacher,Admin")]
    public class TeachersController : Controller
    {
        private SchedulesRepository schedRepo = new SchedulesRepository();
        private UsersRepository usersRepo = new UsersRepository();

        public ActionResult Planning(string teacherId)
        {
            if (teacherId == null || teacherId.Length == 0)
            {
                teacherId = User.Identity.GetUserId();
            }

            if (usersRepo.GetUserRole(teacherId).Name == "Teacher")
            {
                User user = usersRepo.User(teacherId);

                List<Schedule> schedules = schedRepo.TeacherSchedules(teacherId).ToList();

                return View(new UsersScheduleVM
                {
                    UserFullName = user.ToString(),
                    Schedules = schedules,
                    ShowCoursesLink = user.Id == User.Identity.GetUserId(),
                    ShowSchedulesLink = User.IsInRole("Admin")
                });
            }
            else
                return RedirectToAction("Index", "Home");
        }

        public ActionResult DetailedSchedule(int? scheduleId)
        {
            if (scheduleId == null)
            {
                return RedirectToAction("Planning");
            }

            Schedule schedule = schedRepo.Schedule(scheduleId);

            if (schedule == null)
            {
                return RedirectToAction("Planning");
            }

            // Get the list of available documents for the course
            ViewBag.Documents = schedule.Course.Documents.Where(d => d.VisibleFor.Name == "Student" || d.UploaderID == User.Identity.GetUserId());

            return View(schedule);
        }

        [Authorize(Roles = "Teacher")]
        public ActionResult UngradedAssignments()
        {
            return View(new List<Document>());
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