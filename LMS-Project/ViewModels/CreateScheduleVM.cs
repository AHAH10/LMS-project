using LMS_Project.Models.LMS;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace LMS_Project.ViewModels
{
    public class CreateScheduleVM
    {
        [Range(0, 6, ErrorMessage = "Value for {0} must be between {1} and {2}.")]
        public WeekDays WeekDay { get; set; }

        public DateTime BeginningTime { get; set; }

        public DateTime EndingTime { get; set; }

        public int CourseID { get; set; }

        public int ClassroomID { get; set; }

        public string[] Students { get; set; }
    }
}