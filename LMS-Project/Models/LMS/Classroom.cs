using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LMS_Project.Models.LMS
{
    public class Classroom
    {
        [Key]
        public int ID { get; set; }
        
        [Required, MaxLength(50)]
        public string Name { get; set; }

        [Required, MaxLength(255)]
        public string Location { get; set; }

        [MaxLength(255)]
        public string Remarks { get; set; }

        [Required, Range(1, int.MaxValue)]
        [Display(Name="Amount max of Students")]
        public int AmountStudentsMax { get; set; }

        public virtual ICollection<Schedule> Schedules { get; set; }
    }
}
