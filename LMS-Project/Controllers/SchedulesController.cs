using LMS_Project.Models;
using LMS_Project.Models.LMS;
using LMS_Project.Repositories;
using LMS_Project.ViewModels;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;

namespace LMS_Project.Controllers
{
    [Authorize]
    public class SchedulesController : Controller
    {
        private SchedulesRepository repository = new SchedulesRepository();

        // GET: Schedules
        [Authorize(Roles = "Admin")]
        public ActionResult Index()
        {
            List<Schedule> schedules = null;

            if (User.IsInRole("Teacher"))
            {
                schedules = repository.TeacherSchedules(User.Identity.GetUserId()).ToList();
            }
            else if (User.IsInRole("Student"))
            {
                schedules = repository.StudentSchedules(User.Identity.GetUserId()).ToList();
            }
            else if (User.IsInRole("Admin"))
            {
                schedules = repository.Schedules().ToList();
            }

            return View(schedules);
        }

        // GET: Schedules/Details/5
        [Authorize]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index");
            }
            Schedule schedule = repository.Schedule(id);
            if (schedule == null)
            {
                return RedirectToAction("Index");
            }
            return View(schedule);
        }

        // GET: Schedules/Create
        [Authorize(Roles = "Admin")]
        public ActionResult Create()
        {
            ViewBag.Classrooms = Classrooms();
            ViewBag.Courses = Courses();

            return View(new CreateEditScheduleVM
            {
                BeginningTime = new DateTime() + new TimeSpan(8, 0, 0),
                EndingTime = new DateTime() + new TimeSpan(19, 0, 0),
                Students = Students()
            });
        }

        // POST: Schedules/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult Create([Bind(Include = "ID,WeekDay,BeginningTime,EndingTime,CourseID,ClassroomID")] CreateEditScheduleVM viewModel, StudentsInLesson studentsInLesson)
        {
            if (ModelState.IsValid)
            {
                if (TestAvailability(viewModel, studentsInLesson))
                {
                    Schedule schedule = new Schedule
                    {
                        WeekDay = viewModel.WeekDay,
                        BeginningTime = viewModel.BeginningTime.ToString(ScheduleConstants.TIME_FORMAT),
                        EndingTime = viewModel.EndingTime.ToString(ScheduleConstants.TIME_FORMAT),
                        ClassroomID = viewModel.ClassroomID,
                        CourseID = viewModel.CourseID
                    };

                    repository.Add(schedule, studentsInLesson.InLesson.ToList());

                    return RedirectToAction("Index");
                }
                else
                {
                    ViewBag.Classrooms = Classrooms();
                    ViewBag.Courses = Courses();
                    return View(new CreateEditScheduleVM { Students = Students(studentsInLesson.InLesson.ToList()) });
                }
            }

            ViewBag.Classrooms = Classrooms();
            ViewBag.Courses = Courses();
            return View(new CreateEditScheduleVM
            {
                Students = Students(studentsInLesson.InLesson.ToList())
            });
        }

        // GET: Schedules/Edit/5
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index");
            }

            Schedule schedule = repository.Schedule(id);
            if (schedule == null)
            {
                return RedirectToAction("Index");
            }

            ViewBag.Classrooms = Classrooms();
            ViewBag.Courses = Courses();

            return View(new CreateEditScheduleVM
            {
                ID = schedule.ID,
                BeginningTime = DateTime.ParseExact(schedule.BeginningTime, ScheduleConstants.TIME_FORMAT, CultureInfo.InvariantCulture),
                ClassroomID = schedule.ClassroomID,
                CourseID = schedule.CourseID,
                EndingTime = DateTime.ParseExact(schedule.EndingTime, ScheduleConstants.TIME_FORMAT, CultureInfo.InvariantCulture),
                WeekDay = schedule.WeekDay,
                Students = Students(schedule.Students.Select(s => s.Id).ToList())
            });
        }

        // POST: Schedules/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult Edit([Bind(Include = "ID,WeekDay,BeginningTime,EndingTime,CourseId,ClassroomId")] CreateEditScheduleVM viewModel, StudentsInLesson studentsInLesson)
        {
            Schedule schedule = repository.Schedule(viewModel.ID);

            if (ModelState.IsValid)
            {
                if (TestAvailability(viewModel, studentsInLesson))
                {
                    schedule.BeginningTime = viewModel.BeginningTime.ToString(ScheduleConstants.TIME_FORMAT);
                    schedule.ClassroomID = viewModel.ClassroomID;
                    schedule.CourseID = viewModel.CourseID;
                    schedule.EndingTime = viewModel.EndingTime.ToString(ScheduleConstants.TIME_FORMAT);
                    schedule.WeekDay = schedule.WeekDay;

                    repository.Edit(schedule, studentsInLesson.InLesson.ToList());
                    return RedirectToAction("Index");
                }
                else
                {
                    ViewBag.Classrooms = Classrooms();
                    ViewBag.Courses = Courses();

                    viewModel.Students = Students(schedule.Students.Select(s => s.Id).ToList());

                    return View(viewModel);
                }
            }

            ViewBag.Classrooms = Classrooms();
            ViewBag.Courses = Courses();

            viewModel.Students = Students(schedule.Students.Select(s => s.Id).ToList());

            return View(viewModel);
        }

        // GET: Schedules/Delete/5
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index");
            }
            Schedule schedule = repository.Schedule(id);
            if (schedule == null)
            {
                return RedirectToAction("Index");
            }
            return View(schedule);
        }

        // POST: Schedules/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult DeleteConfirmed(int id)
        {
            repository.Delete(id);
            return RedirectToAction("Index");
        }

        private List<SelectListItem> Courses()
        {
            return new CoursesRepository().Courses().Select(c => new SelectListItem
            {
                Text = c.Subject.Name + " (" + c.Teacher.ToString() + ")",
                Value = c.ID.ToString()
            }).ToList();
        }

        private List<SelectListItem> Classrooms()
        {
            return new ClassroomsRepository().Classrooms().Select(c => new SelectListItem
            {
                Text = c.Name + (c.Remarks == null || c.Remarks.Length == 0 ? string.Empty : " - " + c.Remarks),
                Value = c.ID.ToString()
            }).ToList();
        }

        private StudentsInLesson Students(List<string> studentsInLesson = null)
        {
            StudentsInLesson students = new StudentsInLesson();

            students.Initilize(new UsersRepository().Students().OrderBy(s => s.LastName).ThenBy(s => s.FirstName).ToList());

            if (studentsInLesson != null)
                students.InLesson = studentsInLesson.ToArray();

            return students;
        }

        private bool TestAvailability(CreateEditScheduleVM viewModel, StudentsInLesson students)
        {
            // Let's check if all data are available at the given day and times, starting with the teacher
            Course course = new CoursesRepository().Course(viewModel.CourseID);

            Schedule availability = repository.TeacherAvailability(course.TeacherID,
                                                                   viewModel.WeekDay,
                                                                   viewModel.BeginningTime.ToString(ScheduleConstants.TIME_FORMAT),
                                                                   viewModel.EndingTime.ToString(ScheduleConstants.TIME_FORMAT));

            if (availability != null)
            {
                ModelState.AddModelError("", course.Teacher.ToString() + " is not available at the given day and times:");
                ModelState.AddModelError("", availability.ToString());
                return false;
            }

            // Then, we can check if the room is available
            Classroom classroom = new ClassroomsRepository().Classroom(viewModel.ClassroomID);

            availability = repository.ClassroomAvailability(viewModel.ClassroomID,
                                                            viewModel.WeekDay,
                                                            viewModel.BeginningTime.ToString(ScheduleConstants.TIME_FORMAT),
                                                            viewModel.EndingTime.ToString(ScheduleConstants.TIME_FORMAT));

            if (availability != null)
            {
                ModelState.AddModelError("", "The classroom " + classroom.Name + " is not available at the given day and times:");
                ModelState.AddModelError("", availability.ToString());
                return false;
            }

            // Finally, let's check if all the students are available for the lesson
            foreach (string studentId in students.InLesson)
            {
                User student = new UsersRepository().User(studentId);

                availability = repository.StudentAvailability(studentId,
                                                              viewModel.WeekDay,
                                                              viewModel.BeginningTime.ToString(ScheduleConstants.TIME_FORMAT),
                                                              viewModel.EndingTime.ToString(ScheduleConstants.TIME_FORMAT));

                if (availability != null)
                {
                    ModelState.AddModelError("", student.ToString() + " is not available at the given day and times:");
                    ModelState.AddModelError("", availability.ToString());
                    return false;
                }
            }

            return true;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                repository.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
