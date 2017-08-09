using LMS_Project.Models.LMS;
using System.ComponentModel.DataAnnotations;

namespace LMS_Project.ViewModels
{
    public class CoursesVM
    {
        public int? ID { get; set; }
        [Required]
        public string Name { get; set; }
        public Subject Subject { get; set; }
        public User Teacher { get; set; }
        public string TeacherID { get; set; }
        public int SubjectID { get; set; }
        public int DocumentCount { get; set; }
        public int ScheduleCount { get; set; }

    }
}