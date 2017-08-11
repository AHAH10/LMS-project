using LMS_Project.Models.LMS;
using LMS_Project.Repositories;
using LMS_Project.ViewModels;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace LMS_Project.Controllers
{
    [Authorize(Roles = "Student,Teacher")]
    public class DocumentsController : Controller
    {
        private DocumentsRepository repository = new DocumentsRepository();

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
            return RedirectToAction("MyDocuments");
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
                string result = CreateDocument(viewModel, RoleConstants.Teacher);

                if (result.Length == 0)
                    return RedirectToAction("MyDocuments");
                else
                {
                    ViewBag.ErrorMessage = result;
                    ViewBag.Courses = GetCourses(false);

                    return View(viewModel);
                }
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
                string result = CreateDocument(viewModel, RoleConstants.Student);

                if (result.Length == 0)
                    return RedirectToAction("MyDocuments");
                else
                {
                    ViewBag.ErrorMessage = result;
                    ViewBag.Courses = GetCourses(false);

                    return View(viewModel);
                }
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
                string result = CreateDocument(viewModel, RoleConstants.Teacher);

                if (result.Length == 0)
                    return RedirectToAction("MyDocuments");
                else
                {
                    ViewBag.ErrorMessage = result;
                    ViewBag.Courses = GetCourses(true);

                    return View(viewModel);
                }
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
                string result = CreateDocument(viewModel, RoleConstants.Student);

                if (result.Length == 0)
                    return RedirectToAction("MyDocuments");
                else
                {
                    ViewBag.ErrorMessage = result;
                    ViewBag.Courses = GetCourses(true);

                    return View(viewModel);
                }
            }

            ViewBag.Courses = GetCourses(true);
            return View(viewModel);
        }

        public ActionResult MyDocuments()
        {
            return View(repository.Documents(User.Identity.GetUserId()).ToList());
        }

        // Download Document
        public FileResult Download(int id)
        {
            Document fileToRetrieve = repository.Document(id);
            return File(fileToRetrieve.DocumentContent, fileToRetrieve.ContentType, fileToRetrieve.DocumentName);
        }

        private List<SelectListItem> GetCourses(bool fromAStudent)
        {
            IEnumerable<Course> courses = null;

            if (fromAStudent)
                courses = new CoursesRepository().StudentCourses(User.Identity.GetUserId());
            else
                courses = new CoursesRepository().TeacherCourse(User.Identity.GetUserId());

            return courses.Select(c => new SelectListItem
            {
                Text = c.FullName + (fromAStudent ? " (" + c.Teacher.ToString() + ")" : string.Empty),
                Value = c.ID.ToString()
            })
            .ToList();
        }

        private string CreateDocument(UploadDocumentVM viewModel, string roleVisibleTo)
        {
            try
            {
                Document document = new Document
                {
                    DocumentName = viewModel.File.FileName,
                    ContentType = viewModel.File.ContentType,
                    UploaderID = User.Identity.GetUserId(),
                    RoleID = new RolesRepository().RoleByName(roleVisibleTo).Id,
                    UploadingDate = DateTime.Now,
                    CourseID = viewModel.CourseID
                };

                document.DocumentContent = new byte[viewModel.File.ContentLength];
                viewModel.File.InputStream.Read(document.DocumentContent, 0, viewModel.File.ContentLength);

                repository.Add(document);

                return string.Empty;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
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
