using LMS_Project.Models.LMS;
using System;

namespace LMS_Project.ViewModels
{
    public class CreateEditScheduleVM
    {
        public int? ID { get; set; }

        public WeekDays WeekDay { get; set; }

        public DateTime BeginningTime { get; set; }

        public DateTime EndingTime { get; set; }

        public int CourseID { get; set; }

        public int ClassroomID { get; set; }

        public string[] Students { get; set; }
    }
}