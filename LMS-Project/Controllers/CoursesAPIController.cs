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
        public List<Subject> GetAvaibleSubjects(string user)
        { 
            return new SubjectsRepository().AvaibleSubjects(user);
        }
        public List<User> GetAvaibleTeachers(int subject)
        {
            return new UsersRepository().AvailableTeachers(subject).ToList();
        }
        public List<Subject> GetSubjects()
        {
            return new SubjectsRepository().Subjects().ToList();
        }
    }
}