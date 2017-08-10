using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using LMS_Project.Models.LMS;
using LMS_Project.Repositories;

namespace LMS_Project.Controllers
{
    [System.Web.Mvc.ValidateAntiForgeryToken]
    [Authorize(Roles="Admin")] //Only an Admin can access the api
    public class SubjectsAPIController : ApiController
    {
        [HttpGet]
        public List<Subject> GetAllSubjects() //VS 17 returns access violation if a User is returned through api, Courses contains a teacher...
        {
            List<Subject> _subjects = new List<Subject>();
            foreach (Subject s in new SubjectsRepository().Subjects())
            {
                Subject tempS = new Subject();
                tempS.ID = s.ID;
                tempS.Name = s.Name;

                if(s.Courses.Count()>0)
                {
                    List<Course> tList = new List<Course>();

                    foreach(Course c in s.Courses)
                    {
                        Course cTemp = new Course();
                        cTemp.ID = c.ID;
                        tList.Add(cTemp);
                        cTemp = null;
                    }
                    tempS.Courses = tList;
                    tList = null;
                }
                _subjects.Add(tempS);
                tempS = null;
            }
            return _subjects;
        }
    }
}