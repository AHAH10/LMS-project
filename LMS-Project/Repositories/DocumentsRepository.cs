using LMS_Project.Models;
using LMS_Project.Models.LMS;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace LMS_Project.Repositories
{
    public class DocumentsRepository : IDisposable
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public Document document { get; set; }

        public IEnumerable<Document> Documents()
        {
            return db.Documents;
        }

        /// <summary>
        /// Returns the list of documents uploaded for a given course
        /// </summary>
        /// <param name="courseId">Course ID</param>
        /// <returns></returns>
        public IEnumerable<Document> Documents(int courseId)
        {
            return db.Documents.Where(d => d.CourseID == courseId);
        }

        /// <summary>
        /// Returns the list of documents uploaded by a user
        /// </summary>
        /// <param name="userId">User's ID</param>
        /// <returns></returns>
        public IEnumerable<Document> Documents(string userId)
        {
            return db.Documents.Where(d => d.UploaderID == userId);
        }

        //Add
        public void Add(Document document)
        {
            db.Documents.Add(document);
            db.SaveChanges();
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
                db.SaveChanges();
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