using LMS_Project.Models.LMS;
using LMS_Project.Repositories;
using LMS_Project.ViewModels;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;

namespace LMS_Project.Controllers
{
    [Authorize(Roles = "Student,Admin")]
    public class StudentsController : Controller
    {
        private SchedulesRepository schedRepo = new SchedulesRepository();
        private UsersRepository usersRepo = new UsersRepository();

        public ActionResult Planning(string studentId)
        {
            if (studentId == null || studentId.Length == 0)
            {
                studentId = User.Identity.GetUserId();
            }

            User user = usersRepo.User(studentId);

            List<Schedule> schedules = schedRepo.StudentSchedules(studentId).ToList();

            return View(new UsersScheduleVM { UsersFullName = user.ToString(), Schedules = schedules });
        }
    }
}