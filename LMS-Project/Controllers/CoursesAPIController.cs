using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using LMS_Project.Models.LMS;
using LMS_Project.Repositories;
using System.Web.Mvc;
using LMS_Project.ViewModels;

namespace LMS_Project.Controllers
{
    [System.Web.Http.Authorize(Roles="Admin")] //Only an Admin can access the api
    [ValidateAntiForgeryToken]
    public class CoursesAPIController : ApiController
    {
        /// <summary>
        /// Return all courses
        /// </summary>
        /// <returns></returns>
        [System.Web.Http.HttpGet]
        public List<CoursesVM> GetAllCourses()
        {
            //On visual studio 2017, access violation might occur than we are sending the whole user through the api.
            //return new CoursesRepository().Courses() works fine on vs 13 with the api
            //Guess that microsoft want us to avoid sending security information.
            List<CoursesVM> _courses = new List<CoursesVM>();
            foreach (Course c in new CoursesRepository().Courses())
            {
                //Create new objects
                CoursesVM tempC = new CoursesVM();
                Subject tempS = new Subject();
                User tempT = new User();
                //Set data that are needed
                tempT.Id = c.TeacherID;
                tempT.UserName=c.Teacher.FirstName+" "+c.Teacher.LastName;
                tempT.FirstName = c.Teacher.FirstName;
                tempT.LastName = c.Teacher.LastName;

                tempS.ID = c.SubjectID;
                tempS.Name = c.Subject.Name;

                tempC.Id = c.ID;
                tempC.DocumentCount = c.Documents.Count();
                tempC.ScheduleCount = c.Schedules.Count();
                //Binding data
                tempC.SubjectID = c.SubjectID;
                tempC.TeacherID = c.TeacherID;

                tempC.Subject = tempS;
                tempC.Teacher = tempT;
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