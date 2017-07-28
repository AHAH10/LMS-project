﻿using System;
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

        // GET: Documents/Create
        public ActionResult Create()
        {
            ViewBag.CourseID = new SelectList(new ApplicationDbContext().Courses, "ID", "Name");
            ViewBag.RoleID = new SelectList(new ApplicationDbContext().Roles, "Id", "Name");
            return View();
        }

        // POST: Documents/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,UserID,DocumentName,UploadingDate,RoleID")] Document document)
        {
            if (ModelState.IsValid)
            {
                repository.Add(document);
                return RedirectToAction("Index");
            }

            return View(document);
        }

        //// GET: Documents/Edit/5
        //public ActionResult Edit(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Document document = repository.Document(id);
        //    if (document == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    ViewBag.CourseID = new SelectList(new ApplicationDbContext().Courses, "ID", "TeacherID", document.CourseID);
        //    ViewBag.RoleID = new SelectList(new ApplicationDbContext().Roles, "Id", "Name", document.RoleID);
        //    return View(document);
        //}

        // POST: Documents/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Edit([Bind(Include = "ID,UserID,DocumentName,UploadingDate,CourseID,RoleID")] Document document)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        repository.Edit(document);
        //        return RedirectToAction("Index");
        //    }
        //    ViewBag.CourseID = new SelectList(new ApplicationDbContext().Courses, "ID", "TeacherID", document.CourseID);
        //    ViewBag.RoleID = new SelectList(new ApplicationDbContext().Roles, "Id", "Name", document.RoleID);

        //    return View(document);
        //}

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