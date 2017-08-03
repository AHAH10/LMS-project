using LMS_Project.Models.LMS;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace LMS_Project.ViewModels
{
    public class StudentsInLesson
    {
        public string[] InLesson { get; set; }
        public List<User> Students { get; set; }

        public void Initilize(List<User> students)
        {
            Students = students;
            InLesson = new string[] { };

            if (students != null && students.Count > 0)
                InLesson = (students.Select(s => s.Id)).ToArray();
        }
    }

    public class CreateScheduleVM : IValidatableObject
    {
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

        [Required]
        public int CourseID { get; set; }

        [Required]
        public int ClassroomID { get; set; }

        public StudentsInLesson Students { get; set; }

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