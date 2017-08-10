using LMS_Project.Models.LMS;
using LMS_Project.Repositories;
using LMS_Project.ViewModels;
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
