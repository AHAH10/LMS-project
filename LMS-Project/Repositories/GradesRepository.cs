using LMS_Project.Models;
using LMS_Project.Models.LMS;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace LMS_Project.Repositories
{
    public class GradesRepository : IDisposable
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public IEnumerable<Grade> Grades()
        {
            return db.Grades;
        }

        public IEnumerable<Grade> GetStudentGrades(string studentID)
        {
            return db.Grades.Where(g => g.Document.UploaderID == studentID);
        }

        public Grade Grade(int? id)
        {
            return Grades().FirstOrDefault(g => g.ID == id);
        }

        public void Add(Grade grade, string teacherID)
        {
            grade.Date = DateTime.Now;
            db.Grades.Add(grade);

            SaveChanges();

            Document document = db.Documents.FirstOrDefault(d => d.ID == grade.ID);
            document.GradeID = grade.ID;
            db.Entry(document).State = EntityState.Modified;

            SaveChanges();

            grade.ID = new NotificationRepository().Create(grade.ID);
            Edit(grade);
        }

        public void Edit(Grade grade)
        {
            db.Entry(grade).State = EntityState.Modified;
            db.SaveChanges();
        }

        public void Delete(int? id)
        {
            Grade grade = Grade(id);

            if (grade != null)
            {
                db.Grades.Remove(grade);
                SaveChanges();
            }
        }

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