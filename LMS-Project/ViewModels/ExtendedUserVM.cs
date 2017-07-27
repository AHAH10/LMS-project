using LMS_Project.Models.LMS;
using System.ComponentModel.DataAnnotations;

namespace LMS_Project.ViewModels
{
    public class ExtendedUserVM
    {
        public User User { get; set; }
        [Required]
        public string RoleName { get; set; }
    }
}