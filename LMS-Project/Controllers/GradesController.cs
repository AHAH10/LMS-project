using LMS_Project.Models.LMS;
using LMS_Project.Repositories;
using LMS_Project.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.Identity;
using System.Web.Mvc;

namespace LMS_Project.Controllers
{
    [Authorize(Roles = "Teacher , Admin, Student")]
    public class GradesController : Controller
    {
        private GradesRepository gRepo = new GradesRepository();

        public ActionResult Grade(int? id)
        {
            Document d = new DocumentsRepository().Document(id);
            if (d != null)
            {
                if (gRepo.Grades().Where(g => g.Document.ID == d.ID).Count() == 0)
                {
                    return View(new Grade { ID = d.ID, Document = d });
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
                gRepo.Add(gradeVM, User.Identity.GetUserId());

                return RedirectToAction("UngradedAssignments", "Teachers");
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
