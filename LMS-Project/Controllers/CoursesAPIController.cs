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
            List<PartialCoursesVM> _courses = new List<PartialCoursesVM>();
            foreach (Course c in new CoursesRepository().Courses())
            {
                //Create new objects
                Subject tempS = new Subject();
                PartialUserVM tempT = new PartialUserVM
                {
                    //Set data that are needed
                    Id = c.TeacherID,
                    FirstName = c.Teacher.FirstName,
                    LastName = c.Teacher.LastName,
                    Email = c.Teacher.Email
                };

                tempS.ID = c.SubjectID;
                tempS.Name = c.Subject.Name;

                PartialCoursesVM tempC = new PartialCoursesVM
                {
                    ID = c.ID,
                    IsDeletable = c.Documents.Count() + c.Schedules.Count() == 0,
                //Binding data
                    Subject = tempS,
                    Teacher = tempT
                };
                //Add to course list
                _courses.Add(tempC);
                //Clear memory
                tempC = null;
                tempS = null;
                tempT = null;

            }

            return _courses;
        }
    }
}