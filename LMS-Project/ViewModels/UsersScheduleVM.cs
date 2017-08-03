using LMS_Project.Models.LMS;
using System.Collections.Generic;

namespace LMS_Project.ViewModels
{
    public class UsersScheduleVM
    {
        public string UsersFullName { get; set; }
        public List<Schedule> Schedules { get; set; }
    }
}