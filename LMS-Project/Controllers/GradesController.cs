using LMS_Project.Models.LMS;
using LMS_Project.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LMS_Project.Controllers
{
    [Authorize(Roles="Teacher , Admin")]
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
            Document d = new DocumentsRepository().Document(id) as Document;
            if(d!=null){
                if(gRepo.Grades().Where(g=>g.DocumentID==d.ID).Count()==0){
                    return View(new Grade {  DocumentID=d.ID, Document=d});
                }
            }
            return RedirectToAction("index","Documents");
        }

        // POST: Grades/Create
        [HttpPost]
        public ActionResult Grade(Grade grade)
        {
            try
            {
                // TODO: Add insert logic here
                gRepo.Add(grade);
                return RedirectToAction("Index","Documents");
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

        // GET: Grades/Delete/5
        public ActionResult Delete(int id)
        {
            return View(gRepo.Grade(id));
        }

        // POST: Grades/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here
                gRepo.Delete(id);
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
