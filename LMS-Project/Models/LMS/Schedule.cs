using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace LMS_Project.Models.LMS
{
    public enum WeekDays
    {
        Monday,
        Tuesday,
        Wednesday,
        Thursday,
        Friday,
        Saturday,
        Sunday
    }

    public class Schedule : IValidatableObject
    {
        [Key]
        public int ID { get; set; }

        [Required]
        [Display(Name = "Week day")]
        [Range(0, 6, ErrorMessage = "Value for {0} must be between {1} and {2}.")]
        public WeekDays WeekDay { get; set; }

        [Required]
        [Display(Name = "Beginning time")]
        [DataType(DataType.Time)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:HH:mm}")]
        public DateTime BeginningTime { get; set; }

        [Required]
        [Display(Name = "Ending time")]
        [DataType(DataType.Time)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:HH:mm}")]
        public DateTime EndingTime { get; set; }

        [ForeignKey("Course")]
        public int CourseID { get; set; }
        public virtual Course Course { get; set; }

        [ForeignKey("Classroom")]
        public int ClassroomID { get; set; }
        public virtual Classroom Classroom { get; set; }

        public virtual ICollection<User> Students { get; set; }

        IEnumerable<ValidationResult> IValidatableObject.Validate(ValidationContext validationContext)
        {
            List<ValidationResult> res = new List<ValidationResult>();
            if (EndingTime < BeginningTime)
            {
                ValidationResult mss = new ValidationResult("The ending time must be greater than the beginning date");
                res.Add(mss);

            }
            return res;
        }
    }
}