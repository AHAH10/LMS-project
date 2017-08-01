using LMS_Project.Models;
using LMS_Project.Models.LMS;
using LMS_Project.Repositories;
using LMS_Project.ViewModels;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
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
            ViewBag.Classrooms = new ClassroomsRepository().Classrooms().ToList();
            ViewBag.Courses = Courses();

            return View(new CreateScheduleVM { Students = Students() });
        }

        // POST: Schedules/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult Create([Bind(Include = "ID,WeekDay,BeginningTime,EndingTime,CourseID,ClassroomID")] CreateScheduleVM viewModel, StudentsInLesson studentsInLesson)
        {
            if (ModelState.IsValid)
            {
                // Let's check if all data are available at the given day and times, starting with the teacher
                Course course = new CoursesRepository().Course(viewModel.CourseID);

                Schedule availability = repository.TeacherAvailability(course.TeacherID,
                                                                       viewModel.WeekDay,
                                                                       viewModel.BeginningTime.ToString("HH:mm"),
                                                                       viewModel.EndingTime.ToString("HH:mm"));

                if (availability != null)
                {
                    ModelState.AddModelError("", course.Teacher.ToString() + " is not available at the given day and times:");
                    ModelState.AddModelError("", availability.ToString());

                    ViewBag.Classrooms = new ClassroomsRepository().Classrooms().ToList();
                    ViewBag.Courses = Courses();
                    return View(new CreateScheduleVM { Students = Students() });
                }

                // Then, we can check if the room is available
                Classroom classroom = new ClassroomsRepository().Classroom(viewModel.ClassroomID);

                availability = repository.ClassroomAvailability(viewModel.ClassroomID,
                                                                viewModel.WeekDay,
                                                                viewModel.BeginningTime.ToString("HH:mm"),
                                                                viewModel.EndingTime.ToString("HH:mm"));

                if (availability != null)
                {
                    ModelState.AddModelError("", "The classroom " + classroom.Name + " is not available at the given day and times:");
                    ModelState.AddModelError("", availability.ToString());

                    ViewBag.Classrooms = new ClassroomsRepository().Classrooms().ToList();
                    ViewBag.Courses = Courses();
                    return View(new CreateScheduleVM { Students = Students() });
                }

                // Finally, let's check if all the students are available for the lesson
                foreach (string studentId in studentsInLesson.InLesson)
                {
                    User student = new UsersRepository().User(studentId);

                    availability = repository.StudentAvailability(studentId,
                                                                  viewModel.WeekDay,
                                                                  viewModel.BeginningTime.ToString("HH:mm"),
                                                                  viewModel.EndingTime.ToString("HH:mm"));

                    if (availability != null)
                    {
                        ModelState.AddModelError("", student.ToString() + " is not available at the given day and times:");
                        ModelState.AddModelError("", availability.ToString());

                        ViewBag.Classrooms = new ClassroomsRepository().Classrooms().ToList();
                        ViewBag.Courses = Courses();
                        return View(new CreateScheduleVM { Students = Students() });
                    }
                }

                Schedule schedule = new Schedule
                {
                    WeekDay = viewModel.WeekDay,
                    BeginningTime = viewModel.BeginningTime.ToString("HH:mm"),
                    EndingTime = viewModel.EndingTime.ToString("HH:mm"),
                    ClassroomID = viewModel.ClassroomID,
                    CourseID = viewModel.CourseID
                };

                repository.Add(schedule, studentsInLesson.InLesson.ToList());

                return RedirectToAction("Index");
            }

            ViewBag.Classrooms = new ClassroomsRepository().Classrooms().ToList();
            ViewBag.Courses = Courses();
            return View(new CreateScheduleVM { Students = Students() });
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

            ViewBag.Classrooms = new ClassroomsRepository().Classrooms().ToList();
            ViewBag.Courses = Courses();
            return View(schedule);
        }

        // POST: Schedules/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult Edit([Bind(Include = "ID,WeekDay,BeginningTime,EndingTime,CourseId,ClassroomId")] Schedule schedule)
        {
            if (ModelState.IsValid)
            {
                // Let's check if all data are available at the given day and times, starting with the teacher
                Course course = new CoursesRepository().Course(schedule.CourseID);

                Schedule availability = repository.TeacherAvailability(course.TeacherID,
                                                                       schedule.WeekDay,
                                                                       schedule.BeginningTime,
                                                                       schedule.EndingTime);

                if (availability != null)
                {
                    ModelState.AddModelError("", course.Teacher.ToString() + " is not available at the given day and times:");
                    ModelState.AddModelError("", availability.ToString());

                    ViewBag.Classrooms = new ClassroomsRepository().Classrooms().ToList();
                    ViewBag.Courses = Courses();
                    return View(schedule);
                }

                // Then, we can check if the room is available
                Classroom classroom = new ClassroomsRepository().Classroom(schedule.ClassroomID);

                availability = repository.ClassroomAvailability(schedule.ClassroomID,
                                                                schedule.WeekDay,
                                                                schedule.BeginningTime,
                                                                schedule.EndingTime);

                if (availability != null)
                {
                    ModelState.AddModelError("", "The classroom " + classroom.Name + " is not available at the given day and times:");
                    ModelState.AddModelError("", availability.ToString());

                    ViewBag.Classrooms = new ClassroomsRepository().Classrooms().ToList();
                    ViewBag.Courses = Courses();
                    return View(schedule);
                }

                // Finally, let's check if all the students are available for the lesson
                foreach (User student in schedule.Students)
                {
                    availability = repository.StudentAvailability(student.Id,
                                                                  schedule.WeekDay,
                                                                  schedule.BeginningTime,
                                                                  schedule.EndingTime);

                    if (availability != null)
                    {
                        ModelState.AddModelError("", student.ToString() + " is not available at the given day and times:");
                        ModelState.AddModelError("", availability.ToString());

                        ViewBag.Classrooms = new ClassroomsRepository().Classrooms().ToList();
                        ViewBag.Courses = Courses();
                        return View(schedule);
                    }
                }

                repository.Edit(schedule);
                return RedirectToAction("Index");
            }

            ViewBag.Classrooms = new ClassroomsRepository().Classrooms().ToList();
            ViewBag.Courses = Courses();
            return View(schedule);
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

        [Authorize(Roles = "Student,Teacher")]
        public ActionResult ViewSchedule()
        {
            List<Schedule> planning = new List<Schedule>();

            if (User.IsInRole("Student"))
            {
                planning = repository.StudentSchedules(User.Identity.GetUserId()).ToList();
            }
            else if (User.IsInRole("Teacher"))
            {
                planning = repository.TeacherSchedules(User.Identity.GetUserId()).ToList();
            }

            return View(planning);
        }

        private List<SelectListItem> Courses()
        {
            return new CoursesRepository().Courses().Select(c => new SelectListItem
            {
                Text = c.Subject.Name + " (" + c.Teacher.ToString() + ")",
                Value = c.ID.ToString()
            }).ToList();
        }

        private StudentsInLesson Students()
        {
            StudentsInLesson students = new StudentsInLesson();

            students.Initilize(new UsersRepository().Students().OrderBy(es => es.Id).ToList());

            return students;
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
