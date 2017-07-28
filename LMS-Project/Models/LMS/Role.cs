using Microsoft.AspNet.Identity.EntityFramework;
using System.Collections.Generic;

namespace LMS_Project.Models.LMS
{
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