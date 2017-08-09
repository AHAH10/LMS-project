using Microsoft.AspNet.Identity.EntityFramework;
using System.Collections.Generic;

namespace LMS_Project.Models.LMS
{
    public static class RoleConstants
    {
        public static string Student
        {
            get { return "Student"; }
            private set { }
        }

        public static string Teacher
        {
            get { return "Teacher"; }
            private set { }
        }

        public static string Admin
        {
            get { return "Admin"; }
            private set { }
        }

        public static string Password(string roleName)
        {
            string password = string.Empty;

            if (roleName == RoleConstants.Teacher)
                password = "Teacher-Password1";
            else if (roleName == RoleConstants.Student)
                password = "Student-Password1";
            else if (roleName == RoleConstants.Admin)
                password = "Admin-Password1";
            
            return password;
        }
    }

    public class Role : IdentityRole
    {
        public Role()
            : base()
        {

        }

        public Role(string name)
            : base(name)
        {

        }

        public virtual ICollection<Document> Documents { get; set; }
    }
}