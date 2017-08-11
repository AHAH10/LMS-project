﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LMS_Project.Models.LMS
{
    public class Course
    {
        [Key]
        public int ID { get; set; }

        [Required]
        public string Name { get; set; }

        [NotMapped]
        public string FullName
        {
            get
            {
                return Subject.Name + " - " + Name;
            }
            private set { }
        }

        [ForeignKey("Subject")]
        public virtual int SubjectID { get; set; }
        public virtual Subject Subject { get; set; }

        [ForeignKey("Teacher")]
        public virtual string TeacherID { get; set; }
        public virtual User Teacher { get; set; }

        public virtual ICollection<Document> Documents { get; set; }
        public virtual ICollection<Schedule> Schedules { get; set; }
    }
}
