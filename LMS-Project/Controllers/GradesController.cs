using LMS_Project.Models.LMS;
using LMS_Project.Repositories;
using LMS_Project.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LMS_Project.Controllers
{
    [Authorize(Roles = "Teacher , Admin")]
    public class GradesController : Controller
    {
        private GradesRepository gRepo = new GradesRepository();
        // GET: Grades
        public ActionResult Index()
        {
            return View(gRepo.Grades());
        }

        // GET: Grades/Details/5
        public ActionResult Details(int? id)
        {
            Grade g = gRepo.Grade(id) as Grade;
            if (g != null)
            {
                return View(g);
            }
            return RedirectToAction("Index");
        }

        public ActionResult Grade(int? id)
        {
            Document d = new DocumentsRepository().Document(id);
            if (d != null)
            {
                if (gRepo.Grades().Where(g => g.Document.ID == d.ID).Count() == 0)
                {
                    return View(new Grade { ID=d.ID, Document=d });
                }
            }
            return RedirectToAction("UngradedAssignments", "Teachers");
        }

        // POST: Grades/Create
        [HttpPost]
        public ActionResult Grade(Grade gradeVM)
        {
            try
            {
                gRepo.Add(gradeVM);

                return RedirectToAction("UngradedAssignments", "Teachers");
            }
            catch
            {
                return View();
            }
        }

        // GET: Grades/Edit/5
        public ActionResult Edit(int id)
        {
            return View(gRepo.Grade(id));
        }

        // POST: Grades/Edit/5
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

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                gRepo.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
