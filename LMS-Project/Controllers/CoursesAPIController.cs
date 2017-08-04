using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using LMS_Project.Models;
using LMS_Project.Models.LMS;
using LMS_Project.Repositories;

namespace LMS_Project.Controllers
{
    [Authorize(Roles="Admin")] //Only an Admin can access the api
    public class CoursesAPIController : ApiController
    {
        private CoursesRepository db = new CoursesRepository();
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