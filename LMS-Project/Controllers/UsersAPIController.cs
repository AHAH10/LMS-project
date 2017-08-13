using LMS_Project.Repositories;
using LMS_Project.ViewModels;
using Microsoft.AspNet.Identity;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace LMS_Project.Controllers
{
    [Authorize(Roles = "Admin")]
    public class UsersAPIController : ApiController
    {
        UsersRepository repository = new UsersRepository();

        // GET: Users
        public List<PartialUserVM> GetUsers()
        {
            // The user can't be deleted if:
            // - the edited user actually is the current user (can't delete oneself's account)
            // - the user is responsible for some courses
            // - the user takes part to any course
            // - the user has uploaded some documents (whatever purpose they have)
            // - the user has published some news
            return repository.Users().Select(u => new PartialUserVM
            {
                Id = u.Id,
                UserName = u.UserName,
                FirstName = u.FirstName,
                LastName = u.LastName,
                BirthDate = u.BirthDate,
                Email = u.Email,
                PhoneNumber = u.PhoneNumber,
                Role = repository.GetUserRole(u.Id).Name,
                IsDeletable = User.Identity.GetUserId() != u.Id &&
                              u.Courses.Count == 0 &&
                              u.Schedules.Count == 0 &&
                              u.Documents.Count == 0 &&
                              u.News.Count == 0
            }).ToList();
        }

        // GET: Students
        public List<PartialUserVM> GetStudents()
        {
            return repository.Students().Select(s => new PartialUserVM
            {
                Id = s.Id,
                FirstName = s.FirstName,
                LastName = s.LastName,
                BirthDate = s.BirthDate,
                Email = s.Email,
                PhoneNumber = s.PhoneNumber
            }).ToList();
        }

        //To get User information about teacher - An Validate Token must be needed - Security
        [System.Web.Mvc.ValidateAntiForgeryToken]
        public List<PartialUserVM> GetAvailableTeachers(int subjectID)
        {
            return repository.AvailableTeachers(subjectID).Select(t => new PartialUserVM
            {
                Id = t.Id,
                FirstName = t.FirstName,
                LastName = t.LastName
            }).ToList();
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
