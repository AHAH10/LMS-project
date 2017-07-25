using LMS_Project.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS_Project.Models.LMS
{
    public class Course
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey("Subject")]
        public virtual int SubjectId { get; set; }
        public virtual Subject Subject { get; set; }
        [ForeignKey("Teacher")]
        public virtual string TeacherID { get; set; }
        public virtual ApplicationUser Teacher { get; set; }

    }
}
