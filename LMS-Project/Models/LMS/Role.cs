using Microsoft.AspNet.Identity.EntityFramework;
using System.Collections.Generic;

namespace LMS_Project.Models.LMS
{
    public static class DefaultPassword
    {
        public static string Password(string roleName)
        {
            string password = string.Empty;

            switch (roleName)
            {
                case "Teacher":
                    password = "Teacher-Password1";
                    break;
                case "Student":
                    password = "Student-Password1";
                    break;
                case "Admin":
                    password = "Admin-Password1";
                    break;
            }

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