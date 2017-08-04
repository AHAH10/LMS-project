using LMS_Project.Models;
using LMS_Project.Models.LMS;
using LMS_Project.Repositories;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LMS_Project.Controllers
{
    [Authorize(Roles = "Admin")]
    public class CoursesController : Controller
    {
        private CoursesRepository cRepo = new CoursesRepository();
        // GET: Course
        public ActionResult Index()
        {
            return View(cRepo.Courses().ToList());
        }

        // GET: Course/Details/5
        public ActionResult Details(int? id)
        {
            Course c = cRepo.Course(id) as Course;
            if (c != null)
            {
                return View(c);
            }
            return RedirectToAction("Index");
        }

        // GET: Course/Create
        public ActionResult Create()
        {
            ViewBag.Teachers = new UsersRepository().AvailableTeachers(new SubjectsRepository().Subjects().ToList()[0].ID);
            List<Subject> s = new List<Subject>();
            foreach (Subject sub in new SubjectsRepository().Subjects())
            {
                if (new UsersRepository().AvailableTeachers(sub.ID).Count() != 0)
                {
                    s.Add(sub);
                }
            }
            ViewBag.Subjects = s;

            return View();
        }

        // POST: Course/Create
        [HttpPost]
        public ActionResult Create(Course course)
        {
            try
            {
                bool success = cRepo.Add(course);
                if (success)
                {
                    return RedirectToAction("Index");
                }
                ViewBag.Teachers = new UsersRepository().Teachers();
                ViewBag.Subjects = new SubjectsRepository().Subjects().ToList();
                ViewBag.EMessage = "The Course You want to add already exists";
                return View();
            }
            catch
            {
                return View();
            }
        }

        // GET: Course/Edit/5
        public ActionResult Edit(int? id)
        {
            Course c = cRepo.Course(id) as Course;
            if (c != null)
            {
                ViewBag.Teachers = new UsersRepository().AvailableTeachers(c.SubjectID);
                ViewBag.Subjects = new SubjectsRepository().Subjects().ToList();
                return View(c);
            }
            return RedirectToAction("Index");
        }

        // POST: Course/Edit/5
        [HttpPost]
        public ActionResult Edit(Course course)
        {
            try
            {
                // TODO: Add update logic here
                bool success = cRepo.Edit(course);
                if (success)
                {
                    return RedirectToAction("Index");
                }
                ViewBag.EMessage = "Error 203: The teacher already have that subject";
                ViewBag.Teachers = new UsersRepository().Teachers();
                ViewBag.Subjects = new SubjectsRepository().Subjects().ToList();
                return View(course);
            }
            catch
            {
                return View(course);
            }
        }

        // GET: Course/Delete/5
        public ActionResult Delete(int? id)
        {
            Course c = cRepo.Course(id) as Course;
            if (c != null)
            {
                return View(cRepo.Course(id));
            }
            return RedirectToAction("Index");
        }

        // POST: Course/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here
                cRepo.Delete(id);
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
