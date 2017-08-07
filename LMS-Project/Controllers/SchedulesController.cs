using LMS_Project.Models.LMS;
using LMS_Project.Repositories;
using LMS_Project.ViewModels;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;

namespace LMS_Project.Controllers
{
    [Authorize]
    public class SchedulesController : Controller
    {
        private SchedulesRepository repository = new SchedulesRepository();

        // GET: Schedules
        [Authorize(Roles = "Admin")]
        public ActionResult Index()
        {
            return View();
        }

        // GET: Schedules/Details/5
        [Authorize(Roles = "Admin")]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index", "Home");
            }
            Schedule schedule = repository.Schedule(id);
            if (schedule == null)
            {
                return RedirectToAction("Index", "Home");
            }
            return View(schedule);
        }

        // GET: Schedules/Create
        [Authorize(Roles = "Admin")]
        public ActionResult Create()
        {
            return View();
        }

        // GET: Schedules/Edit/5
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index");
            }

            Schedule schedule = repository.Schedule(id);
            if (schedule == null)
            {
                return RedirectToAction("Index");
            }

            ViewBag.ScheduleId = id;

            return View();
        }

        // GET: Schedules/Delete/5
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index");
            }
            Schedule schedule = repository.Schedule(id);
            if (schedule == null)
            {
                return RedirectToAction("Index");
            }
            return View(schedule);
        }

        // POST: Schedules/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult DeleteConfirmed(int id)
        {
            repository.Delete(id);
            return RedirectToAction("Index");
        }

        public ActionResult Documents(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index");
            }

            Schedule schedule = repository.Schedule(id);

            if (schedule == null)
            {
                return RedirectToAction("Index");
            }

            string userId = User.Identity.GetUserId();
            List<Document> documents = schedule.Course.Documents.ToList();

            switch (new UsersRepository().GetUserRole(userId).Name)
            {
                case "Student":
                    // Any student is allowed to see documents for the current course, as long as the
                    // given student takes part of that course
                    if (!repository.TakesPart(userId, (int)id))
                    {
                        return RedirectToAction("Index", "Home");
                    }

                    // But only documents visible to Students are displayed
                    documents = documents.Where(d => d.VisibleFor.Name == "Student").ToList();

                    break;
                case "Teacher":
                    // Any teacher is allowed to see documents for the current course, as long as the
                    // given teacher is in charge of that course
                    if (!repository.IsInCharge(userId, (int)id))
                    {
                        return RedirectToAction("Index", "Home");
                    }

                    break;
                default:
                    return RedirectToAction("Index");
            }

            if (documents.Count == 0)
                return RedirectToAction("Index");

            return View(documents);
        }

        private List<SelectListItem> Courses()
        {
            return new CoursesRepository().Courses()
                                          .OrderBy(c => c.Subject.Name)
                                          .ThenBy(c => c.Teacher.ToString())
                                          .Select(c => new SelectListItem
                                          {
                                              Text = c.Subject.Name + " (" + c.Teacher.ToString() + ")",
                                              Value = c.ID.ToString()
                                          }).ToList();
        }

        private List<SelectListItem> Classrooms()
        {
            return new ClassroomsRepository().Classrooms()
                                             .OrderBy(c => c.Name)
                                             .Select(c => new SelectListItem
                                             {
                                                 Text = c.Name + (c.Remarks == null || c.Remarks.Length == 0 ? string.Empty : " - " + c.Remarks),
                                                 Value = c.ID.ToString()
                                             }).ToList();
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
