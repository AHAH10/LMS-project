using LMS_Project.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LMS_Project.Repositories
{
    public class NotificationRepository : IDisposable
    {
        private ApplicationDbContext db = new ApplicationDbContext();


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