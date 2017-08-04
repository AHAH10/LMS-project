using LMS_Project.Models.LMS;
using System.Collections.Generic;

namespace LMS_Project.ViewModels
{
    public class DetailedScheduleVM
    {
        public List<Schedule> Schedules { get; set; }
        public string UsersFullName { get; set; }
        public string TeachersName { get; set; }
        public List<string> Students { get; set; }
    }
}