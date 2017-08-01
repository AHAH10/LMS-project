using LMS_Project.Models;
using LMS_Project.Models.LMS;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LMS_Project.Repositories
{
    public class DocumentsRepository : IDisposable
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public IEnumerable<Document> Documents()
        {
            return db.Documents;
        }

        //Add
        public void Add(Document document)
        {
            db.Documents.Add(document);
            SaveChanges();
        }

        public Document Document(int? id)
        {
            return Documents().FirstOrDefault(d => d.ID == id);
        }

        //Delete
        public void Delete(int id)
        {
            Document document = Document(id);
            if (document != null)
            {
                db.Documents.Remove(document);
                SaveChanges();
            }
        }

        private void SaveChanges()
        {
            db.SaveChanges();
        }

        #region IDisposable
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