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
        [System.Web.Http.HttpGet]
        public PartialCoursesVM GetCourse(string subjectName)
        {
            Course _course = new CoursesRepository().Courses().Where(c => c.Subject.Name.ToLower() == subjectName.ToLower()).SingleOrDefault();
            return new PartialCoursesVM { ID = _course.ID, Name = _course.Name, DocumentCount = _course.Documents.Count(), ScheduleCount = _course.Schedules.Count(), TeacherID = _course.TeacherID, SubjectID = _course.SubjectID };

        }

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
                PartialCoursesVM tempC = new PartialCoursesVM();
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

                tempC.Name = c.Name;
                tempC.ID = c.ID;
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