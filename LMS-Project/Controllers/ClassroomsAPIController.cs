using LMS_Project.Models.LMS;
using LMS_Project.Repositories;
using LMS_Project.ViewModels;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace LMS_Project.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ClassroomsAPIController : ApiController
    {
        private ClassroomsRepository repository = new ClassroomsRepository();

        // GET: Classrooms
        public List<PartialClassroomVM> Get()
        {
            return repository.Classrooms().Select(c => new PartialClassroomVM
            {
                ID = c.ID,
                Name = c.Name,
                Location = c.Location,
                Remarks = c.Remarks,
                AmountStudentsMax = c.AmountStudentsMax,
                IsEditable = c.Schedules.Count == 0
            }).ToList();
        }
    }
}
