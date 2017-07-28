using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace LMS_Project.Models.LMS
{
    public class User : ApplicationUser
    {
        [Display(Name = "First name")]
        [CustomValidation(typeof(User), "ValidateFirstName")]
        public string FirstName { get; set; }

        [Display(Name = "Last name")]
        [CustomValidation(typeof(User), "ValidateLastName")]
        public string LastName { get; set; }

        public virtual ICollection<Course> Courses { get; set; }
        public virtual ICollection<Document> Documents { get; set; }

        public static ValidationResult ValidateFirstName(string name, ValidationContext context)
        {
            ValidationResult result = null;
            Regex rgx = new Regex(@"^[a-zA-Z][a-zA-Z-]*$");

            if (!string.IsNullOrEmpty(name) && !rgx.IsMatch(name))
            {
                result = new ValidationResult(string.Format("{0} must only contain letters and hyphens.", context.DisplayName));
            }

            return result;
        }

        public static ValidationResult ValidateLastName(string name, ValidationContext context)
        {
            ValidationResult result = null;
            Regex rgx = new Regex(@"^[a-zA-Z][a-zA-Z -]*$");

            if (!string.IsNullOrEmpty(name) && !rgx.IsMatch(name))
            {
                result = new ValidationResult(string.Format("{0} must only contain letters, spaces and hyphens.", context.DisplayName));
            }

            return result;
        }
    }
}