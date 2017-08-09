using LMS_Project.Models.LMS;
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
            {
                string roleName = new UsersRepository().GetUserRole(User.Identity.GetUserId()).Name;

                if (roleName == RoleConstants.Student)
                    return RedirectToAction("Planning", "Students");
                else if (roleName == RoleConstants.Teacher)
                    return RedirectToAction("UngradedAssignments", "Teachers");
            }

            return RedirectToAction("Index", "News");
        }
    }
}