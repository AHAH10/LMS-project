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

namespace LMS_Project.Controllers
{
    public class NotificationsController : Controller
    {
        private NotificationRepository notRepo = new NotificationRepository(); 

        // GET: Notifications
        public ActionResult Index()
        {
            return View(notRepo.UnreadNotifications(User.Identity.GetUserId()));
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                notRepo.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
