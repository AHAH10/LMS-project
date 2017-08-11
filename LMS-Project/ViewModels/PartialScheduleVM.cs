using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace LMS_Project.ViewModels
{
    public class PartialScheduleVM
    {
        public int ID { get; set; }

        [Display(Name="Subject")]
        public string SubjectName { get; set; }
        public string Classroom { get; set; }

        [Display(Name="Teacher")]
        public string TeacherName { get; set; }
        public string WeekDay { get; set; }
        public string BeginningTime { get; set; }
        public string EndingTime { get; set; }
        public bool IsDeletable { get; set; }
    }
}