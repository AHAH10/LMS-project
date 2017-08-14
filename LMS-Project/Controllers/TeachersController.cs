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

            if (usersRepo.GetUserRole(teacherId).Name == RoleConstants.Teacher)
            {
                User user = usersRepo.UserById(teacherId);

                List<Schedule> schedules = schedRepo.TeacherSchedules(teacherId).ToList();

                return View(new UsersScheduleVM
                {
                    UserFullName = user.ToString(),
                    Schedules = schedules,
                    ShowCoursesLink = user.Id == User.Identity.GetUserId(),
                    ShowSchedulesLink = User.IsInRole(RoleConstants.Admin)
                });
            }
            else
                return RedirectToAction("Index", "Home");
        }

        public ActionResult NextLecture()
        {
            Schedule schedule = schedRepo.NextLecture(User.Identity.GetUserId());

            return View(new DetailedScheduleVM
            {
                Schedule = schedule,
                Documents = AvailableDocuments(schedule)
            });
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

            return View(new DetailedScheduleVM
            {
                Schedule = schedule,
                // Get the list of available documents for the course
                Documents = AvailableDocuments(schedule)
            });
        }

        [Authorize(Roles = "Teacher")]
        public ActionResult UngradedAssignments()
        {
            // Gets the ungraded documents from the teacher's courses
            return View(new DocumentsRepository().Documents()
                                                 .Where(d => d.Course.TeacherID == User.Identity.GetUserId() &&
                                                             d.VisibleFor.Name == RoleConstants.Teacher &&
                                                             d.UploaderID != d.Course.TeacherID &&
                                                             d.Grade == null) //Liam You forgot to check if d.Grade is null
                                                 .ToList());
        }

        private List<PartialDocumentVM> AvailableDocuments(Schedule schedule)
        {
            if (schedule == null)
                return new List<PartialDocumentVM>();
            else
            {
                RolesRepository rolesRepository = new RolesRepository();

                return schedule.Course
                               .Documents
                               .OrderBy(d => d.UploadingDate)
                               .Select(d => new PartialDocumentVM
                               {
                                   ID = d.ID,
                                   DocumentName = d.DocumentName,
                                   UploadingDate = d.UploadingDate,
                                   Uploader = d.Uploader.ToString(),
                                   Grade = d.Grade,
                                   CanSetAGrade = (d.UploaderID != d.Course.TeacherID && d.Grade == null),
                                   CanBeDeleted = (d.RoleID == rolesRepository.RoleByName(RoleConstants.Student).Id)
                               })
                               .ToList();
            }
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