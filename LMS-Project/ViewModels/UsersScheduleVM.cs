using LMS_Project.Models.LMS;
using System.Collections.Generic;

namespace LMS_Project.ViewModels
{
    public class UsersScheduleVM
    {
        public string UserFullName { get; set; }
        public List<Schedule> Schedules { get; set; }
        public bool ShowDocumentsLink { get; set; }
        public bool ShowCoursesLink { get; set; }
        public bool ShowSchedulesLink { get; set; }
    }
}