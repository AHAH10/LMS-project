using System.ComponentModel.DataAnnotations;

namespace LMS_Project.ViewModels
{
    public class PartialClassroomVM
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Location { get; set; }
        public string Remarks { get; set; }

        [Display(Name = "Amount max of Students")]
        public int AmountStudentsMax { get; set; }

        public bool IsEditable { get; set; }
    }
}