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
    [Authorize(Roles = "Student,Teacher")]
    public class DocumentsController : Controller
    {
        private DocumentsRepository repository = new DocumentsRepository();

        // GET: Documents
        public ActionResult Index(int? courseId)
        {
            if (courseId == null)
            {
                return RedirectToAction("Index", "Home");
            }

            Course course = new CoursesRepository().Course(courseId);
            if (course == null)
            {
                return RedirectToAction("Index", "Home");
            }

            return View(repository.Documents((int)courseId).ToList());
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
            ViewBag.Courses = GetCourses(false);
            return View();
        }
        // POST  Teacher
        [HttpPost]
        public ActionResult UploadDocumentForMyself(UploadDocumentVM viewModel)
        {
            if (ModelState.IsValid && viewModel.File != null)
            {
                CreateDocument(viewModel, "Teacher");
                return RedirectToAction("Index");
            }

            ViewBag.Courses = GetCourses(false);

            return View(viewModel);
        }

        // Get  Specific Course
        public ActionResult UploadDocumentForSpecificCourse()
        {
            ViewBag.Courses = GetCourses(false);
            return View();
        }

        // POST  Specific Course
        [HttpPost]
        public ActionResult UploadDocumentForSpecificCourse(UploadDocumentVM viewModel)
        {
            if (ModelState.IsValid && viewModel.File != null)
            {
                CreateDocument(viewModel, "Student");
                return RedirectToAction("Index");
            }

            ViewBag.Courses = GetCourses(false);
            return View(viewModel);
        }

        // Get For Assignments 
        public ActionResult UploadDocumentForAssignments()
        {
            ViewBag.Courses = GetCourses(true);
            return View();
        }

        // POST  For Assignments 
        [HttpPost]
        public ActionResult UploadDocumentForAssignments(UploadDocumentVM viewModel)
        {
            if (ModelState.IsValid && viewModel.File != null)
            {
                CreateDocument(viewModel, "Teacher");
                return RedirectToAction("Index");
            }

            ViewBag.Courses = GetCourses(true);
            return View(viewModel);
        }

        // Get  Classroom
        public ActionResult UploadDocumentForClassroom()
        {
            ViewBag.Courses = GetCourses(true);
            return View();
        }

        // POST Classroom
        [HttpPost]
        public ActionResult UploadDocumentForClassroom(UploadDocumentVM viewModel)
        {
            if (ModelState.IsValid && viewModel.File != null)
            {
                CreateDocument(viewModel, "Student");
                return RedirectToAction("Index");
            }

            ViewBag.Courses = GetCourses(true);
            return View(viewModel);
        }

        // Download Document
        public FileResult Download(int id)
        {
            Document fileToRetrieve = repository.Document(id);
            return File(fileToRetrieve.DocumentContent, fileToRetrieve.ContentType, fileToRetrieve.DocumentName);
        }

        private List<SelectListItem> GetCourses(bool forAStudent)
        {
            if (forAStudent)
                return new UsersRepository().User(User.Identity.GetUserId())
                                            .Schedules
                                            .Select(s => s.Course)
                                            .OrderBy(c => c.Subject.Name)
                                            .ThenBy(c => c.Teacher.ToString())
                                            .Select(c => new SelectListItem
                                            {
                                                Text = c.Subject.Name + " (" + c.Teacher.ToString() + ")",
                                                Value = c.ID.ToString()
                                            })
                                            .ToList();
            else
                return new UsersRepository().User(User.Identity.GetUserId())
                                            .Courses
                                            .OrderBy(c => c.Subject.Name)
                                            .ThenBy(c => c.Teacher.ToString())
                                            .Select(c => new SelectListItem
                                            {
                                                Text = c.Subject.Name + " (" + c.Teacher.ToString() + ")",
                                                Value = c.ID.ToString()
                                            })
                                            .ToList();
        }

        private void CreateDocument(UploadDocumentVM viewModel, string roleVisibleTo)
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

            document.DocumentContent = new byte[viewModel.File.ContentLength];
            viewModel.File.InputStream.Read(document.DocumentContent, 0, viewModel.File.ContentLength);

            repository.Add(document);
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
