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
    [Authorize(Roles="Admin")]
    public class CoursesController : Controller
    {
        private CourseRepository cRepo = new CourseRepository();
        // GET: Course
        public ActionResult Index()
        {
            return View(cRepo.Courses().ToList());
        }

        // GET: Course/Details/5
        public ActionResult Details(int id)
        {
            return View(cRepo.Course(id));
        }

        // GET: Course/Create
        public ActionResult Create()
        {
            ApplicationDbContext db = new ApplicationDbContext();
            string roleID = db.Roles.Where(ro => ro.Name == "Teacher").FirstOrDefault().Id;

            List<ApplicationUser> _teachers=new List<ApplicationUser>();
            foreach (var u in db.Users.ToList())
            {
                IEnumerable<IdentityUserRole> roles = u.Roles.Where(r => r.RoleId == roleID);
                if(roles.Count()!=0)
                {
                    _teachers.Add(u);
                }
            }
            ViewBag.Teachers = _teachers;
            //ViewBag.Teachers = db.Users.Where(u=>u.Roles.Where(r=>r.RoleId==roleID)).ToList();
            ViewBag.Subjects = new SubjectRepository().Subjects().ToList();
            roleID = null;
            _teachers = null;
            db.Dispose();
            db = null;
           
            return View();
        }

        // POST: Course/Create
        [HttpPost]
        public ActionResult Create(Course course)
        {
            try
            {
                cRepo.Add(course);
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Course/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Course/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Course/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Course/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
