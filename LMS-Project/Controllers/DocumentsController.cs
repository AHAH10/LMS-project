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
using LMS_Project.ViewModels;
using System.IO;

namespace LMS_Project.Controllers
{
    public class DocumentsController : Controller
    {
        private DocumentsRepository repository = new DocumentsRepository();



        // GET: Documents
        public ActionResult Index()
        {
            var documents = repository.Documents();//.Include(d => d.Course).Include(d => d.Uploader).Include(d => d.VisibleTo);
            return View(repository.document.ToList());   // check if works
        }

        // GET: Documents/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Document document = repository.Document(id);
            if (document == null)
            {
                return HttpNotFound();
            }
            return View(document);
        }

        // GET: Documents/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Document document = repository.Document(id);
            if (document == null)
            {
                return HttpNotFound();
            }
            return View(document);
        }

        // POST: Documents/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            repository.Delete(id);
            return RedirectToAction("Index");
        }

        // UploadDocument Methods

        // Get  Teacher
        [HttpGet]
        public ActionResult UploadDocumentForMyself()
        {
            ViewBag.Courses = new CoursesRepository().Courses().ToList();
            return View();
        }
        // POST  Teacher
        [HttpPost]
        private ActionResult UploadDocumentForMyself(UploadDocumentVM viewModel)
        {
            if (ModelState.IsValid)
            {
                Document document = new Document
                {
                    DocumentName = viewModel.DocumentName,
                    UserID = User.Identity.GetUserId(),
                    RoleID = new RolesRepository().RoleByName("Teacher").Id,  //
                    UploadingDate = DateTime.Now,
                    CourseID = viewModel.CourseID
                };
                // Use your file here
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    viewModel.File.InputStream.CopyTo(memoryStream);
                }
                repository.Add(document);
                return RedirectToAction("Index");
            }

            ViewBag.Courses = new CoursesRepository().Courses().ToList();

            return View(viewModel);
        }
        // Get  Specific Course/ Student
        public ActionResult UploadDocumentForSpecificCourse()
        {
            return View();
        }
        // POST  Specific Course/ Student
        [HttpPost]
        private ActionResult UploadDocumentForSpecificCourse(UploadDocumentVM viewModel)
        {
            if (ModelState.IsValid)
            {
                Document document = new Document
                {
                    DocumentName = viewModel.DocumentName,
                    UserID = User.Identity.GetUserId(),
                    RoleID = new RolesRepository().RoleByName("Teacher").Id,  //
                    UploadingDate = DateTime.Now,
                    CourseID = viewModel.CourseID
                };
                // Use your file here
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    viewModel.File.InputStream.CopyTo(memoryStream);
                }
                repository.Add(document);
                return RedirectToAction("Index");
            }

            ViewBag.Courses = new CoursesRepository().Courses().ToList();
            return View(viewModel);
        }
        // Get For Assignments 
        public ActionResult UploadDocumentForAssignments()
        {
            return View();
        }
        // POST  For Assignments 
        [HttpPost]
        private ActionResult UploadDocumentForAssignments(UploadDocumentVM viewModel)
        {
            if (ModelState.IsValid)
            {
                Document document = new Document
                {
                    DocumentName = viewModel.DocumentName,
                    UserID = User.Identity.GetUserId(),
                    RoleID = new RolesRepository().RoleByName("Student").Id,  //
                    UploadingDate = DateTime.Now,
                    CourseID = viewModel.CourseID
                };
                // Use your file here
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    viewModel.File.InputStream.CopyTo(memoryStream);
                }
                repository.Add(document);
                return RedirectToAction("Index");
            }

            ViewBag.Courses = new CoursesRepository().Courses().ToList();
            return View(viewModel);
        }
        // Get  Classroom
        public ActionResult UploadDocumentForClassroom()
        {
            return View();
        }
        // POST Classroom
        [HttpPost]
        private ActionResult UploadDocumentForClassroom(UploadDocumentVM viewModel)
        {
            if (ModelState.IsValid)
            {
                Document document = new Document
                {
                    DocumentName = viewModel.DocumentName,
                    UserID = User.Identity.GetUserId(),
                    RoleID = new RolesRepository().RoleByName("Teacher").Id,  
                    UploadingDate = DateTime.Now,
                    CourseID = viewModel.CourseID
                };
                // Use your file here
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    viewModel.File.InputStream.CopyTo(memoryStream);
                }
                repository.Add(document);
                return RedirectToAction("Index");
            }

            ViewBag.Courses = new CoursesRepository().Courses().ToList();
            return View(viewModel);
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
