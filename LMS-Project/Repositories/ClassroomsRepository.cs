using LMS_Project.DataContexts;
using LMS_Project.Models.LMS;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace LMS_Project.Repositories
{
    public class ClassroomsRepository: IDisposable
    {
        private LMSDb db = new LMSDb();

        public IEnumerable<Classroom> Classrooms()
        {
            return db.Classrooms;
        }

        public Classroom Classroom(int? id)
        {
            return Classrooms().FirstOrDefault(c => c.ID == id);
        }

        public void Add(Classroom classroom)
        {
            db.Classrooms.Add(classroom);
            SaveChanges();
        }

        public void Edit(Classroom classroom)
        {
            db.Entry(classroom).State = EntityState.Modified;
            db.SaveChanges();
        }

        public void Delete(int? id)
        {
            Classroom classroom = Classroom(id);

            if (classroom != null)
            {
                db.Classrooms.Remove(classroom);
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