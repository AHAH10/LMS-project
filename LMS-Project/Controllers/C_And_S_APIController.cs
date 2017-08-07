using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using LMS_Project.Models.LMS;
using LMS_Project.Repositories;

namespace LMS_Project.Controllers
{
    [Authorize(Roles="Admin")] //Only an Admin can access the api
    public class C_And_S_APIController : ApiController
    {
        /// <summary>
        /// Returns all Available Teachers for a specific subject
        /// </summary>
        /// <param name="subjectID"></param>
        /// <returns></returns>
        [HttpGet]
        public List<User> GetAvailableTeachers(int subjectID)
        {
            return new UsersRepository().AvailableTeachers(subjectID).ToList(); //unsafe code - because of passwordhash and other private info
        }
        /// <summary>
        /// Returns all available Teachers for a specific subject but with less info.
        /// </summary>
        /// <param name="subjectID"></param>
        /// <returns></returns>
        [HttpGet]
        public List<User> GetAvailableTeachersWithLessInfo(int subjectID)
        {
            List<User> _teachers = new List<User>();
            foreach (var t in new UsersRepository().AvailableTeachers(subjectID))
            {
                User tempT = new User();
                tempT.Id = t.Id;
                tempT.FirstName = t.FirstName;
                tempT.LastName = t.LastName;

                _teachers.Add(tempT);
                tempT = null;
            }
            return _teachers;
        }
        /// <summary>
        /// Return all courses
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public List<Course> GetAllCourses()
        {
            //On visual studio 2017, access violation might occur than we are sending the whole user through the api.
            //return new CoursesRepository().Courses() works fine on vs 13 with the api
            //Guess that microsoft want us to avoid sending security information.
            List<Course> _courses = new List<Course>();
            foreach (Course c in new CoursesRepository().Courses())
            {
                //Create new objects
                Course tempC = new Course();
                Subject tempS = new Subject();
                User tempT = new User();
                //Set data that are needed
                tempT.Id = c.TeacherID;
                tempT.UserName=c.Teacher.FirstName+" "+c.Teacher.LastName;
                tempT.FirstName = c.Teacher.FirstName;
                tempT.LastName = c.Teacher.LastName;

                tempS.ID = c.SubjectID;
                tempS.Name = c.Subject.Name;
                //Binding data
                tempC.SubjectID = c.SubjectID;
                tempC.TeacherID = c.TeacherID;

                tempC.Subject = tempS;
                tempC.Teacher = tempT;
                tempC.ID = c.ID;
                //Add to course list
                _courses.Add(tempC);
                //Clear memory
                tempC = null;
                tempS = null;
                tempT = null;

            }

            return _courses;
        }
        [HttpGet]
        public List<Subject> GetAllSubjects() //VS 17 returns access violation if a whole User is returned through api, Courses contains a teacher...
        {
            List<Subject> _subjects = new List<Subject>();
            foreach(Subject s in new SubjectsRepository().Subjects())
            {
                Subject tempS = new Subject();
                tempS.ID = s.ID;
                tempS.Name = s.Name;
                _subjects.Add(tempS);
                tempS = null;
            }
            return _subjects;
        }
    }
}