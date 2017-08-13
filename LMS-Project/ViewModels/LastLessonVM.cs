using LMS_Project.Models.LMS;

namespace LMS_Project.ViewModels
{
    public class LastLessonVM
    {
        public PartialUserVM Student { get; set; }
        public string SubjectName { get; set; }
        public string ClassroomName { get; set; }
        public string ClassroomLocation { get; set; }
        public string EndingTime { get; set; }
    }
}