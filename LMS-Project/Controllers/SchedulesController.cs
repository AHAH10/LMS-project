using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using LMS_Project.Models;
using LMS_Project.Models.LMS;
using LMS_Project.Repositories;
using Microsoft.AspNet.Identity;

namespace LMS_Project.Controllers
{
    [Authorize]
    public class SchedulesController : Controller
    {
        private SchedulesRepository repository= new SchedulesRepository();

        // GET: Schedules
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
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Schedule schedule = repository.Schedule(id);
            if (schedule == null)
            {
                return HttpNotFound();
            }
            return View(schedule);
        }

        // GET: Schedules/Create
        public ActionResult Create()
        {
            ViewBag.Classrooms = new ClassroomsRepository().Classrooms().ToList();
            ViewBag.Courses = new CoursesRepository().Courses().ToList();
            return View();
        }

        // POST: Schedules/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,WeekDay,BeginningTime,EndingTime,CourseId,ClassroomId")] Schedule schedule)
        {
            if (ModelState.IsValid)
            {
                repository.Add(schedule);
                return RedirectToAction("Index");
            }

            ViewBag.Classrooms = new ClassroomsRepository().Classrooms().ToList();
            ViewBag.Courses = new CoursesRepository().Courses().ToList();
            return View(schedule);
        }

        // GET: Schedules/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index");
            }
            Schedule schedule = repository.Schedule(id);
            if (schedule == null)
            {
                return HttpNotFound();
            }

            ViewBag.Classrooms = new ClassroomsRepository().Classrooms().ToList();
            ViewBag.Courses = new CoursesRepository().Courses().ToList();
            return View(schedule);
        }

        // POST: Schedules/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,WeekDay,BeginningTime,EndingTime,CourseId,ClassroomId")] Schedule schedule)
        {
            if (ModelState.IsValid)
            {
                repository.Edit(schedule);
                return RedirectToAction("Index");
            }

            ViewBag.Classrooms = new ClassroomsRepository().Classrooms().ToList();
            ViewBag.Courses = new CoursesRepository().Courses().ToList();
            return View(schedule);
        }

        // GET: Schedules/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Schedule schedule = repository.Schedule(id);
            if (schedule == null)
            {
                return HttpNotFound();
            }
            return View(schedule);
        }

        // POST: Schedules/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            repository.Delete(id);
            return RedirectToAction("Index");
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
