﻿using LMS_Project.Models.LMS;
using LMS_Project.Repositories;
using System.Web.Mvc;

namespace LMS_Project.Controllers
{
    [Authorize(Roles = "Admin")]
    public class SubjectsController : Controller
    {
        SubjectsRepository sRepo = new SubjectsRepository();
        // GET: Subjects
        public ActionResult Index()
        {
            return View(sRepo.Subjects());
        }

        // GET: Subjects/Details/5
        public ActionResult Details(int? id)
        {
            Subject s = sRepo.Subject(id) as Subject;
            if (s != null)
            {
                return View(s);
            }
            return RedirectToAction("Index");
        }

        // GET: Subjects/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Subjects/Create
        [HttpPost]
        public ActionResult Create(Subject subject)
        {
            try
            {
                bool success = sRepo.Add(subject);
                // TODO: Add insert logic here
                if (success)
                {
                    return RedirectToAction("Index");
                }
                ViewBag.EMessage = "Subject already exists.";
                return View();
            }
            catch
            {
                return View();
            }
        }

        // GET: Subjects/Edit/5
        public ActionResult Edit(int? id)
        {
            Subject s = sRepo.Subject(id) as Subject;
            if (s != null)
            {
                return View(s);
            }
            return RedirectToAction("Index");
        }

        // POST: Subjects/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, Subject subject)
        {
            try
            {
                // TODO: Add update logic here
                bool success = sRepo.Edit(subject);
                if (success)
                {
                    return RedirectToAction("Index");
                }
                ViewBag.EMessage = "A subject with same Name already exists";
                return View();
            }
            catch
            {
                return View();
            }
        }

        // GET: Subjects/Delete/5
        public ActionResult Delete(int? id)
        {
            Subject s = sRepo.Subject(id) as Subject;
            if (s != null)
            {
                return View(s);
            }
            return RedirectToAction("Index");
        }

        // POST: Subjects/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                bool success = sRepo.Delete(id);
                if (success)
                {
                    return RedirectToAction("Index");
                }
                ViewBag.EMessage = "Error 666: The subject you want to delete can't be deleted. (Make sure that course(s) are removed.)";
                return View();
            }
            catch
            {
                return View();
            }
        }
    }
}
