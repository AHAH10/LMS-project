using LMS_Project.Models.LMS;
using LMS_Project.Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace LMS_Project.Controllers
{
    [Authorize(Roles="Admin")] //Only an Admin can access the api
    public class CoursesAPIController : ApiController
    {
        private CoursesRepository db = new CoursesRepository();

        public List<Course> Get()
        {
            return db.Courses().ToList();
        }

        /// <summary>
        /// Returns a list of avaible teachers for a specific subject
        /// </summary>
        /// <param name="subjectName"></param>
        /// <returns></returns>
        public User[] GetAvaibleTeachers(int subjectID)
        {
            return new UsersRepository().AvailableTeachers(subjectID).ToArray();
        }

        /// <summary>
        /// Return a list of avaible subjects for a specific teacher
        /// </summary>
        /// <param name="teacherID"></param>
        /// <returns></returns>
        public List<Subject> GetAvaibleSubjects(string teacherID)
        {
            return db.AvaibleSubjects(teacherID);
        }
    }
}