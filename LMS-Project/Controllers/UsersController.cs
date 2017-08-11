using LMS_Project.Models.LMS;
using LMS_Project.Repositories;
using LMS_Project.ViewModels;
using Microsoft.AspNet.Identity;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace LMS_Project.Controllers
{
    [Authorize(Roles = "Admin")]
    public class UsersController : Controller
    {
        private UsersRepository repository = new UsersRepository();

        // GET: Users
        public ActionResult Index()
        {
            List<ExtendedUserVM> viewModel = new List<ExtendedUserVM>();

            foreach (User user in repository.Users().OrderBy(u => u.LastName))
            {
                viewModel.Add(new ExtendedUserVM { User = user, RoleName = new UsersRepository().GetUserRole(user.Id).Name });
            }

            return View(viewModel);
        }

        // GET: Users/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return RedirectToAction("Index");
            }
            User user = repository.UserById(id);
            if (user == null)
            {
                return RedirectToAction("Index");
            }
            return View(new PartialUserVM
            {
                Id = user.Id,
                UserName = user.UserName,
                FirstName = user.FirstName,
                LastName = user.LastName,
                BirthDate = user.BirthDate,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                Role = repository.GetUserRole(user.Id).Name,
                Courses = user.Courses.Select(c => c.Subject.Name).ToList(),
                IsEditable = user.Id != User.Identity.GetUserId()
            });
        }

        // GET: Users/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = repository.UserById(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string submitButton, string id)
        {
            if (submitButton == "yes")
            {
                repository.Delete(id);
            }

            return RedirectToAction("Index");
        }

        // GET: Users/Delete/5
        public ActionResult ResetPassword(string id)
        {
            if (id == null)
            {
                return RedirectToAction("Index");
            }
            User user = repository.UserById(id);
            if (user == null)
            {
                return RedirectToAction("Index");
            }
            return View(user);
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("ResetPassword")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ResetPasswordConfirmed(string submitButton, string id)
        {
            if (submitButton == "yes")
            {
                await repository.ChangePassword(id);
            }

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
