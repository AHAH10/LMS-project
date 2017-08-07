using LMS_Project.Repositories;
using Microsoft.AspNet.Identity;
using System.Web.Mvc;

namespace LMS_Project.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            if (Request.IsAuthenticated)
                switch (new UsersRepository().GetUserRole(User.Identity.GetUserId()).Name)
                {
                    case "Student":
                        return RedirectToAction("Planning", "Students");
                    case "Teacher":
                        return RedirectToAction("UngradedAssignments", "Teachers");
                    case "Admin":
                        break;
                    default:
                        break;
                }

            return RedirectToAction("Index", "News");
        }
    }
}