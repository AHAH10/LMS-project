using LMS_Project.Models;
using LMS_Project.Models.LMS;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace LMS_Project.Repositories
{
    public class NotificationRepository : IDisposable
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public IEnumerable<Notification> Notifications()
        {
            return db.Notifications;
        }

        public List<Notification> Notifications(string userId)
        {
            return Notifications().Where(n => n.Grade.Document.UploaderID == userId).ToList();
        }

        public List<Notification> UnreadNotifications(string userId)
        {
            return Notifications().Where(n => n.Grade.Document.UploaderID == userId && n.ReadingDate == null).ToList();
        }

        //Create
        public void Create(int gID)
        {
            Add(new Notification
            {
                Grade = db.Grades.FirstOrDefault(g => g.ID == gID),
                SendingDate = DateTime.Now
            });
        }

        //Confirmation that Notification is read by student
        public void NotificationRead(int? id)
        {
            Notification n = db.Notifications.Where(N => N.ID == id).SingleOrDefault();
            n.ReadingDate = DateTime.Now;
            db.Entry(n).State = EntityState.Modified;
            SaveChanges();
        }
        //Add
        public void Add(Notification notification)
        {
            db.Notifications.Add(notification);
            db.SaveChanges();
        }

        public Notification Notifications(int? id)
        {
            return Notifications().FirstOrDefault(n => n.ID == id);
        }

        // Delete
        public void Delete(int id)
        {
            Notification notification = Notifications(id);
            if (notification != null)
            {
                db.Notifications.Remove(notification);
                db.SaveChanges();
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