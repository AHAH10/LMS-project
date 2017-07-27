using LMS_Project.Models;
using LMS_Project.Models.LMS;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace LMS_Project.Repositories
{
    public class RolesRepository : IDisposable
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public IEnumerable<Role> Roles()
        {
            return db.LMSRoles;
        }

        public Role Role(string id)
        {
            return Roles().FirstOrDefault(u => u.Id == id);
        }

        public Role RoleByName(string name)
        {
            return Roles().FirstOrDefault(r => r.Name == name);
        }

        public void Add(Role role)
        {
            if (role != null)
            {
                db.Roles.Add(role);
                SaveChanges();
            }
        }

        public void Edit(Role role)
        {
            db.Entry(role).State = EntityState.Modified;
            SaveChanges();
        }

        public void Delete(string id)
        {
            Role role = Role(id);
            if (role != null)
            {
                db.Roles.Remove(role);
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