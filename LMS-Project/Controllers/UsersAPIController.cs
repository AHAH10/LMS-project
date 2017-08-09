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
    }
}
