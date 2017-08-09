using LMS_Project.Models;
using LMS_Project.Models.LMS;
using LMS_Project.Repositories;
using LMS_Project.ViewModels;
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
            List<CoursesVM> _courses = new List<CoursesVM>();
            foreach (Course c in cRepo.Courses())
            {
                CoursesVM tempC = new CoursesVM();
                tempC.ID = c.ID;
                tempC.Teacher = new User { Id = c.TeacherID, UserName = c.Teacher.UserName, Email = c.Teacher.Email, FirstName = c.Teacher.FirstName, LastName = c.Teacher.LastName };
                tempC.TeacherID=c.TeacherID;
                tempC.SubjectID=c.SubjectID;
                tempC.Subject=new Subject{ ID=c.Subject.ID, Name=c.Subject.Name};

                _courses.Add(tempC);
            }
            return View(_courses);
        }

        // GET: Course/Details/5
        public ActionResult Details(int? id)
        {
            Course c = cRepo.Course(id) as Course;
            if (c != null)
            {
                CoursesVM cVM = new CoursesVM { ID = c.ID, SubjectID = c.SubjectID, TeacherID = c.TeacherID, Subject = new Subject { ID = c.SubjectID, Name = c.Subject.Name }, Teacher = new User { Id = c.TeacherID, FirstName = c.Teacher.FirstName, LastName = c.Teacher.LastName, UserName = c.Teacher.UserName, Email = c.Teacher.Email } };
                return View(cVM);
            }
            return RedirectToAction("Index");
        }

        // GET: Course/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Course/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(string tID, string sID)
        {
            sID = sID.Substring(sID.IndexOf(':')+1);
            try
            {
                int sId = int.Parse(sID);
                bool success = cRepo.Add(new Course { SubjectID=sId, TeacherID=tID});
                if (success)
                {
                    return RedirectToAction("Index");
                }
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
                CoursesVM cVM = new CoursesVM { ID = c.ID, SubjectID = c.SubjectID, TeacherID = c.TeacherID, Subject = new Subject { ID = c.SubjectID, Name = c.Subject.Name }, Teacher = new User { Id = c.TeacherID, FirstName = c.Teacher.FirstName, LastName = c.Teacher.LastName, UserName = c.Teacher.UserName, Email = c.Teacher.Email } };
                return View(cVM);
            }
            return RedirectToAction("Index");
        }

        // POST: Course/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id,string tID, int sID)
        {
                // TODO: Add update logic here

                Course cToEdit = new Course() { SubjectID = sID, TeacherID = tID, ID = id };
                bool success = cRepo.Edit(cToEdit);
                if (success)
                {
                    return RedirectToAction("Index");
                }
                ViewBag.EMessage = "Error 203: The teacher already have that subject";
                return View();
        }

        // GET: Course/Delete/5
        public ActionResult Delete(int? id)
        {
            Course c = cRepo.Course(id) as Course;
            if (c != null)
            {
                CoursesVM cVM = new CoursesVM { ID = c.ID, SubjectID = c.SubjectID, TeacherID = c.TeacherID, Subject = new Subject { ID = c.SubjectID, Name = c.Subject.Name }, Teacher = new User { Id = c.TeacherID, FirstName = c.Teacher.FirstName, LastName = c.Teacher.LastName, UserName = c.Teacher.UserName, Email = c.Teacher.Email } };
                return View(cVM);
            }
            return RedirectToAction("Index");
        }

        // POST: Course/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here
                bool success = cRepo.Delete(id);
                if (success)
                {
                    return RedirectToAction("Index");
                }
                ViewBag.EMessage = "Error 615: The course you want to delete can't be deleted. (Make sure that it have no documents or schedule to it.)";
                return View();
            }
            catch
            {
                return View();
            }
        }
    }
}
