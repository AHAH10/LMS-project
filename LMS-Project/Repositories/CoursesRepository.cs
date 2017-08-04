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
    public class CoursesRepository : IDisposable
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
            if (course.TeacherID != null)
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

            if (this.db.Courses.Where(_cRel => _cRel.ID != course.ID && _cRel.SubjectID == course.SubjectID && _cRel.TeacherID == course.TeacherID).Count() == 0)
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
                if (course.Documents.Count() == 0 && course.Schedules.Count() == 0)
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
        /// <summary>
        /// Returns all Avaible Subjects for a specific teacher
        /// </summary>
        /// <param name="teacherID"></param>
        /// <returns></returns>
        public List<Subject> AvaibleSubjects(string teacherID)
        {
            List<Subject> Subject_result = new List<Subject>();

            foreach (Course co in db.Courses)
            {
                if (co.TeacherID != teacherID)
                {
                    Subject_result.Add(new SubjectsRepository().Subject(co.SubjectID));
                }
            }

            return Subject_result;
        }

        ///// <summary>
        ///// Returns all avaible teachers for a specific subject
        ///// </summary>
        ///// <param name="subjectName"></param>
        ///// <returns></returns>
        //public List<User> AvaibleTeachers(string subjectName)
        //{
        //    List<User> _result = new List<User>();

        //    foreach (User t in GetTeachers())
        //    {
        //        if (t.Courses.Where(c => string.Compare(c.Subject.Name, subjectName, true) == 0).Count() == 0)
        //        {
        //            _result.Add(t);
        //        }
        //    }

        //    return _result;
        //}

        //public List<User> AvaibleTeachers(int subjectID)
        //{
        //    List<User> _result = new List<User>();

        //    foreach (User t in GetTeachers())
        //    {
        //        if (t.Courses.Where(c => c.Subject.ID == subjectID).Count() == 0)
        //        {
        //            _result.Add(t);
        //        }
        //    }

        //    return _result;
        //}

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