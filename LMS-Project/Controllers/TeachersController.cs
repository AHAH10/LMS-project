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

            User user = usersRepo.User(teacherId);

            List<Schedule> schedules = schedRepo.TeacherSchedules(teacherId).ToList();

            return View(new UsersScheduleVM { UsersFullName = user.ToString(), Schedules = schedules });
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

            return View(schedule);
        }
    }
}