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
            return View(new NotificationRepository().UnreadNotifications(User.Identity.GetUserId()).ToList());
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