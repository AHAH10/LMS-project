using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LMS_Project.Models.LMS
{
    public class Subject
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public virtual ICollection<Course> Courses { get; set; }
        /*
         *  It seems like I have done something wrong with the relations between Courses and Subject
         *  Right Now : A Subject can have 1..* Courses and a Course can only have 1 Subject.
         *  It feels wrong, but don't know why.
         *  1 Course can only have 1 Teacher, It should be 1 Course can have 1..* Teachers.
         *  Am I thinking wrong? //Anton
         */
    }
}
