using LMS_Project.Models;
using LMS_Project.Models.LMS;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace LMS_Project.Repositories
{
    public class UsersRepository : IDisposable
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public IEnumerable<User> Users()
        {
            return db.LMSUsers;
        }

        public User User(string id)
        {
            return Users().FirstOrDefault(u => u.Id == id);
        }

        public void Add(User user)
        {
            if (user != null)
            {
                db.Users.Add(user);
                SaveChanges();
            }
        }

        public void Edit(User user)
        {
            db.Entry(user).State = EntityState.Modified;
            SaveChanges();
        }

        public void Delete(string id)
        {
            User user = User(id);
            if (user != null)
            {
                db.Users.Remove(user);
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