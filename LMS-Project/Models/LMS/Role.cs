using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LMS_Project.Models.LMS
{
    public class Role : IdentityRole
    {
        public virtual ICollection<Document> Documents { get; set; }
    }
}