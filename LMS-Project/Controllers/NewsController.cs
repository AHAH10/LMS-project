using LMS_Project.Models.LMS;
using LMS_Project.Repositories;
using Microsoft.AspNet.Identity;
using System;
using System.Linq;
using System.Web.Mvc;

namespace LMS_Project.Controllers
{
    public class NewsController : Controller
    {
        private NewsRepository nRepo = new NewsRepository();

        // GET: News
        public ActionResult Index()
        {
            return View(nRepo.News().ToList());
        }
        // GET: Public News
        public ActionResult AdminIndex()
        {
            return View(nRepo.News().ToList());
        }

        // GET: News/Details/5
        [Authorize(Roles = "Admin")]
        public ActionResult Details(int? id)
        {
            if (id == null)
                return RedirectToAction("AdminIndex");

            News news = nRepo.GetSpecificNews(id);
            if (news == null)
                return RedirectToAction("AdminIndex");

            return View(news);
        }

        [Authorize(Roles = "Admin")]
        // GET: News/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: News/Create
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public ActionResult Create(News news)
        {
            try
            {
                // TODO: Add insert logic here
                news.PublisherID = User.Identity.GetUserId();
                news.PublishingDate = DateTime.Now;
                nRepo.Add(news);
                return RedirectToAction("AdminIndex");
            }
            catch
            {
                return View();
            }
        }

        // GET: News/Edit/5
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
                return RedirectToAction("AdminIndex");

            News news = nRepo.GetSpecificNews(id);
            if (news == null)
                return RedirectToAction("AdminIndex");

            return View(news);
        }

        // POST: News/Edit/5
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public ActionResult Edit(int id, News news)
        {
            try
            {
                // TODO: Add update logic here
                news.EditedByID = User.Identity.GetUserId();
                news.EditedDate = DateTime.Now;
                nRepo.Edit(news);
                return RedirectToAction("AdminIndex");
            }
            catch
            {
                return View(news);
            }
        }

        // GET: News/Delete/5
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int id)
        {
            return View(nRepo.GetSpecificNews(id));
        }

        // POST: News/Delete/5
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int id, News news)
        {
            try
            {
                // TODO: Add delete logic here
                nRepo.Delete(id);
                return RedirectToAction("AdminIndex");
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
                nRepo.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
