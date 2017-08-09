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
    [Authorize]
    public class DocumentsController : Controller
    {
        private DocumentsRepository repository = new DocumentsRepository();

        // GET: Documents
        public ActionResult Index()
        {
           
            return View(repository.Documents().ToList());   
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
        public ActionResult UploadDocumentForMyself(UploadDocumentVM viewModel)
        {
            if (ModelState.IsValid && viewModel.File != null)
            {
                Document document = new Document
                {
                    DocumentName = viewModel.File.FileName,
                    ContentType = viewModel.File.ContentType,
                    UploaderID = User.Identity.GetUserId(),
                    RoleID = new RolesRepository().RoleByName("Teacher").Id,  
                    UploadingDate = DateTime.Now,
                    CourseID = viewModel.CourseID

                };
               
                var content = new byte[viewModel.File.ContentLength];
                viewModel.File.InputStream.Read(content, 0, viewModel.File.ContentLength);
                document.DocumentContent = content;

                repository.Add(document);
                return RedirectToAction("Index");
            }

            ViewBag.Courses = new CoursesRepository().Courses().ToList();

            return View(viewModel);
        }
        // Get  Specific Course
        public ActionResult UploadDocumentForSpecificCourse()
        {
            ViewBag.Courses = new CoursesRepository().Courses().ToList();
            return View();
        }
        // POST  Specific Course
        [HttpPost]
        public ActionResult UploadDocumentForSpecificCourse(UploadDocumentVM viewModel)
        {
            if (ModelState.IsValid && viewModel.File != null)
            {
                Document document = new Document
                {
                    DocumentName = viewModel.File.FileName,
                    ContentType = viewModel.File.ContentType,
                    UploaderID = User.Identity.GetUserId(),
                    RoleID = new RolesRepository().RoleByName("Teacher").Id,  
                    UploadingDate = DateTime.Now,
                    CourseID = viewModel.CourseID
                };
                // Use your file here
                var content = new byte[viewModel.File.ContentLength];
                viewModel.File.InputStream.Read(content, 0, viewModel.File.ContentLength);
                document.DocumentContent = content;

                repository.Add(document);
                return RedirectToAction("Index");
            }

            ViewBag.Courses = new CoursesRepository().Courses().ToList();
            return View(viewModel);
        }
        // Get For Assignments 
        public ActionResult UploadDocumentForAssignments()
        {
            List<Course> debug = new List<Course>();

                /*The Course list on User model is empty , please solve that bug
                 List<Course> courses=new UsersRepository().Students(u=>u.Id==User.Identity.GetUserId()).SingleOrDefault().Courses.toList();
                 */

                foreach(Schedule s in 
                    new UsersRepository().Students().Where(u=>u.Id==User.Identity.GetUserId()).SingleOrDefault().Schedules)
                {
                    debug.Add(s.Course);
                }
            ViewBag.Courses = debug;
            return View();
        }
        // POST  For Assignments 
        [HttpPost]
        public ActionResult UploadDocumentForAssignments(UploadDocumentVM viewModel)
        {
            if (ModelState.IsValid && viewModel.File != null)
            {

                Document document = new Document
                {
                    DocumentName = viewModel.File.FileName,
                    ContentType = viewModel.File.ContentType,
                    UploaderID = User.Identity.GetUserId(),
                    RoleID = new RolesRepository().RoleByName("Student").Id,  
                    UploadingDate = DateTime.Now,
                    CourseID = viewModel.CourseID
                };
                // Use your file here
                var content = new byte[viewModel.File.ContentLength];
                viewModel.File.InputStream.Read(content, 0, viewModel.File.ContentLength);
                document.DocumentContent = content;

                repository.Add(document);
                return RedirectToAction("Index");
            }

            ViewBag.Courses = new CoursesRepository().Courses().ToList();
            return View(viewModel);
        }
        // Get  Classroom
        public ActionResult UploadDocumentForClassroom()
        {
            ViewBag.Courses = new CoursesRepository().Courses().ToList();
            return View();
        }
        // POST Classroom
        [HttpPost]
        public ActionResult UploadDocumentForClassroom(UploadDocumentVM viewModel)
        
        {
            if (ModelState.IsValid && viewModel.File != null)
            {
                Document document = new Document
                {
                    DocumentName = viewModel.File.FileName,
                    ContentType = viewModel.File.ContentType,
                    UploaderID = User.Identity.GetUserId(),
                    RoleID = new RolesRepository().RoleByName("Teacher").Id,
                    UploadingDate = DateTime.Now,
                    CourseID = viewModel.CourseID
                };
                // Use your file here
                var content = new byte[viewModel.File.ContentLength];
                viewModel.File.InputStream.Read(content, 0, viewModel.File.ContentLength);
                document.DocumentContent = content;

                repository.Add(document);
                return RedirectToAction("Index");
            }
            ViewBag.Courses = new CoursesRepository().Courses().ToList();
            return View(viewModel);
        }

        // Download Document

        public FileResult Download(int id)
        {
           
            Document fileToRetrieve = repository.Document(id);
            return File(fileToRetrieve.DocumentContent, fileToRetrieve.ContentType, fileToRetrieve.DocumentName);
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
