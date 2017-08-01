using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace LMS_Project.Models.LMS
{
    public enum WeekDays
    {
        Monday,
        Tuesday,
        Wednesday,
        Thursday,
        Friday,
        Saturday,
        Sunday
    }

    public class Schedule
    {
        [Key]
        public int ID { get; set; }

        [Display(Name = "Week day")]
        public WeekDays WeekDay { get; set; }

        [Display(Name = "Beginning time")]
        public string BeginningTime { get; set; }

        [Required]
        [Display(Name = "Ending time")]
        public string EndingTime { get; set; }

        [ForeignKey("Course")]
        public int CourseID { get; set; }
        public virtual Course Course { get; set; }

        [ForeignKey("Classroom")]
        public int ClassroomID { get; set; }
        public virtual Classroom Classroom { get; set; }

        public virtual ICollection<User> Students { get; set; }

        public override string ToString()
        {
            return string.Join(" ",
                               new List<string> { WeekDay.ToString(), 
                                                  BeginningTime + "-" + EndingTime,
                                                  Course.Subject.Name,
                                                  Course.Teacher.ToString(),
                                                  Classroom.Name});
        }
    }
}