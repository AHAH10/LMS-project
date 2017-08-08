using LMS_Project.Models.LMS;
using LMS_Project.Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace LMS_Project.Controllers
{
    [Authorize(Roles="Admin")]
    public class ClassroomsAPIController : ApiController
    {
        private ClassroomsRepository repository = new ClassroomsRepository();

        // GET: Classrooms
        public List<Classroom> Get()
        {
            return repository.Classrooms().ToList();
        }
    }
}
