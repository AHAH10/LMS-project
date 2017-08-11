using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using LMS_Project.Models.LMS;
using LMS_Project.Repositories;
using System.Web.Mvc;
using LMS_Project.ViewModels;

namespace LMS_Project.Controllers
{
    [System.Web.Http.Authorize(Roles = "Admin")] //Only an Admin can access the api
    [ValidateAntiForgeryToken]
    public class CoursesAPIController : ApiController
    {
        CoursesRepository repository = new CoursesRepository();

        /// <summary>
        /// Return all courses
        /// </summary>
        /// <returns></returns>
        [System.Web.Http.HttpGet]
        public List<PartialCoursesVM> GetAllCourses()
        {
            //On visual studio 2017, access violation might occur than we are sending the whole user through the api.
            //return new CoursesRepository().Courses() works fine on vs 13 with the api
            //Guess that microsoft want us to avoid sending security information.
            return repository.Courses()
                             .Select(c => new PartialCoursesVM
                             {
                                 ID = c.ID,
                                 Name = c.Name,
                                 FullName = c.FullName,
                                 IsDeletable = c.Documents.Count() + c.Schedules.Count() == 0,
                                 //Binding data
                                 Subject = new Subject { ID = c.SubjectID, Name = c.Subject.Name },
                                 Teacher = new PartialUserVM
                                 {
                                     //Set data that are needed
                                     Id = c.TeacherID,
                                     FirstName = c.Teacher.FirstName,
                                     LastName = c.Teacher.LastName,
                                     Email = c.Teacher.Email
                                 }
                             })
                             .ToList();
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