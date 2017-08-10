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
                return RedirectToAction("Index", "Home");
            }

            Schedule schedule = repository.Schedule(id);

            if (schedule == null)
            {
                return RedirectToAction("Index", "Home");
            }

            string userId = User.Identity.GetUserId();
            List<Document> documents = schedule.Course.Documents.ToList();

            UsersRepository usersRepo = new UsersRepository();
            string roleName = usersRepo.GetUserRole(userId).Name;
            if (roleName == RoleConstants.Student)
            {
                // Any student is allowed to see documents for the current course, as long as the
                // given student takes part of that course
                if (!repository.TakesPart(userId, (int)id))
                {
                    return RedirectToAction("Index", "Home");
                }

                // But only documents visible to Students are displayed
                documents = documents.Where(d => d.VisibleFor.Name == RoleConstants.Student).ToList();
            }
            else if (roleName == RoleConstants.Teacher)
            {
                // Any teacher is allowed to see documents for the current course, as long as the
                // given teacher is in charge of that course
                if (!repository.IsInCharge(userId, (int)id))
                {
                    return RedirectToAction("Index", "Home");
                }
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }

            if (documents.Count == 0)
                return RedirectToAction("Index", "Home");

            return View(documents.Select(d => new DocumentsScheduleVM
            {
                Document = d,
                UploadersRole = usersRepo.GetUserRole(d.Uploader.Id).Name
            }));
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
