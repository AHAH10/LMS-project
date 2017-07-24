using System.ComponentModel.DataAnnotations;

namespace LMS_Project.Models
{
    public class Subject
    {
        [Key]
        int ID { get; set; }
        string Name { get; set; }
    }
}
