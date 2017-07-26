using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LMS_Project.Models.LMS
{
    public class Subject
    {
        [Key]
        public int ID { get; set; }
        public string Name { get; set; }

        public virtual ICollection<Course> Courses { get; set; }
    }
}
