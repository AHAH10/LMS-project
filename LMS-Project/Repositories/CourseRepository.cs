using LMS_Project.Models;
using LMS_Project.Models.LMS;
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
            var _courses = this.Courses().Where(c=>c.TeacherID==course.TeacherID && c.SubjectID==course.SubjectID);
            if(_courses.Count()!=0)
            {
                _courses = null;
                return false;
            }
            db.Courses.Add(course);
            SaveChanges();
            return true;
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

        public void Delete(int? id)
        {
            Course course = Course(id);

            if (course != null)
            {
                db.Subjects.Where(s => s.Courses.Remove(course));
                db.Courses.Remove(course);
                SaveChanges();
            }
        }

        //Save changed
        private void SaveChanges()
        {
            db.SaveChanges();
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