using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LMS_Project.ViewModels
{
    public class PartialUserVM
    {
        public string Id { get; set; }

        [Display(Name="Username")]
        public string UserName { get; set; }

        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Display(Name = "Birth Date")]
        public string BirthDate { get; set; }
        public string Email { get; set; }

        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; }

        public bool HasSchedules { get; set; }

        // Used for the users management
        public string Role { get; set; }
        public bool IsDeletable { get; set; }
        public List<string> Courses { get; set; }
    }
}