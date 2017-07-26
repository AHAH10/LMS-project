using System.Collections.Generic;

namespace LMS_Project.Models.LMS
{
    public class User : ApplicationUser
    {
        public virtual ICollection<Course> Courses { get; set; }
    }
}