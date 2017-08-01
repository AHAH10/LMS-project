using LMS_Project.Models;
using LMS_Project.Models.LMS;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace LMS_Project.Repositories
{
    public class UsersRepository : IDisposable
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public IEnumerable<User> Users()
        {
            return db.LMSUsers;
        }

        public IEnumerable<User> Students()
        {
            return Users().Where(u => u.Roles.Join(db.LMSRoles, 
                                                   usrRole => usrRole.RoleId,
                                                   role => role.Id,
                                                   (usrRole, role) => role)
                          .Any(r => r.Name.Equals("Student")));
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

        public async Task ChangePassword(string userId)
        {
            User user = User(userId);
            if (user != null)
            {
                UserStore<User> store = new UserStore<User>(db);
                UserManager<User> userManager = new UserManager<User>(store);
                string defaultPassord = DefaultPassword.Password(userManager.GetRoles(userId).First());
                string hashedNewPassword = userManager.PasswordHasher.HashPassword(defaultPassord);
                await store.SetPasswordHashAsync(user, hashedNewPassword);
                await store.UpdateAsync(user);
            }
        }

        public Role GetUserRole(string userId)
        {
            User user = User(userId);
            Role role = null;

            foreach (IdentityUserRole userRole in user.Roles)
            {
                role = db.LMSRoles.FirstOrDefault(r => r.Id == userRole.RoleId);
            }

            return role;
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