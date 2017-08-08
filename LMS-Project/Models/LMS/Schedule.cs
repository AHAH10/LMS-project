using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace LMS_Project.Models.LMS
{
    public static class ScheduleConstants
    {
        public const string TIME_FORMAT = "HH:mm";
    }

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
            return string.Format("{0} {1}-{2}{3}{4} ({5}){3}{6}",
                                 WeekDay.ToString(), 
                                 BeginningTime,
                                 EndingTime,
                                 "\\",
                                 Course.Subject.Name,
                                 Course.Teacher.ToString(),
                                 Classroom.Name);
        }
    }
}