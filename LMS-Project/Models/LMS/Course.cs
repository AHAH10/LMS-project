using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LMS_Project.Models.LMS
{
    public class Course
    {
        [Key]
        public int ID { get; set; }
        [ForeignKey("Subject")]
        public virtual int SubjectID { get; set; }
        public virtual Subject Subject { get; set; }
        [ForeignKey("Teacher")]
        public virtual string TeacherID { get; set; }
        public virtual User Teacher { get; set; }
    }
}
