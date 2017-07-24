using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS_Project.Models
{
    public class Course
    {
        [Key]
        int ID { get; set; }
        [ForeignKey("Subject")]
        public virtual int SubjectID { get; set; }
        public virtual Subject Subject { get; set; }
        [ForeignKey("Teacher")]
        public virtual int TeacherID { get; set; }
        public virtual ApplicationUser Teacher { get; set; }

    }
}
