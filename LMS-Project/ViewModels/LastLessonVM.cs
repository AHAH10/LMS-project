using LMS_Project.Models.LMS;

namespace LMS_Project.ViewModels
{
    public class LastLessonVM
    {
        public PartialUserVM Student { get; set; }
        public string SubjectName { get; set; }
        public Classroom Classroom { get; set; }
        public string EndingTime { get; set; }
    }
}