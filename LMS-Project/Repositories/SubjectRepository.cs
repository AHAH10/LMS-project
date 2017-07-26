using LMS_Project.Models;
using LMS_Project.Models.LMS;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace LMS_Project.Repositories
{
    public class SubjectRepository:IDisposable
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public IEnumerable<Subject> Subjects()
        {
            return db.Subjects;
        }

        public Subject Subject(int? id)
        {
            return Subjects().SingleOrDefault(s => s.ID == id);
        }

        public void Add(Subject subject)
        {
            db.Subjects.Add(subject);
            SaveChanges();
        }

        public void Edit(Subject subject)
        {
            db.Entry(subject).State = EntityState.Modified;
            db.SaveChanges();
        }

        public void Delete(int? id)
        {
            Subject subject = Subject(id);

            if (subject != null)
            {
                db.Subjects.Remove(subject);
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