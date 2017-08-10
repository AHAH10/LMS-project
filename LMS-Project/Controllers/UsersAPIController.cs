using LMS_Project.Models.LMS;
using LMS_Project.Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace LMS_Project.Controllers
{
    [Authorize(Roles = "Admin")]
    public class UsersAPIController : ApiController
    {
        // GET: Users
        public List<User> GetStudents()
        {
            return new UsersRepository().Students().ToList();
        }
        //To get User information about teacher - An Validate Token must be needed - Security
        [System.Web.Mvc.ValidateAntiForgeryToken]
        public List<User> GetAvailableTeachers(int subjectID)
        {
            List<User> _teachers = new List<User>();
            foreach (var t in new UsersRepository().AvailableTeachers(subjectID))
            {
                //Some data shouldn't be sent! , that's why a new instane of the User is created
                User tempT = new User();
                tempT.Id = t.Id;
                tempT.FirstName = t.FirstName;
                tempT.LastName = t.LastName;

                _teachers.Add(tempT);
                tempT = null;
            }
            return _teachers;
        }
    }
}
