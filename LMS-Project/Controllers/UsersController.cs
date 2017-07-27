using LMS_Project.Models;
using LMS_Project.Models.LMS;
using LMS_Project.Repositories;
using LMS_Project.ViewModels;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
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

            foreach (User user in repository.Users())
            {
                Role role = new UsersRepository().GetUserRole(user.Id);

                string roleName = string.Empty;

                if (role != null)
                {
                    roleName = role.Name;
                }

                viewModel.Add(new ExtendedUserVM { User = user, RoleName = roleName });
            }

            return View(viewModel);
        }

        // GET: Users/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = repository.User(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // GET: Users/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = repository.User(id);
            if (user == null)
            {
                return HttpNotFound();
            }

            ViewBag.Roles = new RolesRepository().Roles();
            Role role = repository.GetUserRole(id);

            string roleName = string.Empty;

            if (role != null)
            {
                roleName = role.Name;
            }

            return View(new ExtendedUserVM { User = user, RoleName = roleName });
        }

        // POST: Users/Edit/5
        // Afin de déjouer les attaques par sur-validation, activez les propriétés spécifiques que vous voulez lier. Pour 
        // plus de détails, voir  https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Email,PhoneNumber,UserName")] User user, string roleName)
        {
            if (ModelState.IsValid)
            {
                // The unedited fields of the 'user' variable are set to default values
                // Therefore it's needed to replace the initial values of the only editable fields with the
                // new values

                User originalUser = repository.User(user.Id);

                originalUser.Email = user.Email;
                originalUser.PhoneNumber = user.PhoneNumber;
                originalUser.UserName = user.UserName;

                Role originalRole = repository.GetUserRole(user.Id);

                if (originalRole == null || string.Compare(originalRole.Name, roleName) != 0)
                {
                    var store = new UserStore<User>(new ApplicationDbContext());
                    var userManager = new UserManager<User>(store);

                    if (originalRole != null)
                        userManager.RemoveFromRole(user.Id, originalRole.Name);

                    userManager.AddToRole(user.Id, roleName);
                }

                repository.Edit(originalUser);
                return RedirectToAction("Index");
            }
            return View(user);
        }

        // GET: Users/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = repository.User(id);
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
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = repository.User(id);
            if (user == null)
            {
                return HttpNotFound();
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
