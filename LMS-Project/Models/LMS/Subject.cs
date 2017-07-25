using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Models.LMS
{
    public class Subject
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<Course> Courses { get; set; }
    }
}
