using LMS_Project.Models;
using LMS_Project.Models.LMS;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace LMS_Project.Repositories
{
    public class CourseRepository : IDisposable
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public IEnumerable<Course> Courses()
        {
            return db.Courses;
        }

        public Course Course(int? id)
        {
            return Courses().SingleOrDefault(c => c.ID == id);
        }

        public bool Add(Course course)
        {
            if (course.TeacherID != null && course.Subject != null)
            {
                var _courses = this.Courses().Where(c => c.TeacherID == course.TeacherID && c.SubjectID == course.SubjectID);
                if (_courses.Count() != 0)
                {
                    _courses = null;
                    return false;
                }
                db.Courses.Add(course);
                SaveChanges();
                return true;
            }
            return false;
        }

        public bool Edit(Course course)
        {
           
            if (this.db.Courses.Where(_cRel => _cRel.ID != course.ID && _cRel.SubjectID == course.SubjectID&&_cRel.TeacherID == course.TeacherID).Count() ==0)
            {
                db.Entry(course).State = EntityState.Modified;
                db.SaveChanges();
                return true;
            }
            return false;
        }

        public bool Delete(int? id)
        {
            Course course = Course(id);

            if (course != null)
            {
                if(course.Documents.Count()==0 && course.Schedules.Count()==0)
                {
                    db.Subjects.Where(s => s.Courses.Remove(course));
                    db.Courses.Remove(course);
                    SaveChanges();
                    return true;
                }
            }
            return false;
        }

        //Save changed
        private void SaveChanges()
        {
            db.SaveChanges();
        }
        //Get All Teachers - Remove if we don't need
        public List<User> GetTeachers()
        {
            string roleID = db.Roles.Where(ro => ro.Name == "Teacher").FirstOrDefault().Id;

            List<User> _teachers = new List<User>();
            foreach (var u in db.LMSUsers.ToList())
            {
                IEnumerable<IdentityUserRole> roles = u.Roles.Where(r => r.RoleId == roleID);
                if (roles.Count() != 0)
                {
                    _teachers.Add(u);
                }
            }
            roleID = null;
            return _teachers;
        }
        //AvaibleTeachers
        public List<User> AvaibleTeachers(Subject subject)
        {
            List<User> _result = new List<User>();

            if (subject != null)
            {
                foreach (User t in GetTeachers())
                {
                    if (t.Courses.Where(c => c.Subject.Name == subject.Name).Count() == 0)
                    {
                        _result.Add(t);
                    }
                }
            }
            return _result;
        }
        public List<User> AvaibleTeachers(string subjectName)
        {
            List<User> _result = new List<User>();

                foreach (User t in GetTeachers())
                {
                    if (t.Courses.Where(c => c.Subject.Name.ToLower() == subjectName.ToLower()).Count() == 0)
                    {
                        _result.Add(t);
                    }
                }

            return _result;
        }
        #region IDisposable Support
        private bool disposedValue = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                db.Dispose();
                disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
        }
        #endregion
    }
}