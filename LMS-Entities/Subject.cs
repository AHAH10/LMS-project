using System.ComponentModel.DataAnnotations;

namespace LMS_Entities
{
    public class Subject
    {
        [Key]
        int ID { get; set; }
        string Name { get; set; }
    }
}
