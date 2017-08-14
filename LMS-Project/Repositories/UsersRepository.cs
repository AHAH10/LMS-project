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

        /// <summary>
        /// Returns the list of known teachers
        /// </summary>
        /// <returns></returns>
        public IEnumerable<User> Teachers()
        {
            return UsersInRole(RoleConstants.Teacher);
        }

        /// <summary>
        /// Returns the list of known students
        /// </summary>
        /// <returns></returns>
        public IEnumerable<User> Students()
        {
            return UsersInRole(RoleConstants.Student);
        }

        /// <summary>
        /// Returns the list of users having a given role
        /// </summary>
        /// <param name="roleName">Role the users should have</param>
        /// <returns></returns>
        private IEnumerable<User> UsersInRole(string roleName)
        {
            return Users().Where(u => u.Roles.Join(db.LMSRoles,
                                                   usrRole => usrRole.RoleId,
                                                   role => role.Id,
                                                   (usrRole, role) => role)
                          .Any(r => r.Name.Equals(roleName)));
        }

        public User UserById(string id)
        {
            return Users().FirstOrDefault(u => u.Id == id);
        }

        public User UserByUsername(string userName)
        {
            return Users().FirstOrDefault(u => string.Compare(u.UserName, userName, true) == 0);
        }

        public IEnumerable<User> UsersByUsersname(string userName)
        {
            string[] split = userName.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);

            switch (split.Length)
            {
                case 0:
                    // Empty string
                    return new List<User>();
                case 1:
                    // Whole string without spaces
                    return Users().Where(u => (u.FirstName + u.LastName).ToLower().Contains(userName.ToLower()));
                default:
                    // The string contains at least one space
                    List<User> result = Users().ToList();
                    foreach (string s in split)
                    {
                        string lowerS = s.ToLower();
                        result = result.Intersect(Users().Where(u => (u.FirstName + u.LastName).ToLower().Contains(lowerS))).ToList();
                    }

                    return result;
            }
        }

        /// <summary>
        /// Returns the list of available teachers for a given subject
        /// </summary>
        /// <param name="subjectId">Subject the teachers list is needed</param>
        /// <returns></returns>
        public IEnumerable<User> AvailableTeachers(int subjectId)
        {
            return Teachers().Where(t => !db.Courses.Where(c => c.SubjectID == subjectId).Select(c => c.TeacherID).Contains(t.Id));
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
            User user = UserById(id);
            if (user != null)
            {
                db.Users.Remove(user);
                SaveChanges();
            }
        }

        /// <summary>
        /// Allows the user to change their password or reset someone else's password
        /// </summary>
        /// <param name="userId">User ID which password is to be changed</param>
        /// <param name="newPassword">New password, or if empty, default password</param>
        /// <returns></returns>
        public async Task ChangePassword(string userId, string newPassword = "")
        {
            User user = UserById(userId);
            if (user != null)
            {
                UserStore<User> store = new UserStore<User>(db);
                UserManager<User> userManager = new UserManager<User>(store);

                if (newPassword.Length == 0)
                    newPassword = RoleConstants.Password(userManager.GetRoles(userId).First());

                string hashedNewPassword = userManager.PasswordHasher.HashPassword(newPassword);
                await store.SetPasswordHashAsync(user, hashedNewPassword);
                await store.UpdateAsync(user);
            }
        }

        public Role GetUserRole(string userId)
        {
            User user = UserById(userId);
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