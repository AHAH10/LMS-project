using LMS_Project.Models.LMS;
using System.Collections.Generic;

namespace LMS_Project.ViewModels
{
    public class UsersScheduleVM
    {
        public string UserId { get; set; }
        public string UserFullName { get; set; }
        public List<Schedule> Schedules { get; set; }
    }
}